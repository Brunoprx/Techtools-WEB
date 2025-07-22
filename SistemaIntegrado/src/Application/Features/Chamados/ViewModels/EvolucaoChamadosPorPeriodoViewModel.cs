namespace SistemaIntegrado.Application.Features.Chamados.ViewModels
{
    public class EvolucaoChamadosPorPeriodoViewModel
    {
        public string Periodo { get; set; } = string.Empty; // Ex: "2025-07" ou "2025-S27"
        public int Quantidade { get; set; }
    }
} 