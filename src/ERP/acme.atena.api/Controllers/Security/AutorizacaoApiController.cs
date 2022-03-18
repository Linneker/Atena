using AutoMapper;
using acme.atena.api.ViewModel.Security;
using acme.atena.api.ViewModel.Util;
using acme.atena.domain.DTO.Seguranca;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using acme.atena.api.Configurations.Filtler;
using acme.atena.domain.Interface.Service.Security;

namespace acme.atena.api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacaoApiController : BaseApiController<AutorizacaoApi,AutorizacaoApiViewModel>
    {
        private readonly IAutorizacaoApiService _autorizacaoApiApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "AUTORIZAÇÃO API";

        public AutorizacaoApiController(IAutorizacaoApiService autorizacaoApiApplication, IMapper mapper):base(mapper, autorizacaoApiApplication, NOME_SERVICO)
        {
            _autorizacaoApiApplication = autorizacaoApiApplication;
            _mapper = mapper;
        }
        [UnitOfWorkFilter]
        [HttpPost("AddicionandoSemAutenticacao")]
        public void AddicionandoSemAutenticacao(AutorizacaoApiViewModel entity)
        {
            _autorizacaoApiApplication.Add(_mapper.Map<AutorizacaoApi>(entity));
        }

        [UnitOfWorkFilterAsync]
        [HttpPost("Add/Async")]
        public override void  AddAsync(AutorizacaoApiViewModel entity)
        {
            _autorizacaoApiApplication.AddAsync(_mapper.Map<AutorizacaoApi>(entity));
        }

        [HttpPost]
        public object Post(
      [FromBody] AutorizacaoApi usuario,
      [FromServices] SigningConfigurations signingConfigurations,
      [FromServices] TokenConfigurations tokenConfigurations)
        {
            bool credenciaisValidas = false;
            AutorizacaoApi usuarioBase = null;
            if (!(usuario is null))
            {
                usuarioBase = _autorizacaoApiApplication.GetAutorizacaoByAccessKey(usuario.AccessKey);
                credenciaisValidas = (usuarioBase != null &&
                    usuario.AccessKey == usuarioBase.AccessKey);
            }



            if (credenciaisValidas)
            {
                ICollection<Claim> claims = new HashSet<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
                    new Claim("Servico", JsonConvert.SerializeObject(usuarioBase,Formatting.Indented, new JsonSerializerSettings{
                                                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                    }))
                };

                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity("" + usuario.Id, "Autorizacao"),
                    claims
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha na autenticacao"
                };
            }
        }

        [HttpPost("Async")]
        public async Task<object> PostAsync(
          [FromBody] AutorizacaoApi usuario,
          [FromServices] SigningConfigurations signingConfigurations,
          [FromServices] TokenConfigurations tokenConfigurations)
        {
            bool credenciaisValidas = false;
            AutorizacaoApi usuarioBase = null;
            if (usuario != null)
            {
                usuarioBase = await _autorizacaoApiApplication.GetAutorizacaoByAccessKeyAsync(usuario.AccessKey);
                credenciaisValidas = (usuarioBase != null &&
                    usuario.Id == usuarioBase.Id &&
                    usuario.AccessKey == usuarioBase.AccessKey);
            }


            if (credenciaisValidas)
            {
                ICollection<Claim> claims = new HashSet<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
                    new Claim("Servico", JsonConvert.SerializeObject(usuarioBase))
                };

                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity("" + usuario.Id, "Autorizacao"),
                    claims
                );


                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha na autenticacao"
                };
            }
        }


    }
}
