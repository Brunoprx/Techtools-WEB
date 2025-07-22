using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaIntegrado.Application.Features.BaseConhecimento.Commands;
using SistemaIntegrado.Application.Features.BaseConhecimento.Queries;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SistemaIntegrado.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseConhecimentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BaseConhecimentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Método auxiliar para obter o ID do usuário do token JWT
        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
                return userId;
            return null;
        }

        private int GetEmpresaIdFromToken()
        {
            var empresaIdClaim = User.Claims.FirstOrDefault(c => c.Type == "empresa_id");
            return empresaIdClaim != null ? int.Parse(empresaIdClaim.Value) : 0;
        }

        [HttpGet("artigos")]
        public async Task<IActionResult> ObterArtigos([FromQuery] string? palavraChave, [FromQuery] string? categoria, [FromQuery] string? tags)
        {
            var query = new ObterArtigosBaseConhecimentoQuery
            {
                PalavraChave = palavraChave,
                Categoria = categoria,
                Tags = tags
            };

            var artigos = await _mediator.Send(query);
            return Ok(artigos);
        }

        [HttpGet("artigos/{id}")]
        public async Task<IActionResult> ObterArtigoPorId(int id)
        {
            var query = new ObterArtigoBaseConhecimentoPorIdQuery { Id = id };
            var artigo = await _mediator.Send(query);
            
            if (artigo == null)
                return NotFound();

            return Ok(artigo);
        }

        [HttpPost("artigos")]
        public async Task<IActionResult> CriarArtigo([FromBody] CriarArtigoBaseConhecimentoCommand command)
        {
            try
            {
                var artigoId = await _mediator.Send(command);
                return CreatedAtAction(nameof(ObterArtigoPorId), new { id = artigoId }, new { id = artigoId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("artigos/{id}")]
        public async Task<IActionResult> AtualizarArtigo(int id, [FromBody] AtualizarArtigoBaseConhecimentoCommand command)
        {
            try
            {
                command.Id = id;
                command.AutorId = GetUserIdFromToken(); // Adicionar ID do usuário para validação
                
                var resultado = await _mediator.Send(command);
                
                if (!resultado)
                    return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("artigos/{id}")]
        public async Task<IActionResult> RemoverArtigo(int id)
        {
            try
            {
                var command = new RemoverArtigoBaseConhecimentoCommand 
                { 
                    Id = id,
                    AutorId = GetUserIdFromToken() // Adicionar ID do usuário para validação
                };
                var resultado = await _mediator.Send(command);
                
                if (!resultado)
                    return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("upload-imagem")]
        public async Task<IActionResult> UploadImagem([FromForm] IFormFile imagem)
        {
            if (imagem == null || imagem.Length == 0)
                return BadRequest(new { message = "Nenhuma imagem enviada." });

            var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var ext = Path.GetExtension(imagem.FileName).ToLowerInvariant();
            if (!extensoesPermitidas.Contains(ext))
                return BadRequest(new { message = "Formato de imagem não suportado." });

            var pastaUploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "artigos");
            if (!Directory.Exists(pastaUploads))
                Directory.CreateDirectory(pastaUploads);

            var nomeArquivo = $"{Guid.NewGuid()}{ext}";
            var caminhoCompleto = Path.Combine(pastaUploads, nomeArquivo);
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }
            var urlRelativa = $"/uploads/artigos/{nomeArquivo}";
            return Ok(new { url = urlRelativa });
        }
    }
} 