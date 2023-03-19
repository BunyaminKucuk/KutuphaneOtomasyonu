let loaderCount = 0;
let timer;

function showLoader() {
    if (loaderCount === 0) {
        const loader = document.createElement('div');
        loader.className = 'loader';
        loader.innerHTML = '<div class="spinner"></div>';
        document.body.appendChild(loader);
    }
    loaderCount++;

    // Yükleyicinin otomatik olarak kapanması için bir zamanlayıcı başlatın
    if (!timer) {
        timer = setTimeout(() => {
            hideLoader();
            timer = null;
        }, 300); 
    }
}

function hideLoader() {
    loaderCount--;
    if (loaderCount === 0) {
        const loader = document.querySelector('.loader');
        loader.parentNode.removeChild(loader);
    }

    // Zamanlayıcıyı sıfırlama
    if (timer) {
        clearTimeout(timer);
        timer = null;
    }
}

window.addEventListener('load', () => {
    showLoader();
});
