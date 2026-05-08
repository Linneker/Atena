using FluentValidation;

namespace Acme.Sistemas.Services.V1.Relatorios.PosicaoEstoque;

public sealed class PosicaoEstoqueQueryValidation : AbstractValidator<PosicaoEstoqueQuery>
{
    public PosicaoEstoqueQueryValidation() { /* sem regras */ }
}
