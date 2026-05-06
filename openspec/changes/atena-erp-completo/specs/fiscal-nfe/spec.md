## ADDED Requirements

### Requirement: Configuração Fiscal por Tenant
O sistema SHALL armazenar e gerenciar as configurações fiscais de cada tenant — certificado digital, série, ambiente (homologação/produção), CFOP padrão e regime tributário.

#### Scenario: Upload do certificado digital A1
- **WHEN** o administrador do tenant faz upload do certificado PFX com senha
- **THEN** o sistema armazena o certificado criptografado associado ao tenant
- **THEN** o certificado fica disponível para assinatura de NF-e

#### Scenario: Troca de ambiente homologação para produção
- **WHEN** o administrador muda o ambiente de "Homologação" para "Produção"
- **THEN** todas as NF-e subsequentes são enviadas ao webservice de produção da SEFAZ
- **THEN** NF-e de homologação já emitidas não são afetadas

### Requirement: Emissão de NF-e Modelo 55
O sistema SHALL emitir NF-e modelo 55 com assinatura digital, validação de schema e transmissão ao webservice SEFAZ da UF do emitente.

#### Scenario: Emissão síncrona bem-sucedida
- **WHEN** o faturamento de uma venda dispara a emissão da NF-e
- **THEN** o sistema monta o XML conforme layout NF-e 4.0
- **THEN** assina digitalmente com o certificado do tenant
- **THEN** transmite ao webservice SEFAZ e recebe o protocolo de autorização
- **THEN** armazena o XML autorizado e o protocolo

#### Scenario: Emissão assíncrona em caso de contingência
- **WHEN** o webservice SEFAZ está indisponível
- **THEN** o sistema emite em modo de contingência SVRS (Sefaz Virtual de Regime Especial)
- **THEN** a NF-e é marcada como "Em Contingência" e retransmitida quando o serviço voltar

#### Scenario: Rejeição pela SEFAZ
- **WHEN** a SEFAZ rejeita a NF-e por erro de dados (ex: CFOP inválido)
- **THEN** o sistema armazena o código e mensagem de rejeição
- **THEN** a venda permanece faturada mas sem NF-e autorizada
- **THEN** o usuário é notificado para corrigir e reemitir

### Requirement: DANFE
O sistema SHALL gerar o DANFE (Documento Auxiliar da NF-e) em PDF para NF-e autorizadas.

#### Scenario: Geração do DANFE após autorização
- **WHEN** a NF-e é autorizada pela SEFAZ
- **THEN** o DANFE em PDF é gerado e armazenado
- **THEN** o usuário pode baixar e imprimir o DANFE

#### Scenario: Envio do DANFE por e-mail ao cliente
- **WHEN** a NF-e é autorizada
- **THEN** o sistema envia automaticamente o DANFE por e-mail ao endereço do cliente (se configurado)

### Requirement: Cancelamento de NF-e
O sistema SHALL cancelar NF-e autorizadas dentro do prazo legal (24h) com transmissão do evento de cancelamento à SEFAZ.

#### Scenario: Cancelamento dentro do prazo
- **WHEN** o usuário solicita cancelamento de uma NF-e com justificativa (mínimo 15 caracteres)
- **THEN** o sistema transmite o evento de cancelamento à SEFAZ
- **THEN** ao obter o protocolo de cancelamento, a NF-e é marcada como "Cancelada"
- **THEN** o estoque e a conta a receber são revertidos automaticamente

#### Scenario: Tentativa de cancelamento fora do prazo
- **WHEN** o usuário tenta cancelar uma NF-e emitida há mais de 24h
- **THEN** o sistema bloqueia com mensagem orientando a usar Carta de Correção ou NF-e de Devolução

### Requirement: Carta de Correção Eletrônica (CC-e)
O sistema SHALL emitir CC-e para corrigir informações não essenciais de NF-e já autorizadas.

#### Scenario: Emissão de CC-e
- **WHEN** o usuário registra uma correção com texto descrevendo a mudança (mínimo 15 caracteres)
- **THEN** o sistema transmite o evento de CC-e à SEFAZ
- **THEN** ao receber o protocolo, a CC-e é vinculada à NF-e e disponível para download em PDF

### Requirement: Armazenamento de XML
O sistema SHALL armazenar os XMLs de NF-e autorizadas por no mínimo 5 anos (prazo legal) em storage externo (S3/Azure Blob).

#### Scenario: Armazenamento pós-autorização
- **WHEN** uma NF-e é autorizada
- **THEN** o XML assinado com protocolo é enviado ao storage externo com path `{tenant_id}/{ano}/{mes}/{chave_nfe}.xml`
- **THEN** o link de acesso é armazenado no banco para consulta futura
