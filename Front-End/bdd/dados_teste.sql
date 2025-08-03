-- Script para inserir dados de teste no banco Techtools
-- Execute este script após importar o tech_bd.sql

-- Inserir usuários de teste
INSERT INTO `usuarios` (`id_empresa`, `nome`, `email`, `senha`, `cpf`, `cargo`, `setor`, `banco`, `tipo_contrato`, `perfil_acesso`, `status`) VALUES
(1, 'João Silva', 'joao.silva@empresa.com', '123456', '12345678901', 'Desenvolvedor', 'TI', 'Bradesco', 'CLT', 'Colaborador', 'Ativo'),
(1, 'Maria Santos', 'maria.santos@empresa.com', '123456', '12345678902', 'Analista', 'RH', 'Itaú', 'CLT', 'Colaborador', 'Ativo'),
(1, 'Pedro Oliveira', 'pedro.oliveira@empresa.com', '123456', '12345678903', 'Técnico', 'TI', 'Santander', 'CLT', 'Técnico', 'Ativo'),
(1, 'Ana Costa', 'ana.costa@empresa.com', '123456', '12345678904', 'Gestor', 'TI', 'Bradesco', 'CLT', 'Gestor', 'Ativo'),
(2, 'Carlos Lima', 'carlos.lima@empresa.com', '123456', '12345678905', 'Desenvolvedor', 'TI', 'Itaú', 'CLT', 'Colaborador', 'Ativo'),
(2, 'Fernanda Rocha', 'fernanda.rocha@empresa.com', '123456', '12345678906', 'Técnica', 'TI', 'Santander', 'CLT', 'Técnico', 'Ativo');

-- Inserir chamados de teste
INSERT INTO `chamado` (`id_empresa`, `titulo`, `descricao`, `categoria`, `urgencia`, `prioridade`, `status`, `data_abertura`, `id_Usuario`, `id_tecnico`) VALUES
(1, 'Problema com monitor', 'O monitor não está ligando, tela fica preta', 'Hardware', 'Média', 'Média', 'Aberto', '2025-01-15 09:30:00', 3, NULL),
(1, 'Instalação de antivírus', 'Preciso instalar o antivírus corporativo', 'Software', 'Alta', 'Alta', 'EmAndamento', '2025-01-14 14:20:00', 4, 5),
(1, 'Problema de conexão Wi-Fi', 'Wi-Fi está muito lento no setor de vendas', 'Rede', 'Baixa', 'Baixa', 'Fechado', '2025-01-13 11:15:00', 3, 5),
(1, 'Acesso ao sistema financeiro', 'Não consigo acessar o sistema financeiro', 'Sistema', 'Média', 'Média', 'Cancelado', '2025-01-12 16:45:00', 4, NULL),
(1, 'Impressora não funciona', 'Impressora HP não está imprimindo', 'Hardware', 'Alta', 'Alta', 'Aberto', '2025-01-16 08:00:00', 3, NULL),
(2, 'Problema com e-mail', 'Não consigo enviar e-mails', 'Software', 'Média', 'Média', 'EmAndamento', '2025-01-15 10:30:00', 7, 8),
(2, 'Configuração de VPN', 'Preciso configurar VPN para trabalho remoto', 'Rede', 'Alta', 'Alta', 'Aberto', '2025-01-16 09:15:00', 7, NULL);

-- Inserir artigos da base de conhecimento
INSERT INTO `artigos_base_conhecimento` (`titulo`, `imagem_url`, `conteudo`, `categoria`, `tags`, `autor_id`, `visualizacoes`, `avaliacao`) VALUES
('Como diagnosticar problemas de hardware', NULL, 'Guia completo para diagnóstico de problemas de hardware comuns...', 'Hardware', 'diagnóstico,hardware,problemas', 5, 45, 4.5),
('Instalação de software padrão da empresa', NULL, 'Procedimento passo a passo para instalação de softwares corporativos...', 'Software', 'instalação,software,corporate', 5, 32, 4.2),
('Configuração de rede Wi-Fi', NULL, 'Como configurar e resolver problemas de conectividade Wi-Fi...', 'Rede', 'wifi,rede,configuração', 5, 28, 4.0),
('Procedimentos de segurança', NULL, 'Protocolos de segurança e boas práticas para a equipe...', 'Segurança', 'segurança,protocolos,boas-práticas', 6, 15, 4.8),
('SLA de atendimento técnico', NULL, 'Informações sobre prazos e prioridades de atendimento...', 'Processos', 'sla,prazos,prioridades', 6, 67, 4.3);

-- Inserir especialidades dos técnicos
INSERT INTO `tecnico_especialidade` (`id_usuario`, `categoria_especialidade`) VALUES
(5, 'Hardware'),
(5, 'Software'),
(5, 'Rede'),
(8, 'Software'),
(8, 'Sistema');

-- Inserir alguns registros de ponto (opcional)
INSERT INTO `registroponto` (`id_empresa`, `id_Usuario`, `data`, `entrada`, `almoco_saida`, `almoco_retorno`, `saida`, `status`) VALUES
(1, 3, '2025-01-15', '08:00:00', '12:00:00', '13:00:00', '17:00:00', 'Normal'),
(1, 4, '2025-01-15', '08:30:00', '12:30:00', '13:30:00', '17:30:00', 'Normal'),
(2, 7, '2025-01-15', '09:00:00', '12:00:00', '13:00:00', '18:00:00', 'Normal');

-- Inserir folha de pagamento (opcional)
INSERT INTO `folhapagamento` (`id_empresa`, `id_Usuario`, `mes`, `ano`, `salario_bruto`, `desconto_inss`, `desconto_irrf`, `salario_liquido`, `salario_base`) VALUES
(1, 3, 1, 2025, 5000.00, 550.00, 375.00, 4075.00, 5000.00),
(1, 4, 1, 2025, 6000.00, 660.00, 450.00, 4890.00, 6000.00),
(2, 7, 1, 2025, 4500.00, 495.00, 337.50, 3667.50, 4500.00); 