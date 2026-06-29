"""
Consulta o índice RAG por query em linguagem natural.

Uso:
    python tools/rag/query_rag.py "como funciona o NSR atomico?"
    python tools/rag/query_rag.py "endpoints de bater ponto mobile" --top 3
    python tools/rag/query_rag.py "como assinar XML SEFAZ" --area fiscal-nfe
"""
from __future__ import annotations

import sys
from pathlib import Path

import typer
from rich.console import Console
from rich.markdown import Markdown
from rich.panel import Panel

REPO_ROOT = Path(__file__).resolve().parents[2]
INDEX_DIR = REPO_ROOT / "tools" / "rag" / ".rag-index"

app = typer.Typer(add_completion=False, help="Consulta semântica do RAG do Atena")
console = Console()


def carregar_embedder(nome: str):
    if nome == "openai":
        import os
        from chromadb.utils.embedding_functions import OpenAIEmbeddingFunction
        key = os.environ.get("OPENAI_API_KEY")
        if not key:
            console.print("[red]OPENAI_API_KEY não definida.[/red]")
            sys.exit(1)
        return OpenAIEmbeddingFunction(api_key=key, model_name="text-embedding-3-small")
    from chromadb.utils.embedding_functions import SentenceTransformerEmbeddingFunction
    return SentenceTransformerEmbeddingFunction(model_name="sentence-transformers/all-MiniLM-L6-v2")


@app.command()
def main(
    query: str = typer.Argument(..., help="Pergunta em linguagem natural"),
    top: int = typer.Option(5, "--top", "-k", help="Quantos chunks retornar"),
    area: str | None = typer.Option(None, "--area", help="Filtrar por área (ex.: fiscal-nfe)"),
    embedder: str = typer.Option("local", help="local | openai (mesmo usado no index)"),
    raw: bool = typer.Option(False, "--raw", help="Mostra ids + score sem o conteúdo"),
):
    if not INDEX_DIR.exists():
        console.print(f"[red]Índice não encontrado em {INDEX_DIR}[/red]")
        console.print("Rode antes: [cyan]python tools/rag/index_rag.py[/cyan]")
        sys.exit(1)

    import chromadb
    client = chromadb.PersistentClient(path=str(INDEX_DIR))
    coll = client.get_collection("atena_rag", embedding_function=carregar_embedder(embedder))

    where = {"area": area} if area else None
    res = coll.query(query_texts=[query], n_results=top, where=where)

    ids = res["ids"][0]
    docs = res["documents"][0]
    metas = res["metadatas"][0]
    dists = res["distances"][0]

    if not ids:
        console.print("[yellow]Nenhum resultado.[/yellow]")
        return

    console.print(Panel.fit(f"[bold]Query:[/bold] {query}", style="cyan"))

    for i, (cid, doc, meta, dist) in enumerate(zip(ids, docs, metas, dists), 1):
        score = 1 - dist  # cosine distance → similarity
        header = f"#{i} · [bold cyan]{meta['area']}[/bold cyan] / {meta['section']} · score={score:.3f} · {meta['file']}"
        if raw:
            console.print(f"  {header}")
        else:
            console.print(Panel(Markdown(doc), title=header, border_style="dim"))


if __name__ == "__main__":
    app()
