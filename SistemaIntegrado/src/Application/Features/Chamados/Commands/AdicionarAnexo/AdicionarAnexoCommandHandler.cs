using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AdicionarAnexo
{
    public class AdicionarAnexoCommandHandler : IRequestHandler<AdicionarAnexoCommand, string>
    {
        private readonly IChamadoRepository _chamadoRepository;

        public AdicionarAnexoCommandHandler(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<string> Handle(AdicionarAnexoCommand request, CancellationToken cancellationToken)
        {
            if (request.FileStream == Stream.Null || request.FileStream.Length == 0)
            {
                throw new Exception("Nenhum arquivo foi enviado.");
            }

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "solucoes");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var nomeArquivoUnico = Guid.NewGuid().ToString() + Path.GetExtension(request.FileName);
            var caminhoCompleto = Path.Combine(uploadsFolderPath, nomeArquivoUnico);

            // Usamos o FileStream que veio do Command para criar o arquivo
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await request.FileStream.CopyToAsync(stream, cancellationToken);
            }

            var anexo = new Domain.Entities.Anexo
            {
                ChamadoId = request.ChamadoId,
                NomeArquivo = request.FileName,
                CaminhoArquivo = $"/uploads/solucoes/{nomeArquivoUnico}",
                EmpresaId = request.EmpresaId
            };

            await _chamadoRepository.AdicionarAnexo(anexo);
            await _chamadoRepository.SalvarAlteracoes();

            return anexo.CaminhoArquivo;
        }
    }
}