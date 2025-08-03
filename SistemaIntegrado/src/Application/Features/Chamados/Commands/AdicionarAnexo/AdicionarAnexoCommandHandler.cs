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

            // Determinar a pasta baseada no status do chamado
            string subFolder;
            if (request.IsAnexoSolucao)
            {
                subFolder = "solucoes";
            }
            else
            {
                subFolder = "anexos";
            }

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
            
            // Garantir que a pasta existe
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
                Console.WriteLine($"Pasta criada: {uploadsFolderPath}");
            }
            
            Console.WriteLine($"Salvando anexo em: {uploadsFolderPath}");
            Console.WriteLine($"IsAnexoSolucao: {request.IsAnexoSolucao}");

            var nomeArquivoUnico = Guid.NewGuid().ToString() + Path.GetExtension(request.FileName);
            var caminhoCompleto = Path.Combine(uploadsFolderPath, nomeArquivoUnico);

            // Usamos o FileStream que veio do Command para criar o arquivo
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await request.FileStream.CopyToAsync(stream, cancellationToken);
            }

            var caminhoArquivo = $"/uploads/{subFolder}/{nomeArquivoUnico}";
            
            var anexo = new Domain.Entities.Anexo
            {
                ChamadoId = request.ChamadoId,
                NomeArquivo = request.FileName,
                CaminhoArquivo = caminhoArquivo,
                EmpresaId = request.EmpresaId
            };

            Console.WriteLine($"Anexo criado: {anexo.NomeArquivo} -> {anexo.CaminhoArquivo}");

            await _chamadoRepository.AdicionarAnexo(anexo);
            await _chamadoRepository.SalvarAlteracoes();

            Console.WriteLine($"Anexo salvo no banco com sucesso");
            return anexo.CaminhoArquivo;
        }
    }
}