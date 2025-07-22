using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class RelatorioStatusChamadosQueryHandler : IRequestHandler<RelatorioStatusChamadosQuery, Dictionary<string, int>>
    {
        private readonly IChamadoRepository _chamadoRepository;
        public RelatorioStatusChamadosQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<Dictionary<string, int>> Handle(RelatorioStatusChamadosQuery request, CancellationToken cancellationToken)
        {
            var todos = await _chamadoRepository.ObterTodos();
            return todos
                .GroupBy(c => c.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
} 