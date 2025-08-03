using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Services;
using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Application.Features.Usuarios.Commands
{
    public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, int>
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IPerfilAcessoService _perfilAcessoService;
        private readonly ITecnicoEspecialidadeRepository _tecnicoEspecialidadeRepository;

        public CriarUsuarioCommandHandler(IColaboradorRepository colaboradorRepository, IPerfilAcessoService perfilAcessoService, ITecnicoEspecialidadeRepository tecnicoEspecialidadeRepository)
        {
            _colaboradorRepository = colaboradorRepository;
            _perfilAcessoService = perfilAcessoService;
            _tecnicoEspecialidadeRepository = tecnicoEspecialidadeRepository;
        }

        public async Task<int> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Senha))
                throw new ArgumentException("Senha é obrigatória.");

            if (string.IsNullOrWhiteSpace(request.Cpf))
                throw new ArgumentException("CPF é obrigatório.");

            // Verificar se email já existe
            var usuarioExistente = await _colaboradorRepository.ObterPorEmail(request.Email, request.EmpresaId);
            if (usuarioExistente != null)
                throw new InvalidOperationException("Email já está em uso.");

            // Definir perfil automaticamente se não fornecido
            string perfilAcesso = string.Empty;
            if (!string.IsNullOrWhiteSpace(request.PerfilAcesso))
            {
                // Validar perfil fornecido
                if (!_perfilAcessoService.ValidarPerfil(request.PerfilAcesso))
                    throw new ArgumentException($"Perfil '{request.PerfilAcesso}' não é válido.");
                
                perfilAcesso = request.PerfilAcesso;
            }
            else
            {
                // Definir perfil automaticamente baseado em cargo e setor
                perfilAcesso = _perfilAcessoService.DefinirPerfilAutomaticamente(request.Cargo ?? string.Empty, request.Setor ?? string.Empty);
            }

            // Criar o colaborador
            var colaborador = new Colaborador
            {
                Nome = request.Nome,
                EmailCorporativo = request.Email,
                Senha = request.Senha, // TODO: Implementar hash da senha
                Cpf = request.Cpf,
                Cargo = request.Cargo,
                Setor = request.Setor,
                Banco = request.Banco,
                TipoContrato = request.TipoContrato,
                PerfilAcesso = perfilAcesso,
                EmpresaId = request.EmpresaId
            };

            // LOG para depuração do valor de EmpresaId recebido
            Console.WriteLine($"EmpresaId recebido: {request.EmpresaId} (tipo: {request.EmpresaId.GetType()})");

            // Salvar no repositório
            await _colaboradorRepository.Adicionar(colaborador);
            await _colaboradorRepository.SalvarAlteracoes();

            if (colaborador.PerfilAcesso == "Técnico" && request.Especialidades != null && request.Especialidades.Any())
            {
                await _colaboradorRepository.AtualizarEspecialidades(colaborador.Id, request.Especialidades);
                await _colaboradorRepository.SalvarAlteracoes();
            }

            return colaborador.Id;
        }
    }
} 