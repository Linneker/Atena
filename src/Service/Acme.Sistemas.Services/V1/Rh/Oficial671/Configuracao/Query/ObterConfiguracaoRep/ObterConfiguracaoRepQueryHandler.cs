using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ObterConfiguracaoRep;

public sealed class ObterConfiguracaoRepQueryHandler
    : IRequestHandler<ObterConfiguracaoRepQuery, ResponseDefault<ObterConfiguracaoRepQueryResult>>
{
    private readonly IConfiguracaoRepRepository _repo;

    public ObterConfiguracaoRepQueryHandler(IConfiguracaoRepRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ObterConfiguracaoRepQueryResult>> Handle(
        ObterConfiguracaoRepQuery q, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByEmpresaAsync(q.EmpresaId, cancellationToken);
        if (c is null)
            return ResponseDefault<ObterConfiguracaoRepQueryResult>.NotFound(
                "Configuração REP não cadastrada para esta empresa.");

        return ResponseDefault<ObterConfiguracaoRepQueryResult>.Ok(new ObterConfiguracaoRepQueryResult(
            c.Id, c.EmpresaId, c.Tipo, c.RazaoSocial, c.CnpjCei, c.Cno,
            c.InscricaoEstadual, c.CnaePrincipal,
            new EnderecoRepDto(c.EnderecoLogradouro, c.EnderecoNumero, c.EnderecoComplemento,
                c.EnderecoBairro, c.EnderecoCidade, c.EnderecoUf, c.EnderecoCep),
            c.CertificadoId, c.ResponsavelCpf, c.ResponsavelNome));
    }
}
