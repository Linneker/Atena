"""
Extract inline `*Result` records from Command/Query files into dedicated *Result.cs files.

For each *Command.cs / *Query.cs in src/Service/Acme.Sistemas.Services/V1:
- if a `*Result.cs` sibling already exists, skip
- find inline `public sealed record {base}Result(...);` (and optional supporting `*Item` records)
- extract all `public sealed record` declarations EXCEPT the first one (which is the Command/Query)
- write them into `{base}Result.cs` with the same `using` directives + namespace
- remove them from the original file
"""
import os
import re
import sys
from pathlib import Path

ROOT = Path("src/Service/Acme.Sistemas.Services/V1")

# Match `public sealed record FooBar(...)` possibly multi-line until matching `);`
# We'll use a simple state machine: find lines starting `public sealed record `, then balance parens.
RECORD_RE = re.compile(r'^public sealed record (\w+)\b(.*)$')

def split_records(content_lines, command_query_type):
    """Return (cmd_lines, result_lines, leading_blocks).
    leading_blocks = preamble (using/namespace/comments/blank).
    cmd_lines = the Command/Query record declaration block.
    result_lines = subsequent record declarations that should be moved.
    """
    preamble = []
    blocks = []  # list of (record_name, list_of_lines)
    i = 0
    n = len(content_lines)
    while i < n and not content_lines[i].lstrip().startswith("public sealed record"):
        preamble.append(content_lines[i])
        i += 1

    while i < n:
        line = content_lines[i]
        m = RECORD_RE.match(line.strip())
        if not m:
            # gap between records (blank lines, comments)
            if blocks:
                blocks[-1][1].append(line)
            else:
                preamble.append(line)
            i += 1
            continue
        name = m.group(1)
        # Collect this record block: from current line until terminator `);` at end of line
        # Track parens on the opening line; if `;` already at end of opening line, we're done.
        block_lines = []
        # The record declaration spans until a line ending with `;` AT zero parens depth.
        depth = 0
        record_started = False
        while i < n:
            cur = content_lines[i]
            block_lines.append(cur)
            # Count parens, ignoring those in strings (records' params shouldn't contain ; usually)
            for ch in cur:
                if ch == '(':
                    depth += 1
                    record_started = True
                elif ch == ')':
                    depth -= 1
            stripped = cur.rstrip()
            if record_started and depth == 0 and stripped.endswith(';'):
                i += 1
                break
            if not record_started and stripped.endswith(';'):
                # record without parameters: `public sealed record Foo : IRequest<...>;`
                i += 1
                break
            i += 1
        blocks.append((name, block_lines))

    # First block is the Command/Query itself (matches command_query_type by convention)
    cmd_block = []
    result_blocks = []
    for idx, (name, lines) in enumerate(blocks):
        if idx == 0 and name == command_query_type:
            cmd_block = lines
        else:
            result_blocks.append((name, lines))
    if not cmd_block and blocks:
        # Fallback: treat first as cmd anyway
        cmd_block = blocks[0][1]
        result_blocks = [b for b in blocks[1:]]
    return preamble, cmd_block, result_blocks

def process(file_path: Path):
    base = file_path.stem  # AlterarDespesaCommand
    folder = file_path.parent
    result_path = folder / f"{base}Result.cs"
    if result_path.exists():
        return ("skip:has-result", file_path)

    content = file_path.read_text(encoding="utf-8")
    lines = content.splitlines(keepends=True)
    preamble, cmd_block, result_blocks = split_records(lines, base)
    if not result_blocks:
        return ("skip:no-result-record", file_path)

    # Extract using/namespace from preamble for the new file
    using_lines = [l for l in preamble if l.lstrip().startswith("using ")]
    namespace_line = next((l for l in preamble if l.lstrip().startswith("namespace ")), None)
    if not namespace_line:
        return ("error:no-namespace", file_path)

    # Build Result file content
    new_content_parts = []
    new_content_parts.extend(using_lines)
    if using_lines:
        new_content_parts.append("\n")
    new_content_parts.append(namespace_line)
    new_content_parts.append("\n")
    for idx, (name, blk) in enumerate(result_blocks):
        if idx > 0:
            new_content_parts.append("\n")
        # Strip leading/trailing blank lines from the block
        while blk and blk[0].strip() == "":
            blk = blk[1:]
        while blk and blk[-1].strip() == "":
            blk = blk[:-1]
        new_content_parts.extend(blk)
        # Ensure final newline
        if blk and not blk[-1].endswith("\n"):
            new_content_parts.append("\n")

    result_path.write_text("".join(new_content_parts), encoding="utf-8")

    # Rewrite the original command file removing result blocks
    new_cmd_parts = []
    new_cmd_parts.extend(preamble)
    # Ensure preamble ends with a blank line before cmd block
    if preamble and preamble[-1].strip() != "":
        new_cmd_parts.append("\n")
    new_cmd_parts.extend(cmd_block)
    if cmd_block and not cmd_block[-1].endswith("\n"):
        new_cmd_parts.append("\n")
    file_path.write_text("".join(new_cmd_parts), encoding="utf-8")

    return ("ok", file_path)

def main():
    if not ROOT.exists():
        print(f"ROOT not found: {ROOT}", file=sys.stderr)
        sys.exit(1)
    counts = {"ok": 0, "skip:has-result": 0, "skip:no-result-record": 0, "error:no-namespace": 0}
    for f in sorted(ROOT.rglob("*.cs")):
        n = f.name
        if not (n.endswith("Command.cs") or n.endswith("Query.cs")):
            continue
        if n.endswith("Handler.cs") or n.endswith("Behavior.cs") or n.endswith("Result.cs") or n.endswith("Validation.cs"):
            continue
        status, path = process(f)
        counts[status] = counts.get(status, 0) + 1
        if status not in ("skip:has-result",):
            print(f"{status}\t{path.relative_to('.')}")

    print()
    for k, v in counts.items():
        print(f"{k}: {v}")

if __name__ == "__main__":
    main()
