using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Usuarios.Commands
{
    public class RemoverUsuarioCommandHandler : IRequestHandler<RemoverUsuarioCommand, bool>
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        public RemoverUsuarioCommandHandler(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }

        public async Task<bool> Handle(RemoverUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _colaboradorRepository.ObterPorId(request.Id, request.EmpresaId);
            if (usuario == null) return false;
            await _colaboradorRepository.Remover(request.Id);
            await _colaboradorRepository.SalvarAlteracoes();
            return true;
        }
    }
} 