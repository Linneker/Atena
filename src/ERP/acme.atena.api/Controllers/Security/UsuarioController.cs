using AutoMapper;
using acme.atena.api.ViewModel.Security;
using acme.atena.application.Interface.Security;
using acme.atena.domain.DTO.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;

namespace acme.atena.api.Controllers.Security
{
    [EnableQuery]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseApiController<Usuario, UsuarioViewModel>
    {
        private IMapper _mapper;
        private IUsuarioApplication _usuarioApplication;
        private const string NOME_SERVICO = "USUARIO";
        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(IMapper mapper, IUsuarioApplication usuarioApplication,
            ILogger<UsuarioController> logger) : base(mapper, usuarioApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _usuarioApplication = usuarioApplication;
            _logger = logger;
        }
        
        [HttpPost("Login")]
        public IActionResult Login(UsuarioViewModel usuario)
        {
            var usuarioRetorno = _mapper.Map<UsuarioViewModel>(_usuarioApplication.Login(usuario.Login, usuario.Senha));
            if(usuarioRetorno is null)
            {
                return BadRequest(new { Mensagem = "Login/Senha Invalidos!", Descricao="Verifique os dados informados, e tente novamente!"});
            }
                return Ok(usuarioRetorno);
        }
        [Authorize("Bearer")]
        [HttpGet("RecuperaUsuariosComPermissaoAssync")]
        public Task<IQueryable<Usuario>> RecuperaUsuariosComPermissaoAssync()
        {
            var usuarioRetorno = _usuarioApplication.RecuperaUsuariosComPermissaoAssync();
            return usuarioRetorno;
        }
    }
}
