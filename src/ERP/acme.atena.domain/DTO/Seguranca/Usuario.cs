using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace acme.atena.domain.DTO.Seguranca
{
    public class Usuario : Pessoa
    {
        private string _senha;

        public Usuario()
        {
            PermissaoUsuarios = new HashSet<PermissaoUsuario>();
            Pagamentos = new HashSet<Pagamento>();
            Dividas = new HashSet<Divida>();
            Vendas = new HashSet<Venda>();
            Compras = new HashSet<Compra>();
            EnderecoUsuarios = new HashSet<EnderecoUsuario>();
        }

        public Usuario(string login, string senha) : this()
        {
            Login = login;
            Senha = SHA512(senha);
        }

        public string Login { get; set; }
        public string Senha { get => _senha; set => _senha = SHA512(value); }
        public bool TermoDeAceite { get; set; }

        public virtual ICollection<PermissaoUsuario> PermissaoUsuarios { get; set; }
        public virtual ICollection<Compra> Compras { get; set; }
        public virtual ICollection<Venda> Vendas { get; set; }
        public virtual ICollection<EnderecoUsuario> EnderecoUsuarios { get; set; }


        public  static string SHA512(string valor)
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
