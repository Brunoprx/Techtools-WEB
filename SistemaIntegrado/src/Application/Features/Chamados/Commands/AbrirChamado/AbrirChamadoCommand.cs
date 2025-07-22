using MediatR;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AbrirChamado
{
    public class AbrirChamadoCommand : IRequest<int>
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string NivelUrgencia { get; set; } = string.Empty;
        public int ColaboradorId { get; set; }
        public string? Prioridade { get; set; } // Campo opcional para prioridade (pode ser definida manualmente ou pela IA)
        public int EmpresaId { get; set; }
    }
}