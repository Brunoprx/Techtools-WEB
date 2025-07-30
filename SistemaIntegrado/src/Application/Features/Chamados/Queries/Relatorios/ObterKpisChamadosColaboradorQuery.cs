using MediatR;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class ObterKpisChamadosColaboradorQuery : IRequest<KpisChamadosColaboradorViewModel>
    {
        public int ColaboradorId { get; set; }
        public int EmpresaId { get; set; }
    }
} 