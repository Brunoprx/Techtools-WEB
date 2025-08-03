using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;
using SistemaIntegrado.Infrastructure.Persistence.Context;

namespace SistemaIntegrado.Infrastructure.Persistence.Repositories
{
    public class TecnicoEspecialidadeRepository : ITecnicoEspecialidadeRepository
    {
        private readonly ApplicationDbContext _context;

        public TecnicoEspecialidadeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(TecnicoEspecialidade tecnicoEspecialidade)
        {
            await _context.TecnicoEspecialidades.AddAsync(tecnicoEspecialidade);
        }

        public async Task SalvarAlteracoes()
        {
            await _context.SaveChangesAsync();
        }
    }
}