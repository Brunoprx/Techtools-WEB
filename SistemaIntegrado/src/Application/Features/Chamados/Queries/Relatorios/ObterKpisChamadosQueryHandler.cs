using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class ObterKpisChamadosQueryHandler : IRequestHandler<ObterKpisChamadosQuery, KpisChamadosViewModel>
    {
        private readonly IChamadoRepository _chamadoRepository;
        public ObterKpisChamadosQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<KpisChamadosViewModel> Handle(ObterKpisChamadosQuery request, CancellationToken cancellationToken)
        {
            var chamados = await _chamadoRepository.ObterTodos(request.EmpresaId);

            // Filtros
            if (request.TecnicoId.HasValue)
                chamados = chamados.Where(c => c.TecnicoResponsavelId == request.TecnicoId.Value).ToList();

            if (request.DataInicio.HasValue)
                chamados = chamados.Where(c => (c.DataAbertura ?? DateTime.MinValue) >= request.DataInicio.Value).ToList();

            if (request.DataFim.HasValue)
                chamados = chamados.Where(c => (c.DataAbertura ?? DateTime.MinValue) <= request.DataFim.Value).ToList();

            var total = chamados.Count;
            var fechados = chamados.Where(c => c.Status.ToString() == "Fechado").ToList();

            // Tempo médio de resolução (em horas)
            double tempoMedio = fechados.Count > 0
                ? fechados
                    .Where(c => c.DataAbertura.HasValue && c.DataFechamento.HasValue)
                    .Select(c => (c.DataFechamento.Value - c.DataAbertura.Value).TotalHours)
                    .DefaultIfEmpty(0)
                    .Average()
                : 0;

            // SLA: % de chamados fechados dentro do prazo (DataFechamento <= DataAbertura + 48h)
            double sla = fechados.Count > 0
                ? 100.0 * fechados.Count(c =>
                    c.DataAbertura.HasValue && c.DataFechamento.HasValue &&
                    c.DataFechamento.Value <= c.DataAbertura.Value.AddHours(48)
                ) / fechados.Count
                : 0;

            // Fora do prazo: fechados após o prazo ou abertos e já vencidos
            int foraPrazo = chamados.Count(c =>
                (c.Status.ToString() == "Fechado" && c.DataAbertura.HasValue && c.DataFechamento.HasValue && c.DataFechamento.Value > c.DataAbertura.Value.AddHours(48)) ||
                (c.Status.ToString() != "Fechado" && c.DataAbertura.HasValue && DateTime.Now > c.DataAbertura.Value.AddHours(48))
            );

            return new KpisChamadosViewModel
            {
                TotalChamados = total,
                TempoMedioResolucao = Math.Round(tempoMedio, 1),
                SlaAtendimento = Math.Round(sla, 0),
                ForaDoPrazo = foraPrazo
            };
        }
    }
} 