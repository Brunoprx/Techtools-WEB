using SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaDeChamados;
using SistemaIntegrado.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Interfaces.Repositories
{
    public interface IChamadoRepository
    {
        Task Adicionar(Chamado chamado);
        Task<Chamado?> ObterPorId(int id, int empresaId);
        Task<Chamado?> ObterPorIdComDetalhes(int id, int empresaId);
        Task SalvarAlteracoes();
        Task AdicionarAnexo(Anexo anexo);
        Task<IEnumerable<Chamado>> ObterPorColaboradorId(int colaboradorId, string? status, string? tipo, int empresaId);
        Task<IEnumerable<ChamadoFilaViewModel>> ObterFila(string? status, string? prioridade, int empresaId);
        Task<IEnumerable<ChamadoFilaViewModel>> ObterFilaPorEspecialidades(List<string> categorias, int empresaId);
        Task<List<SistemaIntegrado.Domain.Entities.Chamado>> ObterTodos(int empresaId);
        Task<List<SistemaIntegrado.Application.Features.Chamados.ViewModels.RelatorioTecnicoChamadosViewModel>> ObterRelatorioChamadosPorTecnico(int empresaId);
        Task<List<SistemaIntegrado.Application.Features.Chamados.ViewModels.RelatorioTecnicoChamadosViewModel>> ObterRelatorioChamadosPorTecnico(int tecnicoId, int empresaId);
    }
}