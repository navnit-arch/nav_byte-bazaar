# Python RAG with LangGraph

This project is a small learning example that shows how to build a basic Retrieval-Augmented Generation app with LangGraph, using OKF (Open Knowledge Format) files as the primary knowledge source.

## What it does

- Loads OKF markdown files from `data/knowledge` and parses YAML front matter before chunking
- Supports additional source files from `data/sample_docs` by extension (`.pdf`, `.docx`, `.csv`, `.json`, `.txt`, `.md`)
- Splits documents into chunks
- Creates local deterministic embeddings for chunks
- Stores chunks in a configurable vector database provider (currently Chroma)
- Uses LangGraph to run retrieval, prompt augmentation, generation, and response stages

## Step by Step

### 1. Create a virtual environment

```powershell
python -m venv .venv
.venv\Scripts\Activate.ps1
```

### 2. Install dependencies

```powershell
pip install -r requirements.txt
```

### 3. No model server required

This version is fully offline. You do not need Ollama or an API key.

If you want to experiment, you can still copy `.env.example` to `.env`, but it is not required.

### 4. Build the vector store

```powershell
python src\ingest.py
```

This reads OKF files in `data/knowledge` (plus optional files in `data/sample_docs`) and creates the local Chroma database in `data/chroma_db`.
If you run it again, the demo store is rebuilt from scratch so you do not get duplicate chunks.

Supported input file types for ingest:

- `.pdf` via `PyPDFLoader`
- `.docx` via `Docx2txtLoader`
- `.csv` via `CSVLoader`
- `.json` via `JSONLoader`
- `.txt` and `.md` via `TextLoader` (with UTF-8 direct-read fallback)

If a file cannot be parsed (for example, missing optional parser dependency or malformed file), ingest prints a warning and continues with the remaining files.

### 5. Run the chat app

```powershell
python src\basic_task1.py
```

Ask a question like:

- What is RAG?
- What are the two nodes in the graph?
- Why is LangGraph useful for this demo?

## Offline Notes

- No external model server is required for this offline demo.
- The embeddings are deterministic and local, so the same input produces the same vector every time.
- The final answer is a mock summary step, which keeps the LangGraph flow easy to study without external services.

## Project Files

- `src/okf.py` parses OKF YAML front matter and markdown bodies
- `src/ingest.py` implements RAG stages 1-4 (sources, chunking, embeddings, vector store)
- `src/rag_graph.py` implements RAG stages 5-8 (retrieval, augmented prompt, generation, response)
- `src/basic_task1.py` starts a simple CLI chat loop
- `data/knowledge/` holds OKF knowledge files
- `data/sample_docs/` holds extra learning documents

## Learning Flow

1. Read source files from configured data sources.
2. Parse OKF YAML front matter and markdown body (for `data/knowledge/*.md`).
3. Chunk into smaller coherent pieces.
4. Create embeddings.
5. Store in the vector database.
6. Retrieve top matches for a question.
7. Build an augmented prompt from question + context.  
8. Generate and return the answer with source names.
