// Arquivo Completo e Final: js/meus-chamados.js

document.addEventListener('DOMContentLoaded', () => {
    // Pega o token salvo no login ao carregar a página
    const token = localStorage.getItem('jwt_token');

    // 1. VERIFICAÇÃO DE SEGURANÇA: Se não há token, manda o usuário para a tela de login.
    if (!token) {
        window.location.href = 'login.html';
        return;
    }
    
    // 2. Decodifica o token para pegar o ID do usuário
    const payload = parseJwt(token);
    if (!payload || !payload.sub) {
        // Se o token for inválido, limpa e redireciona
        localStorage.removeItem('jwt_token');
        window.location.href = 'login.html';
        return;
    }
    const colaboradorId = payload.sub;

    // 3. Adiciona "ouvintes" aos filtros, passando o ID e o token para a função de recarga
    const filtroStatus = document.getElementById('filtro-status');
    const filtroTipo = document.getElementById('filtro-tipo');

    if (filtroStatus) filtroStatus.addEventListener('change', () => carregarMeusChamados(colaboradorId, token));
    if (filtroTipo) filtroTipo.addEventListener('change', () => carregarMeusChamados(colaboradorId, token));

    // 4. Carrega a lista inicial de chamados
    carregarMeusChamados(colaboradorId, token);
    // 5. Carrega os KPIs dos cards
    preencherKpisChamados(colaboradorId, token);
});

async function carregarMeusChamados(colaboradorId, token) {
    const container = document.getElementById('meus-chamados-container');
    if (!container) return;
    
    const statusFiltro = document.getElementById('filtro-status')?.value || '';
    const tipoFiltro = document.getElementById('filtro-tipo')?.value || '';
    
    const params = new URLSearchParams();
    if (statusFiltro) params.append('status', statusFiltro);
    if (tipoFiltro) params.append('tipo', tipoFiltro);
    
    const apiUrl = `http://localhost:5000/api/chamados/colaborador/${colaboradorId}?${params.toString()}`;
    
    container.innerHTML = '<p class="text-center text-gray-500">Buscando seus chamados...</p>';

    try {
        const response = await fetch(apiUrl, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.status === 401) {
            window.location.href = 'login.html';
            return;
        }
        if (!response.ok) throw new Error('Falha na resposta da API');

        const chamados = await response.json();
        
        container.innerHTML = ''; 

        if (chamados.length === 0) {
            container.innerHTML = '<p class="text-center text-gray-500 p-8">Nenhum chamado encontrado com esses filtros.</p>';
            return;
        }

        // ==========================================================
        // LÓGICA DE RENDERIZAÇÃO QUE ESTAVA FALTANDO
        // ==========================================================
        chamados.forEach(chamado => {
            const statusInfo = getStatusInfo(chamado.status);
            let acaoBotao = `<a href="detalhes_chamado.html?id=${chamado.id}" class="text-sm text-primary font-medium hover:underline">Ver Detalhes</a>`;

            if (chamado.status === 'PendenteAceite') {
                acaoBotao = `<a href="detalhes_chamado.html?id=${chamado.id}" class="px-3 py-1 bg-blue-600 text-white rounded-md text-xs font-medium hover:bg-blue-700 transition-colors">Ver e Aprovar Solução</a>`;
            }

            const cardHtml = `
                <div class="bg-gray-50 hover:bg-white border border-gray-200 rounded-lg p-4 transition-all duration-200">
                    <div class="flex flex-col sm:flex-row justify-between sm:items-center gap-3">
                        <div class="flex-grow">
                            <div class="flex items-center gap-3 mb-2">
                                <span class="text-sm font-medium text-gray-500">#${chamado.id}</span>
                                <h3 class="font-bold text-lg text-gray-800">${chamado.titulo}</h3>
                            </div>
                            <div class="flex flex-wrap items-center gap-x-4 gap-y-2 text-sm text-gray-500">
                                <span><i class="fas fa-tag mr-1.5 text-gray-400"></i> ${chamado.categoria}</span>
                                <span><i class="fas fa-exclamation-circle mr-1.5 text-gray-400"></i> Prioridade ${chamado.prioridade}</span>
                            </div>
                        </div>
                        <div class="flex flex-col sm:items-end gap-3 mt-2 sm:mt-0">
                            <div class="status-badge ${statusInfo.classeCss}">${statusInfo.texto}</div>
                            ${acaoBotao}
                        </div>
                    </div>
                </div>`;
            container.innerHTML += cardHtml;
        });
        
    } catch (error) {
        console.error('Falha ao buscar chamados:', error);
        container.innerHTML = '<p class="text-center text-red-500">Não foi possível carregar seus chamados.</p>';
    }
}

// Função para preencher os cards de KPIs do colaborador
async function preencherKpisChamados(colaboradorId, token) {
    const url = `https://localhost:5001/api/chamados/kpis-colaborador?colaboradorId=${colaboradorId}`;
    try {
        const response = await fetch(url, { headers: { 'Authorization': `Bearer ${token}` } });
        if (!response.ok) throw new Error('Falha ao buscar KPIs');
        const kpis = await response.json();
        document.querySelector('.card-total-chamados').textContent = kpis.totalChamados || 0;
        document.querySelector('.card-em-andamento').textContent = kpis.emAndamento || 0;
        document.querySelector('.card-finalizados').textContent = kpis.finalizados || 0;
        document.querySelector('.card-cancelados').textContent = kpis.cancelados || 0;
    } catch (error) {
        document.querySelector('.card-total-chamados').textContent = '-';
        document.querySelector('.card-em-andamento').textContent = '-';
        document.querySelector('.card-finalizados').textContent = '-';
        document.querySelector('.card-cancelados').textContent = '-';
        console.error('Erro ao preencher KPIs:', error);
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

function getStatusInfo(status) {
    const estilos = {
        'Aberto': { texto: 'Aberto', classeCss: 'status-aberto' },
        'EmAndamento': { texto: 'Em Andamento', classeCss: 'status-andamento' },
        'Fechado': { texto: 'Finalizado', classeCss: 'status-finalizado' },
        'PendenteAceite': { texto: 'Aguardando Aprovação', classeCss: 'bg-orange-100 text-orange-800' },
        'Cancelado': { texto: 'Cancelado', classeCss: 'status-cancelado' }
    };
    // Adicione as classes de estilo para os status no seu CSS se necessário.
    return estilos[status] || { texto: status, classeCss: 'bg-gray-200 text-gray-800' };
}