# Tasks — programa-rh-folha-esocial

> Documento-mãe. Tasks aqui são **gestão do programa**, não implementação. Cada onda tem seu próprio `tasks.md`.

---

## Fase 0 — Setup do programa

- [x] 0.1 Criar este change-mãe (`programa-rh-folha-esocial`) com proposal + design + roadmap
- [x] 0.2 Criar os 15 changes-filhos (W1-W15) com proposal + design + tasks + specs esqueletos
- [ ] 0.3 Apresentar roadmap ao stakeholder e obter sign-off de Q1-Q6 documentadas no proposal
- [ ] 0.4 Decidir investimento Apple Developer ($99/ano) e Google Play ($25) — gate para W3
- [ ] 0.5 Identificar/contratar consultor contábil (CLT + eSocial) — gate para W5/W6
- [ ] 0.6 Confirmar prazo de execução com produto (12-18 meses)

## Fase 1 — Pré-requisitos

- [ ] 1.1 `seed-tenant-fiscal-br` arquivado (`openspec archive seed-tenant-fiscal-br`)
- [ ] 1.2 `nfse-abrasf-pluggavel` em estabilização (≥80% das tasks)
- [ ] 1.3 `nfe-cliente-sefaz-proprio` 100% concluído e arquivado
- [ ] 1.4 Identificar componentes reutilizáveis no NFe e documentar em `documentacao/rh/reuso-componentes.md`

## Fase 2 — Execução das ondas (gates de aprovação a cada 3)

### Bloco A — Fundação + Ponto (W1-W4)
- [ ] 2.A.1 W1 `rh-fundacao` aplicado e arquivado
- [ ] 2.A.2 W2 `rh-ponto-interno` aplicado e arquivado
- [ ] 2.A.3 W3 `rh-mobile-maui` aplicado e arquivado
- [ ] 2.A.4 W4 `rh-ponto-oficial-671` aplicado e arquivado
- [ ] 2.A.5 Demo fim-a-fim: colaborador bate ponto web+mobile, RH vê espelho mensal, sistema gera AFD válido
- [ ] 2.A.6 **GATE** — aprovação para continuar Bloco B

### Bloco B — Folha + CCT (W5-W7)
- [ ] 2.B.1 W5 `rh-tabelas-legais` aplicado e arquivado
- [ ] 2.B.2 W6 `rh-folha-engine` aplicado e arquivado
- [ ] 2.B.3 W7 `rh-cct-engine` aplicado e arquivado
- [ ] 2.B.4 Demo: fechamento de competência calcula corretamente folha de funcionário com CCT
- [ ] 2.B.5 Validação cruzada com contador: 5 holerites diferentes batem com cálculo contábil externo
- [ ] 2.B.6 **GATE** — aprovação para continuar Bloco C

### Bloco C — Eventos + Bridge (W8-W10)
- [ ] 2.C.1 W8 `rh-eventos-mes` aplicado e arquivado
- [ ] 2.C.2 W9 `rh-rescisao` aplicado e arquivado
- [ ] 2.C.3 W10 `rh-financeiro-bridge` aplicado e arquivado
- [ ] 2.C.4 Demo: fluxo de férias, rescisão e folha mensal geram ContaPagar correto
- [ ] 2.C.5 **GATE** — aprovação para continuar Bloco D

### Bloco D — eSocial fundação + tabelas (W11-W12)
- [ ] 2.D.1 W11 `esocial-fundacao` aplicado e arquivado
- [ ] 2.D.2 W12 `esocial-tabelas` aplicado e arquivado
- [ ] 2.D.3 Demo: S-1000 + S-1005 + S-1010 + S-1020 transmitidos para Restrita com sucesso
- [ ] 2.D.4 **GATE** — aprovação para continuar Bloco E

### Bloco E — eSocial não-periódicos + periódicos (W13-W14)
- [ ] 2.E.1 W13 `esocial-nao-periodicos` aplicado e arquivado
- [ ] 2.E.2 W14 `esocial-periodicos` aplicado e arquivado
- [ ] 2.E.3 Demo: ciclo mensal completo S-1200 + S-1210 + S-1299 aceito em Restrita
- [ ] 2.E.4 **GATE** — aprovação para continuar Bloco F

### Bloco F — Relatórios (W15)
- [ ] 2.F.1 W15 `rh-relatorios` aplicado e arquivado
- [ ] 2.F.2 Demo: relatórios de folha, banco horas, headcount, comprovante anual gerados

## Fase 3 — Arquivamento do programa

- [ ] 3.1 Validar todos os 15 changes arquivados
- [ ] 3.2 Specs do `openspec/specs/` consolidados (rh-cadastros, rh-ponto-interno, ..., esocial-*)
- [ ] 3.3 Documentação `documentacao/rh/` completa
- [ ] 3.4 Treinamento interno do time de suporte
- [ ] 3.5 `openspec archive programa-rh-folha-esocial`

---

## Critérios de saída por bloco

| Bloco | Critério de saída |
|-------|-------------------|
| A | App MAUI publicado interno; ponto interno + 671 funcionais; AFD valida no app verificador MTE |
| B | Folha mensal calcula corretamente para 5 perfis (CLT padrão, com CCT, com HE, com noturno, com peric/insalub) |
| C | Folha → Financeiro funcional ponta-a-ponta; rescisão completa CLT |
| D | eSocial Restrita aceita eventos de tabela; transmissão SOAP mTLS estável |
| E | Ciclo eSocial mensal completo aceito; retificação testada |
| F | Programa pronto para Produção em ao menos 1 tenant piloto |
