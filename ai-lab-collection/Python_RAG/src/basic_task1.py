from dotenv import load_dotenv

from rag_graph import ask


def main() -> None:
    load_dotenv()
    print("LangGraph RAG demo")
    print("Type a question and press Enter. Type 'exit' to quit.\n")

    while True:
        question = input("Question: ").strip()
        if question.lower() in {"exit", "quit"}:
            break
        if not question:
            continue

        result = ask(question)
        print("\nAnswer:")
        print(result["answer"])
        if result["sources"]:
            print("\nSources:")
            for source in result["sources"]:
                print(f"- {source}")
        print()


if __name__ == "__main__":
    main()
