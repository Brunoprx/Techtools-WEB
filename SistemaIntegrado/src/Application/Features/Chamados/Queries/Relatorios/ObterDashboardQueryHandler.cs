using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using SistemaIntegrado.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class ObterDashboardQueryHandler : IRequestHandler<ObterDashboardQuery, DashboardViewModel>
    {
        private readonly IChamadoRepository _chamadoRepository;
        
        public ObterDashboardQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<DashboardViewModel> Handle(ObterDashboardQuery request, CancellationToken cancellationToken)
        {
            var chamados = await _chamadoRepository.ObterTodos(request.EmpresaId);

            var totalChamados = chamados.Count();
            var emAndamento = chamados.Count(c => c.Status == StatusChamado.EmAndamento);
            var finalizados = chamados.Count(c => c.Status == StatusChamado.Fechado);
            var cancelados = chamados.Count(c => c.Status == StatusChamado.Cancelado);

            return new DashboardViewModel
            {
                TotalChamados = totalChamados,
                EmAndamento = emAndamento,
                Finalizados = finalizados,
                Cancelados = cancelados
            };
        }
    }
} 