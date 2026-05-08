#!/usr/bin/env bash
# Gera *Behavior.cs (no-op) para cada *Command.cs / *Query.cs em Services/V1
# que ainda não tem Behavior.
set -euo pipefail

ROOT="src/Service/Acme.Sistemas.Services/V1"
created=0; skipped=0

for f in $(find "$ROOT" \( -name "*Command.cs" -o -name "*Query.cs" \) | grep -vE "Handler|Behavior|Validation|Result"); do
  dir=$(dirname "$f")
  base=$(basename "$f" .cs)            # ex AlterarDespesaCommand
  out="$dir/${base}Behavior.cs"
  if [ -f "$out" ]; then
    skipped=$((skipped+1)); continue
  fi

  # Extract namespace
  ns=$(grep -m1 -E '^namespace ' "$f" | sed -E 's/^namespace[[:space:]]+([^;{]+)[;{]?.*/\1/' | tr -d ' ')

  # Extract response type from `IRequest<TResp>` after the type's "(" parameters.
  # Matches: ` : IRequest<ResponseDefault<XxxResult>>;` or `IRequest<XxxResult>;`
  resp=$(LC_ALL=en_US.UTF-8 grep -oP 'IRequest<\K[^;]+(?=>;|>$)' "$f" | head -1 || true)
  if [ -z "${resp:-}" ]; then
    # Fallback: extract everything between "IRequest<" and the trailing ">;" or ">"
    resp=$(sed -nE 's/.*IRequest<(.+)>[;]?[[:space:]]*$/\1/p' "$f" | head -1)
  fi

  # If response is `Unit`, keep it. If empty, fallback Unit.
  if [ -z "$resp" ]; then resp="Unit"; fi

  # Build the file
  cat > "$out" <<EOF
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace ${ns};

/// <summary>
/// Behavior específico do ${base}. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ${base}Behavior
    : IPipelineBehavior<${base}, ${resp}>
{
    public Task<${resp}> Handle(
        ${base} request,
        RequestHandlerDelegate<${resp}> next,
        CancellationToken cancellationToken) => next();
}
EOF
  created=$((created+1))
done

echo "created=$created skipped=$skipped"
