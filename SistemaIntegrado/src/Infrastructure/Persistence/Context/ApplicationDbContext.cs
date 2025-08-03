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

            // Configuração da tabela TecnicoEspecialidade
            modelBuilder.Entity<TecnicoEspecialidade>()
                .ToTable("tecnico_especialidade");

            // Configurações antigas
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Colaborador)
                .WithMany()
                .HasForeignKey(c => c.ColaboradorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasMany(c => c.Anexos)
                .WithOne(a => a.Chamado)
                .HasForeignKey(a => a.ChamadoId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Chamado>()
                .Property(c => c.Prioridade)
                .HasConversion<string>();

            modelBuilder.Entity<Chamado>()
                .Property(c => c.Status)
                .HasConversion<string>();

            // Configuração da relação com Empresa
            modelBuilder.Entity<Chamado>()
                .Property(c => c.EmpresaId)
                .IsRequired();

            // Configuração da entidade Anexo
            modelBuilder.Entity<Anexo>()
                .Property(a => a.EmpresaId)
                .IsRequired();

            modelBuilder.Entity<Anexo>()
                .Property(a => a.ChamadoId)
                .IsRequired();

            modelBuilder.Entity<Anexo>()
                .HasOne(a => a.Chamado)
                .WithMany(c => c.Anexos)
                .HasForeignKey(a => a.ChamadoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração específica da tabela anexo
            modelBuilder.Entity<Anexo>()
                .ToTable("anexo");

            // Configuração da entidade Colaborador
            modelBuilder.Entity<Colaborador>()
                .Property(c => c.EmpresaId)
                .IsRequired();

            // Configuração da entidade ArtigoBaseConhecimento
            modelBuilder.Entity<ArtigoBaseConhecimento>()
                .Property(a => a.AutorId)
                .IsRequired(false);

            // Configuração da entidade TecnicoEspecialidade
            modelBuilder.Entity<TecnicoEspecialidade>()
                .Property(te => te.IdUsuario)
                .IsRequired();

            modelBuilder.Entity<TecnicoEspecialidade>()
                .Property(te => te.CategoriaEspecialidade)
                .IsRequired();

            // Configuração da relação TecnicoEspecialidade com Colaborador
            modelBuilder.Entity<TecnicoEspecialidade>()
                .HasOne<Colaborador>()
                .WithMany()
                .HasForeignKey(te => te.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}