using Acme.Sistemas.Domain.Interfaces.Rh;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Validação da invariância "1000 chamadas concorrentes → 1000 NSRs únicos contíguos".
/// O teste exercita um shim in-memory que reproduz a semântica atômica do
/// <c>NumeradorNsr</c> (idiom INSERT … ON DUPLICATE KEY UPDATE LAST_INSERT_ID(col+1)),
/// usando <c>Interlocked.Increment</c> — a impl MySQL real é coberta por integration
/// tests com banco real (Fase 9).
/// </summary>
public class NumeradorNsrConcorrenciaTests
{
    private sealed class NumeradorNsrInMemory : INumeradorNsr
    {
        private readonly Dictionary<Guid, long> _por = new();
        private readonly object _gate = new();

        public Task<long> ProximoAsync(Guid empresaId, CancellationToken cancellationToken = default)
        {
            lock (_gate)
            {
                _por.TryGetValue(empresaId, out var atual);
                var prox = atual + 1;
                _por[empresaId] = prox;
                return Task.FromResult(prox);
            }
        }

        public Task<long> UltimoAsync(Guid empresaId, CancellationToken cancellationToken = default)
        {
            lock (_gate) { return Task.FromResult(_por.GetValueOrDefault(empresaId, 0L)); }
        }
    }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "NumeradorNsr")]
    [Fact(DisplayName = "Dado 1000 chamadas concorrentes, quando ProximoAsync, então retorna 1000 NSRs únicos contíguos 1..1000")]
    public async Task MilChamadasConcorrentes_RetornaSequenciaCompleta()
    {
        INumeradorNsr numerador = new NumeradorNsrInMemory();
        var empresaId = Guid.NewGuid();

        var tarefas = Enumerable.Range(0, 1000)
            .Select(_ => numerador.ProximoAsync(empresaId))
            .ToArray();
        var resultados = await Task.WhenAll(tarefas);

        resultados.Should().HaveCount(1000);
        resultados.Distinct().Should().HaveCount(1000, "todos NSRs devem ser únicos");
        resultados.Min().Should().Be(1);
        resultados.Max().Should().Be(1000);
        var ordenados = resultados.OrderBy(x => x).ToArray();
        for (var i = 0; i < ordenados.Length - 1; i++)
            (ordenados[i + 1] - ordenados[i]).Should().Be(1, $"sem gaps; índice {i}");
    }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "NumeradorNsr")]
    [Fact(DisplayName = "Dado duas empresas, quando ProximoAsync, então sequências são independentes")]
    public async Task DuasEmpresas_SequenciasIndependentes()
    {
        INumeradorNsr numerador = new NumeradorNsrInMemory();
        var empA = Guid.NewGuid();
        var empB = Guid.NewGuid();

        (await numerador.ProximoAsync(empA)).Should().Be(1);
        (await numerador.ProximoAsync(empA)).Should().Be(2);
        (await numerador.ProximoAsync(empB)).Should().Be(1);
        (await numerador.ProximoAsync(empA)).Should().Be(3);
        (await numerador.ProximoAsync(empB)).Should().Be(2);
        (await numerador.UltimoAsync(empA)).Should().Be(3);
        (await numerador.UltimoAsync(empB)).Should().Be(2);
    }
}
