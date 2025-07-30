using MediatR;
using SistemaIntegrado.Application.Features.Usuarios.ViewModels;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Usuarios.Queries
{
    public class ObterUsuariosQueryHandler : IRequestHandler<ObterUsuariosQuery, IEnumerable<UsuarioViewModel>>
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        public ObterUsuariosQueryHandler(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }

        public async Task<IEnumerable<UsuarioViewModel>> Handle(ObterUsuariosQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _colaboradorRepository.ObterTodos(request.EmpresaId);
            var query = usuarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Nome))
                query = query.Where(u => u.Nome.ToLower().Contains(request.Nome.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.Email))
                query = query.Where(u => u.EmailCorporativo.ToLower().Contains(request.Email.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.Perfil))
                query = query.Where(u => u.PerfilAcesso == request.Perfil);
            
            // O filtro de Status agora funcionará porque a propriedade existe
            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(u => u.Status == request.Status);

            return query.Select(u => new UsuarioViewModel
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.EmailCorporativo,
                Cargo = u.Cargo,
                Setor = u.Setor,
                PerfilAcesso = u.PerfilAcesso,
                Status = u.Status // <-- A LINHA DE MAPEAMENTO CORRETA
            }).ToList();
        }
    }
}