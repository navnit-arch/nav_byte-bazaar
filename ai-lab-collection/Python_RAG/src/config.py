import os
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent
DATA_DIR = BASE_DIR / "data"
DOCS_DIR = DATA_DIR / "sample_docs"
KNOWLEDGE_DIR = DATA_DIR / "knowledge"
VECTOR_STORE_DIR = DATA_DIR / "chroma_db"

DEFAULT_MODEL = "llama3.1"
DEFAULT_EMBEDDING_MODEL = "nomic-embed-text"

VECTOR_DB_PROVIDER = os.getenv("VECTOR_DB_PROVIDER", "chroma").strip().lower()
VECTOR_COLLECTION_NAME = os.getenv("VECTOR_COLLECTION_NAME", "learning_rag").strip() or "learning_rag"

EMBEDDING_PROVIDER = os.getenv("EMBEDDING_PROVIDER", "local_hash").strip().lower()
EMBEDDING_MODEL = os.getenv("EMBEDDING_MODEL", DEFAULT_EMBEDDING_MODEL).strip()
LOCAL_EMBEDDING_DIMENSIONS = int(os.getenv("LOCAL_EMBEDDING_DIMENSIONS", "64"))

RAG_TOP_K = int(os.getenv("RAG_TOP_K", "3"))
CHUNK_SIZE = int(os.getenv("CHUNK_SIZE", "600"))
CHUNK_OVERLAP = int(os.getenv("CHUNK_OVERLAP", "100"))
