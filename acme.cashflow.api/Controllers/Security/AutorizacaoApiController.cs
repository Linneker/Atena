using AutoMapper;
using acme.cashflow.api.ViewModel.Security;
using acme.cashflow.api.ViewModel.Util;
using acme.cashflow.application.Interface.Security;
using acme.cashflow.domain.DTO.Seguranca;
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

namespace acme.cashflow.api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacaoApiController : BaseApiController<AutorizacaoApi,AutorizacaoApiViewModel>
    {
        private readonly IAutorizacaoApiApplication _autorizacaoApiApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "AUTORIZAÇÃO API";

        public AutorizacaoApiController(IAutorizacaoApiApplication autorizacaoApiApplication, IMapper mapper):base(mapper, autorizacaoApiApplication, NOME_SERVICO)
        {
            _autorizacaoApiApplication = autorizacaoApiApplication;
            _mapper = mapper;
        }
        [HttpPost("Add")]
        public override ResponseApiViewModel Add(AutorizacaoApiViewModel entity)
        {
            ResponseApiViewModel apiViewModel = _mapper.Map<ResponseApiViewModel>(_autorizacaoApiApplication.Add(_mapper.Map<AutorizacaoApi>(entity), NOME_SERVICO));
            this.HttpContext.Response.StatusCode = (int)apiViewModel.ResponseHttp;
            return apiViewModel;
        }

        [HttpPost("Add/Async")]
        public override async Task<ResponseApiViewModel> AddAsync(AutorizacaoApiViewModel entity)
        {
            var apiViewModel = await _mapper.Map<Task<ResponseApiViewModel>>(_autorizacaoApiApplication.AddAsync(_mapper.Map<AutorizacaoApi>(entity), NOME_SERVICO));
            this.HttpContext.Response.StatusCode = (int)apiViewModel.ResponseHttp;
            return apiViewModel;
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
