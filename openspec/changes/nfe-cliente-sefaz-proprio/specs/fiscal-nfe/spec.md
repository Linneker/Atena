## MODIFIED Requirements

### Requirement: Emissão de NF-e Modelo 55
O sistema SHALL emitir NF-e modelo 55 com cliente SEFAZ próprio (sem dependência de bibliotecas externas), incluindo geração de XML v4.00, validação XSD local, assinatura digital ICP-Brasil, transmissão SOAP/HTTPS via mTLS e parsing do retorno SEFAZ.

#### Scenario: Emissão síncrona em homologação SP
- **WHEN** o faturamento de uma venda dispara a emissão da NF-e e o tenant tem ambiente="Homologação", UF="SP"
- **THEN** o sistema monta o XML conforme layout NF-e v4.00
- **THEN** valida o XML contra o XSD oficial embutido
- **THEN** assina digitalmente com `XmlDsigExcC14NTransform` usando o certificado A1 do tenant
- **THEN** transmite ao endpoint real `https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeautorizacao4.asmx`
- **THEN** parseia o retorno SOAP e extrai `protNFe.infProt.cStat`
- **THEN** se cStat=100 (Autorizado), armazena XML autorizado + protocolo

#### Scenario: Emissão em produção real
- **WHEN** o tenant tem ambiente="Produção" e o cliente SEFAZ está implementado
- **THEN** o sistema transmite ao endpoint produção da UF
- **THEN** retorno cStat=100 indica autorização real
- **THEN** NF-e fica disponível para download no portal da SEFAZ

#### Scenario: Rejeição com cStat de negócio
- **WHEN** a SEFAZ retorna cStat=204 (Duplicidade), 539 (Duplicidade de NFe) ou similar
- **THEN** o sistema mapeia para enum `SefazResultadoCodigo` e armazena
- **THEN** a NFe local fica com status correspondente, sem retry automático

### Requirement: Geração da Chave de Acesso
O sistema SHALL gerar a chave de acesso de 44 dígitos conforme manual SEFAZ: cUF(2) + AAMM(4) + CNPJ(14) + mod(2) + serie(3) + nNF(9) + tpEmis(1) + cNF(8) + cDV(1).

#### Scenario: Cálculo de DV mod 11
- **WHEN** o sistema gera a chave para uma nova NF-e
- **THEN** o 44º dígito é calculado pelo algoritmo mod 11 com pesos 2..9 cíclicos
- **THEN** o resultado é igual ao campo `cDV` no XML

### Requirement: Validação XSD Local
O sistema SHALL validar todo XML NF-e contra os schemas oficiais embutidos antes de transmitir, retornando erros estruturados (linha, coluna, mensagem) sem chamar a SEFAZ.

#### Scenario: NFe com campo obrigatório faltando
- **WHEN** o XML é montado sem o campo obrigatório CFOP
- **THEN** a validação XSD local falha
- **THEN** o erro retornado indica o caminho exato (`det/prod/CFOP`) e a mensagem do schema
- **THEN** a NFe não é transmitida à SEFAZ (poupa rejeição remota)

### Requirement: Assinatura Digital ICP-Brasil
O sistema SHALL assinar o XML com XMLDSig + canonicalização C14N exclusive (`http://www.w3.org/2001/10/xml-exc-c14n#`) usando certificado A1 (PFX) ou A3 (PKCS#11).

#### Scenario: Assinatura com A1
- **WHEN** o tenant configura certificado A1 (PFX) com senha
- **THEN** o sistema descriptografa a senha (via IDataProtector com chave em vault)
- **THEN** carrega o `X509Certificate2` em cache (TTL = vencimento - 1 dia)
- **THEN** assina o `infNFe` com Reference URI = #NFe<chave>
- **THEN** o XML resultante valida contra `xmlsec1` externo

#### Scenario: Assinatura com A3 (token físico)
- **WHEN** o tenant configura A3 e o token está conectado ao servidor
- **THEN** o sistema usa `IPkcs11Provider` para assinar via driver do fabricante
- **THEN** o resultado é equivalente ao A1 (mesmo XMLDSig output)

#### Scenario: Cert expirado ou inválido
- **WHEN** o cert do tenant está expirado ou fora da cadeia ICP-Brasil
- **THEN** o sistema retorna erro antes de transmitir
- **THEN** o worker `CertificadoVencimentoVarreduraWorker` já alertou previamente

### Requirement: Numeração Sequencial sem Pulo
O sistema SHALL garantir numeração estritamente sequencial por (tenant, CNPJ, série), com lock pessimista para evitar race condition em emissões paralelas.

#### Scenario: Duas faturas paralelas pedem número
- **WHEN** dois faturamentos disparam emissão simultânea
- **THEN** o sistema obtém os números via `SELECT ... FOR UPDATE` na tabela `nfe_numeracao`
- **THEN** os números são consecutivos (N e N+1), nenhum pulo
- **THEN** ambas as NFes são autorizadas com numeração correta

#### Scenario: Inutilização de faixa
- **WHEN** o admin solicita inutilização da faixa 100-105 da série 1
- **THEN** o sistema chama `NFeInutilizacao4` na SEFAZ
- **THEN** ao receber sucesso, marca a faixa como inutilizada
- **THEN** próxima NFe usa número 106

### Requirement: Contingência SVRS Automática
O sistema SHALL detectar indisponibilidade da SEFAZ-Origem (timeout > 30s ou cStat=108) e automaticamente rotear para SVRS com `tpEmis=6`, retornando à origem quando ela voltar.

#### Scenario: SEFAZ-Origem cai durante emissão
- **WHEN** uma transmissão para SEFAZ-SP retorna timeout
- **THEN** a `ContingenciaPolicy` marca SP como indisponível por 5 min
- **THEN** a próxima emissão vai para SVRS-RS com `tpEmis=6`
- **THEN** a NFe é autorizada via SVRS

#### Scenario: SEFAZ-Origem volta
- **WHEN** o worker `SefazStatusWorker` detecta `cStat=107` (operando) na origem
- **THEN** a contingência é desativada
- **THEN** próximas emissões voltam para a SEFAZ-Origem

### Requirement: Reprocessamento de NFes Pendentes
O sistema SHALL ter worker que reconcilia NFes em status `EmContingencia` ou `EnviadaSemRetorno`, consultando-as via `NFeConsultaProtocolo4` e atualizando status.

#### Scenario: NFe ficou sem retorno por queda de rede
- **WHEN** uma NFe foi transmitida mas o cliente perdeu o retorno (rede caiu)
- **THEN** o worker `NFePendenteReprocessadorWorker` consulta a chave na SEFAZ
- **THEN** se autorizada, atualiza status local + baixa o protocolo
- **THEN** se não autorizada, mantém status pendente para próxima tentativa

### Requirement: Suporte às UFs Prioritárias
O sistema SHALL suportar emissão e eventos contra os webservices SEFAZ das 5 UFs prioritárias: SP, RJ, MG, RS, PR. Outras UFs SHALL ser adicionadas conforme demanda, via configuração no catálogo de URLs.

#### Scenario: Emissão em UF não suportada
- **WHEN** um tenant tenta emitir em UF não cadastrada no catálogo
- **THEN** o sistema retorna erro claro indicando UFs disponíveis
- **THEN** admin pode adicionar a UF via override de config sem mudar código

## REMOVED Requirements

### Requirement: Cliente SEFAZ Stub (StubNFeSefazClient)
**Reason**: Stub explícito que retornava `cStat=100` fake em homologação e bloqueava produção. Substituído pelo cliente real `RealNFeSefazClient` desta change.
**Migration**: O stub é movido para projeto de tests (uso só em unit tests com mock); DI de produção troca registro automaticamente. Tenants que estavam validando contra stub precisarão refazer testes contra ambiente homolog SEFAZ real.
