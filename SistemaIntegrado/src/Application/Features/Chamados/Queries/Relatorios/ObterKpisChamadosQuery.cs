using MediatR;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;
using System;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class ObterKpisChamadosQuery : IRequest<KpisChamadosViewModel>
    {
        public int? TecnicoId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int EmpresaId { get; set; }
    }
} 