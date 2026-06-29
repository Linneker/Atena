## Resumo

<!-- 1-3 bullets: o que mudou e por quê. Foco no "porquê". -->

-

## Checklist

- [ ] `dotnet build Atena.sln` passou local (0 erros)
- [ ] `dotnet test` relevante passou local
- [ ] **Knowledge base RAG atualizada** — `documentacao/rag/<funcionalidade>.md`
  reflete a mudança (ou justifique no PR se não aplicável)
- [ ] CLAUDE.md atualizado quando há nova capability / decisão arquitetural
- [ ] Migration nova segue convenção `Vyyyymmddxxx_Descricao.cs`
- [ ] Endpoint novo segue blueprint (4 arquivos: Request + Response + Map + Endpoint)
- [ ] Command/Query novo segue vertical (5 arquivos: Command + Handler + Behavior + Result + Validation)
- [ ] Permissões novas registradas em `Acme.Sistemas.Core.Const.Permissions`

## Áreas RAG candidatas

Marque qual(is) precisaram de atualização (correlate com `documentacao/rag/INDEX.md`):

- [ ] plataforma
- [ ] auditoria-observabilidade
- [ ] infraestrutura
- [ ] cadastros
- [ ] financeiro
- [ ] estoque
- [ ] compras
- [ ] vendas
- [ ] fiscal-nfe
- [ ] rh-fundacao-w1
- [ ] rh-ponto-interno-w2
- [ ] rh-mobile-w3
- [ ] rh-ponto-oficial-671-w4
- [ ] frontend-angular
- [ ] mobile-maui
- [ ] **Nenhuma** (mudança puramente cosmética / refactor interno sem impacto em entidades, endpoints ou decisões)

## Test plan

- [ ]
- [ ]

## Notas para revisão

<!-- Pontos que merecem atenção do revisor (decisões não-óbvias, trade-offs). -->

🤖 Generated with [Claude Code](https://claude.com/claude-code)
