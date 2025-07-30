using MediatR;
using SistemaIntegrado.Application.Features.BaseConhecimento.ViewModels;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.Queries
{
    public class ObterArtigoBaseConhecimentoPorIdQueryHandler : IRequestHandler<ObterArtigoBaseConhecimentoPorIdQuery, ArtigoBaseConhecimentoViewModel?>
    {
        private readonly IArtigoBaseConhecimentoRepository _artigoRepository;

        public ObterArtigoBaseConhecimentoPorIdQueryHandler(IArtigoBaseConhecimentoRepository artigoRepository)
        {
            _artigoRepository = artigoRepository;
        }

        public async Task<ArtigoBaseConhecimentoViewModel?> Handle(ObterArtigoBaseConhecimentoPorIdQuery request, CancellationToken cancellationToken)
        {
            var artigo = await _artigoRepository.ObterPorId(request.Id, request.EmpresaId);
            
            if (artigo == null)
                return null;

            // Incrementa visualizações
            await _artigoRepository.IncrementarVisualizacao(request.Id, request.EmpresaId);

            return new ArtigoBaseConhecimentoViewModel
            {
                Id = artigo.Id,
                Titulo = artigo.Titulo,
                Conteudo = artigo.Conteudo,
                Categoria = artigo.Categoria,
                Tags = artigo.Tags,
                AutorId = artigo.AutorId,
                NomeAutor = "Autor", // TODO: Buscar nome do autor na tabela de usuários
                DataCriacao = artigo.DataCriacao,
                DataAtualizacao = artigo.DataAtualizacao,
                Visualizacoes = artigo.Visualizacoes + 1, // +1 porque incrementamos acima
                Avaliacao = artigo.Avaliacao
            };
        }
    }
} 