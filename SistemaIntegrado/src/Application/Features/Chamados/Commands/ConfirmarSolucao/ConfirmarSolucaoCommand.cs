using MediatR;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.ConfirmarSolucao
{
    public class ConfirmarSolucaoCommand : IRequest
    {
        public int ChamadoId { get; set; }
        public int ColaboradorId { get; set; } // ID do colaborador que está aceitando a solução
    }
}