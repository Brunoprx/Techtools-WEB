using SistemaIntegrado.Domain.Entities;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Interfaces.Repositories
{
    public interface IColaboradorRepository
    {
        Task<Colaborador?> ObterPorEmail(string email);
        Task<Colaborador?> ObterPorId(int id);
        Task<List<Colaborador>> ObterTodos();
        Task<int?> ObterIdTecnicoPorEspecialidade(string categoria);
        Task<List<string>> ObterEspecialidadesDoTecnico(int tecnicoId);
        Task Adicionar(Colaborador colaborador);
        Task Atualizar(Colaborador colaborador);
        Task Remover(int id);
        Task SalvarAlteracoes();
    }
}