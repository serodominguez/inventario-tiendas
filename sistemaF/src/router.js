import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'
import ExistenciasT from './components/ExistenciasT.vue'
import Inicio from './components/Inicio.vue'
import InventariosT from './components/InventariosT.vue'
import TerminadosT from './components/TerminadosT.vue'
import Roles from './components/Roles.vue'
import Tiendas from './components/Tiendas.vue'
import Usuarios from './components/Usuarios.vue'
import store from './store'

Vue.use(Router)

var router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
      meta: {
        administrador: true,
        auditor: true,
        consignatario: true
      }
    },
    {
      path: '/tiendas',
      name: 'tiendas',
      component: Tiendas,
      meta: {
        administrador: true,
        auditor: true,
      }
    },
    {
      path: '/roles',
      name: 'roles',
      component: Roles,
      meta: {
        administrador: true
      }
    },
    {
      path: '/usuarios',
      name: 'usuarios',
      component: Usuarios,
      meta: {
        administrador: true
      }
    },
    {
      path: '/existenciasT',
      name: 'existenciasT',
      component: ExistenciasT,
      meta: {
        administrador: true,
        auditor: true,
        consignatario: true
      }
    },
    {
      path: '/inventariosT',
      name: 'inventariosT',
      component: InventariosT,
      meta: {
        administrador: true,
        auditor: true,
        consignatario: true
      }
    },
    {
      path: '/terminadosT',
      name: 'terminadosT',
      component: TerminadosT,
      meta: {
        administrador: true,
        auditor: true,
        consignatario: true
      }
    },
    {
      path: '/inicio',
      name: 'inicio',
      component: Inicio,
      meta: {
        libre: true
      }
    }
  ]
})
router.beforeEach((to, from, next) => {
  if(to.matched.some(record => record.meta.libre)){
    next()
  } else if (store.state.usuario && store.state.usuario.rol == 'ADMINISTRADOR'){
    if (to.matched.some(record => record.meta.administrador)){
        next()
    }
  } else if (store.state.usuario && store.state.usuario.rol == 'AUDITOR'){
    if (to.matched.some(record => record.meta.auditor)){
        next()
    }
  } else if (store.state.usuario && store.state.usuario.rol == 'CONSIGNATARIO'){
    if (to.matched.some(record => record.meta.consignatario)){
        next()
    }
  } else {
    next({
      name: 'inicio'
    })
  }
})
export default router
