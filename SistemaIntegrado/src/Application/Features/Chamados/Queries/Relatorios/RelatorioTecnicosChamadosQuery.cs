using MediatR;
using System.Collections.Generic;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class RelatorioTecnicosChamadosQuery : IRequest<List<RelatorioTecnicoChamadosViewModel>>
    {
        public int? TecnicoId { get; set; } // Se informado, filtra por técnico específico
        public int EmpresaId { get; set; }
    }
} 