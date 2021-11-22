using AutoMapper;
using acme.cashflow.api.ViewModel.Security;
using acme.cashflow.application.Interface.Security;
using acme.cashflow.domain.DTO.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.cashflow.api.Controllers.Security
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
