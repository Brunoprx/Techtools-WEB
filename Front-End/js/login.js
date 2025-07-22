// Arquivo de Depuração: js/login.js

document.addEventListener('DOMContentLoaded', () => {
    const formLogin = document.getElementById('form-login');

    if (formLogin) {
        formLogin.addEventListener('submit', async (event) => {
            event.preventDefault();
            const email = document.getElementById('email').value;
            const senha = document.getElementById('password').value;

            if (!email || !senha) {
                alert('Por favor, preencha o e-mail e a senha.');
                return;
            }
            
            const payload = { email, senha };
            const apiUrl = 'http://localhost:5000/api/autenticacao/login';

            try {
                const response = await fetch(apiUrl, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload)
                });

                if (response.ok) {
                    const data = await response.json();
                    const token = data.token;
                    localStorage.setItem('jwt_token', token);
                    
                    // ===============================================
                    //     INÍCIO DA LÓGICA DE DEPURAÇÃO
                    // ===============================================
                    
                    const payloadToken = parseJwt(token);

                    // Salvar o ID do usuário no localStorage
                    const userId = payloadToken.sub;
                    localStorage.setItem('userId', userId);

                    // A linha mais importante: vamos ver todas as informações do token!
                    console.log("INFORMAÇÕES DO TOKEN:", payloadToken);

                    // Pegamos a informação do perfil. A chave pode variar.
                    const perfil = payloadToken.role || payloadToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
                    
                    console.log("Perfil encontrado no token:", perfil);

                    alert('Login realizado com sucesso! Verificando perfil para redirecionamento...');
                    
                    if (perfil === 'Técnico') {
                        window.location.href = 'tecnicos.html';
                    } else if (perfil === 'Administrador' || perfil === 'Gestor') {
                        window.location.href = 'relatorios.html';
                    } else {
                        window.location.href = 'chamados.html';
                    }

                } else {
                    const errorData = await response.json();
                    alert(`Falha no login: ${errorData.message}`);
                }
            } catch (error) {
                console.error('Erro ao conectar com a API:', error);
                alert('Erro de comunicação com o servidor.');
            }
        });
    }
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