/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Athenas.Domain;

namespace Athenas.Config
{
    public class PessoaJuridicaConfiguration : IEntityTypeConfiguration<PessoaJuridica>
    {
        public void Configure(EntityTypeBuilder<PessoaJuridica> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(t => t.Cnpj)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(u => u.Cnpj)
                .IsUnique();

            builder.Property(t => t.NomeFantasia)
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(20);

            builder.Property(t => t.HorarioInicial)
                    .HasColumnType("datetime")
                    .IsRequired();

            builder.Property(t => t.HorarioFinal)
                    .HasColumnType("datetime")
                    .IsRequired();

            builder.Property(t => t.NomeFantasia)
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(20);

            builder.Property(t => t.TiposJuridico)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}*/