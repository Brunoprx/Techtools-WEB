using MediatR;

namespace SistemaIntegrado.Application.Features.Usuarios.Commands
{
    public class RemoverUsuarioCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
    }
} 