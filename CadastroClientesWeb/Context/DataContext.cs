using CadastroClientesWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientesWeb.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Telefones)
                .WithOne(t => t.Cliente)
                .HasForeignKey(t => t.ClienteId);

            modelBuilder.Entity<Cliente>()
                 .HasOne(c => c.Endereco)
                 .WithOne(e => e.Cliente)
                 .HasForeignKey<Endereco>(e => e.ClienteId);
        }


    }

}