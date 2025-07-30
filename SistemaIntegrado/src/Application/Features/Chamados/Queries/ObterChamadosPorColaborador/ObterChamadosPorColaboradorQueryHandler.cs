using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Application.Features.Chamados.ViewModels; // Garanta que este using está correto
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Queries.ObterChamadosPorColaborador
{
    // Confirma que ele retorna uma lista do ViewModel de RESUMO
    public class ObterChamadosPorColaboradorQueryHandler : IRequestHandler<ObterChamadosPorColaboradorQuery, IEnumerable<ChamadoResumoViewModel>>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public ObterChamadosPorColaboradorQueryHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<IEnumerable<ChamadoResumoViewModel>> Handle(ObterChamadosPorColaboradorQuery request, CancellationToken cancellationToken)
        {
            var chamadosDoBanco = await _chamadoRepository.ObterPorColaboradorId(request.ColaboradorId, request.Status, request.Tipo, request.EmpresaId);

            // Mapeia os dados do banco para o ViewModel de resumo, garantindo que nenhum dado vá nulo
            var resultado = chamadosDoBanco.Select(c => new ChamadoResumoViewModel
            {
                Id = c.Id,
                Titulo = c.Titulo,
                Categoria = c.Categoria,
                Prioridade = c.Prioridade?.ToString() ?? "N/A",
                Status = c.Status.ToString()
            }).ToList();

            return resultado;
        }
    }
}