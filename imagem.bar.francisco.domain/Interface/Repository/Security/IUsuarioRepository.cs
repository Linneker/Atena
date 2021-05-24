using imagem.bar.francisco.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.domain.Interface.Repository.Security
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Usuario Login(Usuario usuario);
    }
}
