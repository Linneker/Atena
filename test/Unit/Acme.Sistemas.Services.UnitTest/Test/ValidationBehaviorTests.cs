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

    [Fact]
    public async Task SemValidadores_ChamaProximoSemBloqueio()
    {
        var sut = new ValidationBehavior<SampleRequest, string>(Array.Empty<IValidator<SampleRequest>>());
        var resultado = await sut.Handle(new SampleRequest("ok"), () => Task.FromResult("ok"), default);
        resultado.Should().Be("ok");
    }

    [Fact]
    public async Task RequestInvalido_LancaValidationException()
    {
        var sut = new ValidationBehavior<SampleRequest, string>(new[] { new SampleValidator() });
        Func<Task> act = () => sut.Handle(new SampleRequest(""), () => Task.FromResult("ok"), default);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task RequestValido_ChamaProximo()
    {
        var sut = new ValidationBehavior<SampleRequest, string>(new[] { new SampleValidator() });
        var resultado = await sut.Handle(new SampleRequest("Maria"), () => Task.FromResult("oi Maria"), default);
        resultado.Should().Be("oi Maria");
    }
}
