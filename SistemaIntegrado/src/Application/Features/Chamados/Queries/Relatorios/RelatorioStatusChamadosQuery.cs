using MediatR;
using System.Collections.Generic;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class RelatorioStatusChamadosQuery : IRequest<Dictionary<string, int>>
    {
        public int EmpresaId { get; set; }
        // Pode adicionar filtros no futuro (por período, técnico, etc)
    }
} 