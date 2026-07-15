# Python RAG with LangGraph

This project is a small learning example that shows how to build a basic Retrieval-Augmented Generation app with LangGraph.

## What it does

- Loads files from `data/sample_docs` by extension (`.pdf`, `.docx`, `.csv`, `.json`, `.txt`, `.md`)
- Splits them into chunks
- Stores the chunks in a Chroma vector database
- Uses a deterministic local embedding model and a mock answer step so it works offline
- Uses LangGraph to run a two-step flow:
  - retrieve relevant chunks
  - generate an answer from the retrieved context

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

This reads the files in `data/sample_docs` and creates the local Chroma database in `data/chroma_db`.
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
python src\app.py
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

- `src/ingest.py` builds the vector store
- `src/rag_graph.py` defines the LangGraph workflow
- `src/app.py` starts a simple CLI chat loop
- `data/sample_docs/` holds the learning documents

## Learning Flow

1. Read the sample documents.
2. Chunk them into smaller pieces.
3. Create embeddings.
4. Store them in Chroma.
5. Retrieve the top matches for a question.
6. Send the context to the mock answer step.
7. Return the answer with the source names.
