using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.domain.Interface.Service.Security;
using acme.atena.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Security
{
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEnderecoService _enderecoService;
        public UsuarioService(IUsuarioRepository usuarioRepository, IEnderecoService enderecoService) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _enderecoService = enderecoService;
        }

        public override void Add(Usuario entity)
        {
            if (entity.IsValid())
            {
                Endereco endereco = _enderecoService.GetEnderecoByCep(entity.EnderecoUsuarios.FirstOrDefault().Endereco.Cep);
                if (endereco is not null)
                {
                    entity.EnderecoUsuarios.FirstOrDefault().Endereco = null;
                    entity.EnderecoUsuarios.FirstOrDefault().EnderecoId = endereco.Id;
                }
                _usuarioRepository.Add(entity);
            }
        }
        public override void AddAsync(Usuario entity)
        {
            if (entity.IsValid())
            {
                Endereco endereco = _enderecoService.GetEnderecoByCep(entity.EnderecoUsuarios.FirstOrDefault().Endereco.Cep);
                if (endereco is not null)
                {
                    entity.EnderecoUsuarios.FirstOrDefault().Endereco = null;
                    entity.EnderecoUsuarios.FirstOrDefault().EnderecoId = endereco.Id;
                }
                _usuarioRepository.AddAsync(entity);
            }
        }
        public Usuario Login(string login, string senha)
        {

            return _usuarioRepository.Login(new Usuario(login, senha));
        }

        public Task<IQueryable<Usuario>> RecuperaUsuariosComPermissaoAssync()
        {
            return _usuarioRepository.RecuperaUsuariosComPermissaoAssync();
        }
    }
}
