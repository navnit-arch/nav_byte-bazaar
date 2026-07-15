import math
import hashlib
import re
from collections import Counter

from langchain_core.embeddings import Embeddings


class LocalHashEmbeddings(Embeddings):
    def __init__(self, dimensions: int = 64) -> None:
        if dimensions <= 0:
            raise ValueError("dimensions must be greater than zero")
        self.dimensions = dimensions

    def _vectorize(self, text: str) -> list[float]:
        tokens = re.findall(r"[a-z0-9]+", text.lower())
        counts = Counter(tokens)
        vector = [0.0] * self.dimensions

        for token, count in counts.items():
            digest = hashlib.sha256(token.encode("utf-8")).digest()
            index = int.from_bytes(digest[:4], "big") % self.dimensions
            sign = -1.0 if digest[4] % 2 else 1.0
            vector[index] += sign * float(count)

        magnitude = math.sqrt(sum(value * value for value in vector))
        if magnitude:
            vector = [value / magnitude for value in vector]

        return vector

    def embed_documents(self, texts: list[str]) -> list[list[float]]:
        return [self._vectorize(text) for text in texts]

    def embed_query(self, text: str) -> list[float]:
        return self._vectorize(text)