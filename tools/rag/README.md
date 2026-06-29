# RAG tooling — Atena

Indexa `documentacao/rag/*.md` em ChromaDB local e oferece consulta semântica
CLI. Use para perguntar coisas como "como funciona o NSR atômico?" ou
"endpoints de bater ponto mobile" sem ter que abrir 15 arquivos.

## Setup (uma vez)

```powershell
# Python 3.10+
python -m venv tools/rag/.venv
.\tools\rag\.venv\Scripts\Activate.ps1
pip install -r tools/rag/requirements.txt
```

ou no bash:
```bash
python3 -m venv tools/rag/.venv
source tools/rag/.venv/bin/activate
pip install -r tools/rag/requirements.txt
```

## Indexar

```powershell
python tools/rag/index_rag.py
```

- Lê todos `documentacao/rag/*.md`
- Quebra em chunks por seção (`## headings`)
- Embeddings: `sentence-transformers/all-MiniLM-L6-v2` (local, 384 dims, sem API key)
- Persiste em `tools/rag/.rag-index/` (gitignored)

Quer usar OpenAI embeddings (melhor qualidade, requer key)?
```powershell
$env:OPENAI_API_KEY="sk-..."
python tools/rag/index_rag.py --embedder openai
```

## Consultar

```powershell
# Top 5 chunks mais relevantes
python tools/rag/query_rag.py "como funciona o NSR atômico?"

# Limita a uma área
python tools/rag/query_rag.py "endpoints de exportar AFD" --area rh-ponto-oficial-671-w4

# Top 3 sem o conteúdo (só ids + score)
python tools/rag/query_rag.py "hash chain ponto" --top 3 --raw
```

## Quando reindexar

Sempre que alterar arquivos em `documentacao/rag/`:
```powershell
python tools/rag/index_rag.py
```

(ou `--append` para incremental — `upsert` cuida dos duplicados, mas
chunks renomeados/removidos ficam órfãos no índice; full reindex é o caminho
seguro.)

## Trocar de vector store

ChromaDB foi escolhido por ser embedded (zero ops). Se quiser:

- **Qdrant** — `pip install qdrant-client` + ajuste em `index_rag.py` (~20 linhas
  alteradas). Bom para multi-usuário.
- **pgvector** — se o time já tem Postgres rodando.
- **LanceDB** — outra opção embedded, ótima para datasets enormes.

A interface de query (`query_rag.py`) já isola via função `coll.query(...)`,
então a troca é localizada.

## Integração com o agente (Claude Code)

Atualmente o RAG é **manual** — você roda a CLI e cola o resultado no chat.
Opções futuras:

1. **Wrapper MCP**: expor `query_rag.py` como tool MCP para o agente chamar
   direto. Plano: `tools/rag/mcp_server.py` (~50 linhas, FastMCP).
2. **Pre-prompt hook**: hook `UserPromptSubmit` que roda query automática e
   injeta top-3 chunks no contexto. Plano: `tools/rag/hooks/auto_inject.py`.
3. **Integração com o blueprint Acme**: chamar reindex em cada `git commit`
   que toque `documentacao/rag/` (git hook).

Não implementado ainda — decisão posterior.

## Layout

```
tools/rag/
├── README.md              ← este arquivo
├── requirements.txt
├── index_rag.py           ← indexador
├── query_rag.py           ← consulta CLI
├── .venv/                 ← gitignored
└── .rag-index/            ← gitignored (Chroma store)
```

## Custos

- Local (default): zero. Modelo ~80MB baixado uma vez.
- OpenAI `text-embedding-3-small`: ~$0.02 por 1M tokens. Os 15 arquivos do
  RAG dão ~15-25k tokens — < $0.001 para indexar tudo.
