using MediatR;
using SistemaIntegrado.Application.Features.BaseConhecimento.ViewModels;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Queries
{
    public class ObterArtigosBaseConhecimentoQuery : IRequest<IEnumerable<ArtigoBaseConhecimentoResumoViewModel>>
    {
        public string? PalavraChave { get; set; }
        public string? Categoria { get; set; }
        public string? Tags { get; set; }
    }
} 