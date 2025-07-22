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
            var empresaIdClaim = User.Claims.FirstOrDefault(c => c.Type == "empresa_id");
            return empresaIdClaim != null ? int.Parse(empresaIdClaim.Value) : 0;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var usuarios = await _colaboradorRepository.ObterTodos();
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
                var usuario = await _colaboradorRepository.ObterPorId(id);
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
            try
            {
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
    }

    public class TestarPerfilRequest
    {
        public string? Cargo { get; set; }
        public string? Setor { get; set; }
    }
} 