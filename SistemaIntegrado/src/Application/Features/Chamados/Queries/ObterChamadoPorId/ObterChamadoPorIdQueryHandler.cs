using MediatR;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterChamadoPorId
{
    public class ObterChamadoPorIdQueryHandler : IRequestHandler<ObterChamadoPorIdQuery, ChamadoDetalhesViewModel?>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public ObterChamadoPorIdQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<ChamadoDetalhesViewModel?> Handle(ObterChamadoPorIdQuery request, CancellationToken cancellationToken)
        {
            var chamado = await _chamadoRepository.ObterPorIdComDetalhes(request.Id, request.EmpresaId);

            if (chamado == null)
            {
                return null;
            }

            // O mapeamento agora inclui a solução
            return new ChamadoDetalhesViewModel
            {
                Id = chamado.Id,
                Titulo = chamado.Titulo,
                Descricao = chamado.Descricao,
                Categoria = chamado.Categoria,
                NivelUrgencia = chamado.NivelUrgencia,
                Prioridade = chamado.Prioridade?.ToString() ?? "N/A",
                Status = chamado.Status.ToString(),
                DataAbertura = chamado.DataAbertura,
                NomeColaborador = chamado.Colaborador?.Nome,
                EmailColaborador = chamado.Colaborador?.EmailCorporativo,
                SetorColaborador = chamado.Colaborador?.Setor,
                SolucaoAplicada = chamado.SolucaoAplicada,

                 // LÓGICA NOVA PARA MAPEAMENTO DOS ANEXOS
                    Anexos = chamado.Anexos.Select(anexo => new AnexoViewModel
                    {
                        Id = anexo.Id,
                        NomeArquivo = anexo.NomeArquivo,
                        CaminhoArquivo = anexo.CaminhoArquivo
                    }).ToList(),

                // PROPRIEDADE NOVA ADICIONADA AQUI
                Historico = chamado.Historico
                
            };
        }
    }
}