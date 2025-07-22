using MediatR;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Commands
{
    public class RemoverArtigoBaseConhecimentoCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int? AutorId { get; set; } // Para validação de autorização
    }
} 