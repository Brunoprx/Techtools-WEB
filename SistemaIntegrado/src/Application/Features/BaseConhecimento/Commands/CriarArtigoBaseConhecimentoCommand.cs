using MediatR;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Commands
{
    public class CriarArtigoBaseConhecimentoCommand : IRequest<int>
    {
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string? Tags { get; set; }
        public int AutorId { get; set; }
        public string? ImagemUrl { get; set; }
    }
} 