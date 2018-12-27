/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Athenas.Domain;

namespace Athenas.Config
{
    public class ServicoConfiguration : IEntityTypeConfiguration<Servico>
    {
        public void Configure(EntityTypeBuilder<Servico> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(t => t.Nome)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.Descricao)
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(100);

            builder.HasOne<Categoria>(s => s.Categoria)
                .WithMany(g => g.Servico)
                .HasForeignKey(s => s.IdCategoria);
        }
    }
}*/