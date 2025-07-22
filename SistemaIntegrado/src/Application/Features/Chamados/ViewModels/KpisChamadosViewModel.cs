using System;

namespace SistemaIntegrado.Application.Features.Chamados.ViewModels
{
    public class KpisChamadosViewModel
    {
        public int TotalChamados { get; set; }
        public double TempoMedioResolucao { get; set; }
        public double SlaAtendimento { get; set; }
        public int ForaDoPrazo { get; set; }
    }
} 