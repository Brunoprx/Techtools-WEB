using SistemaIntegrado.Domain.Entities;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Interfaces.Repositories
{
    public interface IColaboradorRepository
    {
        Task<Colaborador?> ObterPorEmail(string email, int empresaId);
        Task<Colaborador?> ObterPorEmail(string email); // Para login - busca sem filtro de empresa
        Task<Colaborador?> ObterPorId(int id, int empresaId);
        Task<List<Colaborador>> ObterTodos(int empresaId);
        Task<int?> ObterIdTecnicoPorEspecialidade(string categoria);
        Task<List<string>> ObterEspecialidadesDoTecnico(int tecnicoId);
        Task Adicionar(Colaborador colaborador);
        Task Atualizar(Colaborador colaborador);
        Task Remover(int id);
        Task SalvarAlteracoes();
        Task AtualizarEspecialidades(int tecnicoId, List<string> especialidades);
    }
}