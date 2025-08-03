using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Autenticacao.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IColaboradorRepository colaboradorRepository, IConfiguration configuration)
        {
            _colaboradorRepository = colaboradorRepository;
            _configuration = configuration;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var colaborador = await _colaboradorRepository.ObterPorEmail(request.Email);

            if (colaborador == null || colaborador.Senha != request.Senha)
            {
                throw new Exception("E-mail ou senha inválidos.");
            }
            
            // Debug: Verificar se o EmpresaId está sendo obtido corretamente
            Console.WriteLine($"Colaborador encontrado: {colaborador.Nome}, EmpresaId: {colaborador.EmpresaId}");
            
            return GerarTokenJwt(colaborador);
        }

        private string GerarTokenJwt(Colaborador colaborador)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT (Jwt:Key) não configurada.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, colaborador.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, colaborador.EmailCorporativo),
                new Claim(ClaimTypes.Name, colaborador.Nome),
                new Claim(ClaimTypes.Role, colaborador.PerfilAcesso),
                new Claim("EmpresaId", colaborador.EmpresaId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}