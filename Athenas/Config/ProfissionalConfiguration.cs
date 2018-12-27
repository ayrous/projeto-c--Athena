/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Athenas.Domain;

namespace Athenas.Config
{
    public class ProfissionalConfiguration : IEntityTypeConfiguration<Profissional>
    {
        public void Configure(EntityTypeBuilder<Profissional> builder)
        {
            builder.HasKey(o => o.Id);


            builder.Property(t => t.NomeCompleto)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Email)
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(20);

            builder.HasIndex(u => u.Email)
                    .IsUnique();

            builder.Property(t => t.Pin)
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(50);

            builder.HasIndex(u => u.Pin)
                    .IsUnique();

            builder.HasOne<Servico>(s => s.Servico)
                .WithMany(g => g.Profissional)
                .HasForeignKey(s => s.IdServico);
        }
    }
}*/