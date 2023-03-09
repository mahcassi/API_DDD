using Entidades.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Configuracoes
{
    public class Contexto : IdentityDbContext<ApplicationUser>
    {
        // esse option que vai passar a string de conexão que vamos colocar na Startup
        public Contexto(DbContextOptions<Contexto> opcoes) : base(opcoes) {}

        public DbSet<Noticia> Noticia {  get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        // customizando a entidade applicationuser do identity
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);
            base.OnModelCreating(builder);
        }

        // método que verifica se vc configurou a string de conexao 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);
            }
        }

        public string ObterStringConexao()
        {
            string strcon = "Server=./;Database=API_DDD_2023;Integrated Security=True;";
            return strcon;
        }
    }
}
