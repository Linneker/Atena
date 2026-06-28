## ADDED Requirements

### Requirement: Eventos de tabela transmitidos automaticamente

O sistema SHALL gerar eventos eSocial de tabela (S-1000, S-1005, S-1010, S-1020, S-1070, S-1280) automaticamente quando entidades-fonte do Atena são salvas/alteradas, com indicador de retificação correto (original na primeira transmissão, retificação nas posteriores).

#### Scenario: Criar empregador gera S-1000 original

- **GIVEN** `EmpregadorEsocial` recém-configurado (primeira vez)
- **WHEN** salva
- **THEN** sistema cria `EventoEsocial` tipo S-1000 com `indRetif=1`
- **AND** evento entra em estado Assinado → worker W11 envia

#### Scenario: Alterar lotação gera S-1005 retificação

- **GIVEN** Lotação X já tem S-1005 Aceito
- **WHEN** RH altera endereço de X
- **THEN** sistema cria S-1005 com `indRetif=2` apontando para evento anterior

### Requirement: Orquestração de ordem de transmissão

S-1000 SHALL ser Aceito antes de qualquer outro evento de tabela do mesmo empregador ser enviado.

#### Scenario: S-1005 espera S-1000

- **GIVEN** S-1000 em estado Enviado (não Aceito ainda)
- **WHEN** RH tenta criar S-1005
- **THEN** evento fica em estado Aguardando (não vira Assinado)
- **AND** transita para Assinado quando S-1000 confirma Aceito

### Requirement: Mapeamento de rubrica tenant → S-1010

Cada `RubricaTenant` (W5) SHALL ter `natureza_esocial_codigo` mapeado para campo `codRubr` no S-1010. Sem mapeamento, salvar rubrica nova falha com erro "Natureza eSocial obrigatória".

#### Scenario: Rubrica sem natureza eSocial é rejeitada

- **GIVEN** tenant tenta criar `RubricaTenant { codigo: "BONUS", naturezaEsocialCodigo: null }`
- **WHEN** sistema processa
- **THEN** retorna 400 com mensagem `Natureza eSocial é obrigatória para rubricas`
