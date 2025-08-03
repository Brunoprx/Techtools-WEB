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
    
    // Debug: Log da URL e token
    console.log("URL da API (detalhes):", apiUrl);
    console.log("ChamadoId:", id);
    console.log("Token:", token ? "Presente" : "Ausente");
    
    try {
        const response = await fetch(apiUrl, { headers: { 'Authorization': `Bearer ${token}` } });
        if (response.status === 401) {
            window.location.href = 'login.html';
            return;
        }
        if (!response.ok) throw new Error(`Chamado não encontrado (status: ${response.status})`);
        const data = await response.json();
        
        // Debug: Log da resposta da API
        console.log("===== DADOS COMPLETOS RECEBIDOS DA API =====");
        console.log(data);
        console.log("Anexos recebidos:", data.anexos);
        console.log("Quantidade de anexos:", data.anexos?.length || 0);
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

        // Exibir anexos da abertura do chamado junto à descrição detalhada
    const anexosContainer = document.getElementById('anexos-container');
    if (anexosContainer && data.anexos && data.anexos.length > 0) {
        // Limpar o container antes de adicionar novos anexos
        anexosContainer.innerHTML = '';
        
        // Filtrar anexos que foram enviados na abertura (que estão na pasta /anexos/)
        const anexosAbertura = data.anexos.filter(anexo => 
            anexo.caminhoArquivo.includes('/anexos/')
        );
        
        console.log("Separação de anexos:", {
            total: data.anexos.length,
            anexosAbertura: anexosAbertura.length,
            anexosSolucao: data.anexos.filter(a => a.caminhoArquivo.includes('/solucoes/')).length,
            detalhes: data.anexos.map(a => ({ nome: a.nomeArquivo, caminho: a.caminhoArquivo }))
        });
        
        if (anexosAbertura.length > 0) {
            const tituloAnexos = document.createElement('h4');
            tituloAnexos.className = 'text-sm font-medium text-gray-600 mb-2 mt-4';
            tituloAnexos.textContent = 'Anexos do Chamado:';
            anexosContainer.appendChild(tituloAnexos);
            
            const divImagens = document.createElement('div');
            divImagens.className = 'grid grid-cols-2 sm:grid-cols-3 gap-3';
            
            anexosAbertura.forEach(anexo => {
                const urlImagem = `http://localhost:5000${anexo.caminhoArquivo}`;
                const anexoElement = document.createElement('div');
                anexoElement.className = 'relative group';
                anexoElement.innerHTML = `
                    <a href="${urlImagem}" target="_blank" title="${anexo.nomeArquivo}" class="block">
                        <img src="${urlImagem}" alt="${anexo.nomeArquivo}" 
                             class="w-full h-24 object-cover rounded-lg border border-gray-200 hover:border-blue-300 transition-colors">
                        <div class="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-20 transition-all rounded-lg flex items-center justify-center">
                            <i class="fas fa-search-plus text-white opacity-0 group-hover:opacity-100 transition-opacity"></i>
                        </div>
                    </a>
                    <p class="text-xs text-gray-500 mt-1 truncate" title="${anexo.nomeArquivo}">${anexo.nomeArquivo}</p>
                `;
                divImagens.appendChild(anexoElement);
            });
            
            anexosContainer.appendChild(divImagens);
        }
    }
}

function controlarVisibilidadeAcoes(data) {
    const { status } = data;
    const token = localStorage.getItem('jwt_token');
    const payload = parseJwt(token);
    if (!payload) return;
    
    // Debug: Verificar todas as claims do token
    console.log("Todas as claims do token:", payload);
    
    const perfilUsuarioLogado = payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    
    console.log(`--- Verificando Ações para Status: '${status}' e Perfil: '${perfilUsuarioLogado}' ---`);

    // Debug: Verificar se os elementos existem
    const secaoAceitar = document.getElementById('secao-aceitar');
    const secaoSolucao = document.getElementById('secao-solucao');
    const secaoValidacaoColaborador = document.getElementById('secao-validacao-colaborador');
    
    console.log("Elementos encontrados:", {
        secaoAceitar: !!secaoAceitar,
        secaoSolucao: !!secaoSolucao,
        secaoValidacaoColaborador: !!secaoValidacaoColaborador
    });

    // Esconder todas as seções primeiro
    secaoAceitar?.setAttribute('hidden', '');
    secaoSolucao?.setAttribute('hidden', '');
    secaoValidacaoColaborador?.setAttribute('hidden', '');
    
    // Remover o display: none inline que está escondendo os elementos
    secaoAceitar?.style.removeProperty('display');
    secaoSolucao?.style.removeProperty('display');
    secaoValidacaoColaborador?.style.removeProperty('display');

    // Debug: Verificar comparações
    console.log("Comparações:", {
        perfilUsuarioLogado,
        perfilEhTecnico: perfilUsuarioLogado === 'Técnico',
        status,
        statusExato: `"${status}"`,
        statusEhAberto: status === 'Aberto',
        statusEhEmAndamento: status === 'EmAndamento',
        statusEhPendenteAceite: status === 'PendenteAceite'
    });
    
    if (perfilUsuarioLogado === 'Técnico') {
        if (status === 'Aberto') {
            console.log("DECISÃO: Mostrar seção 'Aceitar' para técnico.");
            secaoAceitar?.removeAttribute('hidden');
            secaoAceitar?.style.removeProperty('display');
        } else if (status === 'EmAndamento') {
            console.log("DECISÃO: Mostrar seção 'Solução' para técnico.");
            secaoSolucao?.removeAttribute('hidden');
            secaoSolucao?.style.removeProperty('display');
        } else if (status === 'PendenteAceite') {
            console.log("DECISÃO: Técnico não deve ver botões de ação quando status é PendenteAceite.");
            // Técnico não deve ver nenhuma ação quando o chamado está pendente de aceite
        }
                } else {
        // Usuário não é técnico (é colaborador)
        console.log("DECISÃO: Colaborador não deve ver botões de ação de técnico.");
        // Garantir que o colaborador não veja botões de técnico
        secaoAceitar?.setAttribute('hidden', '');
        secaoSolucao?.setAttribute('hidden', '');
        
        // Remover também o CSS inline que pode estar sobrescrevendo
        secaoAceitar?.style.setProperty('display', 'none', 'important');
        secaoSolucao?.style.setProperty('display', 'none', 'important');
        
        // Debug: Verificar se as seções foram escondidas
        console.log("Seções escondidas para colaborador:", {
            secaoAceitarHidden: secaoAceitar?.hasAttribute('hidden'),
            secaoSolucaoHidden: secaoSolucao?.hasAttribute('hidden'),
            secaoAceitarDisplay: secaoAceitar?.style.display,
            secaoSolucaoDisplay: secaoSolucao?.style.display
        });
        
        if (status === 'PendenteAceite') {
            console.log("DECISÃO: Mostrar seção 'Validação do Colaborador'.");
            secaoValidacaoColaborador?.removeAttribute('hidden');
            secaoValidacaoColaborador?.style.removeProperty('display');
            
            // Adicionar anexos à seção de validação do colaborador
            adicionarAnexosASolucao(data.anexos);
        } else {
            // Para outros status, remover anexos da solução se existirem
            const solucaoContainer = document.querySelector('#secao-validacao-colaborador .bg-blue-50');
            if (solucaoContainer) {
                const anexosExistentes = solucaoContainer.querySelectorAll('.anexos-solucao-section');
                anexosExistentes.forEach(anexo => anexo.remove());
            }
        }
    }
}

function adicionarAnexosASolucao(anexos) {
    if (!anexos || anexos.length === 0) return;
    
    const solucaoContainer = document.querySelector('#secao-validacao-colaborador .bg-blue-50');
    if (!solucaoContainer) return;
    
    // Remover seções de anexos existentes antes de adicionar novos
    const anexosExistentes = solucaoContainer.querySelectorAll('.anexos-solucao-section');
    console.log(`Removendo ${anexosExistentes.length} seções de anexos existentes`);
    anexosExistentes.forEach(anexo => anexo.remove());
    
    // Filtrar apenas anexos da solução (que estão na pasta /solucoes/)
    const anexosSolucao = anexos.filter(anexo => anexo.caminhoArquivo.includes('/solucoes/'));
    
    console.log("Anexos da solução encontrados:", anexosSolucao.length);
    
    if (anexosSolucao.length === 0) return;
    
    // Criar seção de anexos
    const anexosSection = document.createElement('div');
    anexosSection.className = 'mt-4 pt-4 border-t border-blue-200 anexos-solucao-section';
    
    const tituloAnexos = document.createElement('h5');
    tituloAnexos.className = 'text-sm font-medium text-gray-700 mb-3';
    tituloAnexos.textContent = 'Anexos da Solução:';
    anexosSection.appendChild(tituloAnexos);
    
    const divImagens = document.createElement('div');
    divImagens.className = 'grid grid-cols-2 sm:grid-cols-3 gap-3';
    
    anexosSolucao.forEach(anexo => {
        const urlImagem = `http://localhost:5000${anexo.caminhoArquivo}`;
        const anexoElement = document.createElement('div');
        anexoElement.className = 'relative group';
        anexoElement.innerHTML = `
            <a href="${urlImagem}" target="_blank" title="${anexo.nomeArquivo}" class="block">
                <img src="${urlImagem}" alt="${anexo.nomeArquivo}" 
                     class="w-full h-24 object-cover rounded-lg border border-gray-200 hover:border-blue-300 transition-colors">
                <div class="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-20 transition-all rounded-lg flex items-center justify-center">
                    <i class="fas fa-search-plus text-white opacity-0 group-hover:opacity-100 transition-opacity"></i>
                </div>
            </a>
            <p class="text-xs text-gray-500 mt-1 truncate" title="${anexo.nomeArquivo}">${anexo.nomeArquivo}</p>
        `;
        divImagens.appendChild(anexoElement);
    });
    
    anexosSection.appendChild(divImagens);
    solucaoContainer.appendChild(anexosSection);
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
        console.log('Carregando técnicos da API...');
        
        const response = await fetch('http://localhost:5000/api/usuarios?perfil=Técnico', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        
        if (!response.ok) {
            throw new Error(`Erro ao buscar técnicos: ${response.status}`);
        }
        
        const tecnicos = await response.json();
        console.log('Técnicos recebidos:', tecnicos);
        
        select.innerHTML = '<option value="">Selecione um técnico...</option>';
        
        if (tecnicos && tecnicos.length > 0) {
            tecnicos.forEach(tecnico => {
                const option = document.createElement('option');
                option.value = tecnico.id;
                option.textContent = tecnico.nome;
                select.appendChild(option);
            });
        } else {
            select.innerHTML = '<option value="">Nenhum técnico disponível</option>';
        }
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