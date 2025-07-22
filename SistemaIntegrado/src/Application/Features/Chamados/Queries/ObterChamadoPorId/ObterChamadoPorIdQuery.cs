using MediatR;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterChamadoPorId
{
    public class ObterChamadoPorIdQuery : IRequest<ChamadoDetalhesViewModel?>
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
    }
}