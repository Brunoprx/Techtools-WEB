using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaIntegrado.Application.Features.Usuarios.Commands;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Services;
using SistemaIntegrado.Domain.Entities;

namespace SistemaIntegrado.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IPerfilAcessoService _perfilAcessoService;

        public UsuariosController(IMediator mediator, IColaboradorRepository colaboradorRepository, IPerfilAcessoService perfilAcessoService)
        {
            _mediator = mediator;
            _colaboradorRepository = colaboradorRepository;
            _perfilAcessoService = perfilAcessoService;
        }

        private int GetEmpresaIdFromToken()
        {
            var empresaIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmpresaId");
            return empresaIdClaim != null ? int.Parse(empresaIdClaim.Value) : 0;
        }

        private string? GetPerfilFromToken()
        {
            var perfilClaim = User.Claims.FirstOrDefault(c => c.Type == "role" || c.Type.EndsWith("/role"));
            return perfilClaim?.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] string? nome, [FromQuery] string? email, [FromQuery] string? perfil, [FromQuery] string? status)
        {
            try
            {
                var query = new SistemaIntegrado.Application.Features.Usuarios.Queries.ObterUsuariosQuery
                {
                    Nome = nome,
                    Email = email,
                    Perfil = perfil,
                    Status = status,
                    EmpresaId = GetEmpresaIdFromToken()
                };
                var usuarios = await _mediator.Send(query);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var usuario = await _colaboradorRepository.ObterPorId(id, GetEmpresaIdFromToken());
                if (usuario == null)
                    return NotFound();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioCommand command)
        {
            var perfil = GetPerfilFromToken();
            if (perfil != "Administrador" && perfil != "RH")
            {
                return Forbid("Apenas administradores ou RH podem criar usuários.");
            }
            try
            {
                command.EmpresaId = GetEmpresaIdFromToken();
                var usuarioId = await _mediator.Send(command);
                return CreatedAtAction(nameof(ObterPorId), new { id = usuarioId }, new { id = usuarioId, message = "Usuário criado com sucesso!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // antes do cursor da problema
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] SistemaIntegrado.Application.Features.Usuarios.Commands.AtualizarUsuarioCommand command)
        {
            var perfil = GetPerfilFromToken();
            if (perfil != "Administrador" && perfil != "RH")
            {
                return Forbid("Apenas administradores ou RH podem editar usuários.");
            }
            try
            {
                command.Id = id;
                command.EmpresaId = GetEmpresaIdFromToken();
                var resultado = await _mediator.Send(command);
                if (!resultado) return NotFound(new { message = "Usuário não encontrado." });
                return Ok(new { message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var perfil = GetPerfilFromToken();
            if (perfil != "Administrador" && perfil != "RH")
            {
                return Forbid("Apenas administradores ou RH podem excluir usuários.");
            }
            try
            {
                var command = new SistemaIntegrado.Application.Features.Usuarios.Commands.RemoverUsuarioCommand
                {
                    Id = id,
                    EmpresaId = GetEmpresaIdFromToken()
                };
                var resultado = await _mediator.Send(command);
                if (!resultado) return NotFound(new { message = "Usuário não encontrado." });
                return Ok(new { message = "Usuário excluído com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("perfis")]
        public IActionResult ObterPerfisDisponiveis()
        {
            try
            {
                var perfis = _perfilAcessoService.ObterPerfisDisponiveis();
                return Ok(perfis);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("testar-perfil")]
        public IActionResult TestarDefinicaoPerfil([FromBody] TestarPerfilRequest request)
        {
            try
            {
                var perfilDefinido = _perfilAcessoService.DefinirPerfilAutomaticamente(request.Cargo, request.Setor);
                return Ok(new { 
                    cargo = request.Cargo, 
                    setor = request.Setor, 
                    perfilDefinido = perfilDefinido 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("cargos")]
        public async Task<IActionResult> ObterCargos()
        {
            try
            {
                var usuarios = await _colaboradorRepository.ObterTodos(GetEmpresaIdFromToken());
                var cargos = usuarios.Select(u => u.Cargo).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                return Ok(cargos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("setores")]
        public async Task<IActionResult> ObterSetores()
        {
            try
            {
                var usuarios = await _colaboradorRepository.ObterTodos(GetEmpresaIdFromToken());
                var setores = usuarios.Select(u => u.Setor).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                if (setores.Count == 0)
                {
                    setores = new List<string> { "TI", "RH", "Financeiro", "Operações" };
                }
                return Ok(setores);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class TestarPerfilRequest
    {
        public string? Cargo { get; set; }
        public string? Setor { get; set; }
    }
} 