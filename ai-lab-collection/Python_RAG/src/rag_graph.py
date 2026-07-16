from typing import TypedDict

from dotenv import load_dotenv
from langchain_core.documents import Document
from langgraph.graph import END, START, StateGraph

from config import RAG_TOP_K
from ingest import create_embedding_function, create_vectorstore
from prompts import RAG_PROMPT


class RAGState(TypedDict):
    question: str
    retrieved_docs: list[Document]
    context: str
    augmented_prompt: str
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


def retrieve_chunks(question: str, top_k: int = RAG_TOP_K) -> list[Document]:
    vectorstore = create_vectorstore(create_embedding_function())
    retriever = vectorstore.as_retriever(search_kwargs={"k": top_k})
    return retriever.invoke(question)


def build_augmented_prompt(question: str, documents: list[Document]) -> tuple[str, str, list[str]]:
    context, sources = format_documents(documents)
    prompt = RAG_PROMPT.format(question=question, context=context)
    return prompt, context, sources


def generate_answer(augmented_prompt: str, context: str) -> str:
    context_excerpt = context.strip()
    if not context_excerpt:
        return "I could not find any relevant context in the indexed notes."

    excerpt = " ".join(context_excerpt.split())
    return (
        "Navnit offline answer: "
        f"{excerpt[:1800]}"
    )


def build_response(answer: str, sources: list[str], context: str) -> dict:
    return {"answer": answer, "sources": sources, "context": context}


def retrieve(state: RAGState) -> dict:
    documents = retrieve_chunks(state["question"])
    return {"retrieved_docs": documents}


def augment(state: RAGState) -> dict:
    prompt, context, sources = build_augmented_prompt(state["question"], state["retrieved_docs"])
    return {"augmented_prompt": prompt, "context": context, "sources": sources}


def generate(state: RAGState) -> dict:
    load_dotenv()
    answer = generate_answer(state["augmented_prompt"], state["context"])
    return {"answer": answer}


def respond(state: RAGState) -> dict:
    return build_response(state["answer"], state["sources"], state["context"])


def build_graph():
    graph = StateGraph(RAGState)
    graph.add_node("retrieve", retrieve)
    graph.add_node("augment", augment)
    graph.add_node("generate", generate)
    graph.add_node("respond", respond)
    graph.add_edge(START, "retrieve")
    graph.add_edge("retrieve", "augment")
    graph.add_edge("augment", "generate")
    graph.add_edge("generate", "respond")
    graph.add_edge("respond", END)
    return graph.compile()


def ask(question: str) -> RAGState:
    app = build_graph()
    return app.invoke(
        {
            "question": question,
            "retrieved_docs": [],
            "context": "",
            "augmented_prompt": "",
            "sources": [],
            "answer": "",
        }
    )
