function actualizaCacheDinamico(dynamicCache, req, res) {

    if (res.ok) {
        return caches.open(dynamicCache).then(cache => {
            cache.put(req, res.clone());
            return res.clone();
        });
    } else {
        return res;
    }

}

function actualizaCacheStatico(staticCache, req, APP_SHELL_INMUTABLE) {


    if (APP_SHELL_INMUTABLE.includes(req.url)) {

    } else {

        return fetch(req)
            .then(res => {
                return actualizaCacheDinamico(staticCache, req, res);
            });
    }

}

function manejoApiMensajes(cacheName, req) {

        return fetch(req).then(res => {

        if (res.ok) {
            actualizaCacheDinamico(cacheName, req, res.clone());
            return res.clone();
        } else {
            return caches.match(req);
        }

    }).catch(err => {
        return caches.match(req);
    });

}