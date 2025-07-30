using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using SistemaIntegrado.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class AnaliseDesempenhoQueryHandler : IRequestHandler<AnaliseDesempenhoQuery, AnaliseDesempenhoViewModel>
    {
        private readonly IChamadoRepository _chamadoRepository;
        public AnaliseDesempenhoQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<AnaliseDesempenhoViewModel> Handle(AnaliseDesempenhoQuery request, CancellationToken cancellationToken)
        {
            var chamados = await _chamadoRepository.ObterTodos(request.EmpresaId);
            if (chamados == null) chamados = new List<Chamado>();
            
            // Filtro por colaborador ou técnico
            if (request.ColaboradorId.HasValue)
                chamados = chamados.Where(c => c.ColaboradorId == request.ColaboradorId.Value).ToList();
            if (request.TecnicoId.HasValue)
                chamados = chamados.Where(c => c.TecnicoResponsavelId == request.TecnicoId.Value).ToList();
            var total = chamados.Count();
            
            // Percentual por categoria
            var porCategoria = total > 0 ? chamados
                .GroupBy(c => c.Categoria ?? "Outros")
                .Select(g => new ChamadosPorCategoriaViewModel
                {
                    Categoria = g.Key,
                    Percentual = Math.Round((double)g.Count() / total * 100, 1)
                })
                .ToList() : new List<ChamadosPorCategoriaViewModel>();
            // Padrões de problemas (variação dos últimos 30 dias vs 30 dias anteriores)
            var hoje = DateTime.Now;
            var ultimos30 = chamados.Where(c => c.DataAbertura.HasValue && c.DataAbertura.Value >= hoje.AddDays(-30)).ToList();
            var anteriores30 = chamados.Where(c => c.DataAbertura.HasValue && c.DataAbertura.Value < hoje.AddDays(-30) && c.DataAbertura.Value >= hoje.AddDays(-60)).ToList();
            var padroes = new List<PadraoProblemaViewModel>();
            var tipos = chamados.Select(c => c.Categoria ?? "Outros").Distinct();
            foreach (var tipo in tipos)
            {
                var atual = ultimos30.Count(c => (c.Categoria ?? "Outros") == tipo);
                var anterior = anteriores30.Count(c => (c.Categoria ?? "Outros") == tipo);
                double variacao = anterior > 0 ? 100.0 * (atual - anterior) / anterior : (atual > 0 ? 100 : 0);
                padroes.Add(new PadraoProblemaViewModel
                {
                    Titulo = $"Problemas de {tipo}",
                    Variacao = Math.Round(variacao, 1)
                });
            }
            return new AnaliseDesempenhoViewModel
            {
                ChamadosPorCategoria = porCategoria,
                PadroesProblemas = padroes
            };
        }
    }
} 