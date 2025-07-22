/**
 * Animações simplificadas para a seção "A Evolução do Suporte de TI"
 */

console.log('🎨 Script de evolução carregado!');

document.addEventListener('DOMContentLoaded', () => {
    console.log('🎨 DOM carregado, iniciando animações...');
    
    // --- 1. ANIMAÇÕES DE HOVER NOS CARDS ---
    
    const cards = document.querySelectorAll('.comparacao-card');
    console.log('Cards encontrados:', cards.length);
    
    cards.forEach((card, index) => {
        console.log(`Card ${index + 1}:`, card);
        
        // Adiciona evento de hover
        card.addEventListener('mouseenter', function() {
            console.log('Hover no card:', index + 1);
            this.style.transform = 'translateY(-8px) scale(1.02)';
            this.style.boxShadow = '0 25px 50px rgba(0,0,0,0.15)';
            this.style.transition = 'all 0.3s ease';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
            this.style.boxShadow = '';
            this.style.transition = 'all 0.3s ease';
        });
    });

    // --- 2. ANIMAÇÕES DOS ÍCONES ---
    
    const icons = document.querySelectorAll('.icone-animado');
    console.log('Ícones encontrados:', icons.length);
    
    icons.forEach((icon, index) => {
        console.log(`Ícone ${index + 1}:`, icon);
        
        icon.addEventListener('mouseenter', function() {
            console.log('Hover no ícone:', index + 1);
            this.style.transform = 'scale(1.2) rotate(360deg)';
            this.style.transition = 'all 0.4s ease';
        });
        
        icon.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1) rotate(0deg)';
            this.style.transition = 'all 0.4s ease';
        });
    });

    // --- 3. ANIMAÇÃO DE CONTADOR PARA OS NÚMEROS ---
    
    const metricNumbers = document.querySelectorAll('.numero-destaque');
    console.log('Números encontrados:', metricNumbers.length);
    
    metricNumbers.forEach((number, index) => {
        console.log(`Número ${index + 1}:`, number.textContent);
        
        const finalValue = number.textContent;
        const numericValue = parseFloat(finalValue.replace(/[^\d.]/g, ''));
        
        if (!isNaN(numericValue)) {
            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        console.log('Animando número:', entry.target.textContent);
                        animateCounter(entry.target, 0, numericValue, 2000, finalValue);
                        observer.unobserve(entry.target);
                    }
                });
            }, { threshold: 0.5 });
            
            observer.observe(number);
        }
    });

    // Função de animação de contador
    function animateCounter(element, start, end, duration, originalText) {
        let startTime = null;
        const suffix = originalText.replace(/[\d.]/g, '');
        
        const step = (timestamp) => {
            if (!startTime) startTime = timestamp;
            const progress = Math.min((timestamp - startTime) / duration, 1);
            const current = Math.floor(progress * (end - start) + start);
            
            element.textContent = current + suffix;
            
            if (progress < 1) {
                requestAnimationFrame(step);
            }
        };
        requestAnimationFrame(step);
    }

    // --- 4. ANIMAÇÃO DE REVEAL PARA ELEMENTOS DA LISTA ---
    
    const listItems = document.querySelectorAll('.lista-item');
    console.log('Itens da lista encontrados:', listItems.length);
    
    listItems.forEach((item, index) => {
        console.log(`Item da lista ${index + 1}:`, item);
        
        item.style.opacity = '0';
        item.style.transform = 'translateX(-20px)';
        item.style.transition = 'all 0.6s ease';
        
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    console.log('Revelando item:', index + 1);
                    setTimeout(() => {
                        entry.target.style.opacity = '1';
                        entry.target.style.transform = 'translateX(0)';
                    }, index * 100);
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.3 });
        
        observer.observe(item);
    });

    // --- 5. ANIMAÇÃO DE DESTAQUE NOS BOXES ---
    
    const highlights = document.querySelectorAll('.destaque-pulse');
    console.log('Destaques encontrados:', highlights.length);
    
    highlights.forEach((highlight, index) => {
        console.log(`Destaque ${index + 1}:`, highlight);
        
        highlight.addEventListener('mouseenter', function() {
            console.log('Hover no destaque:', index + 1);
            this.style.transform = 'scale(1.05)';
            this.style.boxShadow = '0 10px 25px rgba(0,0,0,0.1)';
            this.style.transition = 'all 0.3s ease';
        });
        
        highlight.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
            this.style.boxShadow = '';
            this.style.transition = 'all 0.3s ease';
        });
    });

    // --- 6. ANIMAÇÃO DE ENTRADA PARA MÉTRICAS ---
    
    const metricCards = document.querySelectorAll('.metrica-card');
    console.log('Cards de métrica encontrados:', metricCards.length);
    
    metricCards.forEach((card, index) => {
        console.log(`Card de métrica ${index + 1}:`, card);
        
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';
        card.style.transition = 'all 0.8s ease';
        
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    console.log('Revelando card de métrica:', index + 1);
                    setTimeout(() => {
                        entry.target.style.opacity = '1';
                        entry.target.style.transform = 'translateY(0)';
                    }, index * 200);
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.5 });
        
        observer.observe(card);
    });

    console.log('✅ Todas as animações foram configuradas!');
}); 