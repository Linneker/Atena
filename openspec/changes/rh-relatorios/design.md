# Design — rh-relatorios

## Catálogo dos 20 relatórios

| # | Nome | Categoria | Formatos | Async? |
|---|------|-----------|----------|--------|
| 1 | Espelho de ponto mensal | Operacional | PDF | Não (1 func) / Sim (massa) |
| 2 | Holerite individual | Operacional | PDF | Não |
| 3 | Folha analítica | Operacional | PDF, CSV, XLSX | Sim |
| 4 | Folha sintética | Operacional | PDF, CSV, XLSX | Não |
| 5 | Banco de horas | Operacional | PDF, XLSX | Não |
| 6 | Admissões/demissões do período | Operacional | PDF, CSV | Não |
| 7 | Recibo de férias | Operacional | PDF | Não |
| 8 | Recibo de 13º | Operacional | PDF | Não |
| 9 | TRCT | Operacional | PDF | Não |
| 10 | Comprovante anual de rendimentos | Legal | PDF | Sim |
| 11 | Resumo anual horas trabalhadas | Legal | PDF, XLSX | Sim |
| 12 | GPS detalhada | Legal | PDF, CSV | Não |
| 13 | DARF IRRF detalhada | Legal | PDF, CSV | Não |
| 14 | GRF FGTS detalhada | Legal | PDF, CSV | Não |
| 15 | Conferência folha × eSocial | Legal | PDF, CSV | Sim |
| 16 | Headcount por dept/CC/lotação | Gerencial | PDF, XLSX | Não |
| 17 | Turnover do período | Gerencial | PDF, CSV | Não |
| 18 | Custo total RH | Gerencial | PDF, XLSX | Não |
| 19 | Aniversariantes do mês | Gerencial | PDF | Não |
| 20 | Calendário de férias/afastamentos | Gerencial | PDF, ICS | Não |

## Estrutura por relatório

```
Acme.Sistemas.Services/V1/Rh/Relatorios/
├── EspelhoPontoMensal/
│   ├── EspelhoPontoMensalQuery.cs
│   ├── EspelhoPontoMensalQueryHandler.cs    (extrai dados)
│   ├── EspelhoPontoMensalQueryResult.cs
│   ├── EspelhoPontoMensalQueryValidation.cs
│   ├── EspelhoPontoMensalQueryBehavior.cs    (cache)
│   ├── Renderer/
│   │   ├── EspelhoPontoMensalPdfRenderer.cs  (QuestPDF)
│   │   ├── EspelhoPontoMensalCsvRenderer.cs  (CsvHelper)
│   │   └── EspelhoPontoMensalXlsxRenderer.cs (ClosedXML)
│   └── EspelhoPontoMensalEndpoint.cs
├── FolhaAnalitica/
...
└── ComprovanteAnualRendimentos/
```

## Async com RabbitMQ

Relatórios marcados Async no catálogo:
1. Endpoint enfileira `GerarRelatorioMessage`.
2. Worker pega, executa Query+Renderer, upload S3.
3. Notifica usuário via bell + e-mail com link.

```csharp
public sealed class GeradorRelatorioWorker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await foreach (var msg in _consumer.ConsumeAsync<GerarRelatorioMessage>(ct))
        {
            var query = ResolveQuery(msg.TipoRelatorio, msg.Parametros);
            var resultado = await _mediator.Send(query);
            var bytes = ResolveRenderer(msg.TipoRelatorio, msg.Formato).Render(resultado);
            var url = await _s3.UploadAsync($"rh/relatorios/{msg.TenantId}/{msg.Id}.{msg.Formato}", bytes);
            await _notificacao.NotificarUsuarioAsync(msg.UsuarioId, $"Relatório pronto: {url}");
        }
    }
}
```

## Comprovante anual de rendimentos

```
Para ano fiscal X:
  Para cada funcionário ativo ou desligado em X:
    1. Soma rendimentos tributáveis × mês (de HoleriteFuncionario)
    2. Soma IRRF retido × mês
    3. Soma INSS contribuição × mês
    4. Outras rubricas (pensão alimentícia, etc.)
    5. Renderiza PDF layout CGRT 1.215
    6. Upload S3
    7. Envia e-mail ao funcionário com link
```

Job agendado: 1ª semana de fevereiro do ano seguinte.

## Conferência folha × eSocial

```
Folha 2026-06: 100 holerites
eSocial S-1200 2026-06: 98 Aceitos + 2 Rejeitados
S-1210 2026-06: 95 Aceitos
S-1299 2026-06: Não enviado

Relatório:
  ├── 100 funcionários na folha
  ├── 98 S-1200 Aceitos (✓)
  ├── 2 S-1200 Rejeitados (motivo + ação sugerida) (✗)
  ├── 95 S-1210 Aceitos (pendentes pagamento × pagos)
  ├── 5 S-1210 não emitidos (pagamento ainda não realizado)
  └── S-1299: NÃO ENVIADO → competência não fechada
```

## Cache

Queries pesadas (folha analítica de muitos meses) → cache Redis.

```csharp
public sealed class FolhaAnaliticaQueryBehavior : IPipelineBehavior<FolhaAnaliticaQuery, ResponseDefault<FolhaAnaliticaResult>>
{
    public async Task<ResponseDefault<FolhaAnaliticaResult>> Handle(...)
    {
        var key = $"rel:folha-analitica:{tenantId}:{competencia}";
        var cached = await _cache.GetAsync<FolhaAnaliticaResult>(key);
        if (cached != null) return ResponseDefault.Ok(cached);

        var result = await next();
        await _cache.SetAsync(key, result.Data, TimeSpan.FromHours(1));
        return result;
    }
}
```

Invalidação: FolhaMensal.Reaberta limpa cache desse competência.

## Agendamento

Tabela `agendamentos_relatorios`:
```
tenant_id, usuario_id, tipo_relatorio, parametros_json, periodicidade ('Mensal', 'Diaria'), proxima_execucao, ativo
```

Job hosted service roda hourly, dispara relatórios agendados, marca próxima execução.

## Test strategy

- Unit: cada query handler com fixture
- Unit: renderers (PDF/CSV/XLSX) determinísticos
- Integration: gerar cada um dos 20 relatórios com fixtures small
- Integration: agendamento dispara relatório semanal
- Performance: folha analítica 1000 funcs × 12 meses em < 30s (cache hit) e < 120s (cache miss)
