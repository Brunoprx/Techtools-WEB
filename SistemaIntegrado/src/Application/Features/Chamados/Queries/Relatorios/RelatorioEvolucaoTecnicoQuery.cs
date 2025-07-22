using MediatR;
using System.Collections.Generic;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class RelatorioEvolucaoTecnicoQuery : IRequest<List<EvolucaoChamadosPorPeriodoViewModel>>
    {
        public int TecnicoId { get; set; }
        public string Periodo { get; set; } = "mes"; // "semana" ou "mes"
    }
} 