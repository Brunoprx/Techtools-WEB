using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Commands
{
    public class RemoverArtigoBaseConhecimentoCommandHandler : IRequestHandler<RemoverArtigoBaseConhecimentoCommand, bool>
    {
        private readonly IArtigoBaseConhecimentoRepository _artigoRepository;

        public RemoverArtigoBaseConhecimentoCommandHandler(IArtigoBaseConhecimentoRepository artigoRepository)
        {
            _artigoRepository = artigoRepository;
        }

        public async Task<bool> Handle(RemoverArtigoBaseConhecimentoCommand request, CancellationToken cancellationToken)
        {
            var artigo = await _artigoRepository.ObterPorId(request.Id);
            
            if (artigo == null)
                return false;

            // Verificar se o usuário é o autor do artigo
            if (request.AutorId.HasValue && artigo.AutorId != request.AutorId.Value)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para excluir este artigo.");
            }

            await _artigoRepository.Remover(request.Id);
            await _artigoRepository.SalvarAlteracoes();

            return true;
        }
    }
} 