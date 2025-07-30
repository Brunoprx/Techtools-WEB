using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.RejeitarSolucao
{
    public class RejeitarSolucaoCommandHandler : IRequestHandler<RejeitarSolucaoCommand>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public RejeitarSolucaoCommandHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task Handle(RejeitarSolucaoCommand request, CancellationToken cancellationToken)
        {
            var chamado = await _chamadoRepository.ObterPorId(request.ChamadoId, request.EmpresaId); 
            if (chamado == null)
            {
                throw new Exception("Chamado não encontrado.");
            }

            // A lógica de negócio para rejeitar a solução fica na própria entidade.
            chamado.RejeitarSolucao(request.ColaboradorId, request.MotivoRejeicao);
            chamado.AdicionarHistorico($"Solução rejeitada pelo colaborador ID {request.ColaboradorId}. Motivo: {request.MotivoRejeicao}");

            await _chamadoRepository.SalvarAlteracoes();
        }
    }
}