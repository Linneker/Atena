# Tasks — nfse-abrasf-pluggavel

> Granularidade fina (~1-3h por task). 6 fases: domínio, schema/XML, adapter ABRASF, factory, integração, frontend.

---

## Fase 1 — Domínio NFS-e

### 1.1 Entidades

- [ ] 1.1.1 Criar `src/Service/Acme.Sistemas.Domain/Entities/Fiscal/NFSe.cs`: numero, codigoVerificacao, prestadorId, tomadorId, codigoServico, descricaoServico, valorServico, valorIss, aliquotaIss, status, dataEmissao, codigoIbgeMunicipio, padraoMunicipal, xmlAutorizado, dadosExtras (JSON)
- [ ] 1.1.2 Criar `NFSeItem.cs` (caso emissão multi-item)
- [ ] 1.1.3 Criar `NFSeEvento.cs` (cancelamento, substituição)
- [ ] 1.1.4 Enum `NFSeStatus` (Rascunho, EmTransmissao, Autorizada, Cancelada, Substituida, Rejeitada)
- [ ] 1.1.5 Enum `PadraoMunicipal` (AbrasfV204, SaoPauloSF, NotaCarioca, Ginfes, Ipm, Betha, ...)

### 1.2 Repository

- [ ] 1.2.1 `INFSeRepository` com CRUD + filtros (status, prestador, período, município)
- [ ] 1.2.2 Implementação SQL puro em `Acme.Sistemas.Repository/V1/Fiscal/NFSeRepository.cs`
- [ ] 1.2.3 Migration: tabela `nfse` com tenant_id, índices por (tenant_id, status), (tenant_id, prestador_id, data_emissao)
- [ ] 1.2.4 Migration: tabela `nfse_eventos` (id, nfse_id, tipo, dados, data, protocolo)
- [ ] 1.2.5 Test unitário do repo

### 1.3 Configuração fiscal NFS-e

- [ ] 1.3.1 Migration: tabela `configuracao_fiscal_nfse` (tenant_id, codigo_ibge_municipio, padrao_municipal, usuario_criptografado, senha_criptografada, token_criptografado, cert_id, ambiente)
- [ ] 1.3.2 Repository + endpoints CRUD (admin only)
- [ ] 1.3.3 Test: tenant pode ter configurações para múltiplos municípios

---

## Fase 2 — Schemas e XML ABRASF v2.04

### 2.1 Schemas embutidos

- [ ] 2.1.1 Baixar XSDs oficiais ABRASF v2.04 do site abrasf.org.br
- [ ] 2.1.2 Salvar em `src/Data/Acme.Sistemas.ExternalIntegration/NFSe/Schemas/AbrasfV204/`
- [ ] 2.1.3 Configurar como `EmbeddedResource`
- [ ] 2.1.4 Validador XSD reutilizável (compartilhar com NF-e se possível)

### 2.2 Modelos serializáveis ABRASF

- [ ] 2.2.1 Modelar `EnviarLoteRpsEnvio` (root request)
- [ ] 2.2.2 Modelar `LoteRps`, `Rps`, `InfRps`, `IdentificacaoRps`
- [ ] 2.2.3 Modelar `Servico` (Valores, ItemListaServico, CodigoTributacaoMunicipio, Discriminacao)
- [ ] 2.2.4 Modelar `Prestador`, `Tomador` com IdentificacaoCpfCnpj e Endereco
- [ ] 2.2.5 Modelar response `EnviarLoteRpsResposta` com `ListaNfse > ComplNfse > Nfse > InfNfse`
- [ ] 2.2.6 Modelar `CancelarNfseEnvio` e response
- [ ] 2.2.7 Modelar `ConsultarNfseRpsEnvio` e response
- [ ] 2.2.8 Test golden: serializar sample → comparar com XML real ABRASF

### 2.3 Códigos de serviço LC 116

- [ ] 2.3.1 Migration: tabela `codigo_servico` (codigo_lc116, codigo_municipal, descricao, codigo_ibge_municipio)
- [ ] 2.3.2 Seed da LC 116 nacional (123 códigos) em migration
- [ ] 2.3.3 Endpoint admin para adicionar códigos municipais
- [ ] 2.3.4 Validação na emissão: código deve existir para o município

---

## Fase 3 — Adapter ABRASF v2.04

### 3.1 Interface comum

- [ ] 3.1.1 Definir `INFSeMunicipalClient` em `Domain/Interfaces/Fiscal/`: `EmitirAsync(NFSe)`, `CancelarAsync(numero, justificativa)`, `ConsultarAsync(numero)`, `ConsultarPorRpsAsync(numeroRps)`
- [ ] 3.1.2 DTO `NFSeResultado` (sucesso, codigo, motivo, numeroNfse, codigoVerificacao, xmlRetorno)

### 3.2 Implementação Abrasf

- [ ] 3.2.1 Criar `src/Data/Acme.Sistemas.ExternalIntegration/NFSe/Abrasf/AbrasfV204Client.cs : INFSeMunicipalClient`
- [ ] 3.2.2 `EmitirAsync`: monta `EnviarLoteRpsEnvio` + assina + envia SOAP + parseia retorno
- [ ] 3.2.3 Reutilizar `XmlSignerC14N` da change `nfe-cliente-sefaz-proprio`
- [ ] 3.2.4 SOAP client reutiliza `HttpClient` configurado com mTLS
- [ ] 3.2.5 `CancelarAsync`: monta `CancelarNfseEnvio` + assina + envia
- [ ] 3.2.6 `ConsultarAsync`, `ConsultarPorRpsAsync`
- [ ] 3.2.7 Tratamento de erros: parsing de `MensagemRetorno` com cStat e motivo

### 3.3 Catálogo de URLs ABRASF

- [ ] 3.3.1 Criar `NFSe/Urls/abrasf-municipios.json` com {codigoIbge, nomeMunicipio, ufd, urlHomologacao, urlProducao}
- [ ] 3.3.2 Preencher para top 20 municípios ABRASF (Vitória, Florianópolis, Porto Alegre, Curitiba, Goiânia, ...)
- [ ] 3.3.3 Carregador `AbrasfMunicipiosCatalog` que lê o JSON embutido

### 3.4 Tests do adapter

- [ ] 3.4.1 Unit test: serializer com sample ABRASF reais
- [ ] 3.4.2 Unit test: parser de retorno ABRASF (sucesso, erro de validação, erro de assinatura)
- [ ] 3.4.3 Integration test contra homolog Vitória-ES
- [ ] 3.4.4 Integration test contra homolog Florianópolis-SC
- [ ] 3.4.5 Integration test contra homolog Porto Alegre-RS

---

## Fase 4 — Factory e roteamento

### 4.1 Factory

- [ ] 4.1.1 Criar `INFSeMunicipalClientFactory.Resolve(codigoIbge)` → `INFSeMunicipalClient`
- [ ] 4.1.2 Implementação que consulta `padrao_municipal` na config do tenant + delega ao adapter
- [ ] 4.1.3 Erro claro se município não tem padrão suportado: "Município X (cod IBGE Y) usa padrão Z não suportado pelo Atena"
- [ ] 4.1.4 DI: registrar factory + adapters disponíveis

### 4.2 Catálogo IBGE → padrão

- [ ] 4.2.1 Migration: tabela `municipio_padrao_nfse` (codigo_ibge, padrao_municipal, configuracao_extra_json)
- [ ] 4.2.2 Seed para top 50 municípios mais populosos com padrão correto
- [ ] 4.2.3 Repository + endpoint admin GET (read-only, fonte de verdade do catálogo)

---

## Fase 5 — Service layer e endpoints

### 5.1 Commands

- [ ] 5.1.1 `EmitirNFSeCommand` + Handler + Behavior + Validation + Result
- [ ] 5.1.2 `CancelarNFSeCommand` + Handler + ...
- [ ] 5.1.3 `ConsultarNFSeQuery` + Handler + ...
- [ ] 5.1.4 Reutilizar pipeline transversal (Validation → Cache → Audit → Log → Behavior → Handler)

### 5.2 Endpoints REST

- [ ] 5.2.1 `POST /api/v1/nfse` — emitir
- [ ] 5.2.2 `POST /api/v1/nfse/{id}/cancelar` — cancelar
- [ ] 5.2.3 `GET /api/v1/nfse/{id}` — consultar
- [ ] 5.2.4 `GET /api/v1/nfse?status=...&periodo=...` — listar
- [ ] 5.2.5 `GET /api/v1/nfse/{id}/xml` — download XML
- [ ] 5.2.6 `GET /api/v1/nfse/{id}/pdf` — download PDF (DANFSe)
- [ ] 5.2.7 Cada endpoint segue padrão 4-arquivos (Endpoint, Request, Response, Map)
- [ ] 5.2.8 RequirePermissao com permissões novas (`Recursos.NFSe`, `Acoes.Emitir`/`Cancelar`/`Consultar`)

### 5.3 PDF DANFSe

- [ ] 5.3.1 Estender `QuestPdf*Renderer` com `QuestPdfDanfseRenderer`
- [ ] 5.3.2 Layout padrão DANFSe (não há padrão único, fazer leiaute genérico)
- [ ] 5.3.3 Test: PDF gerado para NFSe sample não falha + tamanho razoável (< 200KB)

### 5.4 Storage XML

- [ ] 5.4.1 Após autorização, salvar XML em S3/MinIO com path `nfse/{tenant}/{ano}/{mes}/{codigoIbge}/{numero}.xml`
- [ ] 5.4.2 Atualizar `NFSe.xml_url` no banco
- [ ] 5.4.3 Endpoint download streama do storage

### 5.5 Integração com Faturamento

- [ ] 5.5.1 Quando `Faturamento` é de serviço (não de produto), disparar `EmitirNFSeCommand` ao invés de `EmitirNFeCommand`
- [ ] 5.5.2 Detecção: tipo do item (`produto.tipo = Servico`) ou flag explícito no faturamento
- [ ] 5.5.3 Test E2E: fluxo serviço completo

---

## Fase 6 — Frontend (telas mínimas)

### 6.1 Configuração

- [ ] 6.1.1 Tela `configuracao/fiscal-nfse/list` — listar municípios configurados pelo tenant
- [ ] 6.1.2 Tela `configuracao/fiscal-nfse/edit` — formulário (município, padrão, credenciais, ambiente)
- [ ] 6.1.3 Validação: padrão deve estar disponível no catálogo

### 6.2 Operação

- [ ] 6.2.1 Tela `fiscal/nfse/list` — lista paginada com filtros
- [ ] 6.2.2 Tela `fiscal/nfse/detail` — detalhes + ações (cancelar, baixar XML, baixar PDF)
- [ ] 6.2.3 Tela `fiscal/nfse/cancel` — modal com justificativa

---

## Fase 7 — Validação final

- [ ] 7.1 `dotnet build` verde
- [ ] 7.2 `dotnet test` (unit + integration) verde
- [ ] 7.3 Emissão real em homologação confirmada em 3 municípios ABRASF
- [ ] 7.4 Cancelamento confirmado em 2 municípios
- [ ] 7.5 Test E2E `Fluxo_Faturamento_Servico_NFSe` passa
- [ ] 7.6 `openspec validate nfse-abrasf-pluggavel --strict` verde
- [ ] 7.7 Frontend usável (smoke test manual em 3 telas)
