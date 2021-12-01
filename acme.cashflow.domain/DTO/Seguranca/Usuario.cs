using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace acme.cashflow.domain.DTO.Seguranca
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
            Vendas = new HashSet<Venda>();
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


        protected internal static string SHA512(string valor)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] HashValue, MessageBytes = UE.GetBytes(valor);
            Criptografia SHhash = new Criptografia();
            string strHex = "";
            HashValue = SHhash.ComputeHash(MessageBytes);
            foreach (byte b in HashValue)
            {
                strHex += String.Format("{0:x2}", b);
            }
            return strHex;
        }
    }
}
