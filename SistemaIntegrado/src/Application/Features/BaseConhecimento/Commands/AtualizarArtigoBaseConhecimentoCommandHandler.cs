using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Commands
{
    public class AtualizarArtigoBaseConhecimentoCommandHandler : IRequestHandler<AtualizarArtigoBaseConhecimentoCommand, bool>
    {
        private readonly IArtigoBaseConhecimentoRepository _artigoRepository;

        public AtualizarArtigoBaseConhecimentoCommandHandler(IArtigoBaseConhecimentoRepository artigoRepository)
        {
            _artigoRepository = artigoRepository;
        }

        public async Task<bool> Handle(AtualizarArtigoBaseConhecimentoCommand request, CancellationToken cancellationToken)
        {
            var artigo = await _artigoRepository.ObterPorId(request.Id);
            
            if (artigo == null)
                return false;

            // Verificar se o usuário é o autor do artigo
            if (request.AutorId.HasValue && artigo.AutorId != request.AutorId.Value)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para editar este artigo.");
            }

            artigo.Titulo = request.Titulo;
            artigo.Conteudo = request.Conteudo;
            artigo.Categoria = request.Categoria;
            artigo.Tags = request.Tags;
            artigo.DataAtualizacao = DateTime.Now;

            await _artigoRepository.Atualizar(artigo);
            await _artigoRepository.SalvarAlteracoes();

            return true;
        }
    }
} 