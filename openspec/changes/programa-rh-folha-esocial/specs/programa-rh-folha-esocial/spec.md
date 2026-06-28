## ADDED Requirements

### Requirement: Programa RH-Folha-eSocial decomposto em 15 ondas

O sistema SHALL organizar a construção do módulo de RH+Folha+eSocial+Ponto Oficial em 15 changes OpenSpec independentes (W1-W15), executados na ordem do grafo de dependências definido em `design.md`, com gates de aprovação a cada 3 ondas.

#### Scenario: Onda só inicia quando suas dependências foram arquivadas

- **GIVEN** a onda W6 (rh-folha-engine) depende de W5 (rh-tabelas-legais)
- **WHEN** o time tenta iniciar `openspec apply rh-folha-engine`
- **THEN** o skill verifica que `rh-tabelas-legais` está em `openspec/changes/archive/`
- **AND** se não estiver, o skill bloqueia a apertura da onda e instrui a concluir W5 primeiro

#### Scenario: Gate de bloco exige demo executável

- **WHEN** as 3 ondas do Bloco A (W1+W2+W3+W4) estão arquivadas
- **THEN** o programa exige uma demo fim-a-fim executável antes de iniciar W5
- **AND** o stakeholder registra aprovação documentada em `documentacao/rh/gates/bloco-A.md`

### Requirement: Decisões fundadoras Q1-Q6 são imutáveis sem proposta de mudança

O programa SHALL congelar as 6 decisões fundadoras (sequência das ondas, rubricas por tenant, CCT estruturada, mobile MAUI, biometria local + foto, tabelas via upload admin) no documento `proposal.md` deste change-mãe. Mudanças exigem um change OpenSpec separado.

#### Scenario: Pedido para trocar MAUI por outra tecnologia

- **GIVEN** Q4 = MAUI já está congelado em `proposal.md`
- **WHEN** stakeholder pede para trocar por Flutter durante execução de W3
- **THEN** o time NÃO altera silenciosamente o `proposal.md`
- **AND** abre um change `repensar-mobile-rh` com proposal próprio descrevendo motivo, impacto e custo

### Requirement: Reuso obrigatório de componentes do NFe

As ondas W4 (Ponto Oficial 671) e W11 (eSocial Fundação) SHALL reusar `XmlSignerC14N`, `CertificadoTenantResolver`, `SefazSoapClient` e `ContingenciaPolicy` do change `nfe-cliente-sefaz-proprio`, em vez de criar implementações paralelas.

#### Scenario: Implementação paralela de assinador XMLDSig é rejeitada em review

- **GIVEN** W4 precisa assinar comprovantes da Portaria 671
- **WHEN** PR cria classe nova `Portaria671XmlSigner` sem reusar `XmlSignerC14N`
- **THEN** code review reprova com referência a esta requirement
- **AND** PR é refatorado para reuso ou justifica em design.md por que reuso não cabe

### Requirement: Cada onda entrega valor isolado e tem ponto de saída segura

O programa SHALL ser estruturado de modo que parar após qualquer onda ainda deixe o sistema em estado funcional e útil, sem código pendurado ou half-baked.

#### Scenario: Parar após W3 entrega RH interno + mobile

- **WHEN** o programa é pausado após arquivamento de W3
- **THEN** o sistema oferece ponto interno (web + mobile) funcionando
- **AND** nenhuma feature parcial fica visível ao usuário final
- **AND** menu /rh apresenta apenas o que está pronto

#### Scenario: Folha calculada parcialmente nunca é exposta

- **GIVEN** W6 está em desenvolvimento mas não arquivada
- **WHEN** usuário tenta acessar tela de folha
- **THEN** sistema retorna 404 ou redireciona para "feature em construção"
- **AND** nenhum cálculo parcial é mostrado
