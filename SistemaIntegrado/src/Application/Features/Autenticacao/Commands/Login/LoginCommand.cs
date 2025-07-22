using MediatR;

namespace SistemaIntegrado.Application.Features.Autenticacao.Commands.Login
{
    public class LoginCommand : IRequest<string> // Retorna o token como string
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}