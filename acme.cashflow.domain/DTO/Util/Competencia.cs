using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Util
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

            Dividas = new HashSet<Divida>();
            Pagamentos = new HashSet<Pagamento>();
            Compras = new HashSet<Compra>();
            Vendas = new HashSet<Venda>();

        }

        public int Ano { get; set; }
        public int Mes { get; set; }

        public virtual ICollection<Despesa> Despesas { get; set; }
        public virtual ICollection<FluxoDeCaixa> FluxosDeCasas { get; set; }
        public virtual ICollection<Receita> Receitas { get; set; }

        public virtual ICollection<Divida> Dividas { get; set; }
        public virtual ICollection<Pagamento> Pagamentos{ get; set; }
        public virtual ICollection<Compra> Compras { get; set; }
        public virtual ICollection<Venda> Vendas { get; set; }


    }
}
