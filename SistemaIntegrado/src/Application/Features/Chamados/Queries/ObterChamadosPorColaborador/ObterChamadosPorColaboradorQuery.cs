using MediatR;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using System.Collections.Generic;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterChamadosPorColaborador
{
    public class ObterChamadosPorColaboradorQuery : IRequest<IEnumerable<ChamadoResumoViewModel>>
    {
        public int ColaboradorId { get; set; }
        public string? Status { get; set; } // Filtro por status
        public string? Tipo { get; set; }   // Filtro por tipo/categoria
    }
}