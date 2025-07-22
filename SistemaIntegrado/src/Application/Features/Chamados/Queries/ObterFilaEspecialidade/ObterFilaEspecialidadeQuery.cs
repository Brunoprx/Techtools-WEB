using MediatR;
using SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaDeChamados;
using System.Collections.Generic;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterFilaEspecialidade
{
    public class ObterFilaEspecialidadeQuery : IRequest<IEnumerable<ChamadoFilaViewModel>>
    {
        public int TecnicoId { get; set; }
        public int EmpresaId { get; set; }
    }
} 