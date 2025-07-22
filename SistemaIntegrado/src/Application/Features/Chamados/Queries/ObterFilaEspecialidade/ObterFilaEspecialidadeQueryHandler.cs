using MediatR;
using SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaDeChamados;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaEspecialidade
{
    public class ObterFilaEspecialidadeQueryHandler : IRequestHandler<ObterFilaEspecialidadeQuery, IEnumerable<ChamadoFilaViewModel>>
    {
        private readonly IChamadoRepository _chamadoRepository;
        private readonly IColaboradorRepository _colaboradorRepository;

        public ObterFilaEspecialidadeQueryHandler(IChamadoRepository chamadoRepository, IColaboradorRepository colaboradorRepository)
        {
            _chamadoRepository = chamadoRepository;
            _colaboradorRepository = colaboradorRepository;
        }

        public async Task<IEnumerable<ChamadoFilaViewModel>> Handle(ObterFilaEspecialidadeQuery request, CancellationToken cancellationToken)
        {
            var especialidades = await _colaboradorRepository.ObterEspecialidadesDoTecnico(request.TecnicoId);
            if (especialidades == null || especialidades.Count == 0)
                return new List<ChamadoFilaViewModel>();
            return await _chamadoRepository.ObterFilaPorEspecialidades(especialidades, request.EmpresaId);
        }
    }
} 