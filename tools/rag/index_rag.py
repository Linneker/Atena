"""
Indexa documentacao/rag/*.md em um vector store ChromaDB local.

Chunking: por seção (## headings) — preserva contexto auto-contido. Metadata:
{file, section, area}.

Embeddings: sentence-transformers/all-MiniLM-L6-v2 (default, 384 dims, local,
sem API key). Para OpenAI, exporta OPENAI_API_KEY e roda com `--embedder openai`.

Uso:
    pip install -r tools/rag/requirements.txt
    python tools/rag/index_rag.py            # indexa do zero (apaga .rag-index/)
    python tools/rag/index_rag.py --append   # incrementa
"""
from __future__ import annotations

import re
import sys
from pathlib import Path
from typing import Iterator

import typer
from rich.console import Console
from rich.progress import track

REPO_ROOT = Path(__file__).resolve().parents[2]
RAG_DIR = REPO_ROOT / "documentacao" / "rag"
INDEX_DIR = REPO_ROOT / "tools" / "rag" / ".rag-index"

app = typer.Typer(add_completion=False, help="Indexador RAG do Atena")
console = Console()


def chunks_de_arquivo(path: Path) -> Iterator[dict]:
    """Quebra um .md em chunks por seção (## headings).
    Sub-seções (###) ficam dentro do chunk do parent (##)."""
    texto = path.read_text(encoding="utf-8")
    linhas = texto.splitlines()

    titulo = path.stem
    secao_atual = "intro"
    buffer: list[str] = []

    def flush(secao: str, conteudo: list[str]):
        body = "\n".join(conteudo).strip()
        if not body:
            return None
        return {
            "id": f"{titulo}::{secao}",
            "document": f"# {titulo} — {secao}\n\n{body}",
            "metadata": {"file": path.name, "area": titulo, "section": secao},
        }

    for linha in linhas:
        m = re.match(r"^##\s+(.+?)\s*$", linha)
        if m:
            doc = flush(secao_atual, buffer)
            if doc:
                yield doc
            secao_atual = m.group(1).strip()
            buffer = []
        else:
            buffer.append(linha)

    doc = flush(secao_atual, buffer)
    if doc:
        yield doc


def carregar_embedder(nome: str):
    if nome == "openai":
        try:
            from chromadb.utils.embedding_functions import OpenAIEmbeddingFunction
        except ImportError:
            console.print("[red]pip install openai chromadb[openai-embed][/red]")
            sys.exit(1)
        import os
        key = os.environ.get("OPENAI_API_KEY")
        if not key:
            console.print("[red]Defina OPENAI_API_KEY no ambiente.[/red]")
            sys.exit(1)
        return OpenAIEmbeddingFunction(api_key=key, model_name="text-embedding-3-small")
    # Default: local sentence-transformers
    from chromadb.utils.embedding_functions import SentenceTransformerEmbeddingFunction
    return SentenceTransformerEmbeddingFunction(model_name="sentence-transformers/all-MiniLM-L6-v2")


@app.command()
def main(
    embedder: str = typer.Option("local", help="local | openai"),
    append: bool = typer.Option(False, help="Não apaga o índice; só adiciona/atualiza"),
):
    if not RAG_DIR.exists():
        console.print(f"[red]Não achei {RAG_DIR}[/red]")
        sys.exit(1)

    import chromadb
    INDEX_DIR.mkdir(parents=True, exist_ok=True)
    client = chromadb.PersistentClient(path=str(INDEX_DIR))

    if not append:
        try:
            client.delete_collection("atena_rag")
            console.print("[yellow]Coleção atena_rag apagada[/yellow]")
        except Exception:
            pass

    coll = client.get_or_create_collection(
        name="atena_rag",
        embedding_function=carregar_embedder(embedder),
        metadata={"hnsw:space": "cosine"},
    )

    arquivos = sorted(RAG_DIR.glob("*.md"))
    console.print(f"Encontrei [cyan]{len(arquivos)}[/cyan] arquivos em {RAG_DIR}")

    ids, docs, metas = [], [], []
    for f in arquivos:
        for chunk in chunks_de_arquivo(f):
            ids.append(chunk["id"])
            docs.append(chunk["document"])
            metas.append(chunk["metadata"])

    console.print(f"Total de chunks: [cyan]{len(ids)}[/cyan]")
    if not ids:
        console.print("[yellow]Nada para indexar.[/yellow]")
        return

    batch = 100
    for i in track(range(0, len(ids), batch), description="Indexando..."):
        coll.upsert(
            ids=ids[i:i + batch],
            documents=docs[i:i + batch],
            metadatas=metas[i:i + batch],
        )

    console.print(f"[green]✓ Indexado em {INDEX_DIR}[/green]")
    console.print(f"  Areas: {sorted(set(m['area'] for m in metas))}")


if __name__ == "__main__":
    app()
