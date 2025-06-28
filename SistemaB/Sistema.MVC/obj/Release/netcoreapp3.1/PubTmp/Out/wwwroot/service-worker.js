importScripts('js/sw-utils.js');

const STATIC_CACHE = 'static-v1';
const DYNAMIC_CACHE = 'dynamic-v1';
const INMUTABLE_CACHE = 'inmutable-v1';


const APP_SHELL = [
    "/",
    "/index.html",
    "/css/chunk-vendors.0336549b.css",
    "/css/app.4569a4f0.css",
    "/js/chunk-vendors.e88d67dd.js",
    "/js/app.f6dac43a.js"
]

const APP_SHELL_INMUTABLE = [

    "https://fonts.googleapis.com/css?family=Material+Icons",
    "https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700,900"

]

self.addEventListener("install", event => {

    const cacheStatic = caches.open(STATIC_CACHE).then(cache =>
        cache.addAll(APP_SHELL));

    const cacheInmutable = caches.open(INMUTABLE_CACHE).then(cache =>
        cache.addAll(APP_SHELL_INMUTABLE));

    event.waitUntil(Promise.all([cacheStatic, cacheInmutable]));
})

self.addEventListener("activate", event => {

    const respuesta = caches.keys().then(keys => {

        keys.forEach(key => {

            if (key !== STATIC_CACHE && key.includes("static")) {
                return caches.delete(key);
            }

            if (key !== DYNAMIC_CACHE && key.includes("dynamic")) {
                return caches.delete(key);
            }

        });
    });

    event.waitUntil(respuesta);
})

self.addEventListener("fetch", event => {

    let respuesta;

    if (event.request.url.includes('/api')) {

        respuesta = manejoApiMensajes(DYNAMIC_CACHE, event.request);

    } else {

        respuesta = caches.match(event.request).then(res => {

            if (res) {

                actualizaCacheStatico(STATIC_CACHE, event.request, APP_SHELL_INMUTABLE);
                return res;

            } else {

                return fetch(event.request).then(newRes => {

                    return actualizaCacheDinamico(DYNAMIC_CACHE, event.request, newRes);

                });

            }

        });

    }

    event.respondWith(respuesta);

})