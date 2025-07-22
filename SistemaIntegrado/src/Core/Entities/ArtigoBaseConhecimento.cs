using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaIntegrado.Domain.Entities
{
    [Table("artigos_base_conhecimento")]
    public class ArtigoBaseConhecimento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Column("conteudo")]
        public string Conteudo { get; set; } = string.Empty;

        [Column("categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Column("tags")]
        public string? Tags { get; set; }

        [Column("imagem_url")]
        public string? ImagemUrl { get; set; }

        [Column("autor_id")]
        public int? AutorId { get; set; }

        [Column("id_empresa")]
        public int EmpresaId { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("data_atualizacao")]
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        [Column("visualizacoes")]
        public int Visualizacoes { get; set; } = 0;

        [Column("avaliacao")]
        public float Avaliacao { get; set; } = 0;
    }
} 