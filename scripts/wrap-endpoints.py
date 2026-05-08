"""
Wrap loose `*Endpoint.cs` files (not yet in per-verb subfolders) into their own folder
plus generate Response.cs / Map.cs markers if missing. Updates the namespace to include
the verb-folder.

Skips: files already inside a per-verb subfolder (3 levels deep under Endpoints/V1).
"""
import re
from pathlib import Path

ROOT = Path("src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1")

NS_RE = re.compile(r'^(namespace\s+)([\w.]+)(\s*;)?\s*$', re.MULTILINE)

def is_in_verb_folder(file: Path) -> bool:
    # ROOT/<recurso>/<file>.cs   → 1 level inside ROOT (loose)
    # ROOT/<recurso>/<verb>/<file>.cs → 2 levels (already wrapped)
    rel = file.relative_to(ROOT)
    return len(rel.parts) >= 3  # recurso/verb/file.cs

def wrap(file: Path):
    name = file.stem  # ex CriarDespesaEndpoint
    if not name.endswith("Endpoint"):
        return ("skip:not-endpoint", file)
    base = name[:-len("Endpoint")]  # CriarDespesa
    if is_in_verb_folder(file):
        return ("skip:already-wrapped", file)

    parent = file.parent  # Endpoints/V1/<Recurso>
    new_dir = parent / base
    new_dir.mkdir(exist_ok=True)
    new_file = new_dir / file.name

    content = file.read_text(encoding="utf-8")

    # Update namespace: append `.{base}` if not already present
    def ns_repl(m):
        kw, ns, semi = m.group(1), m.group(2), m.group(3) or ""
        if ns.endswith(f".{base}"):
            return m.group(0)
        return f"{kw}{ns}.{base}{semi}"
    new_content = NS_RE.sub(ns_repl, content, count=1)

    new_file.write_text(new_content, encoding="utf-8")
    file.unlink()

    # Create Response.cs marker if missing
    resp = new_dir / f"{base}Response.cs"
    if not resp.exists():
        # Read namespace from the moved file
        m = NS_RE.search(new_content)
        ns = m.group(2) if m else ""
        resp.write_text(
            f"namespace {ns};\n\n"
            f"// Response do {base}Endpoint = Result do Command/Query correspondente.\n"
            f"// Endpoint repassa direto sem reshape adicional.\n",
            encoding="utf-8",
        )

    # Create Map.cs marker if missing
    mp = new_dir / f"{base}Map.cs"
    if not mp.exists():
        m = NS_RE.search(new_content)
        ns = m.group(2) if m else ""
        mp.write_text(
            f"namespace {ns};\n\n"
            f"// {base}: mapping inline (parâmetros HTTP → Command/Query) feito no próprio\n"
            f"// {base}Endpoint. Map.cs vazio mantido por convenção do blueprint.\n",
            encoding="utf-8",
        )

    return ("ok", file)

def main():
    counts = {}
    for f in sorted(ROOT.rglob("*Endpoint.cs")):
        status, _ = wrap(f)
        counts[status] = counts.get(status, 0) + 1
        if status == "ok":
            print(f"ok\t{f.relative_to('.')}")
    print()
    for k, v in sorted(counts.items()):
        print(f"{k}: {v}")

if __name__ == "__main__":
    main()
