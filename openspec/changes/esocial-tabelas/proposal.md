## Why

W12. Primeiros eventos eSocial transmitidos: **eventos de TABELA** (S-1000 a S-1280). São cadastrais do empregador — vivem por anos no eSocial e mudam pouco. São pré-requisito de tudo: sem S-1000 (Empregador) e S-1010 (Rubricas) cadastrados na base do governo, nenhum evento de remuneração (W14) é aceito.

## What Changes

### Eventos implementados nesta onda

| Código | Nome | Quando criar |
|--------|------|--------------|
| **S-1000** | Empregador | Ao configurar `EmpregadorEsocial` (W11) ou alterar dados-base da empresa |
| **S-1005** | Estabelecimentos / Obras / Unidades | CRUD de Lotação (W1) → gera/altera S-1005 |
| **S-1010** | Rubricas | CRUD de RubricaTenant (W5) → gera/altera S-1010 |
| **S-1020** | Lotações Tributárias | CRUD de Lotação (W1) — alguns casos exigem evento separado |
| **S-1070** | Tabela de Processos Administrativos/Judiciais | (raro) — admin cria manual |
| **S-1080** | Operadores Portuários | (apenas setor portuário) |
| **S-1200** | Remuneração (vai em W14, não aqui) | — |
| **S-1280** | Informações Complementares (desoneração) | Ao mudar flag de desoneração da folha |

### Implementação

Para cada evento:
- Class POCO + mapeador `XmlSerializer` em `Esocial/Eventos/V1_2/Sxxxx/`
- Builder: `Sxxxx Builder` recebe entidade do Atena → produz POCO
- Hook automático nos repositórios: ao salvar `EmpregadorEsocial`, dispara `GerarEventoS1000Command`
- Padrão: assina → fica em estado Assinado → worker da W11 envia

### Endpoints

```
POST /api/v1/esocial/tabelas/s-1000/gerar              força regeração
POST /api/v1/esocial/tabelas/s-1005/gerar?lotacaoId=
POST /api/v1/esocial/tabelas/s-1010/gerar?rubricaCodigo=
POST /api/v1/esocial/tabelas/s-1020/gerar
POST /api/v1/esocial/tabelas/s-1070/gerar
GET  /api/v1/esocial/tabelas/status                    estado de cada tabela
```

### Sync com Atena

```
Atena                          eSocial
EmpregadorEsocial            S-1000
Lotacao (W1)                 S-1005 + S-1020 (se tributária)
RubricaTenant (W5)           S-1010 com natureza eSocial
Empresa.desoneracao           S-1280
```

Trigger: hook no `Save` de cada entidade dispara `GerarEventoSxxxxCommand` que prepara o evento e enfileira.

## Capabilities

### New Capabilities
- `esocial-tabelas` — Eventos de tabela do empregador (S-1000, S-1005, S-1010, S-1020, S-1070, S-1080, S-1280).

### Modified Capabilities
- `esocial-transmissao` — pipeline criado em W11 transmite estes eventos.
- `rh-cadastros` — alterações em Lotacao geram evento.
- `rh-tabelas-legais` — alterações em RubricaTenant geram evento.

## Out of Scope
- Eventos não-periódicos (W13).
- Eventos periódicos (W14).
- Edição manual de eventos (gerados automaticamente do estado do Atena).

## Risks

- **R1**: XSD por versão — S-1.2 vigente em 2026. Mitigação: versionar.
- **R2**: Ordem de transmissão — S-1000 antes de S-1005, antes de S-1010. Mitigação: orquestrador verifica precedência.
- **R3**: Natureza eSocial em rubricas tenant pode estar errada. Mitigação: validação ao salvar.

## Success Criteria

- S-1000 + S-1005 + S-1010 transmitidos para Restrita com sucesso em smoke.
- Edição de Lotação gera S-1005 atualização automaticamente.
- `openspec validate esocial-tabelas --strict` válido.
