using System.Collections.Generic;

namespace SistemaIntegrado.Application.Features.Chamados.ViewModels
{
    public class EvolucaoGeralPorPeriodoViewModel
    {
        public string Periodo { get; set; } = string.Empty; // Ex: "2025-07" ou "2025-S27"
        public Dictionary<string, int> QuantidadesPorStatus { get; set; } = new();
    }
} 