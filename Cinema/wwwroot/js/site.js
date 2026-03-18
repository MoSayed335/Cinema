<script>
document.addEventListener("DOMContentLoaded", () => {
    const cards = document.querySelectorAll(".cinema-card");

    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.animationDelay = Math.random() + "s";
                entry.target.classList.add("show");
            }
        });
    }, {threshold: 0.2 });

    cards.forEach(card => observer.observe(card));
});
</script>
