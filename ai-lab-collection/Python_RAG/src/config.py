from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent
DATA_DIR = BASE_DIR / "data"
DOCS_DIR = DATA_DIR / "sample_docs"
VECTOR_STORE_DIR = DATA_DIR / "chroma_db"

DEFAULT_MODEL = "llama3.1"
DEFAULT_EMBEDDING_MODEL = "nomic-embed-text"
