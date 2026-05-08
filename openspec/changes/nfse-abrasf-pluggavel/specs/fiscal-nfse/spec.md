## ADDED Requirements

### Requirement: Emissão de NFS-e via Adapter Municipal
O sistema SHALL emitir NFS-e (Nota Fiscal de Serviço Eletrônica) através de adapters específicos por município, começando com suporte ao padrão ABRASF v2.04.

#### Scenario: Emissão em município ABRASF homologação
- **WHEN** um faturamento de serviço é emitido para um tomador em município que usa ABRASF v2.04 (ex: Vitória-ES)
- **THEN** o sistema resolve o adapter correto via `NFSeMunicipalClientFactory.Resolve(codigoIbge)`
- **THEN** monta `EnviarLoteRpsEnvio` no padrão ABRASF v2.04
- **THEN** assina digitalmente o lote com cert ICP-Brasil do prestador
- **THEN** transmite via SOAP ao endpoint homologação do município
- **THEN** parseia retorno e armazena `NumeroNfse` + `CodigoVerificacao`

#### Scenario: Município sem adapter suportado
- **WHEN** o sistema tenta emitir para município cujo `padrao_municipal` não tem adapter implementado
- **THEN** retorna erro estruturado com nome do município + padrão requerido
- **THEN** orienta admin a aguardar release ou usar emissão manual

### Requirement: Configuração NFS-e por Tenant e Município
O sistema SHALL permitir que cada tenant configure NFS-e para múltiplos municípios independentemente, com credenciais separadas (usuário, senha, token, certificado, ambiente).

#### Scenario: Tenant com filiais em municípios diferentes
- **WHEN** um tenant tem operação em SP, RJ e Vitória
- **THEN** admin cadastra 3 entradas em `configuracao_fiscal_nfse`
- **THEN** cada entrada tem padrão e credenciais próprios
- **THEN** emissões de cada filial são roteadas para o adapter/municipio correto

#### Scenario: Senha criptografada
- **WHEN** admin cadastra senha do município
- **THEN** o sistema criptografa via IDataProtector com chave em vault
- **THEN** senha nunca trafega em log nem aparece no front

### Requirement: Cancelamento de NFS-e ABRASF
O sistema SHALL cancelar NFS-e ABRASF v2.04 dentro do prazo configurado para o município, com justificativa.

#### Scenario: Cancelamento dentro do prazo
- **WHEN** admin solicita cancelamento de NFS-e em município com prazo de 24h e a NFS-e foi emitida há 5h
- **THEN** o sistema monta `CancelarNfseEnvio` ABRASF
- **THEN** assina + transmite + parseia
- **THEN** ao receber sucesso, atualiza status para `Cancelada` e armazena protocolo
- **THEN** estoque (se aplicável) e financeiro são revertidos

#### Scenario: Tentativa fora do prazo
- **WHEN** admin tenta cancelar NFS-e fora do prazo do município
- **THEN** o sistema bloqueia com mensagem orientando substituição (se padrão suporta) ou contato com prefeitura

### Requirement: Catálogo IBGE → Padrão Municipal
O sistema SHALL manter catálogo mapeando código IBGE de município para padrão NFS-e (`AbrasfV204`, `SaoPauloSF`, `NotaCarioca`, `Ginfes`, `Ipm`, `Betha`, ...), preenchido via seed para top 50 municípios.

#### Scenario: Lookup automático na configuração
- **WHEN** admin cadastra configuração para um município
- **THEN** sistema sugere o padrão correto a partir do catálogo
- **THEN** admin pode override se sabe que o município mudou

### Requirement: Códigos de Serviço LC 116/03
O sistema SHALL manter lista nacional de códigos de serviço LC 116/03 (123 códigos) preenchida via seed e permitir extensão municipal.

#### Scenario: Validação de código na emissão
- **WHEN** uma NFS-e é emitida com `codigo_servico=1.04` (Elaboração de programas de computador)
- **THEN** o sistema valida que o código existe na lista nacional
- **THEN** transmite com `ItemListaServico=1.04` no XML ABRASF

#### Scenario: Código municipal específico
- **WHEN** município exige `CodigoTributacaoMunicipio` próprio (subdivisão do LC 116)
- **THEN** o sistema permite admin cadastrar e relacionar ao código nacional
- **THEN** emissão envia ambos (LC 116 + municipal)

### Requirement: Storage XML NFS-e
O sistema SHALL armazenar XMLs autorizados de NFS-e em storage externo S3/MinIO no path `nfse/{tenant_id}/{ano}/{mes}/{codigo_ibge}/{numero}.xml` por mínimo de 5 anos.

#### Scenario: Armazenamento pós-autorização
- **WHEN** uma NFS-e é autorizada pela prefeitura
- **THEN** o XML retornado é enviado ao storage externo
- **THEN** o link é armazenado em `nfse.xml_url`
- **THEN** consulta posterior streama do storage sem reconsultar prefeitura

### Requirement: DANFSe (PDF)
O sistema SHALL gerar DANFSe em PDF para NFS-e autorizadas, com layout genérico legível (não há padrão único entre municípios).

#### Scenario: Download do DANFSe
- **WHEN** usuário acessa `/api/v1/nfse/{id}/pdf`
- **THEN** o sistema gera (ou recupera de cache) PDF com cabeçalho prestador, dados tomador, descrição serviço, valores, código verificação, QR Code de consulta na prefeitura
- **THEN** PDF tem tamanho < 200KB

### Requirement: Endpoint REST de NFS-e
O sistema SHALL expor endpoints REST para emissão, cancelamento, consulta, listagem e download de NFS-e, seguindo padrão Acme blueprint (4-arquivos por endpoint).

#### Scenario: Listagem com filtros
- **WHEN** GET `/api/v1/nfse?status=Autorizada&prestador_id=X&periodo_inicio=2026-01-01`
- **THEN** retorna paginação server-side ordenada por data emissão DESC
- **THEN** RequirePermissao(Recursos.NFSe, Acoes.Listar) aplicado

### Requirement: Integração Faturamento → NFS-e
O sistema SHALL detectar automaticamente quando um faturamento é de serviço (vs. mercadoria) e disparar emissão de NFS-e em vez de NF-e.

#### Scenario: Faturamento de serviço dispara NFS-e
- **WHEN** um pedido de venda contém apenas itens de tipo `Servico`
- **THEN** o `Faturamento` correspondente dispara `EmitirNFSeCommand` (não `EmitirNFeCommand`)
- **THEN** município e padrão são resolvidos a partir do tomador (cliente)

#### Scenario: Faturamento misto
- **WHEN** pedido tem produtos E serviços
- **THEN** sistema emite NF-e para os produtos e NFS-e para os serviços (vinculadas ao mesmo faturamento)
