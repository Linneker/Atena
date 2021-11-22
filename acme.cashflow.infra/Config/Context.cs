﻿using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.DTO.Util;
using acme.cashflow.infra.Map.Account;
using acme.cashflow.infra.Map.Security;
using acme.cashflow.infra.Map.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Config
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options):base(options)
        {
        }

        public DbSet<FluxoDeCaixa> FluxoDeCaixas { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public DbSet<Despesa> Despesas { get; set; }
        public DbSet<FluxoDeCaixaDespesa> FluxoDeCaixaDespesas { get; set; }
        public DbSet<FluxoDeCaixaReceita> FluxoDeCaixaReceitas { get; set; }

        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<AutorizacaoApi> AutorizacaoApis { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompetenciaMap());

            modelBuilder.ApplyConfiguration(new DespesaMap());
            modelBuilder.ApplyConfiguration(new ReceitaMap());
            modelBuilder.ApplyConfiguration(new FluxoDeCaixaMap());
            modelBuilder.ApplyConfiguration(new FluxoDeCaixaReceitaMap());
            modelBuilder.ApplyConfiguration(new FluxoDeCaixaDespesaMap());
            
            modelBuilder.ApplyConfiguration(new AutorizacaoApiMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());


        }
    }
}