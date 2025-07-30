-- Script para criar usuários com perfis automáticos
-- Baseado na lógica implementada no PerfilAcessoService

-- =====================================================
-- 1. USUÁRIOS TÉCNICOS (cargo/setor relacionados a TI/suporte)
-- =====================================================

-- Técnico de Suporte
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('João Silva', 'joao.silva@empresa.com', '123456', '11122233344', 'Técnico de Suporte', 'TI', 'Bradesco', 'CLT', 'Técnico');

-- Analista de Sistemas
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Ana Costa', 'ana.costa@empresa.com', '123456', '22233344455', 'Analista de Sistemas', 'TI', 'Itaú', 'CLT', 'Técnico');

-- Desenvolvedor
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Pedro Santos', 'pedro.santos@empresa.com', '123456', '33344455566', 'Desenvolvedor', 'TI', 'Santander', 'PJ', 'Técnico');

-- Analista de Infraestrutura
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Carlos Lima', 'carlos.lima@empresa.com', '123456', '44455566677', 'Analista de Infraestrutura', 'TI', 'Caixa', 'CLT', 'Técnico');

-- =====================================================
-- 2. USUÁRIOS ADMINISTRADORES (cargo/setor de gestão)
-- =====================================================

-- Gerente de TI
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Maria Oliveira', 'maria.oliveira@empresa.com', '123456', '55566677788', 'Gerente de TI', 'TI', 'Banco do Brasil', 'CLT', 'Administrador');

-- Diretor de Tecnologia
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Roberto Almeida', 'roberto.almeida@empresa.com', '123456', '66677788899', 'Diretor de Tecnologia', 'TI', 'Bradesco', 'CLT', 'Administrador');

-- Coordenador de Suporte
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Fernanda Souza', 'fernanda.souza@empresa.com', '123456', '77788899900', 'Coordenador de Suporte', 'TI', 'Itaú', 'CLT', 'Administrador');

-- Supervisor de Sistemas
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Lucas Pereira', 'lucas.pereira@empresa.com', '123456', '88899900011', 'Supervisor de Sistemas', 'TI', 'Santander', 'CLT', 'Administrador');

-- =====================================================
-- 3. USUÁRIOS RH (cargo/setor relacionados a RH)
-- =====================================================

-- Analista de RH
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Juliana Ferreira', 'juliana.ferreira@empresa.com', '123456', '99900011122', 'Analista de RH', 'RH', 'Caixa', 'CLT', 'RH');

-- Coordenador de RH
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Ricardo Mendes', 'ricardo.mendes@empresa.com', '123456', '00011122233', 'Coordenador de RH', 'RH', 'Banco do Brasil', 'CLT', 'RH');

-- Gerente de Recursos Humanos
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Patrícia Santos', 'patricia.santos@empresa.com', '123456', '11122233344', 'Gerente de Recursos Humanos', 'RH', 'Bradesco', 'CLT', 'RH');

-- =====================================================
-- 4. USUÁRIOS COLABORADORES (outros cargos/setores)
-- =====================================================

-- Auxiliar Administrativo
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Gabriela Silva', 'gabriela.silva@empresa.com', '123456', '22233344455', 'Auxiliar Administrativo', 'Administrativo', 'Itaú', 'CLT', 'Colaborador');

-- Analista Financeiro
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Thiago Costa', 'thiago.costa@empresa.com', '123456', '33344455566', 'Analista Financeiro', 'Financeiro', 'Santander', 'CLT', 'Colaborador');

-- Vendedor
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Amanda Lima', 'amanda.lima@empresa.com', '123456', '44455566677', 'Vendedor', 'Comercial', 'Caixa', 'CLT', 'Colaborador');

-- Recepcionista
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso) VALUES
('Diego Oliveira', 'diego.oliveira@empresa.com', '123456', '55566677788', 'Recepcionista', 'Recepção', 'Banco do Brasil', 'CLT', 'Colaborador');

-- =====================================================
-- 5. VERIFICAR OS USUÁRIOS CRIADOS
-- =====================================================

-- Listar todos os usuários com seus perfis
SELECT 
    id_Usuario,
    nome,
    email,
    cargo,
    setor,
    perfil_acesso,
    CASE 
        WHEN perfil_acesso = 'Técnico' THEN '🔧'
        WHEN perfil_acesso = 'Administrador' THEN '👑'
        WHEN perfil_acesso = 'RH' THEN '👥'
        WHEN perfil_acesso = 'Colaborador' THEN '👤'
        ELSE '❓'
    END as icone
FROM usuarios 
ORDER BY perfil_acesso, nome;

-- =====================================================
-- 6. ESTATÍSTICAS POR PERFIL
-- =====================================================

SELECT 
    perfil_acesso,
    COUNT(*) as total_usuarios,
    GROUP_CONCAT(nome SEPARATOR ', ') as usuarios
FROM usuarios 
GROUP BY perfil_acesso 
ORDER BY total_usuarios DESC;

-- =====================================================
-- 7. TESTAR LOGICA DE PERFIS (simulação)
-- =====================================================

-- Simular como a lógica automática funcionaria
SELECT 
    nome,
    cargo,
    setor,
    perfil_acesso as perfil_atual,
    CASE 
        WHEN (cargo LIKE '%técnico%' OR cargo LIKE '%suporte%' OR cargo LIKE '%desenvolvedor%' OR cargo LIKE '%analista de sistemas%' OR cargo LIKE '%infraestrutura%' OR setor LIKE '%ti%' OR setor LIKE '%tecnologia%' OR setor LIKE '%suporte%' OR setor LIKE '%infraestrutura%') THEN 'Técnico'
        WHEN (cargo LIKE '%administrador%' OR cargo LIKE '%gestor%' OR cargo LIKE '%gerente%' OR cargo LIKE '%diretor%' OR cargo LIKE '%supervisor%' OR cargo LIKE '%coordenador%' OR cargo LIKE '%chefe%' OR setor LIKE '%direção%' OR setor LIKE '%gerência%' OR setor LIKE '%coordenação%') THEN 'Administrador'
        WHEN (cargo LIKE '%rh%' OR cargo LIKE '%recursos humanos%' OR setor LIKE '%rh%' OR setor LIKE '%recursos humanos%' OR setor LIKE '%pessoal%') THEN 'RH'
        ELSE 'Colaborador'
    END as perfil_sugerido,
    CASE 
        WHEN perfil_acesso = CASE 
            WHEN (cargo LIKE '%técnico%' OR cargo LIKE '%suporte%' OR cargo LIKE '%desenvolvedor%' OR cargo LIKE '%analista de sistemas%' OR cargo LIKE '%infraestrutura%' OR setor LIKE '%ti%' OR setor LIKE '%tecnologia%' OR setor LIKE '%suporte%' OR setor LIKE '%infraestrutura%') THEN 'Técnico'
            WHEN (cargo LIKE '%administrador%' OR cargo LIKE '%gestor%' OR cargo LIKE '%gerente%' OR cargo LIKE '%diretor%' OR cargo LIKE '%supervisor%' OR cargo LIKE '%coordenador%' OR cargo LIKE '%chefe%' OR setor LIKE '%direção%' OR setor LIKE '%gerência%' OR setor LIKE '%coordenação%') THEN 'Administrador'
            WHEN (cargo LIKE '%rh%' OR cargo LIKE '%recursos humanos%' OR setor LIKE '%rh%' OR setor LIKE '%recursos humanos%' OR setor LIKE '%pessoal%') THEN 'RH'
            ELSE 'Colaborador'
        END THEN '✅ Correto'
        ELSE '❌ Diferente'
    END as status
FROM usuarios 
ORDER BY nome; 