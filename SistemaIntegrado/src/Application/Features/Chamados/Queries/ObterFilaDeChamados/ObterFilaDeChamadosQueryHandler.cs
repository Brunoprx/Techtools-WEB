using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Collections.Generic;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaDeChamados
{
    public class ObterFilaDeChamadosQueryHandler : IRequestHandler<ObterFilaDeChamadosQuery, IEnumerable<ChamadoFilaViewModel>>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public ObterFilaDeChamadosQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<IEnumerable<ChamadoFilaViewModel>> Handle(ObterFilaDeChamadosQuery request, CancellationToken cancellationToken)
        {
            return await _chamadoRepository.ObterFila(request.Status, request.Prioridade, request.EmpresaId);
        }
    }
}