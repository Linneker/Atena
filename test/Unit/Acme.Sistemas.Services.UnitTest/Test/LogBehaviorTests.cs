using Acme.Sistemas.Core.Mediators.Behaviors;
using Acme.Sistemas.Core.Mediators.Handler;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class LogBehaviorTests
{
    public sealed record SampleRequest(string Nome) : IRequest<string>;

    [Fact]
    public async Task Handle_DelegaParaProximoEDevolveResposta()
    {
        var sut = new LogBehavior<SampleRequest, string>(NullLogger<LogBehavior<SampleRequest, string>>.Instance);
        var resultado = await sut.Handle(new SampleRequest("x"), () => Task.FromResult("ok"), default);
        resultado.Should().Be("ok");
    }

    [Fact]
    public async Task Handle_PropagaExcecaoDoProximo()
    {
        var sut = new LogBehavior<SampleRequest, string>(NullLogger<LogBehavior<SampleRequest, string>>.Instance);
        Func<Task> act = () => sut.Handle(
            new SampleRequest("x"),
            () => Task.FromException<string>(new InvalidOperationException("boom")),
            default);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("boom");
    }
}
