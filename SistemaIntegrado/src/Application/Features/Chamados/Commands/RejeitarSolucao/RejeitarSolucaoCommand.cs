using MediatR;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.RejeitarSolucao
{
    public class RejeitarSolucaoCommand : IRequest
    {
        public int ChamadoId { get; set; }
        public int ColaboradorId { get; set; }
        public string MotivoRejeicao { get; set; } = string.Empty;
    }
}