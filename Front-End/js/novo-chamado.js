// Arquivo Completo e Final com Autenticação: js/novo-chamado.js

document.addEventListener('DOMContentLoaded', function() {
  const form = document.getElementById('form-novo-chamado');
  const token = localStorage.getItem('jwt_token');

  // Se não houver token, redireciona para o login
  if (!token) {
    window.location.href = 'login.html';
    return;
  }

  // Decodifica o token para pegar o ID do usuário logado
  const payloadToken = parseJwt(token);
  const colaboradorId = payloadToken ? parseInt(payloadToken.sub) : null;

  if (!colaboradorId) {
    alert('Não foi possível identificar o usuário. Por favor, faça login novamente.');
    window.location.href = 'login.html';
    return;
  }

  if (!form) return;

  form.addEventListener('submit', async function(event) {
    event.preventDefault();

    const titulo = document.getElementById('titulo').value;
    const categoria = document.getElementById('tipo').value;
    const nivelUrgencia = document.getElementById('prioridade').value;
    const descricao = document.getElementById('descricao').value;

    if (!titulo || !categoria || !nivelUrgencia || !descricao) {
        alert('Por favor, preencha todos os campos obrigatórios.');
        return;
    }

    // Usamos o ID do usuário que veio do token, em vez de um número fixo
    const payloadTexto = { titulo, descricao, categoria, nivelUrgencia, colaboradorId };

    try {
        // ETAPA 1: Criar o chamado com os dados de texto
        const responseChamado = await fetch('http://localhost:5000/api/chamados/abrir', {
            method: 'POST',
            headers: { 
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}` // <-- ENVIANDO O TOKEN
            },
            body: JSON.stringify(payloadTexto)
        });

        if (responseChamado.status === 401) {
            alert('Sessão expirada. Faça login novamente.');
            window.location.href = 'login.html';
            return;
        }
        if (!responseChamado.ok) {
            throw new Error('Falha ao criar o chamado.');
        }

        const chamadoCriado = await responseChamado.json();
        const novoChamadoId = chamadoCriado.id;

        // ETAPA 2: Enviar os arquivos, se existirem
        const inputAnexos = document.getElementById('file-upload');
        const arquivos = inputAnexos.files;

        if (arquivos.length > 0) {
            const uploadPromises = [];
            for (const arquivo of arquivos) {
                const formData = new FormData();
                formData.append('file', arquivo);

                const uploadPromise = fetch(`http://localhost:5000/api/chamados/${novoChamadoId}/anexos`, {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${token}` // <-- ENVIANDO O TOKEN TAMBÉM NO UPLOAD
                    },
                    body: formData
                });
                uploadPromises.push(uploadPromise);
            }
            await Promise.all(uploadPromises);
        }

        alert(`Chamado #${novoChamadoId} aberto com sucesso!`);
        form.reset();

    } catch (error) {
        console.error('Erro no processo de abertura de chamado:', error);
        alert('Ocorreu um erro ao abrir o chamado. Verifique o console para mais detalhes.');
    }
  });
});

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