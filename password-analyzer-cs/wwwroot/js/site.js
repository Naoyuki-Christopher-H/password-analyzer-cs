// Password Analyzer - Main JavaScript
// Provides interactive functionality with Apple-style animations

document.addEventListener('DOMContentLoaded', function () {
    // Initialize tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href');
            if (href !== '#' && href.startsWith('#')) {
                e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });

    // Form submission enhancement
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            const submitBtn = this.querySelector('button[type="submit"]');
            if (submitBtn && !submitBtn.classList.contains('no-loading')) {
                submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Analyzing...';
                submitBtn.disabled = true;
            }
        });
    });

    // Password strength real-time feedback
    const passwordInput = document.getElementById('password');
    if (passwordInput) {
        let timeout = null;

        passwordInput.addEventListener('input', function () {
            clearTimeout(timeout);

            // Debounce real-time analysis
            timeout = setTimeout(() => {
                const password = this.value;
                if (password.length > 0) {
                    // Add subtle visual feedback
                    this.classList.add('border-primary');
                    setTimeout(() => {
                        this.classList.remove('border-primary');
                    }, 300);
                }
            }, 300);
        });
    }

    // Toggle password visibility
    const toggleBtn = document.getElementById('togglePassword');
    if (toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            const passwordInput = document.getElementById('password');
            const icon = this.querySelector('i');

            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                if (icon) {
                    icon.className = 'bi bi-eye-slash';
                }
                this.innerHTML = '<i class="bi bi-eye-slash me-2"></i>Hide';
            } else {
                passwordInput.type = 'password';
                if (icon) {
                    icon.className = 'bi bi-eye';
                }
                this.innerHTML = '<i class="bi bi-eye me-2"></i>Show';
            }

            // Add subtle animation
            this.classList.add('btn-primary');
            setTimeout(() => {
                this.classList.remove('btn-primary');
            }, 300);
        });
    }

    // Copy to clipboard functionality
    document.querySelectorAll('[data-copy]').forEach(button => {
        button.addEventListener('click', function () {
            const text = this.getAttribute('data-copy');
            navigator.clipboard.writeText(text).then(() => {
                const originalText = this.innerHTML;
                this.innerHTML = '<i class="bi bi-check me-2"></i>Copied!';
                this.classList.add('btn-success');

                setTimeout(() => {
                    this.innerHTML = originalText;
                    this.classList.remove('btn-success');
                }, 2000);
            });
        });
    });

    // Add subtle hover effects to cards
    document.querySelectorAll('.card').forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-4px)';
        });

        card.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0)';
        });
    });

    // Lazy loading for images
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src;
                    img.classList.remove('lazy');
                    imageObserver.unobserve(img);
                }
            });
        });

        document.querySelectorAll('img.lazy').forEach(img => {
            imageObserver.observe(img);
        });
    }

    // Keyboard shortcuts
    document.addEventListener('keydown', function (e) {
        // Ctrl + / to focus password input
        if (e.ctrlKey && e.key === '/') {
            const passwordInput = document.getElementById('password');
            if (passwordInput) {
                e.preventDefault();
                passwordInput.focus();
            }
        }

        // Escape to blur focused element
        if (e.key === 'Escape') {
            document.activeElement.blur();
        }
    });
});