using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.ReencaminharChamado
{
    public class ReencaminharChamadoCommandHandler : IRequestHandler<ReencaminharChamadoCommand, bool>
    {
        private readonly IChamadoRepository _chamadoRepository;
        private readonly IColaboradorRepository _colaboradorRepository;

        public ReencaminharChamadoCommandHandler(IChamadoRepository chamadoRepository, IColaboradorRepository colaboradorRepository)
        {
            _chamadoRepository = chamadoRepository;
            _colaboradorRepository = colaboradorRepository;
        }

        public async Task<bool> Handle(ReencaminharChamadoCommand request, CancellationToken cancellationToken)
        {
            // Buscar chamado
            var chamado = await _chamadoRepository.ObterPorId(request.ChamadoId, request.EmpresaId);
            if (chamado == null) return false;

            // Validar se está em andamento
            if (chamado.Status != Domain.Enums.StatusChamado.EmAndamento) return false;

            // Validar se novo técnico é diferente do atual
            if (chamado.TecnicoResponsavelId == request.NovoTecnicoId) return false;

            // Validar se novo técnico existe e é técnico
            // Aqui você pode precisar implementar ObterColaboradorPorId se não existir
            // var novoTecnico = await _colaboradorRepository.ObterPorIdAsync(request.NovoTecnicoId);
            // if (novoTecnico == null || novoTecnico.Perfil != "Tecnico") return false;

            // Atualizar técnico responsável
            chamado.TecnicoResponsavelId = request.NovoTecnicoId;
            chamado.AdicionarHistorico($"Chamado reencaminhado para o técnico de ID {request.NovoTecnicoId}");

            await _chamadoRepository.SalvarAlteracoes();
            return true;
        }
    }
} 