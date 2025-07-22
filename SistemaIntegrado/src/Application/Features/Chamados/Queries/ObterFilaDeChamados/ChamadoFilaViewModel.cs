using System; // Necessário para o DateTime

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaDeChamados
{
    public class ChamadoFilaViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string? Prioridade { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataAbertura { get; set; }
        public string? NomeColaborador { get; set; }
    }
}