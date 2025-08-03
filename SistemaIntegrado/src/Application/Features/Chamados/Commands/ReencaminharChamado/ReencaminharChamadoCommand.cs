using MediatR;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.ReencaminharChamado
{
    public class ReencaminharChamadoCommand : IRequest<bool>
    {
        public int ChamadoId { get; set; }
        public int NovoTecnicoId { get; set; }
        public int EmpresaId { get; set; }
    }
}