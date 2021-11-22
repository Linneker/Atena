using acme.cashflow.api.ViewModel.Account;
using acme.cashflow.api.ViewModel.Person;
using acme.cashflow.api.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace acme.cashflow.api.ViewModel.Security
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

        public UsuarioViewModel(string login, string senha, PessoaViewModel pessoa) : this(login, senha)
        {
            Pessoa = pessoa;
        }
        public UsuarioViewModel(string login, string senha, Guid pessoaId) : this(login, senha)
        {
            PessoaId = pessoaId;
        }
        public UsuarioViewModel(string login, string senha, Guid pessoaId, PessoaViewModel pessoa) : this(login, senha, pessoaId)
        {
            Pessoa = pessoa;
        }

        public string Login { get; set; }
        public string Senha { get => _senha; set => _senha = SHA512(value); }
        public bool TermoDeAceite { get; set; }

        public Guid PessoaId { get; set; }
        public PessoaViewModel Pessoa { get; set; }
        public virtual ICollection<PermissaoUsuarioViewModel> PermissaoUsuarios { get; set; }
        public virtual ICollection<PagamentoViewModel> Pagamentos { get; set; }
        public virtual ICollection<DividaViewModel> Dividas { get; set; }
        public virtual ICollection<CompraViewModel> Compras { get; set; }
        public virtual ICollection<VendaViewModel> Vendas { get; set; }

        protected internal static string SHA512(string valor)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] HashValue, MessageBytes = UE.GetBytes(valor);
            SHA512Managed SHhash = new SHA512Managed();
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
