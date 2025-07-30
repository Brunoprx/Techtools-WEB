using MediatR;
using SistemaIntegrado.Application.Features.Usuarios.ViewModels;
using System.Collections.Generic;

namespace SistemaIntegrado.Application.Features.Usuarios.Queries
{
    public class ObterUsuariosQuery : IRequest<IEnumerable<UsuarioViewModel>>
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Perfil { get; set; }
        public string? Status { get; set; }
        public int EmpresaId { get; set; }
    }
} 