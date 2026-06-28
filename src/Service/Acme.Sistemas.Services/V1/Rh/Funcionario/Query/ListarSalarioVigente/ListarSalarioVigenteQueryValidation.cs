using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ListarSalarioVigente;

public sealed class ListarSalarioVigenteQueryValidation : AbstractValidator<ListarSalarioVigenteQuery>
{
    public ListarSalarioVigenteQueryValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
    }
}
