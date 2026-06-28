using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.BaterMobile;

public sealed class BaterMobileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/bater-mobile", async (
            HttpRequest http,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (!http.HasFormContentType)
                return Results.BadRequest(new { error = "multipart/form-data esperado" });

            var form = await http.ReadFormAsync(cancellationToken);
            var tipo = form["tipo"].ToString();
            var deviceId = form["deviceId"].ToString();
            var hashBatida = form["hashBatida"].ToString();
            var provaBio = form["provaBiometriaLocal"].ToString();

            decimal? latitude = decimal.TryParse(form["latitude"].ToString(),
                System.Globalization.CultureInfo.InvariantCulture, out var lat) ? lat : null;
            decimal? longitude = decimal.TryParse(form["longitude"].ToString(),
                System.Globalization.CultureInfo.InvariantCulture, out var lng) ? lng : null;
            DateTime timestampLocal = DateTime.TryParse(form["timestampLocal"].ToString(),
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.RoundtripKind, out var ts) ? ts : DateTime.UtcNow;
            TipoMarcacao? tipoEnum = Enum.TryParse<TipoMarcacao>(tipo, out var t) ? t : null;

            byte[]? fotoBytes = null;
            string? fotoContentType = null;
            var fotoFile = form.Files.GetFile("foto");
            if (fotoFile is { Length: > 0 })
            {
                using var ms = new MemoryStream();
                await fotoFile.CopyToAsync(ms, cancellationToken);
                fotoBytes = ms.ToArray();
                fotoContentType = fotoFile.ContentType;
            }

            var req = new BaterMobileRequest(tipoEnum, latitude, longitude, deviceId,
                timestampLocal, hashBatida, string.IsNullOrEmpty(provaBio) ? null : provaBio);
            var result = await mediator.Send(req.ToCommand(fotoBytes, fotoContentType), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Created($"/api/v1/rh/ponto/proprio/{result.Content.Id}", result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.BaterPonto))
        .WithTags("RH - Ponto Mobile")
        .WithName("BaterPontoMobile")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<BaterMobileResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
