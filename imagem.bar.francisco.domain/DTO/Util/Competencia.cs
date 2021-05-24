using imagem.bar.francisco.domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.domain.DTO.Util
{
    public class Competencia : AbstractEntity
    {
        public Competencia()
        {
            Despesas = new HashSet<Despesa>();
            FluxosDeCasas = new HashSet<FluxoDeCaixa>();
            Receitas = new HashSet<Receita>();
        }
        public Competencia(int ano, int mes)
        {
            Ano = ano;
            Mes = mes;
            Despesas = new HashSet<Despesa>();
            FluxosDeCasas = new HashSet<FluxoDeCaixa>();
            Receitas = new HashSet<Receita>();
        }

        public int Ano { get; set; }
        public int Mes { get; set; }

        public virtual ICollection<Despesa> Despesas { get; set; }
        public virtual ICollection<FluxoDeCaixa> FluxosDeCasas { get; set; }
        public virtual ICollection<Receita> Receitas { get; set; }


    }
}
