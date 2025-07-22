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

        public async Task<ArtigoBaseConhecimento?> ObterPorId(int id)
        {
            return await _context.ArtigosBaseConhecimento.FindAsync(id);
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> ObterTodos()
        {
            return await _context.ArtigosBaseConhecimento
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorTitulo(string titulo)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorCategoria(string categoria)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorTags(string tags)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.Tags != null && a.Tags.Contains(tags, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorPalavraChave(string palavraChave)
        {
            return await _context.ArtigosBaseConhecimento
                .Where(a => a.Titulo.Contains(palavraChave, StringComparison.OrdinalIgnoreCase) ||
                           a.Conteudo.Contains(palavraChave, StringComparison.OrdinalIgnoreCase) ||
                           (a.Tags != null && a.Tags.Contains(palavraChave, StringComparison.OrdinalIgnoreCase)))
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

        public async Task Remover(int id)
        {
            var artigo = await ObterPorId(id);
            if (artigo != null)
            {
                _context.ArtigosBaseConhecimento.Remove(artigo);
            }
        }

        public async Task IncrementarVisualizacao(int id)
        {
            var artigo = await ObterPorId(id);
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