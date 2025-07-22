using SistemaIntegrado.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; // Adicionado para ICollection

namespace SistemaIntegrado.Domain.Entities
{
    [Table("chamado")]
    public class Chamado
    {
        [Key]
        [Column("id_chamado")]
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Column("descricao")]
        public string Descricao { get; set; } = string.Empty;

        [Column("categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Column("urgencia")]
        public string NivelUrgencia { get; set; } = string.Empty;

        [Column("prioridade")]
        public PrioridadeChamado? Prioridade { get; set; }

        [Column("status")]
        public StatusChamado Status { get; set; }

        [Column("data_abertura")]
        public DateTime? DataAbertura { get; set; }

        [Column("data_encerramento")]
        public DateTime? DataFechamento { get; set; }

        [Column("solucao_sugerida")]
        public string? SolucaoSugerida { get; set; }

        [Column("solucao_final")]
        public string? SolucaoAplicada { get; set; }

        [Column("historico")]
        public string Historico { get; set; } = string.Empty;

        [Column("id_Usuario")]
        public int? ColaboradorId { get; set; }

        [Column("id_tecnico")]
        public int? TecnicoResponsavelId { get; set; }

        [Column("id_empresa")]
        public int EmpresaId { get; set; }

        // Propriedades de Navegação
        public Colaborador? Colaborador { get; set; }
        public ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();

        // --- CONSTRUTORES ---

        // Construtor vazio para o Entity Framework
        private Chamado() { }

        // O CONSTRUTOR PÚBLICO QUE ESTAVA FALTANDO
        public Chamado(string titulo, string descricao, string categoria, string nivelUrgencia, int colaboradorId)
        {
            Titulo = titulo;
            Descricao = descricao;
            Categoria = categoria;
            NivelUrgencia = nivelUrgencia;
            ColaboradorId = colaboradorId;
            Status = StatusChamado.Aberto;
            DataAbertura = DateTime.Now;
        }

        // Cole este método dentro da sua classe Chamado, abaixo dos construtores.
        public void AtribuirTecnico(int tecnicoId)
        {
            // Regra de Negócio: Um chamado só pode ser atribuído se não estiver fechado ou cancelado.
            if (Status == StatusChamado.Fechado || Status == StatusChamado.Cancelado)
            {
                throw new InvalidOperationException("Não é possível atribuir um chamado que já foi finalizado.");
            }

            this.TecnicoResponsavelId = tecnicoId;
            this.Status = StatusChamado.EmAndamento;
        }

        // Cole este método dentro da sua classe Chamado.
        public void AdicionarSolucao(string solucao, int tecnicoId)
        {
            // Regra de Negócio: Apenas o técnico responsável pode adicionar uma solução.
            if (this.TecnicoResponsavelId != tecnicoId)
            {
                throw new InvalidOperationException("Apenas o técnico responsável pelo chamado pode adicionar uma solução.");
            }
            if (string.IsNullOrWhiteSpace(solucao))
            {
                throw new InvalidOperationException("A descrição da solução não pode estar vazia.");
            }

            this.SolucaoAplicada = solucao;
            this.Status = StatusChamado.PendenteAceite;
        }

        // Cole este método dentro da sua classe Chamado
        public void ConfirmarSolucao(int colaboradorId)
        {
            // Regra 1: Apenas o colaborador que abriu o chamado pode aceitar a solução.
            if (this.ColaboradorId != colaboradorId)
            {
                throw new InvalidOperationException("Apenas o solicitante original pode fechar o chamado.");
            }

            // Regra 2: O chamado deve estar aguardando o aceite do cliente.
            if (this.Status != StatusChamado.PendenteAceite)
            {
                throw new InvalidOperationException("Este chamado não está aguardando aceite da solução.");
            }

            this.Status = StatusChamado.Fechado;
            this.DataFechamento = DateTime.Now;
            // this.AceiteColaborador = true; // Se você tiver um campo para auditar o aceite.
        }
        
        // Cole este método dentro da sua classe Chamado
        public void RejeitarSolucao(int colaboradorId, string motivo)
        {
            // Regra 1: Apenas o colaborador que abriu o chamado pode rejeitar a solução.
            if (this.ColaboradorId != colaboradorId)
            {
                throw new InvalidOperationException("Apenas o solicitante original pode rejeitar a solução.");
            }

            // Regra 2: O chamado deve estar aguardando o aceite.
            if (this.Status != StatusChamado.PendenteAceite)
            {
                throw new InvalidOperationException("Este chamado não está aguardando aceite da solução.");
            }

            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("O motivo da rejeição não pode estar em branco.");
            }

            // Ação: Voltamos o status para Em Andamento
            this.Status = StatusChamado.EmAndamento;

            // Ação: Adicionamos o histórico da rejeição na descrição para o técnico ver.
            this.Descricao += $"\n\n---\nSOLUÇÃO REJEITADA em {DateTime.Now:dd/MM/yyyy HH:mm} pelo seguinte motivo:\n\"{motivo}\"";

            // Ação: Limpamos a solução anterior para que o técnico possa inserir uma nova.
            this.SolucaoAplicada = null; 
        }

        public void AdicionarHistorico(string mensagem)
        {
            var registro = $"[{DateTime.Now:dd/MM/yyyy HH:mm}] {mensagem}";
            if (string.IsNullOrWhiteSpace(Historico))
                Historico = registro;
            else
                Historico += "\n" + registro;
        }
    }
    
}