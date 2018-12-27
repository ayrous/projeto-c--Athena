using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Athenas.Domain
{
    public class Context : DbContext
    {
      /*  public Context() : base("name=Context")
        {

        }
		*/
		public Context(DbContextOptions<Context> options)
        : base(options)

        { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
    }
}
