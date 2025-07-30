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

document.addEventListener('DOMContentLoaded', () => {
    // --- Checagem de permissão inicial ---
    const token = localStorage.getItem('jwt_token');
    if (!token) {
        window.location.href = 'login.html';
        return;
    }
    const payload = parseJwt(token);
    const perfil = payload?.role || payload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    const empresaId = payload?.EmpresaId;
    if (!empresaId) {
        alert('Erro: ID da empresa não encontrado no token. Faça o login novamente.');
        window.location.href = 'login.html';
        return;
    }
    if (perfil !== 'Administrador' && perfil !== 'RH') {
        alert('Acesso restrito a Administradores e RH.');
        window.location.href = 'chamados.html';
        return;
    }

    // --- Mapeamento dos elementos do HTML ---
    const tabelaBody = document.querySelector('table tbody');
    const modal = document.getElementById('modal-usuario');
    const modalTitulo = document.getElementById('modal-titulo');
    const formModal = document.getElementById('form-usuario');
    const inputIdUsuario = document.getElementById('id-usuario');
    const inputNome = document.getElementById('form-nome');
    const inputEmail = document.getElementById('form-email');
    const inputCargo = document.getElementById('form-cargo');
    const selectSetor = document.getElementById('form-setor');
    const selectPerfil = document.getElementById('form-perfil');
    const campoSenha = document.getElementById('campo-senha');
    const modalBackground = document.getElementById('modal-background');
    const btnFecharModalX = document.getElementById('btn-fechar-modal-x');
    const btnCancelarModal = document.getElementById('btn-cancelar-modal'); 
    const btnAdicionarUsuario = document.getElementById('btn-adicionar-usuario');

    // --- Funções de Feedback e Modal ---
    const feedbackToast = document.getElementById('feedback-toast');
    const feedbackMessage = document.getElementById('feedback-message');
    const showFeedback = (msg, success = true) => {
        feedbackMessage.textContent = msg;
        feedbackToast.style.backgroundColor = success ? '#10B981' : '#EF4444';
        feedbackToast.style.display = 'block';
        feedbackToast.style.transform = 'translateX(0)';
        setTimeout(() => { 
            feedbackToast.style.transform = 'translateX(100%)';
            setTimeout(() => { feedbackToast.style.display = 'none'; }, 300);
        }, 3000);
    };

    const fecharModal = () => modal.classList.add('hidden');

    // --- Adiciona os listeners para fechar o modal ---
modalBackground.addEventListener('click', fecharModal);
btnFecharModalX.addEventListener('click', fecharModal);
btnCancelarModal.addEventListener('click', fecharModal);
    
btnAdicionarUsuario.addEventListener('click', () => {
    modalTitulo.textContent = 'Adicionar Novo Usuário';
    formModal.reset();
    inputIdUsuario.value = '';
    campoSenha.style.display = 'block';
    modal.classList.remove('hidden');
});


    // --- Carregamento de Dados Iniciais ---
    const carregarOpcoesDropdown = async (url, selectElement, placeholder) => {
        try {
            const response = await fetch(url, { headers: { 'Authorization': `Bearer ${token}` } });
            if (!response.ok) throw new Error('Falha ao carregar opções.');
            const data = await response.json();
            selectElement.innerHTML = `<option value="">${placeholder}</option>` + data.map(item => `<option value="${item}">${item}</option>`).join('');
        } catch (error) { console.error(`Erro ao carregar ${placeholder}:`, error); }
    };

    carregarOpcoesDropdown('http://localhost:5000/api/usuarios/perfis', selectPerfil, 'Selecione o perfil');
    carregarOpcoesDropdown('http://localhost:5000/api/usuarios/setores', selectSetor, 'Selecione o setor');
    
    const carregarUsuarios = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/usuarios', { headers: { 'Authorization': `Bearer ${token}` } });
            if (!response.ok) throw new Error('Falha ao carregar usuários.');
            const usuarios = await response.json();
            tabelaBody.innerHTML = '';
            usuarios.forEach(u => {
                tabelaBody.innerHTML += `
                    <tr>
                        <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${u.nome}</td>
                        <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${u.setor || ''} / ${u.cargo || ''}</td>
                        <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${u.perfilAcesso}</td>
                        <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-4">
                            <button onclick="editarUsuario(${u.id})" class="text-gray-400 hover:text-primary"><i class="fas fa-edit"></i></button>
                            <button onclick="excluirUsuario(${u.id})" class="text-gray-400 hover:text-red-600"><i class="fas fa-trash-alt"></i></button>
                        </td>
                    </tr>
                `;
            });
        } catch (error) {
            console.error('Erro ao carregar usuários:', error);
        }
    };
    
    carregarUsuarios();

    // --- Lógica para o Formulário (Adicionar e Editar) ---
    console.log('Anexando event listener ao formulário:', formModal);
    
    if (!formModal) {
        console.error('Formulário não encontrado! Verifique o ID form-usuario');
        return;
    }
    
    formModal.addEventListener('submit', async (e) => {
        console.log('Evento submit disparado!');
        e.preventDefault();
        const id = inputIdUsuario.value;
        const ehEdicao = !!id;
        
        console.log('Dados do formulário:', {
            nome: inputNome.value,
            email: inputEmail.value, // Corrigido para 'email'
            cargo: inputCargo.value,
            setor: selectSetor.value,
            perfilAcesso: selectPerfil.value,
            ehEdicao
        });

        const dadosUsuario = {
            nome: inputNome.value,
            email: inputEmail.value, // Corrigido para 'email'
            cargo: inputCargo.value,
            setor: selectSetor.value,
            perfilAcesso: selectPerfil.value,
            senha: document.getElementById('form-senha').value,
            empresaId: empresaId,
            cpf: document.getElementById('form-cpf').value // Agora o campo CPF é enviado ao backend
        };

        const url = ehEdicao ? `http://localhost:5000/api/usuarios/${id}` : 'http://localhost:5000/api/usuarios';
        const method = ehEdicao ? 'PUT' : 'POST';

        try {
            const response = await fetch(url, {
                method: method,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(dadosUsuario)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Falha ao salvar usuário.');
            }

            const data = await response.json();
            if (response.ok) {
                carregarUsuarios(); // Recarrega a lista de usuários antes de fechar o modal
                fecharModal(); // Fecha o modal
                showFeedback(`Usuário ${ehEdicao ? 'atualizado' : 'criado'} com sucesso!`);
            } else {
                showFeedback(errorData.message || 'Falha ao salvar usuário.', false);
            }
        } catch (error) {
            showFeedback(error.message, false);
            console.error('Erro ao salvar usuário:', error);
        }
    });

    // --- Funções para Editar e Excluir Usuários ---
    window.editarUsuario = async (id) => {
        try {
            const response = await fetch(`http://localhost:5000/api/usuarios/${id}`, { headers: { 'Authorization': `Bearer ${token}` } });
            if (!response.ok) throw new Error('Falha ao carregar dados do usuário.');
            const usuario = await response.json();
            inputIdUsuario.value = usuario.id;
            inputNome.value = usuario.nome;
            inputEmail.value = usuario.emailCorporativo || usuario.email || ''; // Corrigido para usar o campo correto
            inputCargo.value = usuario.cargo || '';
            selectSetor.value = usuario.setor || '';
            selectPerfil.value = usuario.perfilAcesso || '';
            campoSenha.style.display = 'none'; // Esconde o campo de senha ao editar
            modalTitulo.textContent = 'Editar Usuário';
            modal.classList.remove('hidden');
        } catch (error) {
            showFeedback('Erro ao carregar dados do usuário para edição.', false);
            console.error('Erro ao carregar dados do usuário para edição:', error);
        }
    };

    window.excluirUsuario = async (id) => {
        if (!confirm('Tem certeza que deseja excluir este usuário?')) return;
        try {
            const response = await fetch(`http://localhost:5000/api/usuarios/${id}`, {
                method: 'DELETE',
                headers: { 'Authorization': `Bearer ${token}` }
            });
            if (!response.ok) throw new Error('Falha ao excluir usuário.');
            showFeedback('Usuário excluído com sucesso!', true);
            carregarUsuarios(); // Recarrega a lista de usuários
        } catch (error) {
            showFeedback(error.message, false);
            console.error('Erro ao excluir usuário:', error);
        }
    };
});
