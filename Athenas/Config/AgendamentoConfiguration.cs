/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Athenas.Domain;

namespace Athenas.Config
{
    public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(t => t.Horario)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne<Profissional>(s => s.Profissional)
                .WithMany(g => g.Agendamento)
                .HasForeignKey(s => s.IdProfissional);
        }
    }
}*/