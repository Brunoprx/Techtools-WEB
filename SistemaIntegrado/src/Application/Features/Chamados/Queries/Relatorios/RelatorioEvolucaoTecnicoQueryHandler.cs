using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using SistemaIntegrado.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class RelatorioEvolucaoTecnicoQueryHandler : IRequestHandler<RelatorioEvolucaoTecnicoQuery, List<EvolucaoChamadosPorPeriodoViewModel>>
    {
        private readonly IChamadoRepository _chamadoRepository;
        public RelatorioEvolucaoTecnicoQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<List<EvolucaoChamadosPorPeriodoViewModel>> Handle(RelatorioEvolucaoTecnicoQuery request, CancellationToken cancellationToken)
        {
            var chamados = await _chamadoRepository.ObterTodos(request.EmpresaId);
            var chamadosTecnico = chamados.Where(c => 
                c.TecnicoResponsavelId == request.TecnicoId && 
                c.Status == SistemaIntegrado.Domain.Enums.StatusChamado.Fechado && 
                c.DataFechamento.HasValue).ToList();
            var agrupado = request.Periodo == "semana"
                ? chamadosTecnico.GroupBy(c => 
                    System.Globalization.ISOWeek.GetYear(c.DataFechamento!.Value) + 
                    "-S" + 
                    System.Globalization.ISOWeek.GetWeekOfYear(c.DataFechamento!.Value))
                : chamadosTecnico.GroupBy(c => c.DataFechamento!.Value.ToString("yyyy-MM"));
            return agrupado.Select(g => new EvolucaoChamadosPorPeriodoViewModel
            {
                Periodo = g.Key,
                Quantidade = g.Count()
            }).OrderBy(x => x.Periodo).ToList();
        }
    }
} 