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

- [ ] 4.1.1 Implementar `NFeAutorizacaoService.AutorizarSyncAsync(NFe, ambiente, uf)`
- [ ] 4.1.2 Validar XML local (XSD) antes de transmitir; se inválido, retornar `cStat=999` local
- [ ] 4.1.3 Assinar NFe; montar lote `enviNFe` com `idLote=1, indSinc=1`
- [ ] 4.1.4 Transmitir + parsear `retEnviNFe` → `protNFe.infProt`
- [ ] 4.1.5 Mapear `cStat` para enum `SefazResultadoCodigo` (100, 102, 110, 204, 539, ...)
- [ ] 4.1.6 Test integração contra homolog SP (mockado em CI; real em pipeline manual)

### 4.2 NFeRetAutorizacao4 (assíncrono)

- [ ] 4.2.1 `AutorizarAsyncAsync` com `indSinc=0` retorna `nRec` (recibo)
- [ ] 4.2.2 `ConsultarRecibo(nRec)` polling até `cStat=104` (Lote processado)
- [ ] 4.2.3 Test simulando latência > 1s

### 4.3 NFeConsultaProtocolo4

- [ ] 4.3.1 `ConsultarChaveAsync(chave, ambiente, uf)` → status atual da NFe
- [ ] 4.3.2 Útil para reconciliação de NFes "perdidas" (autorizada na SEFAZ mas sem retorno do lote)
- [ ] 4.3.3 Test contra chave conhecida em homolog

### 4.4 NFeStatusServico4

- [ ] 4.4.1 `ConsultarStatusServicoAsync(ambiente, uf)` → `cStat` (107=operando, 108=paralisada momentaneamente, 109=paralisação programada)
- [ ] 4.4.2 Cache de 5 min para evitar flood
- [ ] 4.4.3 Usado pela `ContingenciaPolicy` (ver Fase 5)

### 4.5 NFeRecepcaoEvento4 — Cancelamento

- [ ] 4.5.1 Modelar `evCancNFe` com nProt, xJust (15-255 chars), tpEvento=110111
- [ ] 4.5.2 Assinar evento separadamente (Reference URI = #ID...)
- [ ] 4.5.3 Transmitir + parsear retorno; sucesso = `cStat=135 (Evento registrado)`
- [ ] 4.5.4 Test contra homolog: cancelar NFe autorizada anteriormente

### 4.6 NFeRecepcaoEvento4 — CC-e

- [ ] 4.6.1 Modelar `evCCe` com xCorrecao (15-1000 chars), tpEvento=110110
- [ ] 4.6.2 Assinar + transmitir + parsear
- [ ] 4.6.3 Geração do PDF da CC-e (extensão do `QuestPdfDanfeRenderer`)

### 4.7 NFeInutilizacao4

- [ ] 4.7.1 Modelar `inutNFe` (CNPJ, mod, serie, nNFIni, nNFFin, xJust)
- [ ] 4.7.2 Assinar + transmitir + parsear
- [ ] 4.7.3 Útil para descartar numeração não-utilizada antes do encerramento mensal

---

## Fase 5 — Operação e contingência

### 5.1 Numerador sequencial

- [ ] 5.1.1 Migration: tabela `nfe_numeracao (tenant_id, cnpj, serie, ultimo_numero, atualizado_em)`
- [ ] 5.1.2 Implementar `NumeradorNFe.ProximoAsync(tenant, cnpj, serie)` com `SELECT ... FOR UPDATE` + UPDATE atômico
- [ ] 5.1.3 Test de concorrência: 100 threads paralelas pedindo número → 100 números únicos sequenciais
- [ ] 5.1.4 Recuperação após inutilização: ajustar último_numero para `nNFFin + 1`

### 5.2 Política de contingência SVRS

- [ ] 5.2.1 Implementar `ContingenciaPolicy` com estado por (uf, ambiente): `Operando | Indisponivel(desde, retomar_em)`
- [ ] 5.2.2 Hook após cada chamada: timeout/`cStat=108` → marca indisponível por 5 min
- [ ] 5.2.3 Worker `SefazStatusWorker` (cron 1 min) chama `ConsultarStatusServico` e atualiza estado
- [ ] 5.2.4 `INFeSefazClient` decide URL via `ContingenciaPolicy`: se indisponível, vai para SVRS com `tpEmis=6`
- [ ] 5.2.5 Test: simular indisponibilidade → confirmar fallback automático
- [ ] 5.2.6 Test: simular retomada → confirmar volta para SEFAZ origem

### 5.3 Reprocessamento de pendentes

- [ ] 5.3.1 Worker `NFePendenteReprocessadorWorker` itera NFes em status `EmContingencia` ou `EnviadaSemRetorno`
- [ ] 5.3.2 Para cada: `ConsultarChave` → atualiza status local
- [ ] 5.3.3 Cron 5 min, configurável via feature flag
- [ ] 5.3.4 Test: NFe em contingência aparece autorizada após reprocesso

---

## Fase 6 — Substituição do stub e go-live homologação

### 6.1 Implementação real do `INFeSefazClient`

- [ ] 6.1.1 Criar `RealNFeSefazClient` em `Acme.Sistemas.ExternalIntegration/Sefaz/`
- [ ] 6.1.2 Implementar `AutorizarAsync` orquestrando: build XML → assinar → escolher URL (contingência) → transmitir → parsear
- [ ] 6.1.3 Implementar `EnviarEventoAsync` análogo (cancel + CC-e)
- [ ] 6.1.4 DI: trocar registro de `StubNFeSefazClient` por `RealNFeSefazClient` em `ExternalIntegrationDI`
- [ ] 6.1.5 Manter feature flag `Fiscal.UseStub` para fallback emergencial em dev

### 6.2 Configuração e segurança

- [ ] 6.2.1 Senha do PFX criptografada com `IDataProtector` (chave em vault, não em config)
- [ ] 6.2.2 Endpoint `/api/v1/configuracao-fiscal/upload-certificado` aceita PFX + senha
- [ ] 6.2.3 Test: senha errada falha com erro claro, senha certa carrega cert

### 6.3 Reativação dos testes E2E

- [ ] 6.3.1 Remover `[Skip = "..."]` de `Fluxo_Login_PedidoVenda_Faturamento_NFe_DeveCompletar`
- [ ] 6.3.2 Configurar pipeline CI: gera tenant homolog + cert mock + roda fluxo completo contra SEFAZ-SP homolog
- [ ] 6.3.3 Marcar como `[Trait("Category","HomologReal")]` para rodar manualmente, não em todo PR

### 6.4 Validação fim-a-fim em UFs prioritárias

- [ ] 6.4.1 Emitir NFe homolog em SP → autorização real
- [ ] 6.4.2 Emitir NFe homolog em RJ → autorização real
- [ ] 6.4.3 Emitir NFe homolog em MG → autorização real
- [ ] 6.4.4 Emitir NFe homolog em RS → autorização real
- [ ] 6.4.5 Emitir NFe homolog em PR → autorização real
- [ ] 6.4.6 Cancelar 1 NFe em cada UF
- [ ] 6.4.7 Emitir CC-e em 1 NFe em cada UF

### 6.5 Remoção do stub

- [ ] 6.5.1 Confirmar que stub não é mais referenciado em DI de produção
- [ ] 6.5.2 Mover `StubNFeSefazClient` para projeto de tests (uso só em unit tests)
- [ ] 6.5.3 Atualizar `CLAUDE.md` removendo nota de "stub"

### 6.6 Documentação

- [ ] 6.6.1 Atualizar `documentacao/blueprint.yml` se houver pontos sobre fiscal
- [ ] 6.6.2 Adicionar seção em `documentacao/` sobre upload de certificado e troca de ambiente
- [ ] 6.6.3 Documentar UFs suportadas e roadmap de UFs adicionais

---

## Fase 7 — Validação final

- [ ] 7.1 `dotnet build Atena.sln` verde
- [ ] 7.2 `dotnet test --filter Category!=HomologReal` verde
- [ ] 7.3 `dotnet test --filter Category=HomologReal` verde (manual, requer cert)
- [ ] 7.4 5 UFs prioritárias com autorização real em homologação
- [ ] 7.5 `openspec validate nfe-cliente-sefaz-proprio --strict` verde
- [ ] 7.6 `openspec archive nfe-cliente-sefaz-proprio` ao final
