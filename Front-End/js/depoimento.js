/**
 * Script para controlar um carrossel 3D com autoplay inteligente.
 * * Funcionalidades:
 * 1. Cria um carrossel 3D a partir de elementos HTML.
 * 2. Gira automaticamente a cada intervalo de tempo definido.
 * 3. Pausa a rotação quando o utilizador passa o rato sobre ele.
 * 4. Pausa a rotação quando o utilizador clica nos botões de navegação.
 * 5. Retoma a rotação automaticamente após um período de inatividade após um clique manual.
 */
document.addEventListener('DOMContentLoaded', () => {

    // --- 1. SELEÇÃO DOS ELEMENTOS ---
    // Selecionamos todos os elementos do DOM com os quais vamos interagir.
    const carouselContainer = document.querySelector('.container');
    const carousel = document.querySelector('#carousel');
    const cells = document.querySelectorAll('.carousel-cell');
    const prevButton = document.querySelector('#prev-button');
    const nextButton = document.querySelector('#next-button');
  
    // --- 2. CONFIGURAÇÕES E VARIÁVEIS DE ESTADO ---
    if (cells.length === 0) return; // Segurança: não executa se não houver cartões.
  
    const cellCount = cells.length;
    let selectedIndex = 0;
    
    // Configurações do Carrossel (ajustadas para a Opção 1 que você gostou)
    const rotateAngle = 360 / cellCount;
    const radius = 320; // O raio que definimos para alargar o carrossel
  
    // Configurações do Autoplay
    const autoplayInterval = 4000; // Tempo em milissegundos (4s) para cada rotação
    const restartDelay = 10000; // Tempo em milissegundos (10s) para reiniciar após clique
    let intervalId; // Para guardar o ID do loop de autoplay
    let restartTimeout; // Para guardar o ID do agendamento de reinício
  
    // --- 3. FUNÇÕES PRINCIPAIS ---
  
    /**
     * Gira o carrossel e aplica os estilos (escala e opacidade) a cada cartão.
     */
    function rotateCarousel() {
      const rotationY = selectedIndex * -rotateAngle;
      carousel.style.transform = `translateZ(-${radius}px) rotateY(${rotationY}deg)`;
  
      // Calcula o índice real para lidar com "voltas" infinitas
      const actualSelectedIndex = ((selectedIndex % cellCount) + cellCount) % cellCount;
  
      cells.forEach((cell, index) => {
        const isSelected = index === actualSelectedIndex;
        const scale = isSelected ? 1 : 0.85;
        const cellAngle = index * rotateAngle;
  
        cell.style.transform = `rotateY(${cellAngle}deg) translateZ(${radius}px) scale(${scale})`;
        cell.style.opacity = isSelected ? 1 : 0.6;
      });
    }
  
    /**
     * Inicia o loop de rotação automática.
     */
    function startAutoplay() {
      // Garante que não haja múltiplos loops a correr ao mesmo tempo
      clearInterval(intervalId); 
      intervalId = setInterval(() => {
        selectedIndex++;
        rotateCarousel();
      }, autoplayInterval);
    }
  
    /**
     * Para o loop de rotação automática e cancela qualquer reinício agendado.
     */
    function stopAutoplay() {
      clearInterval(intervalId);
      clearTimeout(restartTimeout);
    }
    
    /**
     * Lida com a navegação manual, pausando o autoplay e agendando o seu reinício.
     */
    function handleManualNavigation() {
      stopAutoplay();
      // Agenda o reinício do autoplay para daqui a `restartDelay` milissegundos
      restartTimeout = setTimeout(startAutoplay, restartDelay); 
    }
  
    // --- 4. EVENT LISTENERS (INTERAÇÃO DO UTILIZADOR) ---
  
    // Ao clicar em "Próximo"
    nextButton.addEventListener('click', () => {
      selectedIndex++;
      rotateCarousel();
      handleManualNavigation(); // Pausa e agenda o reinício
    });
  
    // Ao clicar em "Anterior"
    prevButton.addEventListener('click', () => {
      selectedIndex--;
      rotateCarousel();
      handleManualNavigation(); // Pausa e agenda o reinício
    });
  
    // Pausa o autoplay quando o rato está sobre a área do carrossel
    carouselContainer.addEventListener('mouseenter', stopAutoplay);
  
    // Retoma o autoplay imediatamente quando o rato sai da área
    carouselContainer.addEventListener('mouseleave', startAutoplay);
  
  
    // --- 5. INÍCIO ---
    // Inicia o carrossel na posição correta e começa a rotação automática.
    rotateCarousel();
    startAutoplay();
  });