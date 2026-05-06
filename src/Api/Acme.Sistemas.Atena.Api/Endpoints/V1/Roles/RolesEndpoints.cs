using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Roles.Command.AtribuirPermissaoARole;
using Acme.Sistemas.Services.V1.Roles.Command.AtribuirRoleAUsuario;
using Acme.Sistemas.Services.V1.Roles.Command.CriarRole;
using Acme.Sistemas.Services.V1.Roles.Query.ListarPermissoes;
using Acme.Sistemas.Services.V1.Roles.Query.ListarRoles;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles;

public sealed record CriarRoleRequest(string Nome, string? Descricao, IReadOnlyList<string>? PermissoesCodigos);
public sealed record AtribuirPermissaoRequest(string PermissaoCodigo);
public sealed record AtribuirRoleUsuarioRequest(Guid UserId, DateTime? ExpiresAt);

public sealed class RolesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/roles").WithTags("Roles");

        group.MapPost("/", async (CriarRoleRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CriarRoleCommand(req.Nome, req.Descricao, req.PermissoesCodigos);
            var response = await mediator.Send(command, ct);
            return response.IsSuccess
                ? Results.Created($"/api/v1/roles/{response.Content!.Id}", response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Role, Permissions.Acoes.Criar))
        .WithName("CriarRole");

        group.MapGet("/", async (int? skip, int? take, IMediator mediator, CancellationToken ct) =>
        {
            var response = await mediator.Send(new ListarRolesQuery(skip ?? 0, take ?? 50), ct);
            return Results.Ok(response.Content);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Role, Permissions.Acoes.Ler))
        .WithName("ListarRoles");

        group.MapPost("/{id:guid}/permissoes", async (Guid id, AtribuirPermissaoRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var response = await mediator.Send(new AtribuirPermissaoARoleCommand(id, req.PermissaoCodigo), ct);
            return response.IsSuccess ? Results.NoContent() : Results.Json(response, statusCode: response.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Role, Permissions.Acoes.Editar))
        .WithName("AtribuirPermissaoARole");

        group.MapPost("/{id:guid}/usuarios", async (Guid id, AtribuirRoleUsuarioRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var response = await mediator.Send(new AtribuirRoleAUsuarioCommand(req.UserId, id, req.ExpiresAt), ct);
            return response.IsSuccess ? Results.NoContent() : Results.Json(response, statusCode: response.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Usuario, Permissions.Acoes.Editar))
        .WithName("AtribuirRoleAUsuario");

        app.MapGet("/api/v1/permissoes", async (IMediator mediator, CancellationToken ct) =>
        {
            var response = await mediator.Send(new ListarPermissoesQuery(), ct);
            return Results.Ok(response.Content);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Permissao, Permissions.Acoes.Ler))
        .WithTags("Roles")
        .WithName("ListarPermissoes");
    }
}
