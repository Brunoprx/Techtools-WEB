using MediatR;
using System.IO; // Usaremos Stream

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AdicionarAnexo
{
    public class AdicionarAnexoCommand : IRequest<string>
    {
        public int ChamadoId { get; set; }
        public Stream FileStream { get; set; } = Stream.Null; // O conteúdo do arquivo
        public string FileName { get; set; } = string.Empty;   // O nome original do arquivo
        public int EmpresaId { get; set; }
    }
}