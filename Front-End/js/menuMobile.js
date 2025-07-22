// Inicialização do AOS (Animate on Scroll)
document.addEventListener('DOMContentLoaded', function() {
    AOS.init();
    
    // Funcionalidade do menu mobile
    const menuBtn = document.getElementById('menu-btn');
    const closeMenuBtn = document.getElementById('close-menu');
    const mobileMenu = document.getElementById('mobile-menu');
    
    if (menuBtn && mobileMenu) {
        menuBtn.addEventListener('click', () => {
            mobileMenu.classList.remove('hidden');
        });
        
        if (closeMenuBtn) {
            closeMenuBtn.addEventListener('click', () => {
                mobileMenu.classList.add('hidden');
            });
        }
        
        // Fechar menu ao clicar em um link
        const mobileLinks = mobileMenu.querySelectorAll('a');
        mobileLinks.forEach(link => {
            link.addEventListener('click', () => {
                mobileMenu.classList.add('hidden');
            });
        });
        
        // Fechar menu ao clicar no backdrop
        const backdrop = mobileMenu.querySelector('.fixed.inset-0');
        if (backdrop) {
            backdrop.addEventListener('click', () => {
                mobileMenu.classList.add('hidden');
            });
        }
        
        // Fechar menu ao redimensionar a tela para desktop
        window.addEventListener('resize', () => {
            if (window.innerWidth >= 1024) {
                mobileMenu.classList.add('hidden');
            }
        });
    }
});

// Inicialização do Swiper para o carrossel de parceiros
const swiper = new Swiper('.swiper-partners', {
    slidesPerView: 1,
    spaceBetween: 30,
    loop: true,
    autoplay: {
        delay: 2500,
        disableOnInteraction: false,
    },
    breakpoints: {
        640: {
            slidesPerView: 2,
        },
        768: {
            slidesPerView: 3,
        },
        1024: {
            slidesPerView: 4,
        },
    }
}); 