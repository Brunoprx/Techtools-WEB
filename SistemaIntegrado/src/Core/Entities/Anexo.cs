using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaIntegrado.Domain.Entities
{
    [Table("anexo")]
    public class Anexo
    {
        [Key]
        [Column("id_anexo")]
        public int Id { get; set; }

        [Column("nome_arquivo")]
        public string? NomeArquivo { get; set; }

        [Column("caminho_arquivo")]
        public string? CaminhoArquivo { get; set; }

        [Column("id_chamado")]
        public int ChamadoId { get; set; } // Chave estrangeira

        [Column("id_empresa")]
        public int EmpresaId { get; set; }

        public Chamado? Chamado { get; set; }
    }
}