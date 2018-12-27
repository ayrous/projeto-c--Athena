/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Athenas.Domain;

namespace Athenas.Config
{
    public class AdministradorConfiguration : IEntityTypeConfiguration<Administrador>
    {
            public void Configure(EntityTypeBuilder<Administrador> builder)
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

            builder.Property(t => t.Senha)
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(20);

            }
        }
    }
    */