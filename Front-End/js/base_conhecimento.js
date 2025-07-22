// Configurações da API
const API_BASE_URL = 'http://localhost:5000/api/baseconhecimento';
const ARTICLES_ENDPOINT = `${API_BASE_URL}/artigos`;

// Estado global da aplicação
let artigos = [];
let artigosFiltrados = [];
let artigoParaExcluir = null;
let paginaAtual = 1;
const itensPorPagina = 9;

// Elementos do DOM
const articlesContainer = document.getElementById('articlesContainer');
const searchInput = document.getElementById('searchInput');
const categoryFilter = document.getElementById('categoryFilter');
const sortFilter = document.getElementById('sortFilter');
const articleModal = document.getElementById('articleModal');
const deleteModal = document.getElementById('deleteModal');
const articleForm = document.getElementById('articleForm');

// Inicialização da página
document.addEventListener('DOMContentLoaded', function() {
    carregarHeaderFooter();
    carregarArtigos();
    configurarEventListeners();
});

// Carregar header e footer
async function carregarHeaderFooter() {
    try {
        const headerResponse = await fetch('../header.html');
        const headerHtml = await headerResponse.text();
        document.getElementById('header').innerHTML = headerHtml;

        const footerResponse = await fetch('../footer.html');
        const footerHtml = await footerResponse.text();
        document.getElementById('footer').innerHTML = footerHtml;
    } catch (error) {
        console.error('Erro ao carregar header/footer:', error);
    }
}

// Configurar event listeners
function configurarEventListeners() {
    // Busca em tempo real
    searchInput.addEventListener('input', debounce(buscarArtigos, 300));
    
    // Filtros
    categoryFilter.addEventListener('change', filtrarPorCategoria);
    sortFilter.addEventListener('change', ordenarArtigos);
    
    // Form de artigo
    articleForm.addEventListener('submit', handleSubmitArtigo);
    
    // Fechar modais com ESC
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            fecharModal();
            fecharModalDelete();
        }
    });
    
    // Fechar modais clicando fora
    articleModal.addEventListener('click', function(e) {
        if (e.target === articleModal) {
            fecharModal();
        }
    });
    
    deleteModal.addEventListener('click', function(e) {
        if (e.target === deleteModal) {
            fecharModalDelete();
        }
    });
}

// Função debounce para otimizar busca
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

// Carregar artigos da API
async function carregarArtigos() {
    try {
        mostrarLoading();
        
        const token = localStorage.getItem('jwt_token');
        const response = await fetch(ARTICLES_ENDPOINT, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        artigos = Array.isArray(data) ? data : [];
        artigosFiltrados = [...artigos];
        
        aplicarFiltros();
        renderizarArtigos();
        
    } catch (error) {
        console.error('Erro ao carregar artigos:', error);
        mostrarErro('Erro ao carregar artigos. Tente novamente.');
    }
}

// Buscar artigos
function buscarArtigos() {
    const termo = searchInput.value.toLowerCase().trim();
    
    if (!termo) {
        artigosFiltrados = [...artigos];
    } else {
        artigosFiltrados = artigos.filter(artigo => 
            artigo.titulo.toLowerCase().includes(termo) ||
            artigo.conteudo.toLowerCase().includes(termo) ||
            artigo.categoria.toLowerCase().includes(termo) ||
            (artigo.palavrasChave && artigo.palavrasChave.toLowerCase().includes(termo))
        );
    }
    
    paginaAtual = 1;
    aplicarFiltros();
    renderizarArtigos();
}

// Filtrar por categoria
function filtrarPorCategoria() {
    const categoria = categoryFilter.value;
    
    if (!categoria) {
        artigosFiltrados = artigos.filter(artigo => 
            !searchInput.value || 
            artigo.titulo.toLowerCase().includes(searchInput.value.toLowerCase()) ||
            artigo.conteudo.toLowerCase().includes(searchInput.value.toLowerCase())
        );
    } else {
        artigosFiltrados = artigos.filter(artigo => 
            artigo.categoria === categoria &&
            (!searchInput.value || 
             artigo.titulo.toLowerCase().includes(searchInput.value.toLowerCase()) ||
             artigo.conteudo.toLowerCase().includes(searchInput.value.toLowerCase()))
        );
    }
    
    paginaAtual = 1;
    aplicarFiltros();
    renderizarArtigos();
}

// Ordenar artigos
function ordenarArtigos() {
    const ordenacao = sortFilter.value;
    
    artigosFiltrados.sort((a, b) => {
        switch (ordenacao) {
            case 'titulo':
                return a.titulo.localeCompare(b.titulo);
            case 'categoria':
                return a.categoria.localeCompare(b.categoria);
            case 'dataCriacao':
            default:
                return new Date(b.dataCriacao) - new Date(a.dataCriacao);
        }
    });
    
    renderizarArtigos();
}

// Aplicar filtros
function aplicarFiltros() {
    // Aplicar busca
    const termo = searchInput.value.toLowerCase().trim();
    if (termo) {
        artigosFiltrados = artigosFiltrados.filter(artigo => 
            artigo.titulo.toLowerCase().includes(termo) ||
            artigo.conteudo.toLowerCase().includes(termo) ||
            artigo.categoria.toLowerCase().includes(termo) ||
            (artigo.palavrasChave && artigo.palavrasChave.toLowerCase().includes(termo))
        );
    }
    
    // Aplicar categoria
    const categoria = categoryFilter.value;
    if (categoria) {
        artigosFiltrados = artigosFiltrados.filter(artigo => 
            artigo.categoria === categoria
        );
    }
    
    // Aplicar ordenação
    ordenarArtigos();
}

// Renderizar artigos
function renderizarArtigos() {
    const destaqueGrid = document.getElementById('destaqueGrid');
    
    if (!Array.isArray(artigosFiltrados) || artigosFiltrados.length === 0) {
        destaqueGrid.innerHTML = `<div class='col-span-full text-center text-gray-500 py-12'>Nenhum artigo encontrado</div>`;
        document.getElementById('pagination').style.display = 'none';
        return;
    }
    
    // Calcular artigos para a página atual
    const inicio = (paginaAtual - 1) * itensPorPagina;
    const fim = inicio + itensPorPagina;
    const artigosPagina = artigosFiltrados.slice(inicio, fim);
    
    // Renderizar artigos em destaque com paginação
    destaqueGrid.innerHTML = artigosPagina.map(artigo => criarCardDestaqueArtigo(artigo)).join('');
    
    renderizarPaginacao();
}

function criarCardDestaqueArtigo(artigo) {
    let imagem = (artigo.imagemUrl || artigo.ImagemUrl || artigo.imagem_url || '').trim() || 'https://images.unsplash.com/photo-1587831990711-23d7e9a424d8?q=80&w=2070';
    const backendUrl = 'http://localhost:5000';
    if (imagem.startsWith('/')) {
        imagem = backendUrl + imagem;
    }
    const titulo = (artigo.titulo || artigo.Titulo || '').trim() || '—';
    const categoria = (artigo.categoria || artigo.Categoria || '').trim() || '—';
    const conteudo = (artigo.conteudo || artigo.Conteudo || '').trim() || 'Sem conteúdo';
    const tags = (artigo.palavrasChave || artigo.PalavrasChave || '').split(',').map(t => t.trim()).filter(Boolean);
    const dataFormatada = artigo.dataCriacao ? new Date(artigo.dataCriacao).toLocaleDateString('pt-BR') : '—';
    const autor = (artigo.nomeAutor || artigo.NomeAutor || '').trim() || 'Sistema';
    
    // Verificar se o usuário atual é o autor do artigo
    const userId = localStorage.getItem('userId');
    const autorId = artigo.autorId || artigo.AutorId;
    const isAutor = userId && autorId && parseInt(userId) === parseInt(autorId);
    
    // Botões de ação baseados na permissão
    const botoesAcao = isAutor ? `
        <button onclick="editarArtigo(${artigo.id || artigo.Id})" 
                class="flex-1 bg-blue-500 hover:bg-blue-600 text-white px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 flex items-center justify-center gap-1">
            <i class="fas fa-edit text-xs"></i>
            Editar
        </button>
        <button onclick="excluirArtigo(${artigo.id || artigo.Id}, '${titulo}')" 
                class="flex-1 bg-red-500 hover:bg-red-600 text-white px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 flex items-center justify-center gap-1">
            <i class="fas fa-trash text-xs"></i>
            Excluir
        </button>
    ` : `
        <div class="flex-1"></div>
        <div class="flex-1"></div>
    `;
    
    return `
        <div class="bg-white rounded-xl shadow-sm overflow-hidden group transition-all duration-300 hover:shadow-xl hover:-translate-y-1">
            <img class="h-40 w-full object-cover" src="${imagem}" alt="Imagem do artigo">
            <div class="p-4">
                <div class="flex items-center justify-between mb-2">
                    <span class="text-xs font-medium bg-blue-100 text-blue-800 px-2 py-0.5 rounded-full">${categoria}</span>
                    <span class="text-xs text-gray-500">${dataFormatada}</span>
                </div>
                <div class="flex items-center space-x-2 mb-2">
                    <i class="fas fa-book-open text-lg text-primary"></i>
                    <h3 class="font-bold text-gray-800 group-hover:text-primary line-clamp-2">${titulo}</h3>
                </div>
                <p class="text-gray-600 text-sm mb-3 line-clamp-2">${conteudo}</p>
                <div class="flex items-center justify-between text-xs text-gray-500 mb-3">
                    <div class="flex items-center gap-1">
                        <i class="fas fa-user text-blue-500"></i>
                        <span>${autor}</span>
                    </div>
                    <div class="flex items-center gap-1">
                        <i class="fas fa-eye text-gray-400"></i>
                        <span>${artigo.visualizacoes || 0}</span>
                    </div>
                </div>
                <div class="flex flex-wrap gap-2 mb-3">
                    ${tags.map(tag => `<span class='text-xs font-medium bg-gray-100 text-gray-800 px-2 py-0.5 rounded-full'>${tag}</span>`).join('')}
                </div>
                <div class="flex gap-2">
                    <button onclick="visualizarArtigo(${artigo.id || artigo.Id})" 
                            class="flex-1 bg-gray-100 hover:bg-gray-200 text-gray-700 px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 flex items-center justify-center gap-1">
                        <i class="fas fa-eye text-xs"></i>
                        Ver
                    </button>
                    ${botoesAcao}
                </div>
            </div>
        </div>
    `;
}

// Criar card de artigo
function criarCardArtigo(artigo) {
    const dataFormatada = artigo.dataCriacao ? new Date(artigo.dataCriacao).toLocaleDateString('pt-BR') : '—';
    const conteudo = (artigo.conteudo || artigo.Conteudo || '').trim() || 'Sem conteúdo';
    const titulo = (artigo.titulo || artigo.Titulo || '').trim() || '—';
    const categoria = (artigo.categoria || artigo.Categoria || '').trim() || '—';
    const autor = (artigo.nomeAutor || artigo.NomeAutor || '').trim() || 'Sistema';
    const conteudoResumido = conteudo.length > 120 
        ? conteudo.substring(0, 120) + '...' 
        : conteudo;
    
    return `
        <div class="bg-white rounded-xl shadow-sm hover:shadow-lg transition-all duration-300 border border-gray-100 overflow-hidden group hover:-translate-y-1">
            <div class="p-4">
                <div class="flex items-center justify-between mb-2">
                    <span class="inline-block bg-blue-100 text-blue-800 text-xs font-semibold px-2 py-1 rounded-full">
                        ${categoria}
                    </span>
                    <span class="text-xs text-gray-500">${dataFormatada}</span>
                </div>
                <h3 class="text-lg font-bold text-gray-800 mb-2 line-clamp-2 group-hover:text-blue-600">${titulo}</h3>
                <p class="text-gray-600 text-sm leading-relaxed mb-3 line-clamp-3">${conteudoResumido}</p>
                <div class="flex items-center justify-between text-xs text-gray-500 mb-3">
                    <div class="flex items-center gap-1">
                        <i class="fas fa-user text-blue-500"></i>
                        <span>${autor}</span>
                    </div>
                    <div class="flex items-center gap-1">
                        <i class="fas fa-eye text-gray-400"></i>
                        <span>${artigo.visualizacoes || 0}</span>
                    </div>
                </div>
                <div class="flex gap-2">
                    <button onclick="visualizarArtigo(${artigo.id || artigo.Id})" 
                            class="flex-1 bg-gray-100 hover:bg-gray-200 text-gray-700 px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 flex items-center justify-center gap-1">
                        <i class="fas fa-eye text-xs"></i>
                        Ver
                    </button>
                    <button onclick="editarArtigo(${artigo.id || artigo.Id})" 
                            class="flex-1 bg-blue-500 hover:bg-blue-600 text-white px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 flex items-center justify-center gap-1">
                        <i class="fas fa-edit text-xs"></i>
                        Editar
                    </button>
                </div>
            </div>
        </div>
    `;
}

// Renderizar paginação
function renderizarPaginacao() {
    const totalPaginas = Math.ceil(artigosFiltrados.length / itensPorPagina);
    
    if (totalPaginas <= 1) {
        document.getElementById('pagination').style.display = 'none';
        return;
    }
    
    let paginacaoHTML = '';
    
    // Botão anterior
    paginacaoHTML += `
        <button onclick="mudarPagina(${paginaAtual - 1})" 
                ${paginaAtual === 1 ? 'disabled' : ''}
                class="px-4 py-2 border border-gray-300 bg-white rounded-lg hover:border-blue-500 hover:text-blue-500 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed">
            <i class="fas fa-chevron-left"></i>
        </button>
    `;
    
    // Páginas
    for (let i = 1; i <= totalPaginas; i++) {
        if (i === 1 || i === totalPaginas || (i >= paginaAtual - 2 && i <= paginaAtual + 2)) {
            paginacaoHTML += `
                <button onclick="mudarPagina(${i})" 
                        class="px-4 py-2 border border-gray-300 rounded-lg transition-all duration-200 ${i === paginaAtual ? 'bg-blue-500 text-white border-blue-500' : 'bg-white hover:border-blue-500 hover:text-blue-500'}">
                    ${i}
                </button>
            `;
        } else if (i === paginaAtual - 3 || i === paginaAtual + 3) {
            paginacaoHTML += '<span class="px-2 text-gray-500">...</span>';
        }
    }
    
    // Botão próximo
    paginacaoHTML += `
        <button onclick="mudarPagina(${paginaAtual + 1})" 
                ${paginaAtual === totalPaginas ? 'disabled' : ''}
                class="px-4 py-2 border border-gray-300 bg-white rounded-lg hover:border-blue-500 hover:text-blue-500 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed">
            <i class="fas fa-chevron-right"></i>
        </button>
    `;
    
    document.getElementById('pagination').innerHTML = paginacaoHTML;
    document.getElementById('pagination').style.display = 'flex';
}

// Mudar página
function mudarPagina(novaPagina) {
    const totalPaginas = Math.ceil(artigosFiltrados.length / itensPorPagina);
    
    if (novaPagina >= 1 && novaPagina <= totalPaginas) {
        paginaAtual = novaPagina;
        renderizarArtigos();
    }
}

// Abrir modal para criar artigo
function abrirModalCriar() {
    document.getElementById('modalTitle').textContent = 'Novo Artigo';
    document.getElementById('articleId').value = '';
    document.getElementById('titulo').value = '';
    document.getElementById('categoria').value = '';
    document.getElementById('conteudo').value = '';
    document.getElementById('palavrasChave').value = '';
    
    articleModal.classList.remove('hidden');
    document.getElementById('titulo').focus();
}

// Editar artigo
async function editarArtigo(id) {
    try {
        const token = localStorage.getItem('jwt_token');
        const response = await fetch(`${ARTICLES_ENDPOINT}/${id}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const artigo = await response.json();
        
        document.getElementById('modalTitle').textContent = 'Editar Artigo';
        document.getElementById('articleId').value = artigo.id;
        document.getElementById('titulo').value = artigo.titulo;
        document.getElementById('categoria').value = artigo.categoria;
        document.getElementById('conteudo').value = artigo.conteudo;
        document.getElementById('palavrasChave').value = artigo.palavrasChave || '';
        
        articleModal.classList.remove('hidden');
        
    } catch (error) {
        console.error('Erro ao carregar artigo:', error);
        mostrarErro('Erro ao carregar dados do artigo.');
    }
}

// Visualizar artigo
async function visualizarArtigo(id) {
    try {
        const token = localStorage.getItem('jwt_token');
        const response = await fetch(`${ARTICLES_ENDPOINT}/${id}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const artigo = await response.json();
        
        // Criar modal de visualização
        const modalHTML = `
            <div class="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4">
                <div class="bg-white rounded-xl shadow-2xl w-full max-w-4xl max-h-[90vh] overflow-y-auto">
                    <div class="flex items-center justify-between p-6 border-b border-gray-200">
                        <h2 class="text-2xl font-bold text-gray-800">${artigo.titulo}</h2>
                        <button onclick="this.closest('.fixed').remove()" class="text-gray-400 hover:text-gray-600 text-2xl font-bold">
                            &times;
                        </button>
                    </div>
                    
                    <div class="p-6">
                        <div class="mb-6">
                            <span class="inline-block bg-gradient-to-r from-blue-500 to-indigo-600 text-white text-sm font-semibold px-3 py-1 rounded-full mb-3">
                                ${artigo.categoria}
                            </span>
                            <div class="text-sm text-gray-500 flex items-center gap-4">
                                <span class="flex items-center gap-1">
                                    <i class="fas fa-user text-blue-500"></i>
                                    ${artigo.autor || 'Sistema'}
                                </span>
                                <span class="flex items-center gap-1">
                                    <i class="fas fa-calendar text-gray-400"></i>
                                    ${new Date(artigo.dataCriacao).toLocaleDateString('pt-BR')}
                                </span>
                            </div>
                        </div>
                        
                        <div class="prose max-w-none text-gray-700 leading-relaxed mb-6">
                            ${artigo.conteudo.replace(/\n/g, '<br>')}
                        </div>
                        
                        ${artigo.palavrasChave ? `
                            <div class="pt-6 border-t border-gray-200">
                                <strong class="text-gray-800">Palavras-chave:</strong> 
                                <span class="text-gray-600">${artigo.palavrasChave}</span>
                            </div>
                        ` : ''}
                        
                        <div class="flex gap-3 justify-end pt-6 border-t border-gray-200">
                            <button onclick="this.closest('.fixed').remove()" 
                                    class="px-6 py-3 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition-all duration-200 font-semibold">
                                Fechar
                            </button>
                            <button onclick="editarArtigo(${artigo.id}); this.closest('.fixed').remove()" 
                                    class="bg-gradient-to-r from-blue-500 to-indigo-600 text-white px-6 py-3 rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all duration-200 font-semibold flex items-center gap-2">
                                <i class="fas fa-edit"></i>
                                Editar
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        document.body.insertAdjacentHTML('beforeend', modalHTML);
        
    } catch (error) {
        console.error('Erro ao carregar artigo:', error);
        mostrarErro('Erro ao carregar dados do artigo.');
    }
}

// Excluir artigo
function excluirArtigo(id, titulo) {
    artigoParaExcluir = { id, titulo };
    document.getElementById('deleteArticleTitle').textContent = titulo;
    deleteModal.classList.remove('hidden');
}

// Confirmar exclusão
async function confirmarExclusao() {
    if (!artigoParaExcluir) return;
    
    try {
        const token = localStorage.getItem('jwt_token');
        const response = await fetch(`${ARTICLES_ENDPOINT}/${artigoParaExcluir.id}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        mostrarSucesso('Artigo excluído com sucesso!');
        fecharModalDelete();
        carregarArtigos();
        
    } catch (error) {
        console.error('Erro ao excluir artigo:', error);
        mostrarErro('Erro ao excluir artigo. Tente novamente.');
    }
}

// Preview da imagem ao selecionar arquivo
const imagemArquivoInput = document.getElementById('imagemArquivo');
const previewImagemDiv = document.getElementById('previewImagem');
if (imagemArquivoInput) {
    imagemArquivoInput.addEventListener('change', function() {
        previewImagemDiv.innerHTML = '';
        if (this.files && this.files[0]) {
            const reader = new FileReader();
            reader.onload = function(e) {
                previewImagemDiv.innerHTML = `<img src='${e.target.result}' alt='Preview' class='h-32 rounded-lg border mt-2'>`;
            };
            reader.readAsDataURL(this.files[0]);
        }
    });
}

// Função para upload da imagem
async function uploadImagem(file) {
    const formData = new FormData();
    formData.append('imagem', file);
    const token = localStorage.getItem('jwt_token');
    const response = await fetch('http://localhost:5000/api/baseconhecimento/upload-imagem', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`
        },
        body: formData
    });
    if (!response.ok) throw new Error('Erro ao fazer upload da imagem');
    const data = await response.json();
    return data.url || data.caminho || data.path; // backend deve retornar a url/caminho salvo
}

// Handle submit do formulário
async function handleSubmitArtigo(e) {
    e.preventDefault();
    
    const formData = new FormData(articleForm);
    const userId = localStorage.getItem('userId');
    let imagemUrl = '';
    const imagemArquivo = imagemArquivoInput && imagemArquivoInput.files && imagemArquivoInput.files[0] ? imagemArquivoInput.files[0] : null;
    if (imagemArquivo) {
        try {
            imagemUrl = await uploadImagem(imagemArquivo);
        } catch (err) {
            mostrarErro('Erro ao fazer upload da imagem.');
            return;
        }
    }
    const artigo = {
        Titulo: formData.get('titulo'),
        Categoria: formData.get('categoria'),
        Conteudo: formData.get('conteudo'),
        PalavrasChave: formData.get('palavrasChave'),
        ImagemUrl: imagemUrl,
        AutorId: userId
    };
    const id = formData.get('id');
    const isEditing = id && id !== '';
    try {
        const token = localStorage.getItem('jwt_token');
        const url = isEditing ? `${ARTICLES_ENDPOINT}/${id}` : ARTICLES_ENDPOINT;
        const method = isEditing ? 'PUT' : 'POST';
        const response = await fetch(url, {
            method: method,
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(artigo)
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        mostrarSucesso(isEditing ? 'Artigo atualizado com sucesso!' : 'Artigo criado com sucesso!');
        fecharModal();
        carregarArtigos();
    } catch (error) {
        console.error('Erro ao salvar artigo:', error);
        mostrarErro('Erro ao salvar artigo. Tente novamente.');
    }
}

// Fechar modal
function fecharModal() {
    articleModal.classList.add('hidden');
    articleForm.reset();
}

// Fechar modal de exclusão
function fecharModalDelete() {
    deleteModal.classList.add('hidden');
    artigoParaExcluir = null;
}

// Mostrar loading
function mostrarLoading() {
    const destaqueGrid = document.getElementById('destaqueGrid');
    destaqueGrid.innerHTML = `
        <div class="col-span-full flex items-center justify-center py-12">
            <div class="text-center">
                <div class="inline-flex items-center space-x-2">
                    <div class="w-2 h-2 bg-blue-500 rounded-full animate-bounce"></div>
                    <div class="w-2 h-2 bg-blue-500 rounded-full animate-bounce" style="animation-delay: 0.1s"></div>
                    <div class="w-2 h-2 bg-blue-500 rounded-full animate-bounce" style="animation-delay: 0.2s"></div>
                </div>
                <p class="text-gray-600 mt-4">Carregando artigos...</p>
            </div>
        </div>
    `;
}

// Mostrar estado vazio
function mostrarEstadoVazio() {
    articlesContainer.innerHTML = `
        <div class="text-center py-16">
            <i class="fas fa-book-open text-6xl text-gray-300 mb-6"></i>
            <h3 class="text-2xl font-bold text-gray-700 mb-2">Nenhum artigo encontrado</h3>
            <p class="text-gray-500">Não há artigos que correspondam aos filtros aplicados.</p>
        </div>
    `;
    document.getElementById('pagination').style.display = 'none';
}

// Mostrar mensagem de sucesso
function mostrarSucesso(mensagem) {
    const toast = document.createElement('div');
    toast.className = 'fixed top-5 right-5 bg-gradient-to-r from-green-500 to-emerald-600 text-white px-6 py-4 rounded-lg shadow-lg z-50 font-semibold transform translate-x-full transition-transform duration-300';
    toast.textContent = mensagem;
    
    document.body.appendChild(toast);
    
    // Animar entrada
    setTimeout(() => {
        toast.classList.remove('translate-x-full');
    }, 100);
    
    // Animar saída
    setTimeout(() => {
        toast.classList.add('translate-x-full');
        setTimeout(() => {
            document.body.removeChild(toast);
        }, 300);
    }, 3000);
}

// Mostrar mensagem de erro
function mostrarErro(mensagem) {
    const toast = document.createElement('div');
    toast.className = 'fixed top-5 right-5 bg-gradient-to-r from-red-500 to-pink-600 text-white px-6 py-4 rounded-lg shadow-lg z-50 font-semibold transform translate-x-full transition-transform duration-300';
    toast.textContent = mensagem;
    
    document.body.appendChild(toast);
    
    // Animar entrada
    setTimeout(() => {
        toast.classList.remove('translate-x-full');
    }, 100);
    
    // Animar saída
    setTimeout(() => {
        toast.classList.add('translate-x-full');
        setTimeout(() => {
            document.body.removeChild(toast);
        }, 300);
    }, 4000);
} 