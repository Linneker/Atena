# XSDs oficiais NF-e v4.00

Esta pasta deve conter os schemas XSD oficiais da SEFAZ para validação local
antes da transmissão.

## Como obter

Baixar do Portal da Nota Fiscal Eletrônica (Receita Federal):
- https://www.nfe.fazenda.gov.br/portal/listaConteudo.aspx?tipoConteudo=/fwLvLUSnVU=

Pacotes esperados (após Nota Técnica vigente, p.ex. NT 2024.002 v1.10):

```
nfe_v4.00.xsd            ← schema principal
leiauteNFe_v4.00.xsd
tiposBasico_v4.00.xsd
xmldsig-core-schema_v1.01.xsd
enviNFe_v4.00.xsd
retEnviNFe_v4.00.xsd
consSitNFe_v4.00.xsd
retConsSitNFe_v4.00.xsd
consStatServ_v4.00.xsd
retConsStatServ_v4.00.xsd
envEvento_v1.00.xsd
retEnvEvento_v1.00.xsd
inutNFe_v4.00.xsd
retInutNFe_v4.00.xsd
procNFe_v4.00.xsd
procEventoNFe_v1.00.xsd
procInutNFe_v4.00.xsd
```

## Configuração

Os arquivos `.xsd` aqui são incluídos como `EmbeddedResource` no `.csproj`
do projeto `Acme.Sistemas.ExternalIntegration` e carregados em runtime
pelo `XsdValidator`.

## Por que não estão versionados

Schemas oficiais Receita são públicos mas distribuídos em pacotes ZIP
versionados por Nota Técnica. Optamos por **não** vendor-ear no repo
para evitar sair do sync com a versão vigente; o build de release deve
puxar a NT atual antes de empacotar.

Para desenvolvimento local, baixe e cole os XSDs aqui — o `.gitignore`
desta pasta permite arquivos `.xsd` para iteração local mas o pipeline
oficial substitui pelo pacote vigente.

## Licença

Os XSDs são publicados pela Receita Federal e são de uso público
para fins de emissão de NF-e.
