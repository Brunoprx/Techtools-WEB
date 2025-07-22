using MediatR;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AssumirChamado
{
    public class AssumirChamadoCommand : IRequest<bool>
    {
        public int ChamadoId { get; set; }
        public int TecnicoId { get; set; }
        public int EmpresaId { get; set; }

        public AssumirChamadoCommand(int chamadoId, int tecnicoId, int empresaId)
        {
            ChamadoId = chamadoId;
            TecnicoId = tecnicoId;
            EmpresaId = empresaId;
        }
    }
} 