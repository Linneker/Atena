# Design — nfse-abrasf-pluggavel

## Arquitetura

```
┌────────────────────────────────────────────────────────────┐
│  Service Layer                                             │
│  EmitirNFSeCommand → NFSeService.EmitirAsync(NFSe)        │
└──────────────────────────┬─────────────────────────────────┘
                           ▼
┌────────────────────────────────────────────────────────────┐
│  NFSeMunicipalClientFactory                                │
│  Resolve(codigoIbgeMunicipio) → INFSeMunicipalClient      │
└────────┬─────────────┬─────────────┬─────────────┬─────────┘
         ▼             ▼             ▼             ▼
   ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐
   │ ABRASF   │  │ SP       │  │ NotaRio  │  │ ... (futuros)
   │ v2.04    │  │ próprio  │  │ (RJ)     │  │
   │ ✓ ESTA   │  │ ─ outra  │  │ ─ outra  │  │
   │   CHANGE │  │   change │  │   change │  │
   └──────────┘  └──────────┘  └──────────┘  └──────────┘
```

## Decisões e tradeoffs

### Por que adapter por município, não por padrão?
Padrão (ABRASF, IPM etc.) é uma camada interna. O cliente do sistema pensa em "minha empresa fica em São Paulo" — não em padrão técnico. A factory recebe código IBGE (granularidade do município) e resolve internamente para o adapter certo. Isso permite que dois municípios usando ABRASF sejam servidos pelo mesmo adapter, com config específica.

### Por que ABRASF v2.04 primeiro?
- Cobre ~50% dos municípios brasileiros.
- 1 implementação serve N municípios (ROI alto).
- Schema documentado e estável desde 2014.
- Algumas capitais grandes (Goiânia, Curitiba, Florianópolis, Vitória) usam ABRASF.

**Mas**: SP capital e RJ capital NÃO usam ABRASF (têm padrões próprios). Os 2 maiores mercados ficam para changes posteriores. Aceito.

### Modelo de dados — entidade genérica ou por padrão?
Genérica. Tabela `nfse` única com campos comuns (numero, prestador, tomador, valor, status, codigoServico, descricao, xmlAutorizado, padraoMunicipal, ...). Campos específicos de cada padrão ficam em JSON em coluna `dados_extras` ou em tabelas adicionais por padrão se necessário.

### Login/auth municipal
Configuração por tenant em `configuracao_fiscal_nfse`:
- `padrao_municipal` (enum)
- `usuario_municipal` (criptografado)
- `senha_municipal` (criptografada via IDataProtector)
- `token_municipal` (alguns ABRASF usam)
- `cert_a1_pfx` (opcional, alguns só usam cert)

### Cancelamento — fluxo
ABRASF v2.04 tem operação `CancelarNfseEnvio`:
1. Monta `<Pedido>` com chave + código de cancelamento.
2. Assinatura digital no `<Pedido>`.
3. Transmite via SOAP.
4. Recebe `RetornoCancelamento`.

Prazo varia por município. Configuração `prazo_cancelamento_horas` por município no catálogo.

### Códigos de serviço
LC 116/03 lista 123 códigos nacionais (ex.: "1.04 - Elaboração de programas de computador"). Cada município pode subdividir. Estratégia:
- Migration carrega LC 116 nacional (123 códigos).
- API admin permite adicionar códigos municipais.
- Validação na emissão: código declarado deve existir.

### Storage de XML
Mesmo S3/MinIO já usado para NF-e:
- `nfse/{tenant}/{ano}/{mes}/{codigoIbge}/{numero}.xml`
- Conteúdo: XML de retorno autorizado pela prefeitura.

### Compatibilidade futura com DPS
Padrão nacional DPS (Receita Federal) começou implantação 2024-2026 e vai gradualmente substituir NFS-e municipais. Arquitetura adapter já permite adicionar `DpsNacionalAdapter` sem mexer no resto. Quando DPS virar obrigatório, factory passa a rotear para DPS automaticamente.

## Roadmap incremental

```
   Esta change ─→ ABRASF v2.04 base
                   ↓
   Próxima change ─→ SP-município (sigiss próprio)
                   ↓
   Próxima change ─→ NotaCarioca (RJ próprio)
                   ↓
   Próxima change ─→ Ginfes / IPM (cobre mais ~20%)
                   ↓
   Futuro (2027+) ─→ DPS nacional (substitui tudo)
```

## Testes

- **Unit**: serializer ABRASF gera XML byte-igual a samples conhecidos da abrasf.org.br.
- **Integration**: contra ambiente homolog de pelo menos 3 municípios ABRASF (Vitória, Florianópolis, Porto Alegre — confirmar quais têm homolog público acessível).
- **Adapter contract test**: cada implementação de `INFSeMunicipalClient` passa em 5 cenários canônicos (emitir OK, emitir com erro, cancelar OK, consultar, fluxo completo).

## Riscos não-mitigados

- Municípios anunciam ABRASF mas têm tweaks proprietários — vai pegar bug em produção.
- Endpoints de homologação municipal são instáveis.
- Documentação ABRASF é incompleta (versionamento dos campos).
