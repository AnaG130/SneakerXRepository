const CACHE = 'app-shell-v1';
const ASSETS = [
    '/', // fallback de navegación
    '/manifest.webmanifest',
    '/icons/icon-192.png',
    '/icons/icon-512.png',
    '/css/site.css',
    '/js/site.js'
];

self.addEventListener('install', (event) => {
    event.waitUntil(caches.open(CACHE).then(c => c.addAll(ASSETS)));
    self.skipWaiting();
});

self.addEventListener('activate', (event) => {
    event.waitUntil(
        caches.keys().then(keys => Promise.all(keys.map(k => (k !== CACHE ? caches.delete(k) : null))))
    );
    self.clients.claim();
});

self.addEventListener('fetch', (event) => {
    const req = event.request;

    // Navegación (documentos HTML)
    if (req.mode === 'navigate') {
        event.respondWith(
            fetch(req).catch(async () => (await caches.match('/')) || (await caches.match('/index.html')) || new Response('Offline'))
        );
        return;
    }

    // Estáticos: cache-first
    event.respondWith(
        caches.match(req).then(cached => {
            return cached || fetch(req).then(res => {
                const sameOrigin = new URL(req.url).origin === self.location.origin;
                if (req.method === 'GET' && sameOrigin) {
                    const copy = res.clone();
                    caches.open(CACHE).then(c => c.put(req, copy));
                }
                return res;
            });
        })
    );
});
