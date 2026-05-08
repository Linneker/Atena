using FluentValidation;

namespace Acme.Sistemas.Services.V1.Roles.Query.ListarRoles;

public sealed class ListarRolesQueryValidation : AbstractValidator<ListarRolesQuery>
{
    public ListarRolesQueryValidation() { /* sem regras */ }
}
