// Arquivo Completo e Final: js/fila-tecnico.js

document.addEventListener('DOMContentLoaded', () => {
  // Pega o token de autenticação
  const token = localStorage.getItem('jwt_token');
  if (!token) {
      // Se não tem token, não pode estar aqui. Vai para o login.
      window.location.href = 'login.html';
      return;
  }
  // Carrega a fila de chamados, enviando o token
  carregarFilaDeChamados(token);
  // Carrega a fila de especialidade
  carregarFilaEspecialidade(token);
});

async function carregarFilaDeChamados(token) {
  const apiUrl = 'http://localhost:5000/api/chamados/fila';
  const container = document.getElementById('fila-container'); // Precisamos adicionar este ID no HTML

  if (!container) {
      console.error('Container com ID "fila-container" não foi encontrado no HTML.');
      return;
  }

  container.innerHTML = '<p class="text-center text-gray-500 p-8">Buscando chamados na fila...</p>';

  try {
      // Adiciona o cabeçalho de autorização com o token
      const response = await fetch(apiUrl, {
          headers: {
              'Authorization': `Bearer ${token}`
          }
      });

      if (response.status === 401) {
          alert('Sessão expirada ou inválida. Por favor, faça login novamente.');
          window.location.href = 'login.html';
          return;
      }

      if (!response.ok) {
          throw new Error(`A API respondeu com um erro: ${response.statusText}`);
      }

      const chamados = await response.json();
      container.innerHTML = ''; // Limpa a mensagem de "carregando"

      if (chamados.length === 0) {
          container.innerHTML = '<p class="text-center text-gray-500 p-8">Não há chamados na fila de atendimento no momento.</p>';
          return;
      }

      chamados.forEach(chamado => {
          const cardHtml = `
              <div class="bg-white hover:bg-gray-50 border border-gray-200 rounded-lg p-4 transition-colors">
                <div class="flex flex-col sm:flex-row justify-between sm:items-center gap-3">
                  <div class="flex-grow">
                    <div class="flex items-center gap-3 mb-2">
                      <span class="text-sm font-medium text-gray-500">#${chamado.id}</span>
                      <h3 class="font-bold text-lg text-gray-800">${chamado.titulo}</h3>
                    </div>
                    <div class="flex flex-wrap items-center gap-x-4 gap-y-2 text-sm text-gray-500">
                      <span><i class="fas fa-user mr-1.5 text-gray-400"></i> Solicitante: ${chamado.nomeColaborador}</span>
                      <span class="font-medium text-red-600"><i class="fas fa-exclamation-triangle mr-1.5"></i> Prioridade ${chamado.prioridade}</span>
                    </div>
                  </div>
                  <div class="flex items-center gap-3 mt-2 sm:mt-0">
                    <span class="status-badge status-aberto">${chamado.status}</span>
                    <a href="detalhes_chamado.html?id=${chamado.id}" class="px-5 py-2 bg-primary text-white text-sm font-semibold rounded-lg hover-bg-primary">Atender</a>
                  </div>
                </div>
              </div>
          `;
          container.innerHTML += cardHtml;
      });

  } catch (error) {
      console.error('Falha ao buscar a fila de chamados:', error);
      container.innerHTML = `<p class="text-center text-red-500 p-8">Não foi possível carregar a fila de chamados. Verifique a conexão com a API.</p>`;
  }
}

async function carregarFilaEspecialidade(token) {
  const payload = parseJwt(token);
  const tecnicoId = payload ? payload.sub : null;
  const container = document.getElementById('fila-especialidade-container');
  if (!container || !tecnicoId) return;
  container.innerHTML = '<p class="text-gray-500">Carregando chamados...</p>';
  try {
    const response = await fetch(`http://localhost:5000/api/chamados/fila-especialidade?tecnicoId=${tecnicoId}`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!response.ok) throw new Error('Erro ao buscar chamados da especialidade');
    const chamados = await response.json();
    renderizarChamadosEspecialidade(chamados, container);
  } catch (error) {
    container.innerHTML = '<p class="text-red-500">Erro ao carregar chamados da especialidade.</p>';
  }
}

function renderizarChamadosEspecialidade(chamados, container) {
  if (!chamados || chamados.length === 0) {
    container.innerHTML = '<p class="text-gray-500">Nenhum chamado aberto para sua especialidade.</p>';
    return;
  }
  container.innerHTML = '';
  chamados.forEach(chamado => {
    const card = document.createElement('div');
    card.className = 'bg-white border border-gray-200 rounded-lg p-4 mb-2 flex flex-col sm:flex-row justify-between items-center';
    card.innerHTML = `
      <div>
        <span class="text-sm font-medium text-gray-500">#${chamado.id}</span>
        <h3 class="font-bold text-lg text-gray-800">${chamado.titulo}</h3>
        <div class="text-sm text-gray-500">Categoria: ${chamado.categoria} | Prioridade: ${chamado.prioridade}</div>
        <div class="text-sm text-gray-500">Solicitante: ${chamado.nomeColaborador}</div>
      </div>
      <button class="px-5 py-2 bg-green-600 text-white text-sm font-semibold rounded-lg hover:bg-green-700 mt-3 sm:mt-0" onclick="assumirChamado(${chamado.id})">
        Assumir chamado
      </button>
    `;
    container.appendChild(card);
  });
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

function assumirChamado(id) {
  const token = localStorage.getItem('jwt_token');
  const payload = parseJwt(token);
  const tecnicoId = payload ? payload.sub : null;
  if (!tecnicoId) {
    alert('Não foi possível identificar o técnico logado.');
    return;
  }
  if (!confirm('Deseja realmente assumir este chamado?')) return;
  fetch(`http://localhost:5000/api/chamados/${id}/assumir`, {
    method: 'PUT',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(tecnicoId)
  })
    .then(res => {
      if (res.ok) {
        alert('Chamado assumido com sucesso!');
        carregarFilaDeChamados(token);
        carregarFilaEspecialidade(token);
      } else {
        res.json().then(data => alert(data.message || 'Erro ao assumir chamado.'));
      }
    })
    .catch(() => alert('Erro ao assumir chamado.'));
}