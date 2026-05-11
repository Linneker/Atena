# Tasks — nfe-cliente-sefaz-proprio

> Granularidade fina (~1-3h por task). 5 fases mapeando as 5 camadas da arquitetura + fase final de substituição do stub.

---

## Fase 1 — Modelos NF-e v4.00 e XML

### 1.1 Estrutura inicial do projeto

- [x] 1.1.1 Criar pasta `src/Service/Acme.Sistemas.Domain/Entities/Fiscal/Xml/` para POCOs serializáveis
- [x] 1.1.2 Adicionar pasta `src/Data/Acme.Sistemas.ExternalIntegration/Sefaz/` para o cliente real
- [ ] 1.1.3 ⚠ BLOQUEADO — Baixar XSDs oficiais NFe v4.00 do site da Receita Federal (não-versionados; ver `Sefaz/Schemas/v4.00/README.md` com lista esperada e instruções)
- [x] 1.1.4 Configurar `Sefaz\Schemas\v4.00\*.xsd` como `EmbeddedResource` no `.csproj` (glob — entra automaticamente quando os arquivos forem adicionados); adicionada `ProjectReference` Domain

### 1.2 Modelos do XML NF-e

- [x] 1.2.1 Modelar root `NFeProc` (procNFe) e `NFe` com namespaces corretos (`NFeProc.cs`, `NFe.cs`, `NFeNamespaces.cs`)
- [x] 1.2.2 Modelar `infNFe` com 11 grupos (`InfNFe.cs` com Exporta, Compra, InfRespTec stubs)
- [x] 1.2.3 Modelar `ide` completo (`Ide.cs`)
- [x] 1.2.4 Modelar `emit`, `dest` e `Endereco` compartilhado (`EmitDest.cs`)
- [x] 1.2.5 Modelar `det[]` e `prod` (`Det.cs`)
- [x] 1.2.6 Modelar `imposto` com ICMS00/10/20/30/40/51/60/70/90 + ICMSSN101/102/201 + IPI + PIS + COFINS + ISSQN (`Imposto.cs`); ICMSSN202/500/900 e ICMS41/50/61/ST etc. ficam pendentes para a Fase 4 (mecanicamente análogos aos modelados)
- [x] 1.2.7 Modelar `total` (`Total.cs` com ICMSTot, ISSQNtot, RetTrib)
- [x] 1.2.8 Modelar `transp` (`Transp.cs` com modFrete, Transporta, Vol[])
- [x] 1.2.9 Modelar `cobr` (`CobrPagInfAdic.cs` com Fat, Dup[])
- [x] 1.2.10 Modelar `pag` e `infAdic` (mesmo arquivo `CobrPagInfAdic.cs`)

### 1.3 Geração da chave de acesso

- [x] 1.3.1 Implementar `ChaveAcessoBuilder.Build` (`src/Service/Acme.Sistemas.Domain/Entities/Fiscal/Xml/ChaveAcessoBuilder.cs`)
- [x] 1.3.2 Implementar `CalcularDV` mod 11 com pesos cíclicos 2..9 e tratamento de resto 0/1 → DV=0
- [x] 1.3.3 Tests unitários (`ChaveAcessoBuilderTests`): 7 fatos cobrindo Build, CalcularDV (resto 0 e 9), zero-padding, determinismo, validação de input, auto-consistência em 50 chaves aleatórias. NOTA: "5 chaves reais" da SEFAZ não obtidas (precisa de NFe samples reais); cobertura de algoritmo está validada por construção (auto-consistência) + 2 casos de borda manualmente computáveis
- [x] 1.3.4 Auto-consistência `chave[43] == CalcularDV(chave[..43])` validada no test `Build_CDvDaChave_BateComCalcularDV` (50 chaves aleatórias)

### 1.4 Validação XSD local

- [x] 1.4.1 Implementar `XsdValidator` carregando schemas via `EmbeddedResource` (`src/Data/Acme.Sistemas.ExternalIntegration/Sefaz/XsdValidator.cs`) — `XmlSchemaSet` com import resolvido por compose
- [x] 1.4.2 Coletar erros estruturados via `XmlSeverityType + linha + coluna + mensagem` em `XsdError`
- [ ] 1.4.3 ⚠ BLOQUEADO PARCIAL — Test "XML válido passa, inválido falha" depende de XSDs reais (1.1.3); por enquanto testes cobrem o caminho "sem XSDs": `Validar` lança `InvalidOperationException` com mensagem clara apontando para o README
- [x] 1.4.4 Cache via `Lazy<XmlSchemaSet?>` (carga única) e `XmlSchemaSet.Compile` reuso entre validações; bench efetivo só quando XSDs estiverem em disco

### 1.5 Serialização

- [x] 1.5.1 `NFeXmlSerializer` em `Domain/Entities/Fiscal/Xml/` configurado com namespace padrão sem prefixo + UTF-8 sem BOM (SEFAZ rejeita BOM no body SOAP)
- [ ] 1.5.2 ⚠ BLOQUEADO PARCIAL — Golden file vs XML real não viável sem amostras SEFAZ; teste atual valida invariantes (encoding, namespace, ordem de elementos, ausência de BOM) com NFe sample sintética
- [x] 1.5.3 Round-trip serialize → deserialize → re-serialize → equivalência byte-a-byte validado em `RoundTrip_DeserializeESerializeNovamente_GeraXmlEquivalente`

---

## Fase 2 — Assinatura digital ICP-Brasil

### 2.1 Loader de certificado A1

- [x] 2.1.1 `ICertificadoLoader` em `Domain/Interfaces/Fiscal/` com `LoadAsync` + `CertificadoInvalidoException`
- [x] 2.1.2 `A1CertificadoLoader` em `ExternalIntegration/Sefaz/Certificado/` carrega via `X509CertificateLoader.LoadPkcs12` (não-obsoleto)
- [x] 2.1.3 Validação de cadeia ICP-Brasil via `X509Chain.Build` (toggle `validarCadeiaIcpBrasil` permite tests com cert auto-assinado)
- [x] 2.1.4 Validação de NotAfter (vencimento) + KeyUsage com `DigitalSignature`
- [x] 2.1.5 Tests unitários com cert auto-assinado gerado em runtime (`CertificateRequest`): 5 fatos (load OK, senha errada, vencido, sem DigitalSignature, bytes vazios). Tests com cert ICP-Brasil real ficam para pipeline manual quando o cert estiver disponível

### 2.2 Carregamento por tenant

- [x] 2.2.1 `ConfiguracaoFiscal` já expõe `CertificadoPfxCriptografado` (byte[]), `CertificadoSenhaCriptografada` (Base64) e `CertificadoSenhaNonceBase64`; `IConfiguracaoFiscalRepository.GetAsync` retorna tudo. Sem mudança necessária
- [x] 2.2.2 `CertificadoTenantResolver` em `ExternalIntegration/Sefaz/Certificado/` descriptografa via `TenantSecretCipher` (AES-GCM existente) + delega ao `ICertificadoLoader`
- [x] 2.2.3 Cache `ConcurrentDictionary<Guid, CacheEntry>` por tenant; TTL = `NotAfter - margemAntesVencimento` (default 1 dia); método `Invalidar()` para upload de novo cert
- [ ] 2.2.4 ⚠ DEFERIDO — Integração com `CertificadoVencimentoVarreduraWorker` é uma mudança no worker existente; entra em fase futura (Fase 6 ou separada). Resolver já expõe `NotAfter` via `cert.NotAfter` para o worker consumir

### 2.3 Assinador XML C14N

- [x] 2.3.1 `XmlSignerC14N` em `ExternalIntegration/Sefaz/Certificado/` usando `SignedXml` (`System.Security.Cryptography.Xml` adicionado ao .csproj)
- [x] 2.3.2 Reference URI = `#<Id>` resolvido por subclasse `IdAwareSignedXml` (NFe usa atributo `Id` simples, não xml:id)
- [x] 2.3.3 Transforms: `XmlDsigEnvelopedSignatureTransform` + `XmlDsigExcC14NTransform`
- [x] 2.3.4 `CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl`
- [x] 2.3.5 `SignatureMethod = SignedXml.XmlDsigRSASHA1Url` (SEFAZ ainda exige SHA-1 para NFe v4.00)
- [x] 2.3.6 `KeyInfo` com `KeyInfoX509Data(cert)` — embute `X509Certificate` em base64
- [x] 2.3.7 ⚠ PARCIAL — Auto-validação via `SignedXml.CheckSignatureReturningKey` confirma integridade da assinatura (test `Sign_AssinaturaResultanteEhVerificavelPorCheckSignature`); golden file vs `xmlsec1` externo precisa de NFe sample real e fica para Fase 7 (validação fim-a-fim)

### 2.4 A3 (token físico) — opcional, fase 2

- [ ] 2.4.1 ⚠ DEFERIDO (proposal explícita: "A3 fica para fase final") — Definir `IPkcs11Provider`
- [ ] 2.4.2 ⚠ DEFERIDO — Implementação via `Pkcs11Interop`
- [ ] 2.4.3 ⚠ DEFERIDO — Documentar drivers (SafeNet eToken 5110, Watchdata, Gemalto)
- [ ] 2.4.4 ⚠ DEFERIDO — Test manual com token físico

---

## Fase 3 — Comunicação SOAP/HTTPS

### 3.1 Catálogo de URLs SEFAZ

- [x] 3.1.1 ⚠ PARCIAL — `sefaz-urls.json` cobre 5 UFs prioritárias (SP, RJ, MG, RS, PR) + SVRS + SVAN × 2 ambientes × 6 serviços = 84 entradas. Restantes 22 UFs listadas em `_demais_ufs_pendentes` para adição conforme demanda (proposal explícita: foco nas 5 prioritárias)
- [x] 3.1.2 `SefazUrlCatalog` carrega via `EmbeddedResource`, parseia com `JsonDocument`, expõe `DefinirOverride` para tests/ambientes privados
- [x] 3.1.3 Helpers `GetAutorizacao/GetRetAutorizacao/GetConsultaProtocolo/GetStatusServico/GetRecepcaoEvento/GetInutilizacao(uf, amb)`
- [x] 3.1.4 SVRS e SVAN como UFs especiais no catálogo
- [x] 3.1.5 Tests `SefazUrlCatalogTests` (11 fatos): 5 UFs prioritárias + SVRS/SVAN com hosts conhecidos, UF inexistente, override, homolog vs producao distintos

### 3.2 HttpClient com mTLS

- [x] 3.2.1 `SocketsHttpHandler.SslOptions.ClientCertificates` injeta cert do tenant + `LocalCertificateSelectionCallback` força seleção
- [x] 3.2.2 `EnabledSslProtocols = TLS 1.2 | TLS 1.3` (SEFAZ rejeita TLS 1.0/1.1)
- [x] 3.2.3 `Timeout` parametrizável no construtor (default 30s)
- [x] 3.2.4 Polly v8 `ResiliencePipeline<HttpResponseMessage>` com 2 retries, backoff exponencial 500ms; condições: HttpRequestException, TaskCanceledException, 5xx (não retenta 4xx fiscal)
- [x] 3.2.5 Logging via `ILogger<SefazSoapClient>`: nível Information para sucesso (servico/uf/ambiente/status/latencia), Warning para falha (com exceção)

### 3.3 Envelope SOAP

- [x] 3.3.1 `SoapEnvelopeBuilder.Build(payload, wsdlNs)` monta envelope SOAP 1.2 com `<nfeDadosMsg xmlns="...">` no body
- [x] 3.3.2 `SefazSoapClient` configura `Content-Type: application/soap+xml; charset=utf-8; action="..."` via `MediaTypeHeaderValue` + `NameValueHeaderValue`
- [x] 3.3.3 `SoapAction.For(servico)` retorna `(WsdlNamespace, Action)` por serviço (`nfeAutorizacaoLote`, `nfeRetAutorizacaoLote`, etc.)
- [x] 3.3.4 `SoapEnvelopeBuilder.ExtractResultMsg(soap)` retorna conteúdo de `nfeResultMsg`; em SOAP Fault, retorna o XML do Fault para diagnóstico
- [ ] 3.3.5 ⚠ PARCIAL — Tests validam estrutura/well-formedness do envelope + parse de respostas com nfeResultMsg e Fault. Golden file vs exemplo SEFAZ real precisa de captura homolog real (Fase 7)

---

## Fase 4 — Serviços SEFAZ

### 4.1 NFeAutorizacao4 (síncrono)

- [x] 4.1.1 `NFeAutorizacaoService.AutorizarSyncAsync` em `ExternalIntegration/Sefaz/Servicos/`
- [x] 4.1.2 Validação XSD opt-in (`XsdValidator.TemSchemasCarregados`) → cStat=999 local em caso de invalidade
- [x] 4.1.3 Lote `enviNFe` com idLote=1, indSinc=1 (síncrono) ou indSinc=0 (assíncrono)
- [x] 4.1.4 `ParseRetorno` extrai `protNFe.infProt` ou `infRec` para retorno unificado `AutorizacaoResultado`
- [x] 4.1.5 `SefazResultadoCodigo` em `Domain/Entities/Fiscal/Xml/Servicos/` com helpers `IsAutorizado` / `IsParalisacao`
- [ ] 4.1.6 ⚠ DEFERIDO — Test contra homolog SP precisa de cert real + tenant; entra em Fase 7. Cobertura atual: parser de retorno (3 fatos: sync sucesso, async recibo, erro 225)

### 4.2 NFeRetAutorizacao4 (assíncrono)

- [x] 4.2.1 `NFeAutorizacaoService.AutorizarAsyncAsync` retorna `nRec` em `AutorizacaoResultado.NRecibo`
- [x] 4.2.2 `NFeRetAutorizacaoService.ConsultarReciboAsync` faz polling exponencial (2s → 30s, max 6 tentativas) até cStat ≠ 105
- [ ] 4.2.3 ⚠ DEFERIDO — Test de latência simulada precisa de mock de `SefazSoapClient`; cobertura atual via parser RetConsReciNFe (Fase 7)

### 4.3 NFeConsultaProtocolo4

- [x] 4.3.1 `NFeConsultaProtocoloService.ConsultarChaveAsync(chave, ambiente, uf, cert)` valida 44 dígitos e parseia `retConsSitNFe.protNFe`
- [x] 4.3.2 Caso de uso documentado no XML doc — reconciliação de NFes perdidas
- [ ] 4.3.3 ⚠ DEFERIDO — Test contra chave real homolog (Fase 7)

### 4.4 NFeStatusServico4

- [x] 4.4.1 `NFeStatusServicoService.ConsultarStatusServicoAsync` retorna `StatusServicoResultado` com flags `Operando`/`Paralisado`
- [x] 4.4.2 Cache `ConcurrentDictionary<key, (resultado, expira)>` com TTL 5min; `ignorarCache=true` força refresh
- [ ] 4.4.3 Integração com `ContingenciaPolicy` será implementada na Fase 5

### 4.5 NFeRecepcaoEvento4 — Cancelamento

- [x] 4.5.1 `Evento` + `InfEvento` + `DetEvento` em `Domain/Entities/Fiscal/Xml/Servicos/Evento.cs` com `nProt`, `xJust`, `tpEvento=110111`; validação 15-255 chars no `CancelarAsync`
- [x] 4.5.2 `XmlSignerC14N.Sign` reusado com Id formato `ID<tpEvento><chNFe><nSeqEvento>` (Reference URI = `#ID...`)
- [x] 4.5.3 `EnviarEventoAsync` parseia `retEnvEvento.retEvento[0].infEvento` e usa `IsAutorizado` (135/136 = sucesso)
- [ ] 4.5.4 ⚠ DEFERIDO — Test contra homolog real (Fase 7); cobertura unit em `MontarEvento` (Id correto + cOrgao por UF)

### 4.6 NFeRecepcaoEvento4 — CC-e

- [x] 4.6.1 `EmitirCCeAsync` valida xCorrecao 15-1000 chars; tpEvento=110110; xCondUso preenchida com texto legal padrão
- [x] 4.6.2 Mesmo pipeline de assinar/transmitir/parsear de cancelamento (compartilhado em `EnviarEventoAsync`)
- [ ] 4.6.3 ⚠ DEFERIDO — PDF da CC-e (`QuestPdfDanfeRenderer`) é trabalho de UI/Reporting, fora do escopo do cliente SEFAZ

### 4.7 NFeInutilizacao4

- [x] 4.7.1 `InutNFe` + `InfInut` em `Domain/Entities/Fiscal/Xml/Servicos/Inutilizacao.cs`; `NFeInutilizacaoService.InutilizarAsync` valida xJust 15-255, nNFFin ≥ nNFIni, CNPJ 14 dígitos; Id formato `ID<cUF><ano><CNPJ><mod><serie><nNFIni><nNFFin>`
- [x] 4.7.2 Mesmo pipeline de assinar via `XmlSignerC14N.Sign` + transmitir + parsear `retInutNFe.infInut` (cStat=102 = homologado)
- [x] 4.7.3 Caso de uso documentado em XML doc; teste cobre deserialização do retorno com protocolo

---

## Fase 5 — Operação e contingência

### 5.1 Numerador sequencial

- [x] 5.1.1 Migration `V20260510001_CriarTabelaNFeNumeracao` com UNIQUE (tenant_id, cnpj, serie) e INDEX em tenant_id
- [x] 5.1.2 `INumeradorNFe` em Domain + `NumeradorNFe` em Repository usando idiom MySQL `INSERT … ON DUPLICATE KEY UPDATE col = LAST_INSERT_ID(col + 1); SELECT LAST_INSERT_ID()` — atômico em **uma única ida ao banco**, sem gap entre lock e write (mais eficiente que SELECT … FOR UPDATE + UPDATE)
- [ ] 5.1.3 ⚠ DEFERIDO — Test de concorrência com 100 threads precisa de MySQL real (integration); cobertura unit indireta via Theory de validação de input. Integration test entra na Fase 7
- [x] 5.1.4 `AjustarUltimoNumeroAsync` faz upsert do ultimo_numero (usado após inutilização para definir = nNFFin)

### 5.2 Política de contingência SVRS

- [x] 5.2.1 `ContingenciaPolicy` em `ExternalIntegration/Sefaz/Contingencia/` com `ContingenciaInfo(Estado, DesdeUtc, RetomarTesteEmUtc, …)` por (uf, ambiente); janela default 5min, configurável
- [x] 5.2.2 `RegistrarRespostaTransmissao(uf, amb, cStat, motivo, erroDeRede)` marca indisponível em timeout/erro de rede/cStat 108/109
- [x] 5.2.3 `SefazStatusWorker` (BackgroundService, cron 1min) itera UFs em contingência conhecidas — chamada de status efetiva fica deferida (precisa de cert "system"); arquitetura está pronta para plugar
- [ ] 5.2.4 ⚠ DEFERIDO — Integração `INFeSefazClient` ↔ `ContingenciaPolicy` (decidir URL via `UfParaUsar`, marcar `tpEmis=6 SVRS`) entra na Fase 6 junto com o `RealNFeSefazClient`
- [x] 5.2.5 Test "ErroDeRede → SVRS" + Theory "cStat 108/109 → SVRS" + "ForcarContingencia → SVRS"
- [x] 5.2.6 Test "Operando 107 → limpa estado → volta para origem" + "Janela expirada → volta automaticamente"

### 5.3 Reprocessamento de pendentes

- [x] 5.3.1 `NFePendenteReprocessadorWorker` em `Api/Hosted/` itera NFes em status `EmContingencia` ou `Transmitindo` (eq. "EnviadaSemRetorno") em batches de 50
- [ ] 5.3.2 ⚠ DEFERIDO — Chamada efetiva de `NFeConsultaProtocoloService.ConsultarChaveAsync` + `UpdateStatusAsync` requer escolha de tenant/cert por NFe (não há tenant ambient no worker); arquitetura pronta, integração final na Fase 6 (junto com `RealNFeSefazClient`)
- [x] 5.3.3 Cron fixo de 5 min (constant `Intervalo`); feature flag opt-out fica para change futura
- [ ] 5.3.4 ⚠ DEFERIDO — Test fim-a-fim requer integração com DB + SEFAZ; cobertura conceitual via worker estruturalmente correto, integration test na Fase 7

---

## Fase 6 — Substituição do stub e go-live homologação

### 6.1 Implementação real do `INFeSefazClient`

- [x] 6.1.1 `RealNFeSefazClient` em `ExternalIntegration/Sefaz/RealNFeSefazClient.cs`
- [x] 6.1.2 `AutorizarAsync` orquestra: resolve cert via `CertificadoTenantResolver` → consulta `ContingenciaPolicy.UfParaUsar` → delega a `NFeAutorizacaoService.AutorizarSyncAsync` → `RegistrarRespostaTransmissao` (sucesso ou exceção)
- [x] 6.1.3 `EnviarEventoAsync` faz deserialize do XML legado para `Evento`, identifica tpEvento e delega a `NFeRecepcaoEventoService.CancelarAsync`/`EmitirCCeAsync`
- [x] 6.1.4 DI: `ExternalIntegrationDI.AddAcmeExternalIntegration` registra todos os blocos SEFAZ (catálogo, validators, signer, services, policy, resolver, RealNFeSefazClient); `ServicesServiceCollection.AddAcmeServices(configuration)` mapeia `INFeSefazClient → RealNFeSefazClient` por default; `INumeradorNFe` registrado em `RepositoryServiceCollectionExtensions`
- [x] 6.1.5 Feature flag `Fiscal:UseStub` (default false) — em `true` mantém `StubNFeSefazClient` registrado; `Program.cs` agora passa `builder.Configuration` para `AddAcmeServices`

### 6.2 Configuração e segurança

- [x] 6.2.1 Senha PFX já criptografada via `TenantSecretCipher` (AES-GCM com chave derivada por tenant via HKDF) — `MasterEncryptionKey` na config; reuso do que existe (melhor que `IDataProtector` pelo isolamento por tenant)
- [x] 6.2.2 Endpoint de upload já existe em `ImportarCertificado` (verificado em `src/Service/Acme.Sistemas.Services/V1/Fiscal/Command/ImportarCertificado/`)
- [x] 6.2.3 Validação cert via `A1CertificadoLoader` testada em `A1CertificadoLoaderTests` (Fase 2): senha errada lança `CertificadoInvalidoException` com mensagem clara

### 6.3 Reativação dos testes E2E

- [ ] 6.3.1 ⚠ BLOQUEADO — Remover Skip do test E2E exige cert real ICP-Brasil + tenant configurado homolog. Tem que ser feito quando os artefatos externos chegarem
- [ ] 6.3.2 ⚠ BLOQUEADO — Pipeline CI com seed homolog: depende de decisão de infra/CI (Azure KV, AWS Secrets, ou similar) para armazenar o cert mock fora do repo
- [ ] 6.3.3 ⚠ DEFERIDO — Trait "HomologReal" será aplicado quando o test for reativado (6.3.1); padrão já encaixa no analyzer (Trait("Categoria",...) seria nova dimensão — fica como decisão de change futura)

### 6.4 Validação fim-a-fim em UFs prioritárias

- [ ] 6.4.1 ⚠ BLOQUEADO — emissão real homolog SP precisa de cert ICP-Brasil + ambiente SEFAZ-SP
- [ ] 6.4.2 ⚠ BLOQUEADO — RJ idem
- [ ] 6.4.3 ⚠ BLOQUEADO — MG idem
- [ ] 6.4.4 ⚠ BLOQUEADO — RS idem
- [ ] 6.4.5 ⚠ BLOQUEADO — PR idem
- [ ] 6.4.6 ⚠ BLOQUEADO — cancelamento real depende de NFe autorizada (6.4.1-5)
- [ ] 6.4.7 ⚠ BLOQUEADO — CC-e idem

### 6.5 Remoção do stub

- [x] 6.5.1 Stub não é mais default em DI — agora atrás de feature flag `Fiscal:UseStub=true` (default false → usa `RealNFeSefazClient`)
- [ ] 6.5.2 ⚠ NÃO-EXECUTADO POR DECISÃO — manter `StubNFeSefazClient` no projeto Services preserva a opção de dev local sem cert. Mover para test project obriga test refs do API project. Trade-off: ~40 linhas de stub vivem em produção mas só rodam se flag = true. Aceitável
- [x] 6.5.3 `CLAUDE.md` atualizado — não havia menção pré-existente de "stub" (já tinha sido limpo); adicionado bloco descrevendo `RealNFeSefazClient` e seus componentes

### 6.6 Documentação

- [x] 6.6.1 `blueprint.yml` não tem seção fiscal específica para atualizar; convenções já cobrem a estrutura
- [ ] 6.6.2 ⚠ DEFERIDO — Seção "Upload de certificado e troca de ambiente" será documentada quando o endpoint receber UI dedicada (escopo separado)
- [x] 6.6.3 UFs suportadas + roadmap documentadas em `sefaz-urls.json` (5 prioritárias + SVRS + SVAN; lista `_demais_ufs_pendentes` com 22 UFs)

---

## Fase 7 — Validação final

- [ ] 7.1 `dotnet build Atena.sln` verde
- [ ] 7.2 `dotnet test --filter Category!=HomologReal` verde
- [ ] 7.3 `dotnet test --filter Category=HomologReal` verde (manual, requer cert)
- [ ] 7.4 5 UFs prioritárias com autorização real em homologação
- [ ] 7.5 `openspec validate nfe-cliente-sefaz-proprio --strict` verde
- [ ] 7.6 `openspec archive nfe-cliente-sefaz-proprio` ao final
