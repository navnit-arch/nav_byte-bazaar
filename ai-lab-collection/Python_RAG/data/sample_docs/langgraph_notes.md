# LangGraph Notes

LangGraph is useful when you want an LLM application with explicit steps and state.

In a simple RAG app, the graph can have two nodes:
- retrieve: find relevant documents
- generate: write the final answer using the retrieved context

This project keeps the graph intentionally small so it is easy to study.
