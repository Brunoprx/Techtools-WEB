using MediatR;
using SistemaIntegrado.Application.Features.BaseConhecimento.ViewModels;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Queries
{
    public class ObterArtigoBaseConhecimentoPorIdQuery : IRequest<ArtigoBaseConhecimentoViewModel?>
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
    }
} 