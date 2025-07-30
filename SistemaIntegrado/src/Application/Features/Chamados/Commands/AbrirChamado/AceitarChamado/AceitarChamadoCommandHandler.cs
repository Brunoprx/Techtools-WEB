using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities; // Precisamos da entidade para chamar o método
using SistemaIntegrado.Domain.Enums;   // E do Enum para mudar o status

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AceitarChamado
{
    public class AceitarChamadoCommandHandler : IRequestHandler<AceitarChamadoCommand>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public AceitarChamadoCommandHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task Handle(AceitarChamadoCommand request, CancellationToken cancellationToken)
        {
            var chamado = await _chamadoRepository.ObterPorId(request.ChamadoId, request.EmpresaId);

            if (chamado == null)
            {
                throw new Exception("Chamado não encontrado."); // ou uma exceção mais específica
            }

            // A mágica da nossa arquitetura: a regra de negócio está na própria entidade!
            chamado.AtribuirTecnico(request.TecnicoId);
            chamado.AdicionarHistorico($"Chamado aceito pelo técnico ID {request.TecnicoId}.");

            await _chamadoRepository.SalvarAlteracoes();
        }
    }
}