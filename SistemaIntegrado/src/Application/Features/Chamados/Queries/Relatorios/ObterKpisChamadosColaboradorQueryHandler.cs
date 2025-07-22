using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class ObterKpisChamadosColaboradorQueryHandler : IRequestHandler<ObterKpisChamadosColaboradorQuery, KpisChamadosColaboradorViewModel>
    {
        private readonly IChamadoRepository _chamadoRepository;
        public ObterKpisChamadosColaboradorQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<KpisChamadosColaboradorViewModel> Handle(ObterKpisChamadosColaboradorQuery request, CancellationToken cancellationToken)
        {
            var chamados = await _chamadoRepository.ObterTodos();
            var meusChamados = chamados.Where(c => c.ColaboradorId == request.ColaboradorId).ToList();

            return new KpisChamadosColaboradorViewModel
            {
                TotalChamados = meusChamados.Count,
                EmAndamento = meusChamados.Count(c => c.Status.ToString() == "EmAndamento"),
                Finalizados = meusChamados.Count(c => c.Status.ToString() == "Fechado"),
                Cancelados = meusChamados.Count(c => c.Status.ToString() == "Cancelado")
            };
        }
    }
} 