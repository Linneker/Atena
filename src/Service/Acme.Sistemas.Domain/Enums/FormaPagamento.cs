namespace Acme.Sistemas.Domain.Enums;

public enum FormaPagamento
{
    Dinheiro = 0,
    Pix = 1,
    CartaoCredito = 2,
    CartaoDebito = 3,
    Boleto = 4,
    Transferencia = 5,
    Cheque = 6,
    Outro = 99
}
