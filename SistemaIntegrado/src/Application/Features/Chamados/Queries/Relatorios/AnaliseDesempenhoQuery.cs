using MediatR;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class AnaliseDesempenhoQuery : IRequest<AnaliseDesempenhoViewModel>
    {
        public int? ColaboradorId { get; set; }
        public int? TecnicoId { get; set; }
    }
} 