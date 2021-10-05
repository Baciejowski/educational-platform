// Initialize carousel
M.Carousel.init(document.querySelectorAll('.carousel'), {
    fullWidth: true,
    duration: 500
});

// Initialize side navigation
M.Sidenav.init(document.querySelectorAll('.sidenav'), {
    edge: "right"
});

//Move carousel each 5s
const carousel = M.Carousel.getInstance(document.querySelectorAll('.carousel')[0]);
const interval = setInterval(function () {
    carousel.next();
}, 7000);

