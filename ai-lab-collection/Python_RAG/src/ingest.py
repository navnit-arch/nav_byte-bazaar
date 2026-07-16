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
from langchain_core.embeddings import Embeddings

from config import (
    CHUNK_OVERLAP,
    CHUNK_SIZE,
    DOCS_DIR,
    EMBEDDING_MODEL,
    EMBEDDING_PROVIDER,
    KNOWLEDGE_DIR,
    LOCAL_EMBEDDING_DIMENSIONS,
    VECTOR_COLLECTION_NAME,
    VECTOR_DB_PROVIDER,
    VECTOR_STORE_DIR,
)
from local_embeddings import LocalHashEmbeddings
from okf import load_okf_document


SUPPORTED_EXTENSIONS = {".pdf", ".docx", ".csv", ".json", ".txt", ".md"}


def _load_text_file(file_path: Path) -> list[Document]:
    try:
        return TextLoader(str(file_path), encoding="utf-8").load()
    except Exception:
        text = file_path.read_text(encoding="utf-8")
        return [Document(page_content=text, metadata={})]


def load_file(file_path: Path) -> list[Document]:
    suffix = file_path.suffix.lower()

    if file_path.suffix.lower() == ".md" and KNOWLEDGE_DIR in file_path.parents:
        return [load_okf_document(file_path)]
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


def discover_data_sources() -> list[Path]:
    sources: list[Path] = []
    if KNOWLEDGE_DIR.exists():
        sources.append(KNOWLEDGE_DIR)
    if DOCS_DIR.exists() and DOCS_DIR != KNOWLEDGE_DIR:
        sources.append(DOCS_DIR)
    return sources


def ingest_documents(data_sources: Iterable[Path]) -> tuple[list[Document], list[str]]:
    all_documents: list[Document] = []
    all_warnings: list[str] = []

    for source_dir in data_sources:
        documents, warnings = load_documents(source_dir)
        all_documents.extend(documents)
        all_warnings.extend(warnings)

    return all_documents, all_warnings


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


def chunk_documents(
    documents: Iterable[Document],
    chunk_size: int = CHUNK_SIZE,
    chunk_overlap: int = CHUNK_OVERLAP,
) -> list[Document]:
    chunked_documents: list[Document] = []

    for document in documents:
        chunks = split_text(document.page_content, chunk_size=chunk_size, chunk_overlap=chunk_overlap)
        for index, chunk in enumerate(chunks):
            chunked_documents.append(
                Document(
                    page_content=chunk,
                    metadata={**document.metadata, "chunk": index},
                )
            )

    return chunked_documents


def create_embedding_function() -> Embeddings:
    if EMBEDDING_PROVIDER == "local_hash":
        return LocalHashEmbeddings(dimensions=LOCAL_EMBEDDING_DIMENSIONS)

    raise ValueError(
        f"Unsupported EMBEDDING_PROVIDER={EMBEDDING_PROVIDER!r}. "
        "Set EMBEDDING_PROVIDER=local_hash."
    )


def embed_chunks(documents: list[Document], embedding_function: Embeddings) -> list[list[float]]:
    return embedding_function.embed_documents([document.page_content for document in documents])


def create_vectorstore(embedding_function: Embeddings) -> Chroma:
    if VECTOR_DB_PROVIDER != "chroma":
        raise ValueError(
            f"Unsupported VECTOR_DB_PROVIDER={VECTOR_DB_PROVIDER!r}. "
            "Set VECTOR_DB_PROVIDER=chroma."
        )

    return Chroma(
        collection_name=VECTOR_COLLECTION_NAME,
        persist_directory=str(VECTOR_STORE_DIR),
        embedding_function=embedding_function,
    )


def store_embeddings(documents: list[Document], vectors: list[list[float]], embedding_function: Embeddings) -> Chroma:
    if len(documents) != len(vectors):
        raise ValueError("Mismatch between number of chunk documents and embeddings")

    if VECTOR_STORE_DIR.exists():
        shutil.rmtree(VECTOR_STORE_DIR)

    vectorstore = create_vectorstore(embedding_function)
    vectorstore.add_documents(documents)
    return vectorstore


def build_vectorstore() -> Chroma:
    load_dotenv()

    data_sources = discover_data_sources()
    if not data_sources:
        raise RuntimeError("No data sources found. Expected data/knowledge or data/sample_docs")

    documents, warnings = ingest_documents(data_sources)
    for warning in warnings:
        print(f"Warning: {warning}")

    if not documents:
        raise RuntimeError("No supported files found in configured data sources")

    chunked_documents = chunk_documents(documents)
    embedding_function = create_embedding_function()
    vectors = embed_chunks(chunked_documents, embedding_function)
    return store_embeddings(chunked_documents, vectors, embedding_function)


def main() -> None:
    build_vectorstore()
    print(
        f"Indexed documents into {VECTOR_STORE_DIR} using "
        f"embedding_provider={EMBEDDING_PROVIDER}, embedding_model={EMBEDDING_MODEL}, "
        f"vector_db_provider={VECTOR_DB_PROVIDER}"
    )


if __name__ == "__main__":
    main()
