/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Athenas.Domain;

namespace Athenas.Config
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
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

            builder.Property(t => t.PrimeiroAcesso)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne<Agendamento>(s => s.Agendamento)
                .WithMany(g => g.Cliente)
                .HasForeignKey(s => s.IdAgendamento);
        }
    }
}*/