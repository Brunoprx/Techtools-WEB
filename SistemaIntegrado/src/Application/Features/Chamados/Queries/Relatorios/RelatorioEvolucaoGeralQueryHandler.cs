using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class RelatorioEvolucaoGeralQueryHandler : IRequestHandler<RelatorioEvolucaoGeralQuery, List<EvolucaoGeralPorPeriodoViewModel>>
    {
        private readonly IChamadoRepository _chamadoRepository;
        public RelatorioEvolucaoGeralQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<List<EvolucaoGeralPorPeriodoViewModel>> Handle(RelatorioEvolucaoGeralQuery request, CancellationToken cancellationToken)
        {
            var chamados = await _chamadoRepository.ObterTodos();
            var agrupado = request.Periodo == "semana"
                ? chamados.GroupBy(c => System.Globalization.ISOWeek.GetYear(c.DataAbertura ?? c.DataFechamento ?? System.DateTime.MinValue) + "-S" + System.Globalization.ISOWeek.GetWeekOfYear(c.DataAbertura ?? c.DataFechamento ?? System.DateTime.MinValue))
                : chamados.GroupBy(c => (c.DataAbertura ?? c.DataFechamento ?? System.DateTime.MinValue).ToString("yyyy-MM"));
            var resultado = new List<EvolucaoGeralPorPeriodoViewModel>();
            foreach (var grupo in agrupado)
            {
                var quantidades = grupo.GroupBy(c => c.Status.ToString())
                    .ToDictionary(g => g.Key, g => g.Count());
                resultado.Add(new EvolucaoGeralPorPeriodoViewModel
                {
                    Periodo = grupo.Key,
                    QuantidadesPorStatus = quantidades
                });
            }
            return resultado.OrderBy(x => x.Periodo).ToList();
        }
    }
} 