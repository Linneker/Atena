using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Aej;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.DownloadAej;

/// <summary>
/// Download do AEJ JSON. Regenera determinístico (mesmo bytes, mesmo hash) a partir
/// dos comprovantes do período persistido. JWS detached é separado: trocar `?formato=jws`.
/// </summary>
public sealed class DownloadAejEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/671/aej/{exportacaoId:guid}/download", async (
            Guid exportacaoId, string? formato,
            IExportacaoAejRepository expRepo,
            IComprovantePontoRepository comprovantes,
            IEmpresaRepository empresas,
            IConfiguracaoRepRepository config,
            IMarcacaoPontoRepository marcacoes,
            IFuncionarioRepository funcionarios,
            GeradorAejV1 gerador, AssinadorAej assinador,
            Acme.Sistemas.ExternalIntegration.Sefaz.Certificado.CertificadoTenantResolver certResolver,
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
            var funcList = new List<Acme.Sistemas.Domain.Entities.Cadastros.Funcionario>();
            foreach (var fid in marcList.Select(m => m.FuncionarioId).Distinct())
            {
                var f = await funcionarios.GetByIdAsync(fid, cancellationToken);
                if (f is not null) funcList.Add(f);
            }

            var ctx = new AejContexto(emp, cfg, inicio, fim, exp.GeradoEm ?? DateTime.UtcNow,
                marcList, compList, funcList);
            var bytes = gerador.Gerar(ctx);

            if (string.Equals(formato, "jws", StringComparison.OrdinalIgnoreCase))
            {
                var cert = await certResolver.GetAsync(cancellationToken);
                var sig = assinador.AssinarDetached(bytes, cert);
                return Results.Text(sig, "application/jose", System.Text.Encoding.UTF8);
            }
            var fileName = $"aej-{exp.PeriodoInicio:yyyyMMdd}-{exp.PeriodoFim:yyyyMMdd}.json";
            return Results.File(bytes, "application/json", fileName);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPontoOficial, Permissions.Acoes.ExportarAej))
        .WithTags("RH - Ponto Oficial 671")
        .WithName("DownloadAej")
        .Produces(200, contentType: "application/json")
        .ProducesProblem(404);
    }
}
