using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Application.Interfaces.Repositories
{
    public interface IArtigoBaseConhecimentoRepository
    {
        Task<ArtigoBaseConhecimento?> ObterPorId(int id);
        Task<IEnumerable<ArtigoBaseConhecimento>> ObterTodos();
        Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorTitulo(string titulo);
        Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorCategoria(string categoria);
        Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorTags(string tags);
        Task<IEnumerable<ArtigoBaseConhecimento>> BuscarPorPalavraChave(string palavraChave);
        Task Adicionar(ArtigoBaseConhecimento artigo);
        Task Atualizar(ArtigoBaseConhecimento artigo);
        Task Remover(int id);
        Task IncrementarVisualizacao(int id);
        Task SalvarAlteracoes();
    }
} 