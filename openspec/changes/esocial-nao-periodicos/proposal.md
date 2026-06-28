## Why

W13. **Eventos não-periódicos** do eSocial — disparados por **acontecimentos** na vida do funcionário (admissão, alteração, afastamento, desligamento, acidente). Diferente de tabela (estática) e periódico (mensal).

## What Changes

### Eventos cobertos

| Código | Nome | Trigger Atena |
|--------|------|---------------|
| **S-2200** | Admissão / Início de TSVE | Criação de `Funcionario` ativo (W1) |
| **S-2205** | Alteração de Dados Cadastrais | Edição de dados pessoais do `Funcionario` |
| **S-2206** | Alteração de Contrato de Trabalho | Mudança de cargo/salário (W1) |
| **S-2230** | Afastamento | `Afastamento` registrado (W8) |
| **S-2250** | Aviso Prévio | `Rescisao.Programada` (W9) |
| **S-2298** | Reintegração | Reversão de desligamento (raro) |
| **S-2299** | Desligamento | `Rescisao.Concluida` (W9) |
| **S-2300** | TSVE Início | Trabalhador sem vínculo (estagiário, autônomo) |
| **S-2306** | TSVE Alteração | |
| **S-2399** | TSVE Término | |

### Para cada evento: POCO + Builder + Validator + Hook automático

Mesmo padrão de W12.

### Endpoints

```
POST /api/v1/esocial/nao-periodicos/{tipo}/gerar?referenciaId=
GET  /api/v1/esocial/nao-periodicos?tipo=&status=&funcionarioId=
POST /api/v1/esocial/nao-periodicos/{eventoId}/retificar
POST /api/v1/esocial/nao-periodicos/{eventoId}/excluir
```

### Triggers do Atena → eSocial

```
Funcionario.criado (status=Ativo)               → S-2200
Funcionario.editado dados pessoais              → S-2205
Funcionario.cargo/salario alterado              → S-2206
Afastamento.criado                               → S-2230
Rescisao.Programada (com aviso indenizado)      → S-2250
Rescisao.Concluida                               → S-2299
```

Cada hook chama `GerarEventoSxxxxCommand` que prepara, assina e enfileira.

### Ordem obrigatória

S-2200 (admissão) tem que estar Aceito antes de qualquer outro evento do mesmo trabalhador (alteração, afastamento, desligamento).

## Capabilities

### New Capabilities
- `esocial-nao-periodicos` — Eventos disparados por acontecimentos: admissão, alterações, afastamento, aviso, desligamento, TSVE.

### Modified Capabilities
- `rh-cadastros` — hook em Funcionario.Save dispara S-2200/2205/2206.
- `rh-eventos-mes` — hook em Afastamento.Save dispara S-2230.
- `rh-rescisao` — hook em Rescisao.Programada/Concluida dispara S-2250/S-2299.

## Out of Scope
- Eventos periódicos S-1200/S-1210/S-1299 (W14).
- Importação histórica de admissões antigas (separado se cliente exige).
- S-2210 CAT (acidente trabalho — pode entrar aqui ou em W próxima dependendo demanda).

## Risks

- **R1**: S-2200 grande e complexo (dezenas de campos obrigatórios). Mitigação: builder testado contra fixtures.
- **R2**: Ordem strict (admissão antes de tudo) — esquece e quebra cadeia. Mitigação: orquestrador valida.
- **R3**: Retificação de S-2200 é caso especial (não pode mudar campo `dtAdm`). Mitigação: validador antes de retificar.

## Success Criteria

- S-2200 + S-2299 ciclo completo em Restrita.
- Editar funcionário gera S-2205/2206 conforme campo.
- `openspec validate esocial-nao-periodicos --strict` válido.
