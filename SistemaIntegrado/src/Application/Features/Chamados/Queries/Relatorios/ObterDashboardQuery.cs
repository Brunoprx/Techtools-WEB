using MediatR;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class ObterDashboardQuery : IRequest<DashboardViewModel>
    {
        public int EmpresaId { get; set; }
    }
} 