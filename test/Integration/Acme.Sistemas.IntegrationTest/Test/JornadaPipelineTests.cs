using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.IntegrationTest.Config;
using Acme.Sistemas.Services.V1.Rh.Jornada.Command.AlterarJornada;
using Acme.Sistemas.Services.V1.Rh.Jornada.Command.CriarJornada;
using Acme.Sistemas.Services.V1.Rh.Jornada.Command.RemoverJornada;
using Acme.Sistemas.Services.V1.Rh.Jornada.Query.ListarJornadas;
using Acme.Sistemas.Services.V1.Rh.Jornada.Query.ObterJornada;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// E2E do vertical Jornada: dispara cada um dos 5 commands/queries via
/// <see cref="IMediator"/> contra o pipeline real (Validation + Audit + Log + Behavior +
/// Handler + repositório MySQL). Não usa HTTP/auth — esses são cobertos pelo
/// <c>EndpointConventionTests</c> (forma) e por <c>IsolamentoCrossTenantTests</c>
/// (auth gating em outras rotas).
/// </summary>
public class JornadaPipelineTests : IntegrationTestBase
{
    public JornadaPipelineTests(DockerEnvironment docker) : base(docker) { }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "JornadaPipelineCrud")]
    [SkippableFact(DisplayName = "Dado tenant novo, quando executa CRUD completo via pipeline (criar→obter→listar→alterar→remover), então fluxo end-to-end retorna sucesso em cada etapa")]
    public async Task Pipeline_CrudCompletoDeJornada_FunctionaEndToEnd()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var mediator = sp.GetRequiredService<IMediator>();

        // 1. CRIAR — payload válido cria jornada e retorna 201 + Id
        var criar = await mediator.Send(new CriarJornadaCommand(
            Nome: "12x36 Teste",
            Tipo: TipoJornada.Escala12x36,
            CargaSemanalHoras: 42m,
            CargaDiariaHoras: 12m,
            JanelasJson: "[{\"dia\":\"seg\",\"entrada\":\"07:00\",\"saida\":\"19:00\"}]",
            PermiteMarcarIntervalo: false,
            ToleranciaMinutos: 15));

        criar.IsSuccess.Should().BeTrue();
        criar.Status.Should().Be(201);
        var id = criar.Content!.Id;

        // 2. OBTER — retorna a jornada criada com todos os campos
        var obter = await mediator.Send(new ObterJornadaQuery(id));
        obter.IsSuccess.Should().BeTrue();
        obter.Content!.Tipo.Should().Be(TipoJornada.Escala12x36);
        obter.Content.CargaSemanalHoras.Should().Be(42m);
        obter.Content.ToleranciaMinutos.Should().Be(15);
        obter.Content.JanelasJson.Should().Contain("07:00");

        // 3. LISTAR — inclui a jornada recém-criada
        var listar = await mediator.Send(new ListarJornadasQuery());
        listar.IsSuccess.Should().BeTrue();
        listar.Content!.Items.Should().ContainSingle(i => i.Id == id);

        // 4. ALTERAR — modifica campos editáveis
        var alterar = await mediator.Send(new AlterarJornadaCommand(
            id, "12x36 Renomeada", TipoJornada.Escala12x36, 42m, 12m,
            "[{\"dia\":\"qui\",\"entrada\":\"06:00\",\"saida\":\"18:00\"}]",
            true, 25, true));

        alterar.IsSuccess.Should().BeTrue();
        alterar.Status.Should().Be(200);

        var obterPosAlter = await mediator.Send(new ObterJornadaQuery(id));
        obterPosAlter.Content!.Nome.Should().Be("12x36 Renomeada");
        obterPosAlter.Content.ToleranciaMinutos.Should().Be(25);
        obterPosAlter.Content.PermiteMarcarIntervalo.Should().BeTrue();

        // 5. REMOVER — soft delete
        var remover = await mediator.Send(new RemoverJornadaCommand(id));
        remover.IsSuccess.Should().BeTrue();

        var obterPosRemover = await mediator.Send(new ObterJornadaQuery(id));
        obterPosRemover.IsSuccess.Should().BeFalse();
        obterPosRemover.Status.Should().Be(404, "soft delete deve esconder de GetByIdAsync");
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "JornadaPipelineErros")]
    [SkippableFact(DisplayName = "Dado JanelasJson inválido, quando pipeline executa CriarJornada, então ValidationBehavior intercepta com erro estruturado antes do handler")]
    public async Task Pipeline_JanelasJsonInvalido_ValidationBehaviorIntercepta()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var mediator = sp.GetRequiredService<IMediator>();

        var act = () => mediator.Send(new CriarJornadaCommand(
            "JSON Bad", TipoJornada.Fixa, 44m, 8m, "{ nao eh json válido"));

        // ValidationBehavior lança FluentValidation.ValidationException antes do handler.
        var ex = (await act.Should().ThrowAsync<FluentValidation.ValidationException>()).Which;
        ex.Errors.Should().Contain(e => e.PropertyName == "JanelasJson");
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "JornadaPipelineErros")]
    [SkippableFact(DisplayName = "Dado nome já existente, quando pipeline executa CriarJornada duas vezes, então segunda retorna 409 Conflict")]
    public async Task Pipeline_NomeDuplicado_RetornaConflict()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var mediator = sp.GetRequiredService<IMediator>();
        var cmd = new CriarJornadaCommand("Unica", TipoJornada.Fixa, 44m, 8m, "[]");

        var primeira = await mediator.Send(cmd);
        primeira.IsSuccess.Should().BeTrue();

        var segunda = await mediator.Send(cmd);
        segunda.IsSuccess.Should().BeFalse();
        segunda.Status.Should().Be(409);
        segunda.Message.Should().Contain("Unica");
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "JornadaPipelineErros")]
    [SkippableFact(DisplayName = "Dado ID que não existe, quando pipeline executa AlterarJornada, então retorna 404 mesmo com payload válido")]
    public async Task Pipeline_AlterarIdInexistente_Retorna404()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var mediator = sp.GetRequiredService<IMediator>();
        var result = await mediator.Send(new AlterarJornadaCommand(
            Guid.NewGuid(), "X", TipoJornada.Fixa, 44m, 8m, "[]", true, 10, true));

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(404);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "JornadaPipelineCrud")]
    [SkippableFact(DisplayName = "Dado jornadas em dois tenants distintos, quando lista pelo Tenant A, então só recebe as do A (BaseRepository filtra)")]
    public async Task Pipeline_Isolamento_ListarJornadasNaoVazaEntreTenants()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();

        Guid idA;
        using (var scopeA = Factory.Services.CreateScope())
        {
            await PrepararTenantTeste(scopeA.ServiceProvider);
            var mediatorA = scopeA.ServiceProvider.GetRequiredService<IMediator>();
            var criar = await mediatorA.Send(new CriarJornadaCommand(
                "Jornada Tenant A", TipoJornada.Fixa, 44m, 8m, "[]"));
            idA = criar.Content!.Id;
        }

        using (var scopeB = Factory.Services.CreateScope())
        {
            await PrepararTenantTeste(scopeB.ServiceProvider);
            var mediatorB = scopeB.ServiceProvider.GetRequiredService<IMediator>();
            var listar = await mediatorB.Send(new ListarJornadasQuery());

            listar.IsSuccess.Should().BeTrue();
            listar.Content!.Items.Should().NotContain(i => i.Id == idA,
                "BaseRepository filtra por TenantContext.TenantId — jornada do Tenant A NUNCA pode vazar para Tenant B");
        }
    }

    private static async Task<Guid> PrepararTenantTeste(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<Acme.Sistemas.Infrastructure.Databases.Configuration.IDataConfiguration>();
        var tenantId = Guid.NewGuid();
        var cnpj = Guid.NewGuid().ToString("N")[..14];
        await db.ExecuteAsync(@"
            INSERT INTO tenants (id, razao_social, cnpj, plano, status, created_at)
            VALUES (@id, @razao, @cnpj, 'FREE', 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@id"] = tenantId.ToString(),
                ["@razao"] = "Tenant Pipeline " + cnpj[..6],
                ["@cnpj"] = cnpj
            });
        sp.GetRequiredService<IMutableTenantContext>().Override(tenantId);
        return tenantId;
    }
}
