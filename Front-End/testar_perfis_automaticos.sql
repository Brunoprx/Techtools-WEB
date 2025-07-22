-- Script para testar a criação de usuários com perfis automáticos
-- Execute estes comandos via API ou use para verificar a lógica

-- 1. Testar criação de usuário Técnico (perfil será definido automaticamente)
-- POST http://localhost:5000/api/usuarios
-- {
--   "nome": "João Técnico",
--   "email": "joao.tecnico@empresa.com",
--   "senha": "123456",
--   "cpf": "11122233344",
--   "cargo": "Técnico de Suporte",
--   "setor": "TI",
--   "banco": "Bradesco",
--   "tipoContrato": "CLT"
-- }

-- 2. Testar criação de usuário Administrador
-- POST http://localhost:5000/api/usuarios
-- {
--   "nome": "Maria Gestora",
--   "email": "maria.gestora@empresa.com",
--   "senha": "123456",
--   "cpf": "22233344455",
--   "cargo": "Gerente de TI",
--   "setor": "TI",
--   "banco": "Itaú",
--   "tipoContrato": "CLT"
-- }

-- 3. Testar criação de usuário RH
-- POST http://localhost:5000/api/usuarios
-- {
--   "nome": "Ana RH",
--   "email": "ana.rh@empresa.com",
--   "senha": "123456",
--   "cpf": "33344455566",
--   "cargo": "Analista de RH",
--   "setor": "RH",
--   "banco": "Santander",
--   "tipoContrato": "CLT"
-- }

-- 4. Testar criação de usuário Colaborador (padrão)
-- POST http://localhost:5000/api/usuarios
-- {
--   "nome": "Pedro Colaborador",
--   "email": "pedro.colaborador@empresa.com",
--   "senha": "123456",
--   "cpf": "44455566677",
--   "cargo": "Auxiliar Administrativo",
--   "setor": "Administrativo",
--   "banco": "Caixa",
--   "tipoContrato": "CLT"
-- }

-- 5. Testar endpoint de perfis disponíveis
-- GET http://localhost:5000/api/usuarios/perfis

-- 6. Testar endpoint de teste de definição de perfil
-- POST http://localhost:5000/api/usuarios/testar-perfil
-- {
--   "cargo": "Desenvolvedor",
--   "setor": "TI"
-- }

-- Verificar usuários criados
SELECT id_Usuario, nome, email, cargo, setor, perfil_acesso 
FROM usuarios 
ORDER BY id_Usuario DESC; 