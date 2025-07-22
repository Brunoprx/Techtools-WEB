using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaIntegrado.Application.Features.Chamados.Commands.AbrirChamado;
using SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaDeChamados;
using SistemaIntegrado.Application.Features.Chamados.Queries.ObterChamadoPorId;
using SistemaIntegrado.Application.Features.Chamados.Commands.AceitarChamado;
using SistemaIntegrado.Application.Features.Chamados.Commands.AdicionarSolucao;
using Microsoft.AspNetCore.Http; // Adicione este using
using SistemaIntegrado.Application.Features.Chamados.Commands.AdicionarAnexo; // Adicione este using
using SistemaIntegrado.Application.Features.Chamados.Commands.ConfirmarSolucao;
using SistemaIntegrado.Application.Features.Chamados.Queries.ObterChamadosPorColaborador;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using Microsoft.AspNetCore.Authorization;
using SistemaIntegrado.Application.Features.Chamados.Commands.RejeitarSolucao;

namespace SistemaIntegrado.Api.Controllers
{
    // [Authorize]  // <-- Comente ou remova esta linha temporariamente
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChamadosController(IMediator mediator) => _mediator = mediator;

        private int GetEmpresaIdFromToken()
        {
            var empresaIdClaim = User.Claims.FirstOrDefault(c => c.Type == "empresa_id");
            return empresaIdClaim != null ? int.Parse(empresaIdClaim.Value) : 0;
        }

        [HttpPost("abrir")]
        public async Task<IActionResult> AbrirChamado([FromBody] AbrirChamadoCommand command)
        {
            var chamadoId = await _mediator.Send(command);
            // Retorna um status 201 (Created) com um objeto simples contendo o novo ID.
            return StatusCode(201, new { id = chamadoId });
        }

        [HttpGet("fila")]
        public async Task<IActionResult> ObterFila([FromQuery] ObterFilaDeChamadosQuery query)
        {
            var chamados = await _mediator.Send(query);
            return Ok(chamados);
        }

        [HttpGet("fila-especialidade")]
        public async Task<IActionResult> ObterFilaEspecialidade([FromQuery] int tecnicoId)
        {
            var query = new SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaEspecialidade.ObterFilaEspecialidadeQuery { TecnicoId = tecnicoId };
            var chamados = await _mediator.Send(query);
            return Ok(chamados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChamadoPorId(int id)
        {
            var query = new ObterChamadoPorIdQuery { Id = id };
            var chamado = await _mediator.Send(query);
            if (chamado == null) return NotFound();
            return Ok(chamado);
        }

        [HttpPut("aceitar")]
        public async Task<IActionResult> AceitarChamado([FromBody] AceitarChamadoCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("adicionar-solucao")]
        public async Task<IActionResult> AdicionarSolucao([FromBody] AdicionarSolucaoCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ===== MÉTODO CORRIGIDO =====
        // Substitua o método AdicionarAnexo existente por este
        [HttpPost("{id}/anexos")]
        public async Task<IActionResult> AdicionarAnexo(int id, IFormFile file)
        {
            // O Controller abre o "malote da web" (IFormFile) e pega os dados genéricos
            var command = new AdicionarAnexoCommand
            {
                ChamadoId = id,
                FileStream = file.OpenReadStream(), // Pega o conteúdo como Stream
                FileName = file.FileName           // Pega o nome do arquivo
            };

            try
            {
                var caminhoDoAnexo = await _mediator.Send(command);
                return Ok(new { caminho = caminhoDoAnexo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Adicione este novo método dentro da classe ChamadosController
        [HttpPut("confirmar-solucao")]
        public async Task<IActionResult> ConfirmarSolucao([FromBody] ConfirmarSolucaoCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Adicione este novo método dentro da classe ChamadosController
        // Substitua o método existente no ChamadosController.cs
        [HttpGet("colaborador/{id}")]
        public async Task<IActionResult> ObterChamadosPorColaborador(int id, [FromQuery] string? status, [FromQuery] string? tipo)
        {
            var query = new ObterChamadosPorColaboradorQuery
            {
                ColaboradorId = id,
                Status = status, // Passando o filtro de status
                Tipo = tipo     // Passando o filtro de tipo
            };
            var chamados = await _mediator.Send(query);
            return Ok(chamados);
        }

        // Adicione este novo método dentro da classe ChamadosController
        [HttpPut("rejeitar-solucao")]
        public async Task<IActionResult> RejeitarSolucao([FromBody] RejeitarSolucaoCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/reencaminhar")]
        public async Task<IActionResult> ReencaminharChamado(int id, [FromBody] int novoTecnicoId)
        {
            var command = new Application.Features.Chamados.Commands.ReencaminharChamado.ReencaminharChamadoCommand(id, novoTecnicoId);
            var resultado = await _mediator.Send(command);
            if (resultado)
                return NoContent();
            else
                return BadRequest(new { message = "Não foi possível reencaminhar o chamado. Verifique as regras de negócio." });
        }

        [HttpPut("{id}/assumir")]
        public async Task<IActionResult> AssumirChamado(int id, [FromBody] int tecnicoId)
        {
            var command = new SistemaIntegrado.Application.Features.Chamados.Commands.AssumirChamado.AssumirChamadoCommand(id, tecnicoId);
            var resultado = await _mediator.Send(command);
            if (resultado)
                return NoContent();
            else
                return BadRequest(new { message = "Não foi possível assumir o chamado. Verifique as regras de negócio." });
        }

        [AllowAnonymous]
        [HttpGet("relatorio-status")]
        public async Task<IActionResult> RelatorioStatus()
        {
            var query = new SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios.RelatorioStatusChamadosQuery();
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("relatorio-tecnicos")]
        public async Task<IActionResult> RelatorioTecnicos([FromQuery] int? tecnicoId = null)
        {
            var query = new SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios.RelatorioTecnicosChamadosQuery { TecnicoId = tecnicoId };
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("evolucao-tecnico")]
        public async Task<IActionResult> EvolucaoTecnico([FromQuery] int tecnicoId, [FromQuery] string periodo = "mes")
        {
            var query = new SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios.RelatorioEvolucaoTecnicoQuery { TecnicoId = tecnicoId, Periodo = periodo };
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("evolucao-geral")]
        public async Task<IActionResult> EvolucaoGeral([FromQuery] string periodo = "mes")
        {
            var query = new SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios.RelatorioEvolucaoGeralQuery { Periodo = periodo };
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("kpis")]
        public async Task<IActionResult> ObterKpis([FromQuery] int? tecnicoId = null, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var query = new SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios.ObterKpisChamadosQuery
            {
                TecnicoId = tecnicoId,
                DataInicio = dataInicio,
                DataFim = dataFim
            };
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("kpis-colaborador")]
        public async Task<IActionResult> ObterKpisColaborador([FromQuery] int colaboradorId)
        {
            var query = new SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios.ObterKpisChamadosColaboradorQuery
            {
                ColaboradorId = colaboradorId
            };
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("analise-desempenho")]
        public async Task<IActionResult> AnaliseDesempenho([FromQuery] int? colaboradorId = null, [FromQuery] int? tecnicoId = null)
        {
            var query = new SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios.AnaliseDesempenhoQuery
            {
                ColaboradorId = colaboradorId,
                TecnicoId = tecnicoId
            };
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }
    }
}