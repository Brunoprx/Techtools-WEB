using Microsoft.EntityFrameworkCore;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;
using SistemaIntegrado.Infrastructure.Persistence.Context;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SistemaIntegrado.Infrastructure.Persistence.Repositories
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly ApplicationDbContext _context;

        public ColaboradorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Colaborador?> ObterPorEmail(string email)
        {
            return await _context.Colaboradores.FirstOrDefaultAsync(c => c.EmailCorporativo == email);
        }

        public async Task<Colaborador?> ObterPorId(int id)
        {
            return await _context.Colaboradores.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Colaborador>> ObterTodos()
        {
            return await _context.Colaboradores.ToListAsync();
        }

        public async Task<int?> ObterIdTecnicoPorEspecialidade(string categoria)
        {
            var tecnicoId = await _context.TecnicoEspecialidades
                                          .Where(te => te.CategoriaEspecialidade == categoria)
                                          .Select(te => (int?)te.IdUsuario)
                                          .FirstOrDefaultAsync();
            return tecnicoId;
        }

        public async Task<List<string>> ObterEspecialidadesDoTecnico(int tecnicoId)
        {
            return await _context.TecnicoEspecialidades
                .Where(te => te.IdUsuario == tecnicoId)
                .Select(te => te.CategoriaEspecialidade)
                .ToListAsync();
        }

        public async Task Adicionar(Colaborador colaborador)
        {
            await _context.Colaboradores.AddAsync(colaborador);
        }

        public async Task Atualizar(Colaborador colaborador)
        {
            _context.Colaboradores.Update(colaborador);
            await Task.CompletedTask;
        }

        public async Task Remover(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador != null)
            {
                _context.Colaboradores.Remove(colaborador);
            }
        }

        public async Task SalvarAlteracoes()
        {
            await _context.SaveChangesAsync();
        }
    }
}