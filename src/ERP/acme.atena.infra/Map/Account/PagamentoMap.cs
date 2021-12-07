using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.infra.Map.Account
{
    public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("Pagamento");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.DataCredito);
            builder.Property(t => t.DataPagamento);
            builder.Property(t => t.EnumTipoPagamento);
            builder.Property(t => t.NumeroDaParcela).HasDefaultValue(1);
            builder.Property(t => t.Parcelado).HasDefaultValue(false);
            builder.Property(t => t.ValorPago);

            builder.HasOne(t => t.Usuario).WithMany(t => t.Pagamentos).HasForeignKey(t => t.ClienteId);
            builder.HasOne(t => t.Cliente).WithMany(t => t.Pagamentos).HasForeignKey(t => t.UsuarioId);
            builder.HasOne(t => t.Fornecedor).WithMany(t => t.Pagamentos).HasForeignKey(t => t.FornecedorId);
            builder.HasOne(t => t.Competencia).WithMany(t => t.Pagamentos).HasForeignKey(t => t.CompetenciaId);
            builder.HasOne(t => t.Empresa).WithMany(t => t.Pagamentos).HasForeignKey(t => t.EmpresaId);
            builder.HasOne(t => t.PagamentoVenda).WithMany(t => t.Pagamentos).HasForeignKey(t => t.PagamentoVendaId);

        }
    }
}
