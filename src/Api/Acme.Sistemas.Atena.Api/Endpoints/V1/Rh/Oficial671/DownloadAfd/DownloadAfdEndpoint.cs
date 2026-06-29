using System.Text;
using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Afd;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.DownloadAfd;

/// <summary>
/// Download do AFD. Como o storage S3 real ainda é stub, este endpoint regenera o conteúdo
/// on-the-fly a partir dos comprovantes do período persistido — determinístico e idempotente
/// (mesmo hash do registro de exportação). Substituir por fetch S3 quando upload entrar.
/// </summary>
public sealed class DownloadAfdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/671/afd/{exportacaoId:guid}/download", async (
            Guid exportacaoId,
            IExportacaoAfdRepository expRepo,
            IComprovantePontoRepository comprovantes,
            Acme.Sistemas.Domain.Interfaces.Repository.IEmpresaRepository empresas,
            IConfiguracaoRepRepository config,
            IMarcacaoPontoRepository marcacoes,
            Acme.Sistemas.Domain.Interfaces.Repository.IFuncionarioRepository funcionarios,
            LayoutAfd003Writer writer,
            CancellationToken cancellationToken) =>
        {
            var exp = await expRepo.GetByIdAsync(exportacaoId, cancellationToken);
            if (exp is null) return Results.NotFound();

            var emp = await empresas.GetByIdAsync(exp.EmpresaId, cancellationToken);
            if (emp is null) return Results.NotFound();
            var cfg = await config.GetByEmpresaAsync(exp.EmpresaId, cancellationToken);
            if (cfg is null) return Results.Problem("Config REP ausente.");

            var inicio = exp.PeriodoInicio.ToDateTime(TimeOnly.MinValue);
            var fim = exp.PeriodoFim.AddDays(1).ToDateTime(TimeOnly.MinValue);

            var compList = await comprovantes.ListByEmpresaPeriodoAsync(exp.EmpresaId, inicio, fim, cancellationToken);
            var marcacaoIds = compList.Select(c => c.MarcacaoId).ToHashSet();
            var marcList = new List<Acme.Sistemas.Domain.Entities.Rh.MarcacaoPonto>();
            foreach (var mid in marcacaoIds)
            {
                var m = await marcacoes.GetByIdAsync(mid, cancellationToken);
                if (m is not null) marcList.Add(m);
            }
            var funcIds = marcList.Select(m => m.FuncionarioId).Distinct();
            var funcList = new List<Acme.Sistemas.Domain.Entities.Cadastros.Funcionario>();
            foreach (var fid in funcIds)
            {
                var f = await funcionarios.GetByIdAsync(fid, cancellationToken);
                if (f is not null) funcList.Add(f);
            }

            var ctx = new AfdContexto(emp, cfg, inicio, fim,
                exp.GeradoEm ?? DateTime.UtcNow,
                marcList, compList, funcList);
            var afd = writer.Escrever(ctx);

            var fileName = $"afd-{exp.PeriodoInicio:yyyyMMdd}-{exp.PeriodoFim:yyyyMMdd}.txt";
            return Results.File(afd.Conteudo, "text/plain; charset=utf-8", fileName);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPontoOficial, Permissions.Acoes.ExportarAfd))
        .WithTags("RH - Ponto Oficial 671")
        .WithName("DownloadAfd")
        .Produces(200, contentType: "text/plain")
        .ProducesProblem(404);
    }
}
