// Dashboard - Carregamento dinâmico dos dados
document.addEventListener('DOMContentLoaded', function() {
    carregarDashboard();
});

async function carregarDashboard() {
    try {
        const token = localStorage.getItem('jwt_token');
        if (!token) {
            console.error('Token não encontrado');
            return;
        }

        const response = await fetch('http://localhost:5000/api/chamados/dashboard', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`Erro HTTP: ${response.status}`);
        }

        const data = await response.json();
        atualizarDashboard(data);
        
    } catch (error) {
        console.error('Erro ao carregar dashboard:', error);
        // Em caso de erro, manter os valores padrão
    }
}

function atualizarDashboard(data) {
    // Atualizar os elementos do dashboard
    const elementos = {
        'card-total-chamados': data.totalChamados || 0,
        'card-em-andamento': data.emAndamento || 0,
        'card-finalizados': data.finalizados || 0,
        'card-cancelados': data.cancelados || 0
    };

    // Aplicar as atualizações
    Object.keys(elementos).forEach(className => {
        const elemento = document.querySelector(`.${className}`);
        if (elemento) {
            elemento.textContent = elementos[className];
            
            // Adicionar animação de contagem se o valor mudou
            if (elemento.textContent !== '-') {
                animarContagem(elemento, 0, elementos[className]);
            }
        }
    });
}

function animarContagem(elemento, inicio, fim) {
    const duracao = 1000; // 1 segundo
    const incremento = (fim - inicio) / (duracao / 16); // 60fps
    let valorAtual = inicio;
    
    const animacao = setInterval(() => {
        valorAtual += incremento;
        
        if (valorAtual >= fim) {
            elemento.textContent = fim;
            clearInterval(animacao);
        } else {
            elemento.textContent = Math.floor(valorAtual);
        }
    }, 16);
}

// Função para recarregar o dashboard (pode ser chamada após ações)
function recarregarDashboard() {
    carregarDashboard();
}

// Exportar para uso global
window.recarregarDashboard = recarregarDashboard; 