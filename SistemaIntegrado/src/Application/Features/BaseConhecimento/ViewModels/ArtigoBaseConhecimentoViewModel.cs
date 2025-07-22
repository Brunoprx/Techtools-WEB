using System;

namespace SistemaIntegrado.Application.Features.BaseConhecimento.ViewModels
{
    public class ArtigoBaseConhecimentoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string? Tags { get; set; }
        public int? AutorId { get; set; }
        public string? NomeAutor { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int Visualizacoes { get; set; }
        public float Avaliacao { get; set; }
        public string? ImagemUrl { get; set; }
    }

    public class ArtigoBaseConhecimentoResumoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string? Tags { get; set; }
        public string? NomeAutor { get; set; }
        public DateTime DataCriacao { get; set; }
        public int Visualizacoes { get; set; }
        public float Avaliacao { get; set; }
        public string? ImagemUrl { get; set; }
    }
} 