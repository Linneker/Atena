using imagem.bar.francisco.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.application.Interface.Security
{
    public interface IUsuarioApplication: IApplicationBase<Usuario>
    {
        Usuario Login(string usuario, string senha);
    }
}
