using acme.atena.api.ViewModel.Account;
using acme.atena.api.ViewModel.Person;
using acme.atena.api.ViewModel.Product;
using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace acme.atena.api.ViewModel.Security
{
    public class UsuarioViewModel : AbstractEntityViewModel
    {
        private string _senha;

        public UsuarioViewModel()
        {
            PermissaoUsuarios = new HashSet<PermissaoUsuarioViewModel>();
            Pagamentos = new HashSet<PagamentoViewModel>();
            Dividas = new HashSet<DividaViewModel>();
            Vendas = new HashSet<VendaViewModel>();
            Compras = new HashSet<CompraViewModel>();
        }

        public UsuarioViewModel(string login, string senha) : this()
        {
            Login = login;
            Senha = SHA512(senha);
        }

        public UsuarioViewModel(string login, string senha, ClienteViewModel pessoa) : this(login, senha)
        {
            Pessoa = pessoa;
        }
        public UsuarioViewModel(string login, string senha, Guid pessoaId) : this(login, senha)
        {
            PessoaId = pessoaId;
        }
        public UsuarioViewModel(string login, string senha, Guid pessoaId, ClienteViewModel pessoa) : this(login, senha, pessoaId)
        {
            Pessoa = pessoa;
        }

        public string Login { get; set; }
        public string Senha { get => _senha; set => _senha = SHA512(value); }
        public bool TermoDeAceite { get; set; }

        public Guid PessoaId { get; set; }
        public ClienteViewModel Pessoa { get; set; }
        public virtual ICollection<PermissaoUsuarioViewModel> PermissaoUsuarios { get; set; }
        public virtual ICollection<PagamentoViewModel> Pagamentos { get; set; }
        public virtual ICollection<DividaViewModel> Dividas { get; set; }
        public virtual ICollection<CompraViewModel> Compras { get; set; }
        public virtual ICollection<VendaViewModel> Vendas { get; set; }

        protected internal static string SHA512(string valor)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            byte[] arrHash = sha512.ComputeHash(Encoding.UTF8.GetBytes(valor));

            StringBuilder sbHash = new StringBuilder();

            for (int i = 0; i < arrHash.Length; i++)
            {
                sbHash.Append(arrHash[i].ToString("x2"));
            }

            return sbHash.ToString();
        }
    }
}
