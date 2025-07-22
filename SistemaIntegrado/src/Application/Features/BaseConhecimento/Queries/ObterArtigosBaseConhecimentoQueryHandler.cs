using MediatR;
using SistemaIntegrado.Application.Features.BaseConhecimento.ViewModels;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Queries
{
    public class ObterArtigosBaseConhecimentoQueryHandler : IRequestHandler<ObterArtigosBaseConhecimentoQuery, IEnumerable<ArtigoBaseConhecimentoResumoViewModel>>
    {
        private readonly IArtigoBaseConhecimentoRepository _artigoRepository;

        public ObterArtigosBaseConhecimentoQueryHandler(IArtigoBaseConhecimentoRepository artigoRepository)
        {
            _artigoRepository = artigoRepository;
        }

        public async Task<IEnumerable<ArtigoBaseConhecimentoResumoViewModel>> Handle(ObterArtigosBaseConhecimentoQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ArtigoBaseConhecimento> artigos;

            if (!string.IsNullOrEmpty(request.PalavraChave))
            {
                artigos = await _artigoRepository.BuscarPorPalavraChave(request.PalavraChave);
            }
            else if (!string.IsNullOrEmpty(request.Categoria))
            {
                artigos = await _artigoRepository.BuscarPorCategoria(request.Categoria);
            }
            else if (!string.IsNullOrEmpty(request.Tags))
            {
                artigos = await _artigoRepository.BuscarPorTags(request.Tags);
            }
            else
            {
                artigos = await _artigoRepository.ObterTodos();
            }

            return artigos.Select(a => new ArtigoBaseConhecimentoResumoViewModel
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Categoria = a.Categoria,
                Tags = a.Tags,
                NomeAutor = "Autor", // TODO: Buscar nome do autor na tabela de usuários
                DataCriacao = a.DataCriacao,
                Visualizacoes = a.Visualizacoes,
                Avaliacao = a.Avaliacao,
                ImagemUrl = a.ImagemUrl
            });
        }
    }
} 