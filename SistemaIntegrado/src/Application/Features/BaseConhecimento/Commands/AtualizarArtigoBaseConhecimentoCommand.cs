using MediatR;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Commands
{
    public class AtualizarArtigoBaseConhecimentoCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string? Tags { get; set; }
        public int? AutorId { get; set; } // Para validação de autorização
    }
} 