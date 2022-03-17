using acme.atena.api.ViewModel.Util;
using acme.atena.application.Interface.Util;
using acme.atena.domain.DTO.Util;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Util
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : BaseApiController<Endereco,EnderecoViewModel>
    {
        private IMapper _mapper;
        private IEnderecoApplication _enderecoApplication;
        private const string NOME_SERVICO = "ENDEREÇO";

        public EnderecoController(IMapper mapper, IEnderecoApplication enderecoApplication)
            :base(mapper,enderecoApplication,NOME_SERVICO)
        {
            _mapper = mapper;
            _enderecoApplication = enderecoApplication;
        }

        [EnableQuery]
        [Authorize("Bearer")]
        [HttpGet("ByCep/{cep}")]
        public Endereco GetEnderecoByCep(string cep)
        {
            return _enderecoApplication.GetEnderecoByCep(cep);
        }

        [EnableQuery]
        [Authorize("Bearer")]
        [HttpGet("ByCepAsync/{cep}")]
        public Task<Endereco> GetEnderecoByCepAsync(string cep)
        {
            return _enderecoApplication.GetEnderecoByCepAsync(cep);
        }
    }
}
