using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AdicionarSolucao
{
    public class AdicionarSolucaoCommandHandler : IRequestHandler<AdicionarSolucaoCommand>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public AdicionarSolucaoCommandHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task Handle(AdicionarSolucaoCommand request, CancellationToken cancellationToken)
        {
            var chamado = await _chamadoRepository.ObterPorId(request.ChamadoId, request.EmpresaId); 
            if (chamado == null) throw new Exception("Chamado não encontrado.");

            // Novamente, a entidade é quem sabe como se comportar.
            chamado.AdicionarSolucao(request.DescricaoSolucao, request.TecnicoId);
            chamado.AdicionarHistorico($"Solução adicionada pelo técnico ID {request.TecnicoId}.");

            await _chamadoRepository.SalvarAlteracoes();
        }
    }
}