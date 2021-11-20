using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Enum
{
    public enum EnumResponseHttp
    {
        SUCESSO = 200,
        ERRO_INTERNO = 500,
        WARNING = 400,
        ERRO_SELECAO = 422
    }
}
