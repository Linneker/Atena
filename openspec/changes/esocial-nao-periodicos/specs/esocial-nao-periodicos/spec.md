## ADDED Requirements

### Requirement: Eventos não-periódicos disparados por acontecimentos

O sistema SHALL gerar automaticamente eventos eSocial S-2200, S-2205, S-2206, S-2230, S-2250, S-2299 (e TSVE 2300/2306/2399) ao detectar mudanças nas entidades correspondentes do Atena (Funcionario, Afastamento, Rescisao).

#### Scenario: Admitir funcionário gera S-2200

- **WHEN** RH cria `Funcionario { status: Ativo, ... }`
- **THEN** sistema gera `EventoEsocial { tipo: "S-2200", indRetif: 1 }`
- **AND** evento entra em pipeline de envio W11

#### Scenario: Alterar cargo gera S-2206

- **GIVEN** funcionário com S-2200 Aceito
- **WHEN** RH muda cargo via `PUT /rh/funcionarios/{id}` ou registra reajuste salarial
- **THEN** sistema gera S-2206 com `indRetif=1` (original — primeira vez para esse fato)

#### Scenario: Concluir rescisão gera S-2299

- **GIVEN** rescisão Homologada
- **WHEN** RH chama `/rescisoes/{id}/concluir`
- **THEN** sistema gera S-2299 com motivo eSocial mapeado de `TipoRescisao`

### Requirement: Ordem obrigatória S-2200 → demais

S-2205, S-2206, S-2230, S-2250, S-2299 SHALL exigir S-2200 do mesmo trabalhador em estado Aceito. Tentativa de gerá-los antes resulta em evento em estado Aguardando.

#### Scenario: S-2205 espera S-2200

- **GIVEN** funcionário recém-criado com S-2200 ainda Enviado (não Aceito)
- **WHEN** RH muda dados cadastrais
- **THEN** S-2205 é gerado mas fica em estado Aguardando
- **AND** transita para Assinado/Enviado quando S-2200 confirma Aceito

### Requirement: Mapeamento de motivo de rescisão

`TipoRescisao` do W9 SHALL ter mapeamento determinístico para código de motivo eSocial S-2299 (tabela 19 do leiaute).

#### Scenario: Sem Justa Causa → código 02

- **GIVEN** rescisão tipo `SemJustaCausaEmpresa`
- **WHEN** sistema gera S-2299
- **THEN** campo `mtvDeslig=02`
