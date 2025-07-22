// Configurações do AOS (Animate on Scroll)
const initAnimations = () => {
    // Inicializa o AOS com configurações personalizadas
    AOS.init({
        duration: 1000,
        once: true,
        offset: 100,
        easing: 'ease-in-out'
    });

    // Animação do contador para a seção de estatísticas
    const animateValue = (element, start, end, duration) => {
        let startTimestamp = null;
        const step = (timestamp) => {
            if (!startTimestamp) startTimestamp = timestamp;
            const progress = Math.min((timestamp - startTimestamp) / duration, 1);
            const value = Math.floor(progress * (end - start) + start);
            element.textContent = value + (element.dataset.suffix || '');
            if (progress < 1) {
                window.requestAnimationFrame(step);
            }
        };
        window.requestAnimationFrame(step);
    };

    // Observador de interseção para animar os números quando visíveis
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const el = entry.target;
                const value = el.dataset.value;
                const suffix = el.dataset.suffix;
                animateValue(el, 0, parseInt(value), 2000);
                observer.unobserve(el);
            }
        });
    }, { threshold: 0.5 });

    // Adiciona observador aos elementos com números
    document.querySelectorAll('.animate-number').forEach(el => {
        observer.observe(el);
    });

    // Animação suave do header no scroll
    let lastScroll = 0;
    const header = document.querySelector('header');
    
    window.addEventListener('scroll', () => {
        const currentScroll = window.pageYOffset;
        
        if (currentScroll <= 0) {
            header.classList.remove('scroll-up');
            return;
        }
        
        if (currentScroll > lastScroll && !header.classList.contains('scroll-down')) {
            // Scroll Down
            header.classList.remove('scroll-up');
            header.classList.add('scroll-down');
        } else if (currentScroll < lastScroll && header.classList.contains('scroll-down')) {
            // Scroll Up
            header.classList.remove('scroll-down');
            header.classList.add('scroll-up');
        }
        lastScroll = currentScroll;
    });

    // Efeito de parallax no hero
    const hero = document.querySelector('.hero');
    window.addEventListener('scroll', () => {
        const scroll = window.pageYOffset;
        if (hero) {
            hero.style.backgroundPositionY = `${scroll * 0.5}px`;
        }
    });
}

// Inicializa as animações quando o DOM estiver pronto
document.addEventListener('DOMContentLoaded', initAnimations); 