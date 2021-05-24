using AutoMapper;
using imagem.bar.francisco.api.ViewModel.Security;
using imagem.bar.francisco.application.Interface.Security;
using imagem.bar.francisco.domain.DTO.Seguranca;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseApiController<Usuario, UsuarioViewModel>
    {
        private IMapper _mapper;
        private IUsuarioApplication _usuarioApplication;
        private const string NOME_SERVICO = "USUARIO";
        public UsuarioController(IMapper mapper, IUsuarioApplication usuarioApplication) : base(mapper, usuarioApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _usuarioApplication = usuarioApplication;
        }

        [HttpPost("Login")]
        public UsuarioViewModel Login(UsuarioViewModel usuario)
        {
            var usuarioRetorno = _mapper.Map<UsuarioViewModel>(_usuarioApplication.Login(usuario.Login, usuario.Senha));
            return usuarioRetorno;
        }
    }
}
