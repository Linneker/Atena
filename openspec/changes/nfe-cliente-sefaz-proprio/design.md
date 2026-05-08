# Design — nfe-cliente-sefaz-proprio

## Arquitetura em 5 camadas

```
┌─────────────────────────────────────────────────────────────┐
│  CAMADA 5 — Operação                                        │
│  NumeradorNFe, ContingenciaPolicy, ReprocessadorPendentes   │
├─────────────────────────────────────────────────────────────┤
│  CAMADA 4 — Serviços SEFAZ (high-level)                     │
│  INFeSefazClient real → NFeAutorizacaoService,              │
│    NFeRetAutorizacaoService, NFeConsultaProtocoloService,   │
│    NFeStatusServicoService, NFeRecepcaoEventoService,       │
│    NFeInutilizacaoService                                   │
├─────────────────────────────────────────────────────────────┤
│  CAMADA 3 — Comunicação SOAP/HTTPS                          │
│  SefazSoapClient (HttpClient + mTLS), SefazUrlCatalog       │
├─────────────────────────────────────────────────────────────┤
│  CAMADA 2 — Assinatura digital                              │
│  ICertificadoLoader (A1/A3), XmlSignerC14N                  │
├─────────────────────────────────────────────────────────────┤
│  CAMADA 1 — XML & Schema                                    │
│  Modelos NFe v4.00, ChaveAcessoBuilder, XsdValidator        │
└─────────────────────────────────────────────────────────────┘
```

## Decisões e tradeoffs

### Por que reescrever do zero ao invés de usar lib?
- Independência tecnológica: bibliotecas open-source de NF-e mudam mantenedores frequentemente; algumas viram pagas.
- Auditabilidade: código fiscal precisa ser revisável internamente para compliance.
- Controle: bugs em libs externas geram dependências de terceiros para correção.
- Risco aceito: ~5 sprints de esforço, com escopo bem definido.

### Por que `HttpClient` puro em vez de WCF/`ChannelFactory`?
WCF e bindings SOAP gerados via `dotnet-svcutil` funcionam mas:
- Suporte WCF no .NET moderno é limitado em alguns features avançados.
- Configuração de mTLS + custom headers (`cUF`, `versaoDados`) é mais explícita com `HttpClient`.
- SOAP 1.2 com WS-Addressing pode ser construído à mão (envelope é estável).

`HttpClient` + `XmlSerializer` no envelope SOAP dá controle total sem dependência de geração de código.

### Por que A1 antes de A3?
A1 (PFX em arquivo) é 100% gerenciável via .NET puro (`X509Certificate2`). A3 (smartcard/token USB) requer PKCS#11 ou CSP Windows — driver do fabricante, contexto thread-affinity, PIN management. Implementar A1 primeiro entrega 90% dos clientes; A3 fica como adição opcional.

### Modelos NFe — codegen ou hand-coded?
SEFAZ publica XSDs oficiais. Duas opções:
- **xsd.exe** geraria classes automaticamente — rápido, mas gera código verboso e às vezes incorreto em namespaces aninhados.
- **Hand-coded com `[XmlElement]` attributes** — mais trabalho inicial mas total controle sobre serialização.

Decisão: **híbrido**. Usar `xsd.exe` como rascunho inicial, refinar manualmente os tipos críticos (assinatura, eventos). Schemas oficiais embutidos como recursos para validação runtime.

### Cantonização C14N exclusive
A SEFAZ exige `http://www.w3.org/2001/10/xml-exc-c14n#`. .NET tem `XmlDsigExcC14NTransform` nativo. O ponto crítico é a ordem dos elementos e namespaces — qualquer reordenação após assinar invalida.

### Numeração sequencial e race condition
Cenário: dois faturamentos paralelos pedem número de NF-e simultaneamente. Solução:
- Tabela `nfe_numeracao` com `(tenant_id, cnpj, serie, ultimo_numero)`.
- Update atômico: `UPDATE ... SET ultimo_numero = ultimo_numero + 1 WHERE ... RETURNING ultimo_numero` (MySQL: `SELECT ... FOR UPDATE` + UPDATE + COMMIT).
- Lock pessimista evita pulos; pulos são proibidos por lei fiscal.

### Contingência SVRS — quando ativar?
Política:
1. Tenta SEFAZ-Origem (UF do emitente).
2. Se timeout > 30s ou status `cStat=108` (paralisação), marca origem como "indisponível por 5 min".
3. Próximas transmissões vão para SVRS (`https://nfe.svrs.rs.gov.br/...`).
4. Cron a cada minuto re-testa a origem com `NFeStatusServico4`; ao voltar a `cStat=107`, desativa contingência.
5. NF-e emitidas em contingência ficam com flag `tpEmis=6 (SVRS)`; depois reprocessadas.

### Storage de XMLs
Já existe convenção `{tenant_id}/{ano}/{mes}/{chave}.xml` em S3/MinIO. Mantém. XML autorizado + protocolo concatenado em `procNFe`.

## Riscos e mitigações

| Risco | Probabilidade | Impacto | Mitigação |
|-------|---------------|---------|-----------|
| Schema XSD muda (4.00 → 4.10) | Baixa anual | Alto | Versionar schemas embutidos, feature flag por versão |
| Cert expirado não detectado | Média | Alto | Worker `CertificadoVencimentoVarreduraWorker` já existe; integrar |
| Race em numeração | Baixa | Crítico | Lock pessimista + test de concorrência com 100 threads |
| Driver A3 inconsistente | Alta | Médio | Documentar fabricantes testados; A3 opcional |
| SVRS também cair | Baixa | Crítico | EPEC offline (fora do escopo desta change) |
| URL de UF muda | Baixa | Médio | Catálogo JSON versionado, override via config por tenant |

## Test strategy

- **Unit**: XML golden files vs NFes de exemplo da SEFAZ (anexo `documentacao/sefaz/exemplos/`). Assinatura testada com `xmlsec1` (build CI).
- **Integration**: contra ambiente homolog SEFAZ-SP (mais estável). Tenant + cert mock A1 gerado por OpenSSL.
- **Concorrência**: numeração com 100 threads paralelas → 0 pulos, 0 duplicatas.
- **Contingência**: simulação via mock que injeta timeout/erro → confirma fallback SVRS.
