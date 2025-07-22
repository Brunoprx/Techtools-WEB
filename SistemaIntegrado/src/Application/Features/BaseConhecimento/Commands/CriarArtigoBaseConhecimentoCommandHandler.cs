using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Commands
{
    public class CriarArtigoBaseConhecimentoCommandHandler : IRequestHandler<CriarArtigoBaseConhecimentoCommand, int>
    {
        private readonly IArtigoBaseConhecimentoRepository _artigoRepository;

        public CriarArtigoBaseConhecimentoCommandHandler(IArtigoBaseConhecimentoRepository artigoRepository)
        {
            _artigoRepository = artigoRepository;
        }

        public async Task<int> Handle(CriarArtigoBaseConhecimentoCommand request, CancellationToken cancellationToken)
        {
            var artigo = new ArtigoBaseConhecimento
            {
                Titulo = request.Titulo,
                Conteudo = request.Conteudo,
                Categoria = request.Categoria,
                Tags = request.Tags,
                AutorId = request.AutorId,
                ImagemUrl = request.ImagemUrl,
                DataCriacao = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            await _artigoRepository.Adicionar(artigo);
            await _artigoRepository.SalvarAlteracoes();

            return artigo.Id;
        }
    }
} 