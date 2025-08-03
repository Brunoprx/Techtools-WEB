using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Application.Interfaces.Repositories
{
    public interface ITecnicoEspecialidadeRepository
    {
        Task Adicionar(TecnicoEspecialidade tecnicoEspecialidade);
        Task SalvarAlteracoes();
    }
}