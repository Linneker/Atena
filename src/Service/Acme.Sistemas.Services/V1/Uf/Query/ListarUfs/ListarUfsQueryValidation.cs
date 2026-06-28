using FluentValidation;

namespace Acme.Sistemas.Services.V1.Uf.Query.ListarUfs;

public sealed class ListarUfsQueryValidation : AbstractValidator<ListarUfsQuery>
{
    public ListarUfsQueryValidation()
    {
    }
}
