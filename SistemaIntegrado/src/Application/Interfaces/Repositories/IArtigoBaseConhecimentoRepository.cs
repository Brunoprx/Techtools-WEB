using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Application.Interfaces.Repositories
{
    public interface IArtigoBaseConhecimentoRepository
    {
        Task<ArtigoBaseConhecimento?> ObterPorId(int id, int empresaId);
        Task<IEnumerable<ArtigoBaseConhecimento>> ObterTodos(int empresaId);
        Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorTitulo(string titulo, int empresaId);
        Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorCategoria(string categoria, int empresaId);
        Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorTags(string tags, int empresaId);
        Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorPalavraChave(string palavraChave, int empresaId);
        Task Adicionar(ArtigoBaseConhecimento artigo);
        Task Atualizar(ArtigoBaseConhecimento artigo);
        Task Remover(int id, int empresaId);
        Task IncrementarVisualizacao(int id, int empresaId);
        Task SalvarAlteracoes();
    }
} 