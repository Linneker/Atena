using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace imagem.bar.francisco.domain.DTO.Seguranca
{
    public class Usuario : AbstractEntity
    {
        private string _senha;

        public Usuario()
        {
        }

        public Usuario(string login, string senha)
        {
            Login = login;
            Senha = senha;
        }

        public Usuario(string login, string senha, string nome)
        {
            Login = login;
            Senha = senha;
            Nome = nome;
        }

        public string Login { get; set; }
        public string Senha { get=> SHA512(_senha); set => _senha = value; }
        public string Nome { get; set; }

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
