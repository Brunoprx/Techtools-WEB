using System;

namespace SistemaIntegrado.Application.Features.Chamados.ViewModels
{
    public class ChamadoDetalhesViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string NivelUrgencia { get; set; } = string.Empty;
        public string Prioridade { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? DataAbertura { get; set; }
        public string? NomeColaborador { get; set; }
        public string? EmailColaborador { get; set; }
        public string? SetorColaborador { get; set; }

        // PROPRIEDADE NOVA ADICIONADA AQUI
        public string? SolucaoAplicada { get; set; }

        // PROPRIEDADE NOVA ADICIONADA AQUI
        public List<AnexoViewModel> Anexos { get; set; } = new List<AnexoViewModel>();

        // PROPRIEDADE NOVA ADICIONADA AQUI
        public string? Historico { get; set; }
    }
}