---
title: RAG Architecture
type: architecture
owner: Platform Engineering
tags:
  - rag
  - architecture
  - retrieval
---
# RAG Architecture
This system uses an OKF-based ingestion pipeline where each markdown knowledge file is parsed into:
1. YAML front matter metadata
2. Markdown body content

## Retrieval Strategy
- Chunking keeps semantically coherent sections together
- Top-K retrieval returns the most relevant context
- Answer generation is grounded only in retrieved context
