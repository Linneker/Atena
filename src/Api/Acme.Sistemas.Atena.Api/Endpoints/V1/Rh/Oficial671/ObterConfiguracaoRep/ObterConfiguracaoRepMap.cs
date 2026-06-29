using Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ObterConfiguracaoRep;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ObterConfiguracaoRep;

public static class ObterConfiguracaoRepMap
{
    public static ObterConfiguracaoRepQuery ToQuery(this ObterConfiguracaoRepRequest r)
        => new(r.EmpresaId);

    public static ObterConfiguracaoRepResponse ToResponse(this ObterConfiguracaoRepQueryResult r)
        => new(r.Id, r.EmpresaId, r.Tipo, r.RazaoSocial, r.CnpjCei, r.Cno,
               r.InscricaoEstadual, r.CnaePrincipal,
               new EnderecoRepOutput(r.Endereco.Logradouro, r.Endereco.Numero, r.Endereco.Complemento,
                   r.Endereco.Bairro, r.Endereco.Cidade, r.Endereco.Uf, r.Endereco.Cep),
               r.CertificadoId, r.ResponsavelCpf, r.ResponsavelNome);
}
