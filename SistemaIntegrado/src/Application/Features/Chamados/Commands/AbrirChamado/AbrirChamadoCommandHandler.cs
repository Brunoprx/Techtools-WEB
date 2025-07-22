using MediatR;
using SistemaIntegrado.Application.Interfaces.Repositories;
using SistemaIntegrado.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq; // Adicionado para usar .Any()

namespace SistemaIntegrado.Application.Features.Chamados.Commands.AbrirChamado
{
    public class AbrirChamadoCommandHandler : IRequestHandler<AbrirChamadoCommand, int>
    {
        private readonly IChamadoRepository _chamadoRepository;
        private readonly IColaboradorRepository _colaboradorRepository; // Adicionamos o novo repositório

        // O construtor agora recebe os dois repositórios
        public AbrirChamadoCommandHandler(IChamadoRepository chamadoRepository, IColaboradorRepository colaboradorRepository)
        {
            _chamadoRepository = chamadoRepository;
            _colaboradorRepository = colaboradorRepository;
        }

        public async Task<int> Handle(AbrirChamadoCommand request, CancellationToken cancellationToken)
        {
            // Validação de obrigatoriedade
            if (string.IsNullOrWhiteSpace(request.Titulo) ||
                string.IsNullOrWhiteSpace(request.Descricao) ||
                string.IsNullOrWhiteSpace(request.Categoria) ||
                string.IsNullOrWhiteSpace(request.NivelUrgencia))
            {
                throw new Exception("Todos os campos obrigatórios devem ser preenchidos: título, descrição, categoria e urgência.");
            }

            // Validação de duplicidade: mesmo colaborador, mesma descrição, em menos de 5 minutos
            var chamadosRecentes = await _chamadoRepository.ObterPorColaboradorId(request.ColaboradorId, null, null);
            var agora = DateTime.Now;
            if (chamadosRecentes.Any(c =>
                c.Descricao.Trim().Equals(request.Descricao.Trim(), StringComparison.OrdinalIgnoreCase) &&
                c.DataAbertura.HasValue &&
                (agora - c.DataAbertura.Value).TotalMinutes < 5))
            {
                throw new Exception("Já existe um chamado com a mesma descrição aberto por você nos últimos 5 minutos.");
            }

            var chamado = new Chamado(
                request.Titulo,
                request.Descricao,
                request.Categoria,
                request.NivelUrgencia,
                request.ColaboradorId
            );
            chamado.EmpresaId = request.EmpresaId;

            // Definir prioridade automaticamente com base na urgência (se não fornecida)
            if (string.IsNullOrWhiteSpace(request.Prioridade))
            {
                request.Prioridade = DefinirPrioridadePorUrgencia(request.NivelUrgencia);
            }
            chamado.Prioridade = ParsePrioridade(request.Prioridade);

            // Calcular e registrar o prazo máximo de atendimento (SLA)
            var prazoMaximo = CalcularPrazoMaximo(chamado.Prioridade ?? Domain.Enums.PrioridadeChamado.Media);
            chamado.AdicionarHistorico($"Prazo máximo de atendimento definido: {prazoMaximo} horas (Prioridade: {chamado.Prioridade}).");
            chamado.AdicionarHistorico($"Chamado aberto por colaborador ID {request.ColaboradorId}.");

            await _chamadoRepository.Adicionar(chamado);
            await _chamadoRepository.SalvarAlteracoes();

            // ==================================================
            //     NOVA LÓGICA DE ROTEAMENTO DINÂMICO
            // ==================================================
            
            // Busca no banco um técnico para a categoria do chamado
            var tecnicoId = await _colaboradorRepository.ObterIdTecnicoPorEspecialidade(chamado.Categoria);

            // Se encontrou um especialista, atribui a ele.
            if (tecnicoId.HasValue)
            {
                chamado.AtribuirTecnico(tecnicoId.Value);
                chamado.AdicionarHistorico($"Chamado atribuído automaticamente ao técnico ID {tecnicoId.Value}.");
            }
            else
            {
                // Se não encontrou, busca um técnico geral para a categoria "Outros"
                var tecnicoGeralId = await _colaboradorRepository.ObterIdTecnicoPorEspecialidade("Outros");
                if (tecnicoGeralId.HasValue)
                {
                    chamado.AtribuirTecnico(tecnicoGeralId.Value);
                    chamado.AdicionarHistorico($"Chamado atribuído automaticamente ao técnico geral ID {tecnicoGeralId.Value}.");
                }
                // Se nem isso encontrar, o chamado fica na fila geral (status "Aberto")
            }

            // Se um técnico foi atribuído, o status já mudou para EmAndamento.
            // Precisamos salvar essa alteração.
            if (chamado.Status == Domain.Enums.StatusChamado.EmAndamento)
            {
                await _chamadoRepository.SalvarAlteracoes();
            }
            
            return chamado.Id;
        }

        // Métodos auxiliares para priorização e SLA
        private string DefinirPrioridadePorUrgencia(string urgencia)
        {
            return urgencia.ToLower() switch
            {
                "baixa" => "Baixa",
                "média" or "media" => "Média",
                "alta" => "Alta",
                "crítica" or "critica" => "Crítica",
                _ => "Média" // Padrão
            };
        }

        private Domain.Enums.PrioridadeChamado ParsePrioridade(string prioridade)
        {
            return prioridade.ToLower() switch
            {
                "baixa" => Domain.Enums.PrioridadeChamado.Baixa,
                "média" or "media" => Domain.Enums.PrioridadeChamado.Media,
                "alta" => Domain.Enums.PrioridadeChamado.Alta,
                "crítica" or "critica" => Domain.Enums.PrioridadeChamado.Critica,
                _ => Domain.Enums.PrioridadeChamado.Media // Padrão
            };
        }

        private int CalcularPrazoMaximo(Domain.Enums.PrioridadeChamado prioridade)
        {
            return prioridade switch
            {
                Domain.Enums.PrioridadeChamado.Baixa => 48,
                Domain.Enums.PrioridadeChamado.Media => 24,
                Domain.Enums.PrioridadeChamado.Alta => 8,
                Domain.Enums.PrioridadeChamado.Critica => 2,
                _ => 24 // Padrão
            };
        }
    }
}