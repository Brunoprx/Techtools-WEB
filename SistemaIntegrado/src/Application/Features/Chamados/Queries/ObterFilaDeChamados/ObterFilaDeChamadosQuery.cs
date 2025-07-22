using MediatR;
using System.Collections.Generic; // Necessário para o IEnumerable

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaDeChamados
{
    public class ObterFilaDeChamadosQuery : IRequest<IEnumerable<ChamadoFilaViewModel>>
    {
        public string? Status { get; set; }
        public string? Prioridade { get; set; }
        public int EmpresaId { get; set; }
    }
}