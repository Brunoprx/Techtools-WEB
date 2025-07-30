namespace SistemaIntegrado.Application.Features.Usuarios.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Setor { get; set; }
        public string PerfilAcesso { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
} 