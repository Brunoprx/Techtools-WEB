using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaIntegrado.Domain.Entities
{
    [Table("usuarios")]
    public class Colaborador
    {
        [Key]
        [Column("id_Usuario")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("email")]
        public string EmailCorporativo { get; set; } = string.Empty;

        [Column("senha")]
        public string Senha { get; set; } = string.Empty;

        [Column("cpf")]
        public string Cpf { get; set; } = string.Empty;

        [Column("cargo")]
        public string? Cargo { get; set; }

        [Column("setor")]
        public string? Setor { get; set; }

        [Column("banco")]
        public string? Banco { get; set; }

        [Column("tipo_contrato")]
        public string? TipoContrato { get; set; }

        [Column("perfil_acesso")]
        public string PerfilAcesso { get; set; } = string.Empty;

        [Column("id_empresa")]
        public int EmpresaId { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Ativo";
    }
}