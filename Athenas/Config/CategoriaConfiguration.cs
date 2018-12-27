/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Athenas.Domain;

namespace Athenas.Config
{
    public class CartegoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
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

            builder.HasOne<PessoaJuridica>(s => s.PessoaJuridica)
                .WithMany(g => g.Categoria)
                .HasForeignKey(s => s.IdPessoaJuridica);
        }
    }
}*/