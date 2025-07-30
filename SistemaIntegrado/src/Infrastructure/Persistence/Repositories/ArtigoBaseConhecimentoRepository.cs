using Microsoft.EntityFrameworkCore;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;
using SistemaIntegrado.Infrastructure.Persistence.Context;

namespace SistemaIntegrado.Infrastructure.Persistence.Repositories
{
    public class ArtigoBaseConhecimentoRepository : IArtigoBaseConhecimentoRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtigoBaseConhecimentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ArtigoBaseConhecimento?> ObterPorId(int id, int empresaId)
        {
            return await _context.ArtigosBaseConhecimento.FirstOrDefaultAsync(a => a.Id == id && a.EmpresaId == empresaId);
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> ObterTodos(int empresaId)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.EmpresaId == empresaId)
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorTitulo(string titulo, int empresaId)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.EmpresaId == empresaId && a.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorCategoria(string categoria, int empresaId)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.EmpresaId == empresaId && a.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorTags(string tags, int empresaId)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.EmpresaId == empresaId && a.Tags != null && a.Tags.Contains(tags, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorPalavraChave(string palavraChave, int empresaId)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.EmpresaId == empresaId && (a.Titulo.Contains(palavraChave, StringComparison.OrdinalIgnoreCase) ||
                           a.Conteudo.Contains(palavraChave, StringComparison.OrdinalIgnoreCase) ||
                           (a.Tags != null && a.Tags.Contains(palavraChave, StringComparison.OrdinalIgnoreCase))))
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task Adicionar(ArtigoBaseConhecimento artigo)
        {
            await _context.ArtigosBaseConhecimento.AddAsync(artigo);
        }

        public async Task Atualizar(ArtigoBaseConhecimento artigo)
        {
            artigo.DataAtualizacao = DateTime.Now;
            _context.ArtigosBaseConhecimento.Update(artigo);
        }

        public async Task Remover(int id, int empresaId)
        {
            var artigo = await ObterPorId(id, empresaId);
            if (artigo != null)
            {
                _context.ArtigosBaseConhecimento.Remove(artigo);
            }
        }

        public async Task IncrementarVisualizacao(int id, int empresaId)
        {
            var artigo = await ObterPorId(id, empresaId);
            if (artigo != null)
            {
                artigo.Visualizacoes++;
                await SalvarAlteracoes();
            }
        }

        public async Task SalvarAlteracoes()
        {
            await _context.SaveChangesAsync();
        }
    }
} 