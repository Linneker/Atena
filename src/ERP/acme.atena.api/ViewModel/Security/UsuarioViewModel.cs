using acme.atena.api.ViewModel.Account;
using acme.atena.api.ViewModel.Person;
using acme.atena.api.ViewModel.Product;
using acme.atena.api.ViewModel.Util;
using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace acme.atena.api.ViewModel.Security
{
    public class UsuarioViewModel : PessoaViewModel
    {
        private string _senha;

        public UsuarioViewModel()
        {
            PermissaoUsuarios = new HashSet<PermissaoUsuarioViewModel>();
            Pagamentos = new HashSet<PagamentoViewModel>();
            Dividas = new HashSet<DividaViewModel>();
            Vendas = new HashSet<VendaViewModel>();
            Compras = new HashSet<CompraViewModel>();
            EnderecoUsuarios = new HashSet<EnderecoUsuarioViewModel>();
        }

        public UsuarioViewModel(string login, string senha) : this()
        {
            Login = login;
            Senha = SHA512(senha);
        }

        public string Login { get; set; }
        public string Senha { get => _senha; set => _senha = SHA512(value); }
        public bool TermoDeAceite { get; set; }

        public virtual ICollection<PermissaoUsuarioViewModel> PermissaoUsuarios { get; set; }
        public virtual ICollection<CompraViewModel> Compras { get; set; }
        public virtual ICollection<VendaViewModel> Vendas { get; set; }
        public virtual ICollection<EnderecoUsuarioViewModel> EnderecoUsuarios { get; set; }


        public static string SHA512(string valor)
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
