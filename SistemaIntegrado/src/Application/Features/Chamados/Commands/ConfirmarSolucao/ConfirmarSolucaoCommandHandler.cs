using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.ConfirmarSolucao
{
    public class ConfirmarSolucaoCommandHandler : IRequestHandler<ConfirmarSolucaoCommand>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public ConfirmarSolucaoCommandHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task Handle(ConfirmarSolucaoCommand request, CancellationToken cancellationToken)
        {
          var chamado = await _chamadoRepository.ObterPorId(request.ChamadoId, request.EmpresaId); 
            if (chamado == null)
            {
                throw new Exception("Chamado não encontrado.");
            }

            // A lógica de negócio para confirmar a solução fica na própria entidade.
            chamado.ConfirmarSolucao(request.ColaboradorId);
            chamado.AdicionarHistorico($"Solução aceita pelo colaborador ID {request.ColaboradorId}. Chamado fechado.");

            await _chamadoRepository.SalvarAlteracoes();
        }
    }
}