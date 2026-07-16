from pathlib import Path
from typing import Any

import yaml
from langchain_core.documents import Document


class OKFParseError(ValueError):
    pass


def split_okf_front_matter(markdown_text: str) -> tuple[str, str]:
    lines = markdown_text.splitlines()
    if not lines or lines[0].strip() != "---":
        raise OKFParseError("Missing YAML front matter start delimiter '---'")

    end_index = None
    for idx in range(1, len(lines)):
        if lines[idx].strip() == "---":
            end_index = idx
            break

    if end_index is None:
        raise OKFParseError("Missing YAML front matter end delimiter '---'")

    front_matter_text = "\n".join(lines[1:end_index]).strip()
    body = "\n".join(lines[end_index + 1 :]).strip()
    return front_matter_text, body


def parse_okf_markdown(markdown_text: str) -> tuple[dict[str, Any], str]:
    front_matter_text, body = split_okf_front_matter(markdown_text)
    parsed = yaml.safe_load(front_matter_text) if front_matter_text else {}

    if parsed is None:
        parsed = {}
    if not isinstance(parsed, dict):
        raise OKFParseError("YAML front matter must parse into an object/map")

    return parsed, body


def load_okf_document(file_path: Path) -> Document:
    raw_text = file_path.read_text(encoding="utf-8")
    front_matter, body = parse_okf_markdown(raw_text)

    metadata = {
        "source": str(file_path),
        "name": file_path.name,
        "extension": file_path.suffix.lower(),
        "okf": True,
        **front_matter,
    }

    return Document(page_content=body, metadata=metadata)
