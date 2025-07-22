using MediatR;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AceitarChamado
{
    public class AceitarChamadoCommand : IRequest
    {
        public int ChamadoId { get; set; }
        public int TecnicoId { get; set; } // O ID do técnico que está aceitando
    }
}