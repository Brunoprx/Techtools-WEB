using MediatR;

namespace SistemaIntegrado.Application.Features.Usuarios.Commands
{
    public class CriarUsuarioCommand : IRequest<int>
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Setor { get; set; }
        public string? Banco { get; set; }
        public string? TipoContrato { get; set; }
        public string? PerfilAcesso { get; set; } // Opcional - será definido automaticamente se não fornecido
        public int EmpresaId { get; set; }
    }
} 