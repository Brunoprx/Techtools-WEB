using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SistemaIntegrado.Application.Features.Chamados.ViewModels;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.Relatorios
{
    public class RelatorioTecnicosChamadosQueryHandler : IRequestHandler<RelatorioTecnicosChamadosQuery, List<RelatorioTecnicoChamadosViewModel>>
    {
        private readonly IChamadoRepository _chamadoRepository;
        public RelatorioTecnicosChamadosQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<List<RelatorioTecnicoChamadosViewModel>> Handle(RelatorioTecnicosChamadosQuery request, CancellationToken cancellationToken)
        {
            if (request.TecnicoId.HasValue)
            {
                // Retorna apenas o técnico solicitado
                return await _chamadoRepository.ObterRelatorioChamadosPorTecnico(request.TecnicoId.Value, request.EmpresaId);
            }
            // Retorna todos
            return await _chamadoRepository.ObterRelatorioChamadosPorTecnico(request.EmpresaId);
        }
    }
} 