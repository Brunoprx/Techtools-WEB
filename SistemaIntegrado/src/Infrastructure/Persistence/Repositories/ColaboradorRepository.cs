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

        public async Task<Colaborador?> ObterPorEmail(string email, int empresaId)
        {
            return await _context.Colaboradores.FirstOrDefaultAsync(c => c.EmailCorporativo == email && c.EmpresaId == empresaId);
        }

        public async Task<Colaborador?> ObterPorEmail(string email)
        {
            return await _context.Colaboradores.FirstOrDefaultAsync(c => c.EmailCorporativo == email);
        }

        public async Task<Colaborador?> ObterPorId(int id, int empresaId)
        {
            return await _context.Colaboradores.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
        }

        public async Task<List<Colaborador>> ObterTodos(int empresaId)
        {
            return await _context.Colaboradores.Where(c => c.EmpresaId == empresaId).ToListAsync();
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

        public async Task AtualizarEspecialidades(int tecnicoId, List<string> especialidades)
        {
            var especialidadesAntigas = await _context.TecnicoEspecialidades
                .Where(te => te.IdUsuario == tecnicoId)
                .ToListAsync();

            if (especialidadesAntigas.Any())
            {
                _context.TecnicoEspecialidades.RemoveRange(especialidadesAntigas);
            }

            if (especialidades != null && especialidades.Any())
            {
                var novasEspecialidades = especialidades.Select(e => new TecnicoEspecialidade
                {
                    IdUsuario = tecnicoId,
                    CategoriaEspecialidade = e
                });
                await _context.TecnicoEspecialidades.AddRangeAsync(novasEspecialidades);
            }
        }
    }

    
}