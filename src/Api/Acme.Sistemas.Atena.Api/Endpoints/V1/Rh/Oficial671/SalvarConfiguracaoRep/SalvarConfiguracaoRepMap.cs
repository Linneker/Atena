using Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Command.SalvarConfiguracaoRep;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.SalvarConfiguracaoRep;

public static class SalvarConfiguracaoRepMap
{
    public static SalvarConfiguracaoRepCommand ToCommand(this SalvarConfiguracaoRepRequest r)
        => new(r.EmpresaId, r.Tipo, r.RazaoSocial, r.CnpjCei, r.Cno,
               r.InscricaoEstadual, r.CnaePrincipal,
               r.Endereco.Logradouro, r.Endereco.Numero, r.Endereco.Complemento,
               r.Endereco.Bairro, r.Endereco.Cidade, r.Endereco.Uf, r.Endereco.Cep,
               r.CertificadoId, r.ResponsavelCpf, r.ResponsavelNome);

    public static SalvarConfiguracaoRepResponse ToResponse(this SalvarConfiguracaoRepCommandResult r)
        => new(r.ConfiguracaoId, r.Criada);
}
