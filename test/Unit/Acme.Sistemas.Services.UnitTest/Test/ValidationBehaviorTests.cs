using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Behaviors;
using Acme.Sistemas.Core.Mediators.Handler;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class ValidationBehaviorTests
{
    public sealed record SampleRequest(string Nome) : IRequest<string>;

    public sealed class SampleValidator : AbstractValidator<SampleRequest>
    {
        public SampleValidator() { RuleFor(x => x.Nome).NotEmpty().MinimumLength(3); }
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "ValidationBehavior")]
    [Fact(DisplayName = "Dado nenhum validador registrado, quando o ValidationBehavior executa, então chama o próximo sem bloqueio")]
    public async Task SemValidadores_ChamaProximoSemBloqueio()
    {
        var sut = new ValidationBehavior<SampleRequest, string>(Array.Empty<IValidator<SampleRequest>>());
        var resultado = await sut.Handle(new SampleRequest("ok"), () => Task.FromResult("ok"), default);
        resultado.Should().Be("ok");
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "ValidationBehavior")]
    [Fact(DisplayName = "Dado request inválido, quando o ValidationBehavior executa, então lança ValidationException")]
    public async Task RequestInvalido_LancaValidationException()
    {
        var sut = new ValidationBehavior<SampleRequest, string>(new[] { new SampleValidator() });
        Func<Task> act = () => sut.Handle(new SampleRequest(""), () => Task.FromResult("ok"), default);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "ValidationBehavior")]
    [Fact(DisplayName = "Dado request válido, quando o ValidationBehavior executa, então chama o próximo")]
    public async Task RequestValido_ChamaProximo()
    {
        var sut = new ValidationBehavior<SampleRequest, string>(new[] { new SampleValidator() });
        var resultado = await sut.Handle(new SampleRequest("Maria"), () => Task.FromResult("oi Maria"), default);
        resultado.Should().Be("oi Maria");
    }
}
