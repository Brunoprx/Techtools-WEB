// Arquivo de funcionalidades comuns para todas as páginas
// common.js

// Configurações globais
const API_BASE_URL = 'http://localhost:5000/api';

// Estado global
let usuarioLogado = null;
let notificacoes = [];

// Inicialização comum para todas as páginas
document.addEventListener('DOMContentLoaded', async () => {
    await inicializarPagina();
});

// Função principal de inicialização
async function inicializarPagina() {
    try {
        // Verificar autenticação
        if (!verificarAutenticacao()) {
            return;
        }

        // Carregar dados do usuário
        await carregarDadosUsuario();
        
        // Atualizar interface com dados do usuário
        atualizarInterfaceUsuario();
        
        // Carregar notificações
        await carregarNotificacoes();
        
        // Configurar navegação baseada em permissões
        configurarNavegacao();
        
        // Configurar event listeners comuns
        configurarEventListenersComuns();
        
    } catch (error) {
        console.error('Erro na inicialização da página:', error);
        mostrarErro('Erro ao carregar dados da página');
    }
}

// Verificar se o usuário está autenticado
function verificarAutenticacao() {
    const token = localStorage.getItem('jwt_token');
    if (!token) {
        window.location.href = 'login.html';
        return false;
    }
    
    try {
        const payload = parseJwt(token);
        if (!payload || !payload.sub) {
            localStorage.removeItem('jwt_token');
            window.location.href = 'login.html';
            return false;
        }
        return true;
    } catch (error) {
        localStorage.removeItem('jwt_token');
        window.location.href = 'login.html';
        return false;
    }
}

// Carregar dados do usuário logado
async function carregarDadosUsuario() {
    const token = localStorage.getItem('jwt_token');
    const payload = parseJwt(token);
    
    try {
        const response = await fetch(`${API_BASE_URL}/usuarios/${payload.sub}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        
        if (!response.ok) throw new Error('Falha ao carregar dados do usuário');
        
        usuarioLogado = await response.json();
        localStorage.setItem('usuarioLogado', JSON.stringify(usuarioLogado));
        
    } catch (error) {
        console.error('Erro ao carregar dados do usuário:', error);
        // Usar dados do token como fallback
        usuarioLogado = {
            id: payload.sub,
            nome: payload.name || 'Usuário',
            email: payload.email || '',
            perfilAcesso: payload.role || 'Colaborador',
            empresaId: payload.EmpresaId || 1
        };
    }
}

// Atualizar interface com dados do usuário
function atualizarInterfaceUsuario() {
    if (!usuarioLogado) return;
    
    // Atualizar avatar e informações do usuário
    const avatarElements = document.querySelectorAll('[data-user-avatar]');
    const nomeElements = document.querySelectorAll('[data-user-name]');
    const perfilElements = document.querySelectorAll('[data-user-perfil]');
    
    avatarElements.forEach(element => {
        element.src = usuarioLogado.avatarUrl || 'https://storage.googleapis.com/a1aa/image/e4a14e45-6fc6-4f88-0967-8b29ec919448.jpg';
        element.alt = `Avatar de ${usuarioLogado.nome}`;
    });
    
    nomeElements.forEach(element => {
        element.textContent = usuarioLogado.nome;
    });
    
    perfilElements.forEach(element => {
        element.textContent = formatarPerfil(usuarioLogado.perfilAcesso);
    });
}

// Carregar notificações
async function carregarNotificacoes() {
    const token = localStorage.getItem('jwt_token');
    
    try {
        const response = await fetch(`${API_BASE_URL}/notificacoes`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        
        if (response.ok) {
            notificacoes = await response.json();
            atualizarContadorNotificacoes();
        }
    } catch (error) {
        console.error('Erro ao carregar notificações:', error);
        // Usar dados mock para desenvolvimento
        notificacoes = [
            { id: 1, titulo: 'Novo chamado atribuído', lida: false },
            { id: 2, titulo: 'SLA próximo do prazo', lida: false },
            { id: 3, titulo: 'Sistema de manutenção', lida: true }
        ];
        atualizarContadorNotificacoes();
    }
}

// Atualizar contador de notificações
function atualizarContadorNotificacoes() {
    const contadores = document.querySelectorAll('[data-notification-count]');
    const naoLidas = notificacoes.filter(n => !n.lida).length;
    
    contadores.forEach(element => {
        element.textContent = naoLidas;
        element.style.display = naoLidas > 0 ? 'flex' : 'none';
    });
}

// Configurar navegação baseada em permissões
function configurarNavegacao() {
    if (!usuarioLogado) return;
    
    const perfil = usuarioLogado.perfilAcesso;
    
    // Esconder links baseado no perfil
    const linksRestritos = {
        'gestao_usuarios.html': ['Administrador', 'RH'],
        'relatorios.html': ['Administrador', 'Gestor'],
        'tecnicos.html': ['Técnico', 'Administrador', 'Gestor']
    };
    
    Object.entries(linksRestritos).forEach(([pagina, perfisPermitidos]) => {
        const links = document.querySelectorAll(`a[href*="${pagina}"]`);
        links.forEach(link => {
            if (!perfisPermitidos.includes(perfil)) {
                link.style.display = 'none';
            }
        });
    });
}

// Configurar event listeners comuns
function configurarEventListenersComuns() {
    // Logout
    const logoutButtons = document.querySelectorAll('[data-logout]');
    logoutButtons.forEach(button => {
        button.addEventListener('click', fazerLogout);
    });
    
    // Notificações
    const notificationButtons = document.querySelectorAll('[data-notifications]');
    notificationButtons.forEach(button => {
        button.addEventListener('click', mostrarNotificacoes);
    });
    
    // Busca global
    const searchInputs = document.querySelectorAll('[data-global-search]');
    searchInputs.forEach(input => {
        input.addEventListener('input', debounce(realizarBuscaGlobal, 300));
    });
}

// Função de logout
function fazerLogout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('usuarioLogado');
    window.location.href = 'login.html';
}

// Mostrar notificações
function mostrarNotificacoes() {
    // Implementar modal de notificações
    console.log('Mostrar notificações:', notificacoes);
    // TODO: Criar modal de notificações
}

// Busca global
function realizarBuscaGlobal(event) {
    const termo = event.target.value.trim();
    if (termo.length < 3) return;
    
    // Implementar busca global
    console.log('Busca global:', termo);
    // TODO: Implementar busca global
}

// Funções utilitárias
function parseJwt(token) {
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

function formatarPerfil(perfil) {
    const perfis = {
        'Administrador': 'Administrador',
        'Gestor': 'Gestor',
        'Técnico': 'Técnico',
        'Colaborador': 'Colaborador',
        'RH': 'RH'
    };
    return perfis[perfil] || perfil;
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Funções de feedback
function mostrarSucesso(mensagem) {
    mostrarToast(mensagem, 'success');
}

function mostrarErro(mensagem) {
    mostrarToast(mensagem, 'error');
}

function mostrarToast(mensagem, tipo) {
    const toast = document.createElement('div');
    toast.className = `fixed top-4 right-4 z-50 px-6 py-3 rounded-lg text-white font-medium shadow-lg transform transition-all duration-300 ${
        tipo === 'success' ? 'bg-green-500' : 'bg-red-500'
    }`;
    toast.textContent = mensagem;
    
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.style.transform = 'translateX(100%)';
        setTimeout(() => {
            document.body.removeChild(toast);
        }, 300);
    }, 3000);
}

// Exportar funções para uso em outros arquivos
window.CommonUtils = {
    parseJwt,
    formatarPerfil,
    debounce,
    mostrarSucesso,
    mostrarErro,
    carregarDadosUsuario,
    verificarAutenticacao
}; 