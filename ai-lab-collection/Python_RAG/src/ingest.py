import os
import shutil
from pathlib import Path
from typing import Iterable

from dotenv import load_dotenv
from langchain_chroma import Chroma
from langchain_community.document_loaders import (
    CSVLoader,
    Docx2txtLoader,
    JSONLoader,
    PyPDFLoader,
    TextLoader,
)
from langchain_core.documents import Document

from config import DEFAULT_EMBEDDING_MODEL, DOCS_DIR, VECTOR_STORE_DIR
from local_embeddings import LocalHashEmbeddings


SUPPORTED_EXTENSIONS = {".pdf", ".docx", ".csv", ".json", ".txt", ".md"}


def _load_text_file(file_path: Path) -> list[Document]:
    try:
        return TextLoader(str(file_path), encoding="utf-8").load()
    except Exception:
        text = file_path.read_text(encoding="utf-8")
        return [Document(page_content=text, metadata={})]


def load_file(file_path: Path) -> list[Document]:
    suffix = file_path.suffix.lower()

    if suffix == ".pdf":
        loaded_documents = PyPDFLoader(str(file_path)).load()
    elif suffix == ".docx":
        loaded_documents = Docx2txtLoader(str(file_path)).load()
    elif suffix == ".csv":
        loaded_documents = CSVLoader(str(file_path), encoding="utf-8").load()
    elif suffix == ".json":
        loaded_documents = JSONLoader(str(file_path), jq_schema=".", text_content=False).load()
    elif suffix in {".txt", ".md"}:
        loaded_documents = _load_text_file(file_path)
    else:
        return []

    for document in loaded_documents:
        document.metadata = {
            **document.metadata,
            "source": str(file_path),
            "name": file_path.name,
            "extension": suffix,
        }

    return loaded_documents


def load_documents(source_dir: Path) -> tuple[list[Document], list[str]]:
    documents: list[Document] = []
    warnings: list[str] = []

    for file_path in sorted(source_dir.rglob("*")):
        if not file_path.is_file():
            continue
        if file_path.suffix.lower() not in SUPPORTED_EXTENSIONS:
            continue
        try:
            documents.extend(load_file(file_path))
        except Exception as exc:
            warnings.append(f"Skipped {file_path.name}: {exc}")

    return documents, warnings


def split_text(text: str, chunk_size: int = 600, chunk_overlap: int = 100) -> list[str]:
    if chunk_size <= 0:
        raise ValueError("chunk_size must be greater than zero")

    if chunk_overlap >= chunk_size:
        raise ValueError("chunk_overlap must be smaller than chunk_size")

    chunks: list[str] = []
    start = 0
    text_length = len(text)

    while start < text_length:
        end = min(start + chunk_size, text_length)
        chunks.append(text[start:end].strip())
        if end == text_length:
            break
        start = end - chunk_overlap

    return [chunk for chunk in chunks if chunk]


def chunk_documents(documents: Iterable[Document]) -> list[Document]:
    chunked_documents: list[Document] = []

    for document in documents:
        chunks = split_text(document.page_content)
        for index, chunk in enumerate(chunks):
            chunked_documents.append(
                Document(
                    page_content=chunk,
                    metadata={**document.metadata, "chunk": index},
                )
            )

    return chunked_documents


def build_vectorstore() -> Chroma:
    load_dotenv()

    documents, warnings = load_documents(DOCS_DIR)
    for warning in warnings:
        print(f"Warning: {warning}")

    if not documents:
        raise RuntimeError(f"No supported files found in {DOCS_DIR}")

    chunked_documents = chunk_documents(documents)
    embeddings = LocalHashEmbeddings(dimensions=int(os.getenv("LOCAL_EMBEDDING_DIMENSIONS", "64")))

    if VECTOR_STORE_DIR.exists():
        shutil.rmtree(VECTOR_STORE_DIR)

    vectorstore = Chroma(
        collection_name="learning_rag",
        persist_directory=str(VECTOR_STORE_DIR),
        embedding_function=embeddings,
    )
    vectorstore.add_documents(chunked_documents)
    return vectorstore


def main() -> None:
    build_vectorstore()
    print(f"Indexed documents into {VECTOR_STORE_DIR}")


if __name__ == "__main__":
    main()
