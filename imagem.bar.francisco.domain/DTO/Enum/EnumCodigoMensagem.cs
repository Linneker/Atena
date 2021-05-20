using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.domain.DTO.Enum
{
    public enum EnumCodigoMensagem
    {
        SUCESSO = 200,

        ERRO_ADD = 10,
        ERRO_UPDATE = 11,
        ERRO_DELETE = 12,
        ERRO_COMMIT = 13,
        
        ERRO_ADD_ASYNC = 20,
        ERRO_UPDATE_ASYNC = 21,
        ERRO_DELETE_ASYNC = 22,
        ERRO_COMMIT_ASYNC = 23,

        ERRO_VALOR_ZERADO = 100,
        ERRO_VALOR_NEGATIVO = 101,
        
    }
}
