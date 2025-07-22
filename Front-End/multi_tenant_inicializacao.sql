-- 1. Criar tabela de empresas
CREATE TABLE empresas (
  id_empresa INT NOT NULL AUTO_INCREMENT,
  nome_empresa VARCHAR(100) NOT NULL,
  data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP(),
  PRIMARY KEY (id_empresa)
) ENGINE=InnoDB;

-- 2. Adicionar coluna id_empresa nas tabelas principais
ALTER TABLE usuarios ADD COLUMN id_empresa INT NOT NULL AFTER id_Usuario;
ALTER TABLE chamado ADD COLUMN id_empresa INT NOT NULL AFTER id_chamado;
ALTER TABLE anexo ADD COLUMN id_empresa INT NOT NULL AFTER id_anexo;
ALTER TABLE folhapagamento ADD COLUMN id_empresa INT NOT NULL AFTER id_folha;
ALTER TABLE eventofolha ADD COLUMN id_empresa INT NOT NULL AFTER id_evento;
ALTER TABLE registroponto ADD COLUMN id_empresa INT NOT NULL AFTER id_registro;

-- 3. Adicionar chaves estrangeiras
ALTER TABLE usuarios ADD CONSTRAINT fk_usuarios_empresa FOREIGN KEY (id_empresa) REFERENCES empresas(id_empresa);
ALTER TABLE chamado ADD CONSTRAINT fk_chamado_empresa FOREIGN KEY (id_empresa) REFERENCES empresas(id_empresa);
ALTER TABLE anexo ADD CONSTRAINT fk_anexo_empresa FOREIGN KEY (id_empresa) REFERENCES empresas(id_empresa);
ALTER TABLE folhapagamento ADD CONSTRAINT fk_folha_empresa FOREIGN KEY (id_empresa) REFERENCES empresas(id_empresa);
ALTER TABLE eventofolha ADD CONSTRAINT fk_eventofolha_empresa FOREIGN KEY (id_empresa) REFERENCES empresas(id_empresa);
ALTER TABLE registroponto ADD CONSTRAINT fk_registroponto_empresa FOREIGN KEY (id_empresa) REFERENCES empresas(id_empresa);

-- 4. Inserir empresas
INSERT INTO empresas (nome_empresa) VALUES ('Empresa A'), ('Empresa B');

-- 5. Inserir usuários administradores de teste
INSERT INTO usuarios (nome, email, senha, cpf, cargo, setor, banco, tipo_contrato, perfil_acesso, id_empresa)
VALUES
('Admin Empresa A', 'admin.a@empresa.com', '123456', '11111111111', 'Administrador', 'TI', 'Bradesco', 'CLT', 'Administrador', 1),
('Admin Empresa B', 'admin.b@empresa.com', '123456', '22222222222', 'Administrador', 'TI', 'Itaú', 'CLT', 'Administrador', 2); 