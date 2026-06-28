## Why

W2 do programa `programa-rh-folha-esocial`. Com cadastros prontos (W1), construímos o **ponto interno**: registro de batidas, banco de horas, ajustes com workflow de aprovação e espelho mensal. **Ainda sem conformidade legal Portaria 671** (isso é W4) — esta onda entrega ponto **gerencial**, suficiente para empresas que não são fiscalizadas e para a base de cálculo de folha em W6.

## What Changes

### Backend — entidades em `Domain/Entities/Rh/`

- `MarcacaoPonto`
  - id, tenant_id, funcionario_id
  - tipo (`Entrada`, `SaidaAlmoco`, `VoltaAlmoco`, `Saida`, `Pausa`, `RetornoPausa`)
  - data_hora (datetime UTC)
  - origem (`Web`, `MobileApp`, `Kiosk`, `Manual`, `Importacao`)
  - lat, lng (geolocalização — registrada, não restringe)
  - ip_origem, user_agent, device_id
  - foto_url (S3, opcional; obrigatória em mobile sem biometria — vem em W3)
  - hash_integridade (SHA-256 da linha + da batida anterior — cadeia para detectar adulteração)
  - status (`Valida`, `AjusteSolicitado`, `Ajustada`, `Invalida`)
- `AjustePonto`
  - id, marcacao_original_id (FK), funcionario_id solicitante
  - tipo_ajuste (`AlteracaoHora`, `Inclusao`, `Exclusao`, `Justificativa`)
  - data_hora_proposta, motivo, anexo_url
  - status (`Pendente`, `Aprovado`, `Rejeitado`)
  - aprovador_id, decisao_em, justificativa_decisao
- `BancoHorasSaldo`
  - tenant_id, funcionario_id, competencia (YYYY-MM)
  - horas_devidas (decimal), horas_realizadas (decimal)
  - saldo_minutos (positivo = a favor do funcionário; negativo = devedor)
  - politica_compensacao (FK opcional para regra)
- `BancoHorasPolitica`
  - tenant_id, nome, vigencia
  - limite_horas_acumular, prazo_compensacao_dias
  - permite_pagar_excedente (BOOL)
  - fator_pagamento (1.0 = hora normal, 1.5 = 50%, etc.)
- `MovimentoBancoHoras`
  - tenant_id, funcionario_id, data
  - origem (`Acumulo`, `Compensacao`, `Pagamento`, `Ajuste`, `Expiracao`)
  - minutos (positivo ou negativo)
  - referencia_marcacao_id (opcional)
- `FechamentoPonto`
  - tenant_id, funcionario_id, competencia (YYYY-MM)
  - status (`Aberto`, `EmConferencia`, `Fechado`)
  - fechado_em, fechado_por, observacoes

### API — `/api/v1/rh/ponto/*` e `/api/v1/rh/banco-horas/*`

- `POST /ponto/bater` (qualquer funcionário autenticado pode bater o **próprio** ponto)
- `GET /ponto/proprio?dataInicio=&dataFim=` (próprias batidas)
- `GET /ponto/equipe?gestorId=&dataInicio=&dataFim=` (gestor vê equipe — permissão `gerir-equipe`)
- `GET /ponto/espelho?funcionarioId=&competencia=YYYY-MM` (espelho mensal, retorna JSON estruturado)
- `GET /ponto/espelho.pdf?funcionarioId=&competencia=` (PDF assinado por servidor)
- `POST /ponto/{marcacaoId}/ajuste` (solicita ajuste)
- `GET /ponto/ajustes/pendentes` (lista ajustes para aprovar)
- `POST /ponto/ajustes/{id}/aprovar`
- `POST /ponto/ajustes/{id}/rejeitar`
- `POST /ponto/manual` (RH inclui batida manualmente — sempre auditado)
- `POST /ponto/competencia/{ano-mes}/fechar`
- `POST /ponto/competencia/{ano-mes}/reabrir` (admin RH)
- `GET /banco-horas/saldo?funcionarioId=&em=YYYY-MM`
- `GET /banco-horas/movimentos?funcionarioId=&competencia=`
- `POST /banco-horas/politicas` (CRUD)
- `POST /banco-horas/{funcionarioId}/compensar { data, minutos, motivo }`
- `POST /banco-horas/{funcionarioId}/pagar { competencia, minutos }` (gera linha em folha em W6)

### Permissions novas

- `Recursos.RhPonto`, `Recursos.RhBancoHoras`, `Recursos.RhPoliticasPonto`
- `Acoes.BaterPonto`, `Acoes.AjustarPonto`, `Acoes.AprovarPonto`, `Acoes.FecharCompetencia`, `Acoes.ReabrirCompetencia`

Funcionários comuns recebem `rh-ponto:bater-ponto` + `rh-ponto:listar` próprio automaticamente quando funcionário é criado.

### Engine de cálculo de horas (`src/Service/Acme.Sistemas.Services/V1/Rh/Ponto/Engine/`)

- `CalculadoraJornadaDiaria` — dado uma lista de batidas + jornada vigente do dia → produz: horas trabalhadas, atrasos, faltas parciais, horas extras (sem aplicar adicional, só identifica), pausas legais.
- `CalculadoraSaldoBancoHoras` — dado calendário do mês × marcações × jornada × política → produz movimentos do banco.
- `GeradorEspelhoMensal` — produz estrutura JSON do espelho (linhas dia a dia).
- `GeradorEspelhoPdf` — usa QuestPDF (lib MIT) para PDF assinado.

### Hash de integridade

Cada `MarcacaoPonto` armazena `hash_integridade = SHA-256(funcionarioId|dataHora|tipo|origem|hashAnterior)`. Detecta adulteração de banco. Não é assinatura ICP-Brasil (isso é W4) — é hash chain interno.

### Workflow de aprovação

```
Funcionário solicita ajuste ──► AjustePonto.Pendente
                                         │
              ┌──────────────────────────┴──────────────────────────┐
              ▼                                                     ▼
   Gestor (rh-ponto:aprovar) aprova                   Gestor rejeita
              │                                                     │
              ▼                                                     ▼
   AjustePonto.Aprovado                                AjustePonto.Rejeitado
   MarcacaoPonto.Ajustada                              (sem mudança em MarcacaoPonto)
   Nova versão de MarcacaoPonto gravada                Funcionário notificado
   (audit chain mantida)
```

### Frontend — área `/rh/ponto/*`

- **Meu ponto** — visualização semanal + botão "Bater agora" (web)
- **Espelho** — calendário do mês com horas/saldo
- **Solicitar ajuste** — modal/wizard
- **Aprovações pendentes** (gestor/RH) — lista + ação inline
- **Banco de horas** — saldo, movimentos, política aplicada
- **Fechamento de competência** (RH) — wizard que rola conferência → fecha → notifica funcionários

## Capabilities

### New Capabilities
- `rh-ponto-interno` — registro, ajuste, espelho e banco de horas (gerencial).
- `rh-banco-horas` — políticas e movimentos.

### Modified Capabilities
- `rh-cadastros` — `Funcionario` ganha computed `ponto_status_atual` em `ObterFichaCompleta`.
- `seed-tenant-administrativo` — semeia política de banco de horas default "Sem banco" (saldo não acumula, pago como HE).

## Out of Scope
- Conformidade Portaria 671 (W4).
- Bater ponto no app mobile (W3 implementa o endpoint cliente; o endpoint do servidor é o mesmo).
- Adicional noturno / HE com adicional %  — engine de folha em W6 (aqui só registramos minutos).
- CCT diferenciada por categoria (W7).
- Relatórios analíticos avançados (W15).
- Bater ponto por terceiros/RPA além do `POST /ponto/manual` do RH.

## Risks

- **R1**: Adulteração do banco — sem ICP-Brasil ainda, hash chain é mitigação parcial. Documentar que ponto interno **não substitui** o oficial em empresas fiscalizadas.
- **R2**: Mudança de jornada vigente no meio do mês exige recálculo histórico. Mitigação: engine recalcula dia a dia usando jornada vigente naquele dia.
- **R3**: Banco de horas com regras complexas (HE só conta acima de 8h/dia? acima de 44h/semana? ou 220h/mês?). Mitigação: política configurável; default seguro (acima de 44h/semana).
- **R4**: PDF do espelho pesado para tenant com 1000 funcionários no fechamento — mitigação: geração assíncrona via RabbitMQ + notificação quando pronto.

## Success Criteria

- Funcionário bate ponto (web), vê batidas do dia em < 1s.
- Espelho mensal de 30 dias gerado em < 2s.
- PDF do espelho gerado e baixado em < 5s para 1 funcionário; < 30s assíncrono para 100.
- Banco de horas calcula 100% correto em 20 cenários fixture (com/sem CCT-like, com HE, com noturno-naive, com escala 12x36).
- Workflow de aprovação funciona ponta-a-ponta com notificação de e-mail.
- `openspec validate rh-ponto-interno --strict` válido.
