using Microsoft.EntityFrameworkCore;
using SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaDeChamados;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;
using SistemaIntegrado.Infrastructure.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaIntegrado.Infrastructure.Persistence.Repositories
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly ApplicationDbContext _context;
        public ChamadoRepository(ApplicationDbContext context) => _context = context;

        public async Task Adicionar(Chamado chamado)
        {
            await _context.Chamados.AddAsync(chamado);
        }

        public async Task AdicionarAnexo(Anexo anexo)
        {
            await _context.Anexos.AddAsync(anexo);
        }

        public async Task<Chamado?> ObterPorId(int id, int empresaId)
        {
            return await _context.Chamados.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
        }

        public async Task<Chamado?> ObterPorIdComDetalhes(int id, int empresaId)
        {
            return await _context.Chamados
                .Include(c => c.Colaborador)
                .Include(c => c.Anexos)
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
        }

        public async Task<IEnumerable<Chamado>> ObterPorColaboradorId(int colaboradorId, string? status, string? tipo, int empresaId)
        {
            var query = _context.Chamados.Include(c => c.Colaborador).Where(c => c.ColaboradorId == colaboradorId && c.EmpresaId == empresaId);

            if (!string.IsNullOrEmpty(status) && System.Enum.TryParse<Domain.Enums.StatusChamado>(status, true, out var statusEnum))
            {
                query = query.Where(c => c.Status == statusEnum);
            }
            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(c => c.Categoria == tipo);
            }
            return await query.OrderByDescending(c => c.DataAbertura).ToListAsync();
        }
        
        public async Task SalvarAlteracoes()
        {
            await _context.SaveChangesAsync();
        }

        // ==========================================================
        // MÉTODO QUE ESTAVA FALTANDO, ADICIONADO DE VOLTA AQUI
        // ==========================================================
        public async Task<IEnumerable<ChamadoFilaViewModel>> ObterFila(string? status, string? prioridade, int empresaId)
        {
            var query = _context.Chamados
                .Include(c => c.Colaborador)
                .Where(c => c.EmpresaId == empresaId && c.Status != Domain.Enums.StatusChamado.Fechado && c.Status != Domain.Enums.StatusChamado.Cancelado);

            if (!string.IsNullOrEmpty(status) && System.Enum.TryParse<Domain.Enums.StatusChamado>(status, true, out var statusEnum))
            {
                query = query.Where(c => c.Status == statusEnum);
            }

            if (!string.IsNullOrEmpty(prioridade) && System.Enum.TryParse<Domain.Enums.PrioridadeChamado>(prioridade, true, out var prioridadeEnum))
            {
                query = query.Where(c => c.Prioridade == prioridadeEnum);
            }

            // Ordena para que os chamados críticos fiquem sempre no topo, seguido pela data de abertura.
            return await query
                .OrderByDescending(c => c.Prioridade == Domain.Enums.PrioridadeChamado.Critica)
                .ThenBy(c => c.DataAbertura)
                .Select(c => new ChamadoFilaViewModel
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Categoria = c.Categoria,
                    Prioridade = c.Prioridade.ToString(),
                    Status = c.Status.ToString(),
                    DataAbertura = c.DataAbertura,
                    NomeColaborador = c.Colaborador != null ? c.Colaborador.Nome : "N/A"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ChamadoFilaViewModel>> ObterFilaPorEspecialidades(List<string> categorias, int empresaId)
        {
            var query = _context.Chamados
                .Include(c => c.Colaborador)
                .Where(c => c.EmpresaId == empresaId && (c.Status == Domain.Enums.StatusChamado.Aberto || c.Status == Domain.Enums.StatusChamado.EmAndamento) && categorias.Contains(c.Categoria));

            return await query
                .OrderByDescending(c => c.Prioridade == Domain.Enums.PrioridadeChamado.Critica)
                .ThenBy(c => c.DataAbertura)
                .Select(c => new ChamadoFilaViewModel
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Categoria = c.Categoria,
                    Prioridade = c.Prioridade.ToString(),
                    Status = c.Status.ToString(),
                    DataAbertura = c.DataAbertura,
                    NomeColaborador = c.Colaborador != null ? c.Colaborador.Nome : "N/A"
                })
                .ToListAsync();
        }

        public async Task<List<Chamado>> ObterTodos(int empresaId)
        {
            return await _context.Chamados.Where(c => c.EmpresaId == empresaId).ToListAsync();
        }

        // Método ObterContagemChamadosPorTecnico removido pois não é mais utilizado.

        public async Task<List<SistemaIntegrado.Application.Features.Chamados.ViewModels.RelatorioTecnicoChamadosViewModel>> ObterRelatorioChamadosPorTecnico(int empresaId)
        {
            return await _context.Chamados
                .Where(c => c.EmpresaId == empresaId && c.TecnicoResponsavelId.HasValue)
                .GroupBy(c => c.TecnicoResponsavelId!.Value)
                .Select(g => new SistemaIntegrado.Application.Features.Chamados.ViewModels.RelatorioTecnicoChamadosViewModel
                {
                    TecnicoId = g.Key,
                    Nome = _context.Colaboradores.Where(u => u.Id == g.Key).Select(u => u.Nome).FirstOrDefault() ?? "Desconhecido",
                    Quantidade = g.Count()
                })
                .ToListAsync();
        }

        public async Task<List<SistemaIntegrado.Application.Features.Chamados.ViewModels.RelatorioTecnicoChamadosViewModel>> ObterRelatorioChamadosPorTecnico(int tecnicoId, int empresaId)
        {
            return await _context.Chamados
                .Where(c => c.EmpresaId == empresaId && c.TecnicoResponsavelId.HasValue && c.TecnicoResponsavelId.Value == tecnicoId)
                .GroupBy(c => c.TecnicoResponsavelId!.Value)
                .Select(g => new SistemaIntegrado.Application.Features.Chamados.ViewModels.RelatorioTecnicoChamadosViewModel
                {
                    TecnicoId = g.Key,
                    Nome = _context.Colaboradores.Where(u => u.Id == g.Key).Select(u => u.Nome).FirstOrDefault() ?? "Desconhecido",
                    Quantidade = g.Count()
                })
                .ToListAsync();
        }
    }
}