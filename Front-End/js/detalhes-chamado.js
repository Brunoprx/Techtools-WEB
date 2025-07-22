// Arquivo Completo e Final de Depuração: js/detalhes-chamado.js

document.addEventListener('DOMContentLoaded', () => {
    const token = localStorage.getItem('jwt_token');
    if (!token) {
        window.location.href = 'login.html';
        return;
    }
    const params = new URLSearchParams(window.location.search);
    const chamadoId = parseInt(params.get('id'));
    if (!chamadoId) {
        document.body.innerHTML = '<h1>Erro: ID do chamado não fornecido na URL.</h1>';
        return;
    }
    fetchDetalhesDoChamado(chamadoId, token);
});

async function fetchDetalhesDoChamado(id, token) {
    const apiUrl = `http://localhost:5000/api/chamados/${id}`;
    try {
        const response = await fetch(apiUrl, { headers: { 'Authorization': `Bearer ${token}` } });
        if (response.status === 401) {
            window.location.href = 'login.html';
            return;
        }
        if (!response.ok) throw new Error(`Chamado não encontrado (status: ${response.status})`);
        const data = await response.json();
        
        console.log("===== DADOS COMPLETOS RECEBIDOS DA API =====");
        console.log(data);
        console.log("============================================");

        preencherPagina(data);
        controlarVisibilidadeAcoes(data);
        adicionarListenersDeAcao(id, token);
    } catch (error) {
        document.body.innerHTML = `<h1>Erro ao carregar dados. ${error.message}</h1>`;
        console.error('Erro ao buscar detalhes do chamado:', error);
    }
}

function preencherPagina(data) {
    const preencher = (id, valor) => {
        const el = document.getElementById(id);
        if (el && (valor || valor === 0)) {
            ['INPUT', 'TEXTAREA'].includes(el.tagName) ? el.value = valor : el.textContent = valor;
        } else if (el) {
            ['INPUT', 'TEXTAREA'].includes(el.tagName) ? el.value = '' : el.textContent = '';
        }
    };
    preencher('chamado-titulo', `#${data.id} - ${data.titulo}`);
    preencher('form-titulo', data.titulo);
    preencher('form-categoria', data.categoria);
    preencher('form-urgencia', data.nivelUrgencia);
    preencher('form-descricao', data.descricao);
    preencher('solicitante-nome', data.nomeColaborador);
    preencher('solicitante-email', data.emailColaborador);
    preencher('solicitante-setor', data.setorColaborador);
    preencher('chamado-status', data.status);
    preencher('chamado-prioridade', data.prioridade);
    preencher('texto-solucao', data.solucaoAplicada);
    if (data.dataAbertura) {
        preencher('chamado-data-abertura', new Date(data.dataAbertura).toLocaleString('pt-BR'));
    }

    // Preencher histórico
    const historicoContainer = document.getElementById('historico-container');
    if (historicoContainer) {
        if (data.historico && data.historico.trim()) {
            const linhas = data.historico.split('\n');
            historicoContainer.innerHTML = '';
            linhas.forEach(linha => {
                if (linha.trim()) {
                    const div = document.createElement('div');
                    div.className = 'text-sm text-gray-700 p-2 bg-gray-50 rounded';
                    div.textContent = linha.trim();
                    historicoContainer.appendChild(div);
                }
            });
        } else {
            historicoContainer.innerHTML = '<p class="text-sm text-gray-500">Nenhum registro de histórico disponível.</p>';
        }
    }

    const anexosAberturaContainer = document.getElementById('anexos-abertura-container');
    if (anexosAberturaContainer) {
        anexosAberturaContainer.innerHTML = '';
        if (data.anexos && data.anexos.length > 0) {
            const tituloAnexos = document.createElement('h4');
            tituloAnexos.className = 'text-sm font-medium text-gray-600 mb-2';
            tituloAnexos.textContent = 'Anexos:';
            anexosAberturaContainer.appendChild(tituloAnexos);
            const divImagens = document.createElement('div');
            divImagens.className = 'grid grid-cols-2 sm:grid-cols-3 gap-4';
            data.anexos.forEach(anexo => {
                const urlImagem = `http://localhost:5000${anexo.caminhoArquivo}`;
                divImagens.innerHTML += `<a href="${urlImagem}" target="_blank" title="${anexo.nomeArquivo}"><img src="${urlImagem}" alt="${anexo.nomeArquivo}" class="inline-block h-24 w-24 object-cover rounded-lg border mr-2 mb-2"></a>`;
            });
            anexosAberturaContainer.appendChild(divImagens);
        }
    }
}

function controlarVisibilidadeAcoes(data) {
    const { status } = data;
    const token = localStorage.getItem('jwt_token');
    const payload = parseJwt(token);
    if (!payload) return;
    const perfilUsuarioLogado = payload.role;

    console.log(`--- Verificando Ações para Status: '${status}' e Perfil: '${perfilUsuarioLogado}' ---`);

    document.getElementById('secao-aceitar')?.setAttribute('hidden', '');
    document.getElementById('secao-solucao')?.setAttribute('hidden', '');
    document.getElementById('secao-validacao-colaborador')?.setAttribute('hidden', '');

    if (perfilUsuarioLogado === 'Técnico') {
        if (status === 'Aberto') {
            console.log("DECISÃO: Mostrar seção 'Aceitar'.");
            document.getElementById('secao-aceitar')?.removeAttribute('hidden');
        } else if (status === 'EmAndamento') {
            console.log("DECISÃO: Mostrar seção 'Solução'.");
            document.getElementById('secao-solucao')?.removeAttribute('hidden');
        }
    }
    
    if (status === 'PendenteAceite') {
        if(perfilUsuarioLogado !== 'Técnico') {
            console.log("DECISÃO: Mostrar seção 'Validação do Colaborador'.");
            document.getElementById('secao-validacao-colaborador')?.removeAttribute('hidden');
        }
    }
}

function adicionarListenersDeAcao(chamadoId, token) {
    const btnAceitar = document.getElementById('btn-aceitar-chamado');
    if (btnAceitar) btnAceitar.onclick = () => aceitarChamado(chamadoId, token);
    
    const btnEnviar = document.getElementById('btn-enviar-solucao');
    if (btnEnviar) btnEnviar.onclick = () => enviarSolucao(chamadoId, token);

    const btnConfirmar = document.getElementById('btn-confirmar-solucao');
    if (btnConfirmar) btnConfirmar.onclick = () => confirmarSolucao(chamadoId, token);

    const btnRejeitar = document.getElementById('btn-rejeitar-solucao');
    if (btnRejeitar) btnRejeitar.onclick = () => rejeitarSolucao(chamadoId, token);

    const btnReencaminhar = document.getElementById('btn-reencaminhar-chamado');
    if (btnReencaminhar) btnReencaminhar.onclick = () => abrirModalReencaminhar(chamadoId, token);
}

async function aceitarChamado(chamadoId, token) {
    const payloadToken = parseJwt(token);
    const tecnicoId = parseInt(payloadToken.sub);
    await fetchApi('PUT', 'aceitar', { chamadoId, tecnicoId }, token, 'Chamado aceito com sucesso!', chamadoId);
}

async function enviarSolucao(chamadoId, token) {
    const payloadToken = parseJwt(token);
    const tecnicoId = parseInt(payloadToken.sub);
    const descricaoSolucao = document.getElementById('textarea-solucao').value;
    if (!descricaoSolucao.trim()) return alert('Por favor, descreva a solução.');

    await fetchApi('PUT', 'adicionar-solucao', { chamadoId, tecnicoId, descricaoSolucao }, token, 'Solução registrada. Verificando anexos...', chamadoId, false);

    const anexoInput = document.getElementById('input-anexo-solucao');
    const anexoFile = anexoInput.files[0];
    if (anexoFile) {
        const formData = new FormData();
        formData.append('file', anexoFile);
        await fetchApi('POST', `${chamadoId}/anexos`, formData, token, 'Anexo enviado. O chamado aguarda aprovação.', chamadoId);
    } else {
        alert('Solução registrada. O chamado aguarda aprovação do cliente.');
        fetchDetalhesDoChamado(chamadoId, token);
    }
}

async function confirmarSolucao(chamadoId, token) {
    const payloadToken = parseJwt(token);
    const colaboradorId = parseInt(payloadToken.sub);
    if (confirm('Deseja realmente aceitar esta solução e fechar o chamado?')) {
        await fetchApi('PUT', 'confirmar-solucao', { chamadoId, colaboradorId }, token, 'Chamado fechado com sucesso!', chamadoId);
    }
}

async function rejeitarSolucao(chamadoId, token) {
    const motivo = prompt("Por favor, descreva por que a solução não resolveu o problema:");
    if (!motivo || !motivo.trim()) return alert("A rejeição foi cancelada. Um motivo é obrigatório.");
    
    const payloadToken = parseJwt(token);
    const colaboradorId = parseInt(payloadToken.sub);
    await fetchApi('PUT', 'rejeitar-solucao', { chamadoId, colaboradorId, motivoRejeicao: motivo }, token, 'Solução rejeitada. O chamado foi reaberto.', chamadoId);
}

async function fetchApi(method, endpoint, body, token, successMessage, chamadoId, reload = true) {
    const apiUrl = `http://localhost:5000/api/chamados/${endpoint}`;
    const options = { method, headers: { 'Authorization': `Bearer ${token}` }};
    if (body) {
        if (body instanceof FormData) {
            options.body = body;
        } else {
            options.headers['Content-Type'] = 'application/json';
            options.body = JSON.stringify(body);
        }
    }
    try {
        const response = await fetch(apiUrl, options);
        if (!response.ok) {
            const error = await response.json().catch(() => ({ message: response.statusText }));
            throw new Error(error.message);
        }
        alert(successMessage);
        if (reload) fetchDetalhesDoChamado(chamadoId, token);
    } catch (error) {
        alert(`Erro: ${error.message}`);
        console.error(`Falha na operação ${endpoint}:`, error);
    }
}

function parseJwt (token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}

// Funções para reencaminhamento de chamado
async function abrirModalReencaminhar(chamadoId, token) {
    const modal = document.getElementById('modal-reencaminhar');
    const select = document.getElementById('select-novo-tecnico');
    
    // Carregar lista de técnicos
    await carregarTecnicos(select, token);
    
    // Adicionar listeners do modal
    document.getElementById('btn-cancelar-reencaminhar').onclick = () => fecharModalReencaminhar();
    document.getElementById('btn-confirmar-reencaminhar').onclick = () => confirmarReencaminhar(chamadoId, token);
    
    modal.style.display = 'block';
}

function fecharModalReencaminhar() {
    const modal = document.getElementById('modal-reencaminhar');
    modal.style.display = 'none';
}

async function carregarTecnicos(select, token) {
    try {
        // Por enquanto, vamos usar uma lista estática de técnicos
        // Em uma implementação real, você faria uma chamada para a API
        const tecnicos = [
            { id: 2, nome: 'Carlos Técnico' },
            { id: 9, nome: 'João Gestor' }
        ];
        
        select.innerHTML = '<option value="">Selecione um técnico...</option>';
        tecnicos.forEach(tecnico => {
            const option = document.createElement('option');
            option.value = tecnico.id;
            option.textContent = tecnico.nome;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Erro ao carregar técnicos:', error);
        select.innerHTML = '<option value="">Erro ao carregar técnicos</option>';
    }
}

async function confirmarReencaminhar(chamadoId, token) {
    const select = document.getElementById('select-novo-tecnico');
    const novoTecnicoId = parseInt(select.value);
    
    if (!novoTecnicoId) {
        alert('Por favor, selecione um técnico.');
        return;
    }
    
    try {
        const response = await fetch(`http://localhost:5000/api/chamados/${chamadoId}/reencaminhar`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(novoTecnicoId)
        });
        
        if (!response.ok) {
            const error = await response.json().catch(() => ({ message: response.statusText }));
            throw new Error(error.message);
        }
        
        alert('Chamado reencaminhado com sucesso!');
        fecharModalReencaminhar();
        
        // Recarregar a página para mostrar as mudanças
        window.location.reload();
    } catch (error) {
        alert(`Erro ao reencaminhar chamado: ${error.message}`);
        console.error('Erro ao reencaminhar chamado:', error);
    }
}