// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => {
    self.skipWaiting();
    event.waitUntil(onInstall(event));
});
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.webp$/, /\.ico$/, /\.blat$/, /\.dat$/, /\.webmanifest$/];
const offlineAssetsExclude = [/^service-worker\.js$/, /^service-worker-assets\.js$/, /^\.gitignore$/, /^image\//];
const offlineAssetsIntegrityExclude = [/\.html$/];
const offlineAssetsPreferLoadFromOrigin = [/^data\//];

// Replace with your base path if you are hosting on a subfolder. Ensure there is a trailing '/'.
const base = "/SlimeIMWiki/";
const baseUrl = new URL(base, self.origin);
const manifestUrlList = self.assetsManifest.assets.map(asset => new URL(asset.url, baseUrl).href);

async function onInstall(event) {
    console.info('Service worker: Install');

    // Fetch and cache all matching items from the assets manifest
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => {
            const skipIntegrityCheck = offlineAssetsIntegrityExclude.some(pattern => pattern.test(asset.url));

            if (skipIntegrityCheck) {
                return new Request(asset.url, {cache: 'no-cache'});
            } else {
                return new Request(asset.url, {integrity: asset.hash, cache: 'no-cache'})
            }
        });

    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Delete unused caches
    const cacheKeys = await caches.keys();

    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    let request = event.request;
    let response;

    if (event.request.method === 'GET') {
        // For all navigation requests, try to serve index.html from cache,
        // unless that request is for an offline resource.
        // If you need some URLs to be server-rendered, edit the following check to exclude those URLs
        const shouldServeIndexHtml = event.request.mode === 'navigate' && !manifestUrlList.some(url => url === event.request.url);

        if (shouldServeIndexHtml) {
            request = new Request(new URL('index.html', baseUrl));
        }
    }

    const preferLoadFromOrigin = offlineAssetsPreferLoadFromOrigin.some(pattern => pattern.test(new URL(request.url).pathname.slice(1)));

    if (preferLoadFromOrigin) {
        const cache = await caches.open(cacheName);

        try {
            response = await fetch(request, {cache: 'no-cache'});

            if (!response.ok) {
                response = await cache.match(request);
            } else {
                await cache.put(request, response.clone());
            }
        } catch (e) {
            response = await cache.match(request);
        }
    } else {
        const cache = await caches.open(cacheName);
        response = await cache.match(request);

        if (!response) {
            try {
                const skipIntegrityCheck = offlineAssetsIntegrityExclude.some(pattern => pattern.test(new URL(request.url).pathname.slice(1)));

                if (skipIntegrityCheck) {
                    response = await fetch(request, {cache: 'no-cache'});
                    await cache.put(request, response.clone());
                } else {
                    const asset = self.assetsManifest.assets.find(asset => new URL(asset.url, baseUrl).href === request.url);

                    if (asset) {
                        response = await fetch(request, {integrity: asset.hash, cache: 'no-cache'});
                    } else {
                        response = await fetch(request, {cache: 'no-cache'});
                    }

                    await cache.put(request, response.clone());
                }
            } catch (e) {
                response = Response.error();
            }
        }
    }

    return response;
}