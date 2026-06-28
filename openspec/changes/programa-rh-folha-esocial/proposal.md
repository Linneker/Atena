## Why

Atena não tem absolutamente nada de RH/Folha hoje — a entidade `Funcionario` é cadastro puro (nome, CPF, cargo, departamento, status), sem jornada, sem ponto, sem salário, sem cálculo, sem folha, sem eSocial.

A pedido do produto, vamos construir **o pacote completo brasileiro de gestão de pessoas**, cobrindo:

- **Ponto interno** (web + mobile MAUI nativo).
- **Ponto oficial fiscalizável** (Portaria 671/2021 — REP-P/REP-C, AFD, AEJ, NSR, ICP-Brasil).
- **Folha de pagamento completa** (INSS, IRRF, FGTS, DSR, HE, adicional noturno, peric/insalub, VT, faltas, holerite).
- **Eventos mensais** (férias, 13º, adiantamento, afastamento, licenças).
- **Rescisão** (aviso, multa 40%, TRCT).
- **CCTs estruturadas** com aplicação automática de regras (piso, HE diferenciada, anuênio).
- **eSocial completo** — 45 eventos XML/SOAP (tabelas, não-periódicos, periódicos, retificação).
- **Bridge financeira** (folha vira N ContaPagar + guias INSS/IRRF/FGTS).
- **Mobile nativo .NET MAUI** (cross-plataforma Android/iOS/Windows/macOS).

Como o esforço total é comparável ao Atena inteiro (estimado **~700-1000 tasks, 12-18 meses**), **este change é um documento-mãe**: define escopo global, decisões transversais e o roadmap de **15 changes-filhos** (W1 a W15) que serão executados em sequência. Cada onda tem seu próprio `proposal.md`, `tasks.md`, `design.md` e specs.

## What Changes

Este change **não introduz código**. Ele:

1. Documenta o **escopo do programa** e suas decisões fundadoras (Q1-Q6 abaixo).
2. Define a **ordem de execução** das 15 ondas e suas dependências.
3. Estabelece **diretrizes técnicas transversais** que toda onda deve seguir.
4. Registra **capabilities novas** que serão criadas no programa.
5. Lista **risks** e **success criteria** do programa inteiro.

## Decisões fundadoras (Q1–Q6)

| # | Pergunta | Decisão |
|---|----------|---------|
| Q1 | Sequência das waves | **Ordem do grafo W1 → W15** (entrega valor cedo: ponto interno em ~2 meses, folha em ~6, eSocial em ~9-12) |
| Q2 | Rubricas | **Por tenant** — cada empresa define seu catálogo de rubricas (mais flexível, similar a TOTVS/SAP) |
| Q3 | CCTs | **Estruturadas com aplicação automática** — vira a Wave 7 (`rh-cct-engine`) entre folha e eventos |
| Q4 | Mobile | **.NET MAUI nativo** — novo projeto `src/Mobile/Acme.Sistemas.Atena.Mobile.csproj` em C#/XAML, single-codebase Android/iOS/Windows/macOS |
| Q5 | Biometria no ponto mobile | **Câmera (foto na batida) OU digital local do device** — pelo menos um obrigatório. Preferência: câmera. Digital usa `BiometricAuthentication` do MAUI (matching local do device, não server-side template matching) |
| Q6 | Tabelas anuais (INSS/IRRF/SM) | **Endpoint admin de upload JSON/CSV** com vigência (anual ou mensal). Adicional decreto/MP pode mudar no meio do ano sem release |

## Roadmap das 15 ondas

```
W1   rh-fundacao              ─ cadastros base + role RH + UsuarioId obrigatório
W2   rh-ponto-interno         ─ marcação, ajuste, espelho, banco de horas
W3   rh-mobile-maui           ─ projeto .NET MAUI nativo (Android/iOS/Win/macOS)
W4   rh-ponto-oficial-671     ─ REP-P/REP-C, NSR, AFD, AEJ, ICP-Brasil
W5   rh-tabelas-legais        ─ INSS, IRRF, FGTS, SM, Rubricas tenant, upload admin
W6   rh-folha-engine          ─ motor de cálculo mensal, holerite PDF
W7   rh-cct-engine            ─ acordos coletivos estruturados, regras automáticas
W8   rh-eventos-mes           ─ férias, 13º, adiantamento, afastamento, licenças
W9   rh-rescisao              ─ aviso, multa 40%, TRCT
W10  rh-financeiro-bridge     ─ folha → N ContaPagar + guias INSS/IRRF/FGTS
W11  esocial-fundacao         ─ SOAP mTLS + XMLDSig + NSR + contingência (reuso NFe)
W12  esocial-tabelas          ─ S-1000 a S-1280 (eventos de TABELA)
W13  esocial-nao-periodicos   ─ S-2200 a S-2299 (admissão, desligamento, afastamento, CAT)
W14  esocial-periodicos       ─ S-1200, S-1210, S-1299, S-3000 (retificação)
W15  rh-relatorios            ─ holerite, folha, banco horas, headcount, comprovante anual
```

Dependências detalhadas em `design.md`.

## Capabilities

### New Capabilities

- `rh-cadastros` (W1) — Cadastros de pessoas estendidos: jornada, cargo, salário, benefícios, dependentes.
- `rh-ponto-interno` (W2) — Marcação, ajuste, espelho, banco de horas.
- `rh-mobile` (W3) — App mobile nativo MAUI para colaboradores.
- `rh-ponto-oficial-671` (W4) — Conformidade Portaria 671/2021 (REP-P/C, AFD, AEJ).
- `rh-tabelas-legais` (W5) — Tabelas tributárias e rubricas versionadas.
- `rh-folha` (W6) — Engine de cálculo de folha de pagamento.
- `rh-cct` (W7) — Convenções coletivas estruturadas.
- `rh-eventos-mes` (W8) — Férias, 13º, afastamento.
- `rh-rescisao` (W9) — Rescisão CLT.
- `rh-financeiro-bridge` (W10) — Integração folha ↔ Financeiro.
- `esocial-transmissao` (W11) — Cliente SOAP eSocial.
- `esocial-tabelas` (W12) — Eventos de tabela.
- `esocial-nao-periodicos` (W13) — Eventos não-periódicos.
- `esocial-periodicos` (W14) — Eventos periódicos.
- `rh-relatorios` (W15) — Relatórios gerenciais e legais.

### Modified Capabilities

- `multi-tenancy` (toda onda) — toda tabela RH carrega `tenant_id`.
- `seed-tenant-administrativo` (W1) — passa a semear role `RH` e (opcional) jornada padrão.
- `endpoint-organization` (toda onda) — novas áreas `/api/v1/rh/...`, `/api/v1/esocial/...`.

## Out of Scope (do programa inteiro)

- **Recrutamento e seleção** (ATS — Applicant Tracking System). Vira programa separado se demandado.
- **PDI/avaliação de desempenho**, **treinamentos/LMS**. Programas separados.
- **Geo-fence no ponto** (decidido: registra GPS mas não restringe). Pode entrar em change posterior.
- **Reconhecimento facial server-side com matching biométrico** (apenas foto-prova + biometria local do device).
- **REP-A** (relógio físico hardware autônomo Portaria 1.510/2009 — substituído por 671 em 2022; só REP-P/REP-C neste programa).
- **PIS/PASEP saque, FGTS saque-aniversário, programas governamentais avulsos** — fora.
- **Importação de bases externas** (TOTVS, Senior, Domínio) — change separado se cliente exigir.
- **Conformidade LGPD avançada** (consentimento granular, DPO portal) — usa o que o Atena já tem.
- **App mobile para gestor com aprovação inline** — gestor usa web. App MAUI é só para colaborador bater ponto + ver espelho + solicitar ajuste.

## Risks

| # | Risco | Mitigação |
|---|-------|-----------|
| R1 | **Escopo monstruoso (~700-1000 tasks)** — desistir no meio é provável | Quebrar em 15 changes independentes; cada onda entrega valor isolado; revalidar continuidade a cada 3 ondas |
| R2 | **Tabelas legais mudam fora do release** (decreto, MP) | Q6: endpoint admin de upload com vigência por competência |
| R3 | **CCTs por categoria são infinitas** | Estrutura genérica de "regra" + ferramenta de import; tenant cadastra a sua CCT |
| R4 | **eSocial: ambientes (Produção, Restrita, Homologação) com layouts XML que mudam por versão** | Versionar XSD; ContractResolver por versão (igual NFe); ambiente configurável por tenant |
| R5 | **MAUI ainda jovem em produção, especialmente iOS** | Plano B documentado: se MAUI travar, congela em Android+Windows; iOS via build separado |
| R6 | **Folha tem erros financeiros visíveis aos funcionários** | Cobertura de testes 90%+ no engine (W6); cada rubrica com cenário fixture brasileiro |
| R7 | **Migração de tenants existentes que nunca tiveram RH** | Toda onda é additive; UsuarioId obrigatório em Funcionario só vale para funcionário ativo pós-W1 (existentes recebem flag de legacy) |
| R8 | **Conflito com NFe e NFSe em andamento** | Ondas RH começam só após `seed-tenant-fiscal-br` arquivado e `nfse-abrasf-pluggavel` em estabilização |
| R9 | **Biometria em devices sem hardware** | Q5: câmera obrigatória nesses casos; documentar no requisito mínimo do app |
| R10 | **Custo de loja de aplicativo** (Apple Developer $99/ano, Google Play $25 único) | Custo cliente, não Atena; documentar no onboarding |

## Success Criteria do programa

- ✅ Demo end-to-end completa: colaborador bate ponto no app mobile → gestor vê na web → folha calculada no fechamento → contracheque gerado → ContaPagar lançada → S-1200 + S-1210 transmitidos ao eSocial restrita → S-1299 fechamento aceito.
- ✅ Cada uma das 15 ondas: `openspec validate <change> --strict` válido + tarefas 100% + arquivada.
- ✅ Cobertura de testes ≥ 85% em `Acme.Sistemas.Services.UnitTest` para módulos RH/Folha.
- ✅ ~30 fixtures de folha brasileira (CLT + diferenciais regionais) com valores conferidos contra cálculo de um contador.
- ✅ App MAUI publicado em Google Play (interno) e TestFlight (interno).
- ✅ eSocial em ambiente Restrita: ao menos 1 ciclo mensal completo aceito (S-1200 + S-1210 + S-1299).
- ✅ Conformidade Portaria 671/2021: AFD validado pelo aplicativo verificador do MTE.
- ✅ Documentação: `documentacao/rh/` com guia operacional + 15 docs por onda.

## Dependências externas e pré-requisitos

- **seed-tenant-fiscal-br** arquivado (precisa de tenant funcional + role-seeding pluggável).
- **nfe-cliente-sefaz-proprio** arquivado (queremos reusar `XmlSignerC14N`, `SefazSoapClient`, `CertificadoTenantResolver`, `ContingenciaPolicy` em W4 e W11).
- **nfse-abrasf-pluggavel** estabilizado (libera bandwidth fiscal antes do eSocial).
- Decisão sobre conta Apple Developer ($99/ano) e Google Play ($25 único) — antes de W3.
- Acesso a especialista em folha CLT (consultoria contábil ou contador interno) — antes de W6.
