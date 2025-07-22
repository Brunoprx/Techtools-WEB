using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaIntegrado.Domain.Entities
{
    [Table("tecnico_especialidade")]
    public class TecnicoEspecialidade
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Key]
        [Column("categoria_especialidade")]
        public string CategoriaEspecialidade { get; set; } = string.Empty;
    }
}