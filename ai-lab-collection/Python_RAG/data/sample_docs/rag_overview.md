# RAG Overview

Retrieval-Augmented Generation (RAG) combines document retrieval with a language model.

The basic flow is:
1. Load documents.
2. Split them into chunks.
3. Create embeddings for the chunks.
4. Store the chunks in a vector database.
5. Retrieve the most relevant chunks for a question.
6. Ask the language model to answer using the retrieved context.
