using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Usuarios.Commands
{
    public class AtualizarUsuarioCommandHandler : IRequestHandler<AtualizarUsuarioCommand, bool>
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        public AtualizarUsuarioCommandHandler(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }

        public async Task<bool> Handle(AtualizarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _colaboradorRepository.ObterPorId(request.Id, request.EmpresaId);
            if (usuario == null) return false;

            var outroUsuarioComMesmoEmail = await _colaboradorRepository.ObterPorEmail(request.Email, request.EmpresaId);
            if (outroUsuarioComMesmoEmail != null && outroUsuarioComMesmoEmail.Id != usuario.Id)
                throw new System.InvalidOperationException("Já existe outro usuário com este e-mail.");

            // Atualiza todas as propriedades do usuário com os dados recebidos
            usuario.Nome = request.Nome;
            usuario.EmailCorporativo = request.Email;
            usuario.Cargo = request.Cargo;
            usuario.Setor = request.Setor;
            usuario.PerfilAcesso = request.PerfilAcesso;
            usuario.Status = request.Status ?? usuario.Status; // <-- A LINHA DE ATUALIZAÇÃO CORRETA

            await _colaboradorRepository.Atualizar(usuario);
            await _colaboradorRepository.SalvarAlteracoes();
            return true;
        }
    }
}