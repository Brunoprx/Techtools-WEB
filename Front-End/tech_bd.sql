-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 10/06/2025 às 00:22
-- Versão do servidor: 10.4.28-MariaDB
-- Versão do PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `tech_bd`
--

-- --------------------------------------------------------

--
-- Estrutura para tabela `anexo`
--

CREATE TABLE `anexo` (
  `id_anexo` int(11) NOT NULL,
  `nome_arquivo` varchar(255) DEFAULT NULL,
  `caminho_arquivo` text DEFAULT NULL,
  `id_chamado` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `anexo`
--

INSERT INTO `anexo` (`id_anexo`, `nome_arquivo`, `caminho_arquivo`, `id_chamado`) VALUES
(1, 'erro_sistema.png', '/uploads/erro_sistema.png', 1);

-- --------------------------------------------------------

--
-- Estrutura para tabela `chamado`
--

CREATE TABLE `chamado` (
  `id_chamado` int(11) NOT NULL,
  `titulo` varchar(100) NOT NULL,
  `descricao` text NOT NULL,
  `categoria` varchar(50) NOT NULL,
  `urgencia` varchar(20) NOT NULL,
  `prioridade` varchar(20) DEFAULT NULL,
  `status` varchar(20) NOT NULL DEFAULT 'aberto',
  `data_abertura` datetime DEFAULT current_timestamp(),
  `data_encerramento` datetime DEFAULT NULL,
  `solucao_sugerida` text DEFAULT NULL,
  `solucao_final` text DEFAULT NULL,
  `id_Usuario` int(11) DEFAULT NULL,
  `id_tecnico` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `chamado`
--

INSERT INTO `chamado` (`id_chamado`, `titulo`, `descricao`, `categoria`, `urgencia`, `prioridade`, `status`, `data_abertura`, `data_encerramento`, `solucao_sugerida`, `solucao_final`, `id_Usuario`, `id_tecnico`) VALUES
(1, 'Erro no sistema', 'Sistema não inicia ao clicar', 'Sistema', 'Alta', 'Crítica', 'fechado', '2025-05-12 16:30:41', '2025-05-13 16:09:51', NULL, 'Foi identificado um problema de configuração no servidor. Realizada a correção no arquivo de configuração e reiniciado o serviço.', 1, NULL),
(2, 'Erro de login', 'Não consigo acessar o sistema', 'Acesso', 'Média', 'Alta', 'aberto', '2025-05-12 16:30:41', NULL, NULL, NULL, 1, 2),
(3, 'Problema de senha', 'Esqueci minha senha do sistema e não consigo acessar', 'Acesso', 'Média', 'Alta', 'aberto', '2025-05-12 16:50:12', NULL, 'Clique em \"Esqueci minha senha\" na tela de login para redefinir.', NULL, 1, NULL),
(7, 'Sistema inoperante', 'O sistema com problemas críticos, todos os módulos estão fora do ar. O sistema com problema afeta toda a operação.', 'Sistema', 'alta', 'crítica', 'aberto', '2025-05-13 16:03:52', NULL, NULL, NULL, 2, NULL),
(9, 'Erro de login', 'Não consigo acessar o sistema', 'Acesso', 'alta', NULL, 'aberto', '2025-05-13 16:11:41', NULL, NULL, NULL, 2, NULL),
(10, 'Problema no Relatório', 'Não consigo gerar relatório de vendas', 'Sistema', 'média', NULL, 'aberto', '2025-04-10 14:25:00', NULL, NULL, NULL, 2, NULL),
(11, 'Computador lento', 'PC extremamente lento após atualização', 'Hardware', 'baixa', NULL, 'aberto', '2025-04-15 09:12:00', NULL, NULL, NULL, 6, NULL),
(12, 'Problema na VPN', 'Não consigo conectar na VPN corporativa', 'Rede', 'alta', NULL, 'aberto', '2025-04-22 11:30:00', NULL, NULL, NULL, 7, NULL);

-- --------------------------------------------------------

--
-- Estrutura para tabela `eventofolha`
--

CREATE TABLE `eventofolha` (
  `id_evento` int(11) NOT NULL,
  `id_folha` int(11) DEFAULT NULL,
  `tipo_evento` varchar(50) DEFAULT NULL,
  `valor` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `eventofolha`
--

INSERT INTO `eventofolha` (`id_evento`, `id_folha`, `tipo_evento`, `valor`) VALUES
(1, 1, 'Hora Extra', 150.00),
(2, 1, 'Falta', -200.00);

-- --------------------------------------------------------

--
-- Estrutura para tabela `folhapagamento`
--

CREATE TABLE `folhapagamento` (
  `id_folha` int(11) NOT NULL,
  `id_Usuario` int(11) DEFAULT NULL,
  `mes` int(11) NOT NULL,
  `ano` int(11) NOT NULL,
  `salario_bruto` decimal(10,2) NOT NULL,
  `desconto_inss` decimal(10,2) NOT NULL,
  `desconto_irrf` decimal(10,2) NOT NULL,
  `salario_liquido` decimal(10,2) NOT NULL,
  `pdf_holerite` text DEFAULT NULL,
  `salario_base` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `folhapagamento`
--

INSERT INTO `folhapagamento` (`id_folha`, `id_Usuario`, `mes`, `ano`, `salario_bruto`, `desconto_inss`, `desconto_irrf`, `salario_liquido`, `pdf_holerite`, `salario_base`) VALUES
(1, 1, 5, 2025, 3000.00, 330.00, 100.00, 2570.00, 'holerites/2025_05_usuario1.pdf', 3000.00),
(2, 2, 4, 2025, 2500.00, 275.00, 90.00, 2135.00, '/holerites/holerite_carlos_abril2025.pdf', 2500.00),
(3, 3, 4, 2025, 3200.00, 352.00, 120.00, 2728.00, '/holerites/holerite_maria_abril2025.pdf', 3200.00),
(4, 1, 5, 2025, 5500.00, 550.00, 750.00, 4200.00, NULL, 5000.00);

-- --------------------------------------------------------

--
-- Estrutura para tabela `registroponto`
--

CREATE TABLE `registroponto` (
  `id_registro` int(11) NOT NULL,
  `id_Usuario` int(11) DEFAULT NULL,
  `data` date NOT NULL,
  `entrada` time NOT NULL,
  `almoco_saida` time DEFAULT NULL,
  `almoco_retorno` time DEFAULT NULL,
  `saida` time NOT NULL,
  `status` varchar(20) NOT NULL,
  `justificativa` text DEFAULT NULL,
  `validado_por_lideranca` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `registroponto`
--

INSERT INTO `registroponto` (`id_registro`, `id_Usuario`, `data`, `entrada`, `almoco_saida`, `almoco_retorno`, `saida`, `status`, `justificativa`, `validado_por_lideranca`) VALUES
(1, 1, '2025-05-12', '08:10:00', '12:00:00', '13:00:00', '17:00:00', 'completo', NULL, 1),
(2, 2, '2025-05-12', '08:40:00', '12:10:00', '13:10:00', '17:30:00', 'atraso', 'Trânsito intenso', 1);

-- --------------------------------------------------------

--
-- Estrutura para tabela `usuarios`
--

CREATE TABLE `usuarios` (
  `id_Usuario` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `senha` varchar(100) NOT NULL,
  `cpf` varchar(14) NOT NULL,
  `cargo` varchar(50) DEFAULT NULL,
  `setor` varchar(50) DEFAULT NULL,
  `banco` varchar(50) DEFAULT NULL,
  `tipo_contrato` varchar(50) DEFAULT NULL,
  `perfil_acesso` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `usuarios`
--

INSERT INTO `usuarios` (`id_Usuario`, `nome`, `email`, `senha`, `cpf`, `cargo`, `setor`, `banco`, `tipo_contrato`, `perfil_acesso`) VALUES
(1, 'Bruno Silva', 'bruno@empresa.com', '123456', '12345678900', 'Analista', 'TI', 'Bradesco', 'CLT', 'Administrador'),
(2, 'Carlos Técnico', 'carlos@empresa.com', 'tecnico123', '98765432100', 'Técnico de Suporte', 'TI', 'Caixa', 'CLT', 'Técnico'),
(3, 'Maria RH', 'maria@empresa.com', 'rhsenha', '11122233344', 'Recursos Humanos', 'RH', 'Itaú', 'CLT', 'RH'),
(6, 'Fernanda RH', 'fernanda@empresa.com', 'rh2025', '44455566677', 'Analista RH', 'RH', 'Santander', 'CLT', 'RH'),
(7, 'João Gestor', 'joao@empresa.com', 'gestor456', '55566677788', 'Gestor de TI', 'TI', 'Banco do Brasil', 'PJ', 'Administrador'),
(8, 'Usuário Teste', 'teste@empresa.com', '1234', '12345678901', 'Analista', 'TI', 'Bradesco', 'CLT', 'Colaborador');

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `anexo`
--
ALTER TABLE `anexo`
  ADD PRIMARY KEY (`id_anexo`),
  ADD KEY `id_chamado` (`id_chamado`);

--
-- Índices de tabela `chamado`
--
ALTER TABLE `chamado`
  ADD PRIMARY KEY (`id_chamado`),
  ADD KEY `id_Usuario` (`id_Usuario`),
  ADD KEY `id_tecnico` (`id_tecnico`);

--
-- Índices de tabela `eventofolha`
--
ALTER TABLE `eventofolha`
  ADD PRIMARY KEY (`id_evento`),
  ADD KEY `id_folha` (`id_folha`);

--
-- Índices de tabela `folhapagamento`
--
ALTER TABLE `folhapagamento`
  ADD PRIMARY KEY (`id_folha`),
  ADD KEY `id_Usuario` (`id_Usuario`);

--
-- Índices de tabela `registroponto`
--
ALTER TABLE `registroponto`
  ADD PRIMARY KEY (`id_registro`),
  ADD KEY `id_Usuario` (`id_Usuario`);

--
-- Índices de tabela `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_Usuario`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `cpf` (`cpf`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `anexo`
--
ALTER TABLE `anexo`
  MODIFY `id_anexo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de tabela `chamado`
--
ALTER TABLE `chamado`
  MODIFY `id_chamado` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de tabela `eventofolha`
--
ALTER TABLE `eventofolha`
  MODIFY `id_evento` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de tabela `folhapagamento`
--
ALTER TABLE `folhapagamento`
  MODIFY `id_folha` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de tabela `registroponto`
--
ALTER TABLE `registroponto`
  MODIFY `id_registro` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de tabela `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_Usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `anexo`
--
ALTER TABLE `anexo`
  ADD CONSTRAINT `anexo_ibfk_1` FOREIGN KEY (`id_chamado`) REFERENCES `chamado` (`id_chamado`);

--
-- Restrições para tabelas `chamado`
--
ALTER TABLE `chamado`
  ADD CONSTRAINT `chamado_ibfk_1` FOREIGN KEY (`id_Usuario`) REFERENCES `usuarios` (`id_Usuario`),
  ADD CONSTRAINT `chamado_ibfk_2` FOREIGN KEY (`id_tecnico`) REFERENCES `usuarios` (`id_Usuario`);

--
-- Restrições para tabelas `eventofolha`
--
ALTER TABLE `eventofolha`
  ADD CONSTRAINT `eventofolha_ibfk_1` FOREIGN KEY (`id_folha`) REFERENCES `folhapagamento` (`id_folha`);

--
-- Restrições para tabelas `folhapagamento`
--
ALTER TABLE `folhapagamento`
  ADD CONSTRAINT `folhapagamento_ibfk_1` FOREIGN KEY (`id_Usuario`) REFERENCES `usuarios` (`id_Usuario`);

--
-- Restrições para tabelas `registroponto`
--
ALTER TABLE `registroponto`
  ADD CONSTRAINT `registroponto_ibfk_1` FOREIGN KEY (`id_Usuario`) REFERENCES `usuarios` (`id_Usuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

-- Deletar artigos de teste
DELETE FROM artigos_base_conhecimento WHERE id IN (5, 6, 7, 8, 9);

-- Inserir artigos reais para a base de conhecimento
INSERT INTO artigos_base_conhecimento (titulo, categoria, conteudo, tags, autor_id, data_criacao, data_atualizacao, visualizacoes, avaliacao, imagem_url) VALUES
('Como Abrir um Chamado Técnico', 'Processos', 
'Para abrir um chamado técnico no sistema:\n\n1. Acesse o portal de suporte\n2. Clique em "Novo Chamado"\n3. Preencha as informações obrigatórias:\n   - Título do problema\n   - Descrição detalhada\n   - Categoria (Hardware/Software/Rede)\n   - Prioridade\n4. Anexe screenshots se necessário\n5. Clique em "Enviar"\n\nO chamado será automaticamente direcionado para o técnico responsável pela categoria selecionada.',
'chamado,suporte,processo', 1, NOW(), NOW(), 0, 0, NULL),

('Diagnóstico Básico de Hardware', 'Hardware',
'Passos para diagnóstico de problemas de hardware:\n\n1. Verificar se o equipamento está ligado\n2. Testar conectores e cabos\n3. Verificar indicadores LED\n4. Executar diagnóstico do sistema\n5. Verificar temperatura e ventilação\n6. Testar componentes individuais\n\nSintomas comuns:\n- Tela azul: Problema de memória ou driver\n- Não liga: Fonte de alimentação ou placa-mãe\n- Travamentos: Superaquecimento ou HD com problema',
'diagnostico,hardware,troubleshooting', 1, NOW(), NOW(), 0, 0, NULL),

('Instalação de Software Padrão', 'Software',
'Procedimento para instalação de software padrão na empresa:\n\n1. Verificar compatibilidade do sistema\n2. Fazer backup dos dados importantes\n3. Desinstalar versões anteriores\n4. Baixar versão oficial do site\n5. Executar como administrador\n6. Seguir o assistente de instalação\n7. Configurar conforme políticas da empresa\n8. Testar funcionalidade básica\n\nSoftwares padrão:\n- Microsoft Office\n- Antivírus corporativo\n- VPN da empresa\n- Ferramentas de desenvolvimento',
'instalacao,software,procedimento', 1, NOW(), NOW(), 0, 0, NULL),

('Configuração de VPN Corporativa', 'Rede',
'Como configurar a VPN corporativa:\n\n1. Baixar cliente VPN oficial\n2. Instalar com permissões de administrador\n3. Configurar servidor VPN:\n   - Endereço: vpn.empresa.com\n   - Protocolo: OpenVPN\n   - Porta: 1194\n4. Importar certificado digital\n5. Configurar credenciais de acesso\n6. Testar conectividade\n7. Verificar acesso aos recursos internos\n\nTroubleshooting:\n- Verificar firewall\n- Testar conectividade de internet\n- Reinstalar certificados se necessário',
'vpn,rede,configuracao', 1, NOW(), NOW(), 0, 0, NULL),

('SLA de Atendimento - Prazos', 'Processos',
'Prazos de atendimento conforme SLA:\n\nPRIORIDADE ALTA (Crítico):\n- Tempo de resposta: 1 hora\n- Tempo de resolução: 4 horas\n- Exemplos: Sistema principal fora do ar, segurança comprometida\n\nPRIORIDADE MÉDIA (Importante):\n- Tempo de resposta: 4 horas\n- Tempo de resolução: 24 horas\n- Exemplos: Problemas de produtividade, funcionalidades essenciais\n\nPRIORIDADE BAIXA (Normal):\n- Tempo de resposta: 24 horas\n- Tempo de resolução: 72 horas\n- Exemplos: Melhorias, dúvidas gerais\n\nEscalação automática após 80% do tempo limite.',
'sla,prazos,atendimento', 1, NOW(), NOW(), 0, 0, NULL),

('Remoção de Malware - Procedimento', 'Segurança',
'Procedimento para remoção de malware:\n\n1. Isolar o equipamento da rede\n2. Identificar o tipo de malware\n3. Executar antivírus em modo seguro\n4. Remover arquivos suspeitos\n5. Limpar registros do sistema\n6. Atualizar antivírus e sistema\n7. Verificar integridade dos dados\n8. Reintegrar à rede gradualmente\n\nFerramentas recomendadas:\n- Malwarebytes\n- Windows Defender Offline\n- RKill\n- AdwCleaner\n\nPrevenção:\n- Manter sistemas atualizados\n- Não abrir anexos suspeitos\n- Usar navegação segura',
'malware,seguranca,limpeza', 1, NOW(), NOW(), 0, 0, NULL);

-- Criar novo usuário técnico para teste de autorização
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Técnico Teste', 'tecnico.teste@empresa.com', '123456', '99988877766', 'Técnico de Suporte', 'TI', 'Bradesco', 'CLT', 'Técnico');

-- Verificar se foi criado corretamente
SELECT id_Usuario, nome, email, perfil_acesso FROM usuarios WHERE email = 'tecnico.teste@empresa.com';
