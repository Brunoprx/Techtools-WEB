using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaIntegrado.Application.Features.Autenticacao.Commands.Login; // <-- A LINHA QUE FALTAVA
using System.Threading.Tasks;

namespace SistemaIntegrado.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutenticacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var token = await _mediator.Send(command);
                // Retorna um objeto JSON contendo o token
                return Ok(new { token = token });
            }
            catch (System.Exception ex)
            {
                // Retorna um status 401 (Não Autorizado) com a mensagem de erro
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}