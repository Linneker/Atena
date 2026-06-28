using System.Security.Cryptography;
using System.Text;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

/// <summary>
/// Hash-chain de integridade das marcações de ponto. Cada marcação é encadeada à
/// anterior do mesmo funcionário, formando uma cadeia detectável de adulteração.
/// Não substitui assinatura ICP-Brasil (W4); é mecanismo interno de detecção.
/// </summary>
public static class MarcacaoPontoIntegridade
{
    /// <summary>
    /// Calcula SHA-256 hex do payload canônico da marcação encadeado ao hash anterior.
    /// Formato canônico: "funcionarioId|dataHora(ISO 8601)|tipo|origem|hashAnterior".
    /// </summary>
    public static string Calcular(
        Guid funcionarioId,
        DateTime dataHora,
        TipoMarcacao tipo,
        OrigemMarcacao origem,
        string? hashAnterior)
    {
        var payload = string.Join("|", new[]
        {
            funcionarioId.ToString("D"),
            dataHora.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            tipo.ToString(),
            origem.ToString(),
            hashAnterior ?? string.Empty,
        });

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    /// <summary>
    /// Verifica se a cadeia de marcações está íntegra. Retorna a primeira marcação
    /// inválida (índice + hash esperado vs encontrado) ou null se OK.
    /// </summary>
    public static IntegridadeQuebra? VerificarCadeia(
        IReadOnlyList<(Guid Id, Guid FuncionarioId, DateTime DataHora, TipoMarcacao Tipo,
                       OrigemMarcacao Origem, string? HashAnterior, string HashIntegridade)> marcacoes)
    {
        string? hashAnteriorEsperado = null;
        for (var i = 0; i < marcacoes.Count; i++)
        {
            var m = marcacoes[i];
            if (m.HashAnterior != hashAnteriorEsperado)
                return new IntegridadeQuebra(i, m.Id, "hash_anterior_divergente",
                    hashAnteriorEsperado, m.HashAnterior);

            var hashEsperado = Calcular(m.FuncionarioId, m.DataHora, m.Tipo, m.Origem, hashAnteriorEsperado);
            if (!string.Equals(m.HashIntegridade, hashEsperado, StringComparison.OrdinalIgnoreCase))
                return new IntegridadeQuebra(i, m.Id, "hash_integridade_divergente",
                    hashEsperado, m.HashIntegridade);

            hashAnteriorEsperado = m.HashIntegridade;
        }
        return null;
    }
}

public sealed record IntegridadeQuebra(
    int Indice,
    Guid MarcacaoId,
    string TipoQuebra,
    string? HashEsperado,
    string? HashEncontrado);
