using System.Text.RegularExpressions;

namespace SistemaIntegrado.Application.Services
{
    public interface IPerfilAcessoService
    {
        string DefinirPerfilAutomaticamente(string cargo, string setor);
        bool ValidarPerfil(string perfil);
        List<string> ObterPerfisDisponiveis();
    }

    public class PerfilAcessoService : IPerfilAcessoService
    {
        private readonly List<string> _perfisValidos = new()
        {
            "Administrador",
            "Técnico", 
            "RH",
            "Colaborador"
        };

        public string DefinirPerfilAutomaticamente(string cargo, string setor)
        {
            if (string.IsNullOrWhiteSpace(cargo) && string.IsNullOrWhiteSpace(setor))
                return "Colaborador";

            var cargoLower = cargo?.ToLowerInvariant() ?? "";
            var setorLower = setor?.ToLowerInvariant() ?? "";

            // 1. Administradores e Gestores
            if (ContemPalavras(cargoLower, new[] { "administrador", "gestor", "gerente", "diretor", "supervisor", "coordenador", "chefe" }) ||
                ContemPalavras(setorLower, new[] { "direção", "gerência", "coordenação" }))
            {
                return "Administrador";
            }

            // 2. Técnicos e Suporte
            if (ContemPalavras(cargoLower, new[] { "técnico", "suporte", "analista de suporte", "help desk", "desenvolvedor", "programador", "analista de sistemas", "infraestrutura" }) ||
                ContemPalavras(setorLower, new[] { "ti", "tecnologia", "suporte", "infraestrutura" }))
            {
                return "Técnico";
            }

            // 3. RH e Recursos Humanos
            if (ContemPalavras(cargoLower, new[] { "rh", "recursos humanos", "analista rh", "assistente rh", "coordenador rh", "gerente rh" }) ||
                ContemPalavras(setorLower, new[] { "rh", "recursos humanos", "pessoal" }))
            {
                return "RH";
            }

            // 4. Padrão: Colaborador
            return "Colaborador";
        }

        public bool ValidarPerfil(string perfil)
        {
            return _perfisValidos.Contains(perfil, StringComparer.OrdinalIgnoreCase);
        }

        public List<string> ObterPerfisDisponiveis()
        {
            return new List<string>(_perfisValidos);
        }

        private bool ContemPalavras(string texto, string[] palavras)
        {
            return palavras.Any(palavra => texto.Contains(palavra, StringComparison.OrdinalIgnoreCase));
        }
    }
} 