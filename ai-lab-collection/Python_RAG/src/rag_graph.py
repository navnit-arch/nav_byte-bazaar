import os
from typing import TypedDict

from dotenv import load_dotenv
from langchain_chroma import Chroma
from langchain_core.documents import Document
from langgraph.graph import END, START, StateGraph

from config import VECTOR_STORE_DIR
from local_embeddings import LocalHashEmbeddings


class RAGState(TypedDict):
    question: str
    context: str
    sources: list[str]
    answer: str


def format_documents(documents: list[Document]) -> tuple[str, list[str]]:
    context_parts: list[str] = []
    sources: list[str] = []

    for index, document in enumerate(documents, start=1):
        source_name = document.metadata.get("name") or document.metadata.get("source") or f"document_{index}"
        context_parts.append(f"Source {index} ({source_name}):\n{document.page_content}")
        sources.append(str(source_name))

    return "\n\n".join(context_parts), sources


def create_vectorstore() -> Chroma:
    embeddings = LocalHashEmbeddings(dimensions=int(os.getenv("LOCAL_EMBEDDING_DIMENSIONS", "64")))
    return Chroma(
        collection_name="learning_rag",
        persist_directory=str(VECTOR_STORE_DIR),
        embedding_function=embeddings,
    )


def retrieve(state: RAGState) -> dict:
    vectorstore = create_vectorstore()
    retriever = vectorstore.as_retriever(search_kwargs={"k": 3})
    documents = retriever.invoke(state["question"])
    context, sources = format_documents(documents)
    return {"context": context, "sources": sources}


def generate(state: RAGState) -> dict:
    load_dotenv()
    context_excerpt = state["context"].strip()
    if not context_excerpt:
        answer = "I could not find any relevant context in the indexed notes."
    else:
        excerpt = " ".join(context_excerpt.split())
        answer = (
            "Navnit offline answer: , "
            f"{excerpt[:1800]}"
        )
    return {"answer": answer}


def build_graph():
    graph = StateGraph(RAGState)
    graph.add_node("retrieve", retrieve)
    graph.add_node("generate", generate)
    graph.add_edge(START, "retrieve")
    graph.add_edge("retrieve", "generate")
    graph.add_edge("generate", END)
    return graph.compile()


def ask(question: str) -> RAGState:
    app = build_graph()
    return app.invoke({"question": question, "context": "", "sources": [], "answer": ""})
