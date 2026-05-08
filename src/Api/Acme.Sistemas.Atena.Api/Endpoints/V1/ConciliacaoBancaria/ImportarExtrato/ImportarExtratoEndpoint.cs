using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.ConciliacaoBancaria.Command.ImportarExtrato;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ConciliacaoBancaria.ImportarExtrato;
public sealed class ImportarExtratoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/conciliacao-bancaria/importar", async (
            HttpRequest http,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (!http.HasFormContentType)
                return Results.BadRequest(new { message = "Use multipart/form-data." });

            var form = await http.ReadFormAsync(cancellationToken);
            var banco = form["banco"].ToString();
            var agencia = form["agencia"].ToString();
            var conta = form["conta"].ToString();
            var formato = form["formato"].ToString();
            var file = form.Files.FirstOrDefault(f => f.Name == "arquivo") ?? form.Files.FirstOrDefault();
            if (file is null || file.Length == 0)
                return Results.BadRequest(new { message = "Arquivo obrigatório (campo 'arquivo')." });

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, cancellationToken);

            var cmd = new ImportarExtratoCommand(
                banco,
                string.IsNullOrWhiteSpace(agencia) ? null : agencia,
                string.IsNullOrWhiteSpace(conta) ? null : conta,
                string.IsNullOrWhiteSpace(formato) ? "CSV" : formato,
                ms.ToArray());

            var response = await mediator.Send(cmd, cancellationToken);
            return response.IsSuccess
                ? Results.Created($"/api/v1/conciliacao-bancaria/{response.Content!.ConciliacaoId}", response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("ConciliacaoBancaria")
        .WithName("ImportarExtrato")
        .DisableAntiforgery()
        .Produces<ImportarExtratoCommandResult>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
