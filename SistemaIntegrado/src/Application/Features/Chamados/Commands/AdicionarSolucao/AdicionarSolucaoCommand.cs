using MediatR;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AdicionarSolucao
{
    public class AdicionarSolucaoCommand : IRequest
    {
        public int ChamadoId { get; set; }
        public int TecnicoId { get; set; }
        public string DescricaoSolucao { get; set; } = string.Empty;
    }
}