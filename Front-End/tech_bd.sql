-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 03/08/2025 às 19:18
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
  `id_empresa` int(11) NOT NULL,
  `nome_arquivo` varchar(255) DEFAULT NULL,
  `caminho_arquivo` text DEFAULT NULL,
  `id_chamado` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `anexo`
--

INSERT INTO `anexo` (`id_anexo`, `id_empresa`, `nome_arquivo`, `caminho_arquivo`, `id_chamado`) VALUES
(2, 1, '1322308.jpeg', '/uploads/solucoes/d6b6dbd2-730b-4743-9232-5bd7d1accf9c.jpeg', 4),
(3, 1, 'alebbb3ho4c51.webp', '/uploads/solucoes/6c53c5df-1766-4b08-b135-d68b7ca93780.webp', 5),
(4, 1, 'avatar.jpg', '/uploads/solucoes/720b7c5e-b3bc-41c8-b464-9444d9f2864e.jpg', 5),
(5, 1, 'Captura de tela 2025-04-26 194755.png', '/uploads/anexos/bf1be599-8f18-483d-96d0-4b536dc11141.png', 5),
(6, 1, 'Captura de tela 2025-04-28 172211.png', '/uploads/anexos/c73f4e66-949d-4182-a544-9261be6ee09c.png', 7),
(7, 1, 'Captura de tela 2025-05-02 190137.png', '/uploads/solucoes/47b4ad02-e5d6-4d6e-a1a6-ce0963dda535.png', 7);

-- --------------------------------------------------------

--
-- Estrutura para tabela `artigos_base_conhecimento`
--

CREATE TABLE `artigos_base_conhecimento` (
  `id` int(11) NOT NULL,
  `titulo` varchar(255) NOT NULL,
  `imagem_url` varchar(255) DEFAULT NULL,
  `conteudo` text NOT NULL,
  `categoria` varchar(100) NOT NULL,
  `tags` varchar(255) DEFAULT NULL,
  `autor_id` int(11) DEFAULT NULL,
  `data_criacao` datetime DEFAULT current_timestamp(),
  `data_atualizacao` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `visualizacoes` int(11) DEFAULT 0,
  `avaliacao` float DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `chamado`
--

CREATE TABLE `chamado` (
  `id_chamado` int(11) NOT NULL,
  `id_empresa` int(11) NOT NULL,
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
  `id_tecnico` int(11) DEFAULT NULL,
  `historico` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `chamado`
--

INSERT INTO `chamado` (`id_chamado`, `id_empresa`, `titulo`, `descricao`, `categoria`, `urgencia`, `prioridade`, `status`, `data_abertura`, `data_encerramento`, `solucao_sugerida`, `solucao_final`, `id_Usuario`, `id_tecnico`, `historico`) VALUES
(2, 1, 'teste', 'teste', 'hardware', 'baixa', 'Baixa', 'Fechado', '2025-07-31 16:36:14', '2025-07-31 17:02:05', NULL, 'solução teste', 17, 19, '[31/07/2025 16:36] Prazo máximo de atendimento definido: 48 horas (Prioridade: Baixa).\n[31/07/2025 16:36] Chamado aberto por colaborador ID 17.\n[31/07/2025 17:01] Chamado aceito pelo técnico ID 19.\n[31/07/2025 17:01] Solução adicionada pelo técnico ID 19.\n[31/07/2025 17:02] Solução aceita pelo colaborador ID 17. Chamado fechado.'),
(3, 1, 'teste img', 'img', 'hardware', 'baixa', 'Baixa', 'EmAndamento', '2025-07-31 17:06:35', NULL, NULL, NULL, 17, 19, '[31/07/2025 17:06] Prazo máximo de atendimento definido: 48 horas (Prioridade: Baixa).\n[31/07/2025 17:06] Chamado aberto por colaborador ID 17.\n[31/07/2025 17:06] Chamado atribuído automaticamente ao técnico ID 19.'),
(4, 1, 'teste anexo imagem', 'img', 'hardware', 'baixa', 'Baixa', 'EmAndamento', '2025-07-31 17:13:52', NULL, NULL, NULL, 17, 19, '[31/07/2025 17:13] Prazo máximo de atendimento definido: 48 horas (Prioridade: Baixa).\n[31/07/2025 17:13] Chamado aberto por colaborador ID 17.\n[31/07/2025 17:13] Chamado atribuído automaticamente ao técnico ID 19.'),
(5, 1, 't', 't', 'hardware', 'baixa', 'Baixa', 'PendenteAceite', '2025-08-01 12:54:44', NULL, NULL, 'solução img', 17, 19, '[01/08/2025 12:54] Prazo máximo de atendimento definido: 48 horas (Prioridade: Baixa).\n[01/08/2025 12:54] Chamado aberto por colaborador ID 17.\n[01/08/2025 12:54] Chamado atribuído automaticamente ao técnico ID 19.\n[01/08/2025 13:05] Chamado aceito pelo técnico ID 19.\n[01/08/2025 13:05] Solução adicionada pelo técnico ID 19.'),
(6, 1, 't2', 't2', 'hardware', 'baixa', 'Baixa', 'EmAndamento', '2025-08-01 13:30:36', NULL, NULL, NULL, 17, 19, '[01/08/2025 13:30] Prazo máximo de atendimento definido: 48 horas (Prioridade: Baixa).\n[01/08/2025 13:30] Chamado aberto por colaborador ID 17.\n[01/08/2025 13:30] Chamado atribuído automaticamente ao técnico ID 19.'),
(7, 1, 'vai', 'vad', 'hardware', 'baixa', 'Baixa', 'Fechado', '2025-08-01 14:04:09', '2025-08-01 14:06:14', NULL, 'vai', 17, 19, '[01/08/2025 14:04] Prazo máximo de atendimento definido: 48 horas (Prioridade: Baixa).\n[01/08/2025 14:04] Chamado aberto por colaborador ID 17.\n[01/08/2025 14:04] Chamado atribuído automaticamente ao técnico ID 19.\n[01/08/2025 14:04] Chamado aceito pelo técnico ID 19.\n[01/08/2025 14:05] Solução adicionada pelo técnico ID 19.\n[01/08/2025 14:06] Solução aceita pelo colaborador ID 17. Chamado fechado.');

-- --------------------------------------------------------

--
-- Estrutura para tabela `empresas`
--

CREATE TABLE `empresas` (
  `id_empresa` int(11) NOT NULL,
  `nome_empresa` varchar(100) NOT NULL,
  `data_cadastro` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `empresas`
--

INSERT INTO `empresas` (`id_empresa`, `nome_empresa`, `data_cadastro`) VALUES
(1, 'Empresa A', '2025-07-27 14:10:49'),
(2, 'Empresa B', '2025-07-27 14:10:49');

-- --------------------------------------------------------

--
-- Estrutura para tabela `eventofolha`
--

CREATE TABLE `eventofolha` (
  `id_evento` int(11) NOT NULL,
  `id_empresa` int(11) NOT NULL,
  `id_folha` int(11) DEFAULT NULL,
  `tipo_evento` varchar(50) DEFAULT NULL,
  `valor` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `folhapagamento`
--

CREATE TABLE `folhapagamento` (
  `id_folha` int(11) NOT NULL,
  `id_empresa` int(11) NOT NULL,
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

-- --------------------------------------------------------

--
-- Estrutura para tabela `registroponto`
--

CREATE TABLE `registroponto` (
  `id_registro` int(11) NOT NULL,
  `id_empresa` int(11) NOT NULL,
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

-- --------------------------------------------------------

--
-- Estrutura para tabela `tecnico_especialidade`
--

CREATE TABLE `tecnico_especialidade` (
  `id_usuario` int(11) NOT NULL,
  `categoria_especialidade` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `tecnico_especialidade`
--

INSERT INTO `tecnico_especialidade` (`id_usuario`, `categoria_especialidade`) VALUES
(19, 'Hardware'),
(19, 'Rede'),
(19, 'Sistema'),
(19, 'Software');

-- --------------------------------------------------------

--
-- Estrutura para tabela `usuarios`
--

CREATE TABLE `usuarios` (
  `id_Usuario` int(11) NOT NULL,
  `id_empresa` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `senha` varchar(100) NOT NULL,
  `cpf` varchar(14) NOT NULL,
  `cargo` varchar(50) DEFAULT NULL,
  `setor` varchar(50) DEFAULT NULL,
  `banco` varchar(50) DEFAULT NULL,
  `tipo_contrato` varchar(50) DEFAULT NULL,
  `perfil_acesso` varchar(20) NOT NULL,
  `status` varchar(50) NOT NULL DEFAULT 'Ativo'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `usuarios`
--

INSERT INTO `usuarios` (`id_Usuario`, `id_empresa`, `nome`, `email`, `senha`, `cpf`, `cargo`, `setor`, `banco`, `tipo_contrato`, `perfil_acesso`, `status`) VALUES
(1, 1, 'Admin Empresa A', 'admin.a@empresa.com', '123456', '11111111111', 'Administrador', 'TI', 'Bradesco', 'CLT', 'Administrador', 'Ativo'),
(2, 2, 'Admin Empresa B', 'admin.b@empresa.com', '123456', '22222222222', 'Administrador', 'TI', 'Itaú', 'CLT', 'Administrador', 'Ativo'),
(17, 1, 'Colaborador A', 'colaborador.a@empresa.com', '1234', '33333333333', 'Colaborador', 'TI', NULL, NULL, 'Colaborador', 'Ativo'),
(19, 1, 'Técnico A', 'tecnico.a@empresa.com', '1234', '55555555555', 'Técnico', 'TI', NULL, NULL, 'Técnico', 'Ativo');

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `anexo`
--
ALTER TABLE `anexo`
  ADD PRIMARY KEY (`id_anexo`),
  ADD KEY `id_chamado` (`id_chamado`),
  ADD KEY `fk_anexo_empresa` (`id_empresa`);

--
-- Índices de tabela `artigos_base_conhecimento`
--
ALTER TABLE `artigos_base_conhecimento`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idx_artigos_titulo` (`titulo`),
  ADD KEY `idx_artigos_categoria` (`categoria`),
  ADD KEY `idx_artigos_tags` (`tags`),
  ADD KEY `fk_artigo_autor` (`autor_id`);

--
-- Índices de tabela `chamado`
--
ALTER TABLE `chamado`
  ADD PRIMARY KEY (`id_chamado`),
  ADD KEY `id_Usuario` (`id_Usuario`),
  ADD KEY `id_tecnico` (`id_tecnico`),
  ADD KEY `fk_chamado_empresa` (`id_empresa`);

--
-- Índices de tabela `empresas`
--
ALTER TABLE `empresas`
  ADD PRIMARY KEY (`id_empresa`);

--
-- Índices de tabela `eventofolha`
--
ALTER TABLE `eventofolha`
  ADD PRIMARY KEY (`id_evento`),
  ADD KEY `id_folha` (`id_folha`),
  ADD KEY `fk_eventofolha_empresa` (`id_empresa`);

--
-- Índices de tabela `folhapagamento`
--
ALTER TABLE `folhapagamento`
  ADD PRIMARY KEY (`id_folha`),
  ADD KEY `id_Usuario` (`id_Usuario`),
  ADD KEY `fk_folha_empresa` (`id_empresa`);

--
-- Índices de tabela `registroponto`
--
ALTER TABLE `registroponto`
  ADD PRIMARY KEY (`id_registro`),
  ADD KEY `id_Usuario` (`id_Usuario`),
  ADD KEY `fk_registroponto_empresa` (`id_empresa`);

--
-- Índices de tabela `tecnico_especialidade`
--
ALTER TABLE `tecnico_especialidade`
  ADD PRIMARY KEY (`id_usuario`,`categoria_especialidade`);

--
-- Índices de tabela `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_Usuario`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `cpf` (`cpf`),
  ADD KEY `fk_usuarios_empresa` (`id_empresa`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `anexo`
--
ALTER TABLE `anexo`
  MODIFY `id_anexo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de tabela `artigos_base_conhecimento`
--
ALTER TABLE `artigos_base_conhecimento`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `chamado`
--
ALTER TABLE `chamado`
  MODIFY `id_chamado` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de tabela `empresas`
--
ALTER TABLE `empresas`
  MODIFY `id_empresa` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de tabela `eventofolha`
--
ALTER TABLE `eventofolha`
  MODIFY `id_evento` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `folhapagamento`
--
ALTER TABLE `folhapagamento`
  MODIFY `id_folha` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `registroponto`
--
ALTER TABLE `registroponto`
  MODIFY `id_registro` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_Usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `anexo`
--
ALTER TABLE `anexo`
  ADD CONSTRAINT `anexo_ibfk_1` FOREIGN KEY (`id_chamado`) REFERENCES `chamado` (`id_chamado`),
  ADD CONSTRAINT `fk_anexo_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `empresas` (`id_empresa`);

--
-- Restrições para tabelas `artigos_base_conhecimento`
--
ALTER TABLE `artigos_base_conhecimento`
  ADD CONSTRAINT `fk_artigo_autor` FOREIGN KEY (`autor_id`) REFERENCES `usuarios` (`id_Usuario`);

--
-- Restrições para tabelas `chamado`
--
ALTER TABLE `chamado`
  ADD CONSTRAINT `chamado_ibfk_1` FOREIGN KEY (`id_Usuario`) REFERENCES `usuarios` (`id_Usuario`),
  ADD CONSTRAINT `chamado_ibfk_2` FOREIGN KEY (`id_tecnico`) REFERENCES `usuarios` (`id_Usuario`),
  ADD CONSTRAINT `fk_chamado_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `empresas` (`id_empresa`);

--
-- Restrições para tabelas `eventofolha`
--
ALTER TABLE `eventofolha`
  ADD CONSTRAINT `eventofolha_ibfk_1` FOREIGN KEY (`id_folha`) REFERENCES `folhapagamento` (`id_folha`),
  ADD CONSTRAINT `fk_eventofolha_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `empresas` (`id_empresa`);

--
-- Restrições para tabelas `folhapagamento`
--
ALTER TABLE `folhapagamento`
  ADD CONSTRAINT `fk_folha_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `empresas` (`id_empresa`),
  ADD CONSTRAINT `folhapagamento_ibfk_1` FOREIGN KEY (`id_Usuario`) REFERENCES `usuarios` (`id_Usuario`);

--
-- Restrições para tabelas `registroponto`
--
ALTER TABLE `registroponto`
  ADD CONSTRAINT `fk_registroponto_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `empresas` (`id_empresa`),
  ADD CONSTRAINT `registroponto_ibfk_1` FOREIGN KEY (`id_Usuario`) REFERENCES `usuarios` (`id_Usuario`);

--
-- Restrições para tabelas `tecnico_especialidade`
--
ALTER TABLE `tecnico_especialidade`
  ADD CONSTRAINT `tecnico_especialidade_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_Usuario`);

--
-- Restrições para tabelas `usuarios`
--
ALTER TABLE `usuarios`
  ADD CONSTRAINT `fk_usuarios_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `empresas` (`id_empresa`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
