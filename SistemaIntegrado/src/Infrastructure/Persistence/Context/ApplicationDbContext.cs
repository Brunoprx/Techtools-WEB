using Microsoft.EntityFrameworkCore;
using SistemaIntegrado.Domain.Entities;
using SistemaIntegrado.Domain.Enums;

namespace SistemaIntegrado.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Anexo> Anexos { get; set; }
        public DbSet<TecnicoEspecialidade> TecnicoEspecialidades { get; set; } // <-- LINHA NOVA
        public DbSet<ArtigoBaseConhecimento> ArtigosBaseConhecimento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuração da chave primária composta para a nova tabela
            modelBuilder.Entity<TecnicoEspecialidade>()
                .HasKey(te => new { te.IdUsuario, te.CategoriaEspecialidade });

            // Configurações antigas
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Colaborador)
                .WithMany()
                .HasForeignKey(c => c.ColaboradorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasMany(c => c.Anexos)
                .WithOne(a => a.Chamado)
                .HasForeignKey(a => a.ChamadoId);
            
            modelBuilder.Entity<Chamado>()
                .Property(c => c.Prioridade)
                .HasConversion<string>();

            modelBuilder.Entity<Chamado>()
                .Property(c => c.Status)
                .HasConversion<string>();
        }
    }
}