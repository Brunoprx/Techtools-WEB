using System.Collections.Generic;

namespace SistemaIntegrado.Application.Features.Chamados.ViewModels
{
    public class AnaliseDesempenhoViewModel
    {
        public List<ChamadosPorTipoViewModel> ChamadosPorTipo { get; set; } = new();
        public List<PadraoProblemaViewModel> PadroesProblemas { get; set; } = new();
    }

    public class ChamadosPorTipoViewModel
    {
        public string Tipo { get; set; } = string.Empty;
        public double Percentual { get; set; }
    }

    public class PadraoProblemaViewModel
    {
        public string Titulo { get; set; } = string.Empty;
        public double Variacao { get; set; } // Ex: +15, -5
    }
} 