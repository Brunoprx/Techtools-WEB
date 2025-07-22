using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AssumirChamado
{
    public class AssumirChamadoCommandHandler : IRequestHandler<AssumirChamadoCommand, bool>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public AssumirChamadoCommandHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<bool> Handle(AssumirChamadoCommand request, CancellationToken cancellationToken)
        {
            var chamado = await _chamadoRepository.ObterPorId(request.ChamadoId, request.EmpresaId);
            if (chamado == null) return false;
            if (chamado.Status != Domain.Enums.StatusChamado.Aberto) return false;
            chamado.TecnicoResponsavelId = request.TecnicoId;
            chamado.Status = Domain.Enums.StatusChamado.EmAndamento;
            chamado.AdicionarHistorico($"Chamado assumido pelo técnico ID {request.TecnicoId}.");
            await _chamadoRepository.SalvarAlteracoes();
            return true;
        }
    }
} 