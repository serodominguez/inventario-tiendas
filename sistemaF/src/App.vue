<template>
  <v-app id="inspire">
    <v-navigation-drawer :clipped="$vuetify.breakpoint.lgAndUp" v-model="drawer" v-if="logueado" fixed app temporary>
      <v-list dense>
        <template v-if="
            esAdministrador ||
            esAuditor||
            esConsignatario">
          <v-list-tile :to="{ name: 'home'}">
            <v-list-tile-action>
              <v-icon>home</v-icon>
            </v-list-tile-action>
            <v-list-tile-title>Inicio</v-list-tile-title>
          </v-list-tile>
        </template>
        <template v-if="
            esAdministrador ||
            esAuditor||
            esConsignatario">
          <v-list-group>
            <v-list-tile slot="activator">
              <v-list-tile-content>
                <v-list-tile-title> Inventario Tiendas </v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'existenciasT' }">
              <v-list-tile-action>
                <v-icon>upload</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title> Importar Existencias </v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'inventariosT' }">
              <v-list-tile-action>
                <v-icon>warehouse</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title> Inventario Artículos </v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'terminadosT' }">
              <v-list-tile-action>
                <v-icon>description</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title> Inventarios Terminados</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list-group>
        </template>
        <template v-if="
            esAdministrador ||
            esAuditor">
        <v-list-group>
          <v-list-tile slot="activator">
              <v-list-tile-content>
                <v-list-tile-title> Tiendas </v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'tiendas' }">
              <v-list-tile-action>
                <v-icon>storefront</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title> Consignatarios </v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
        </v-list-group>
      </template>
        <template v-if="
            esAdministrador">
          <v-list-group>
            <v-list-tile slot="activator">
              <v-list-tile-content>
                <v-list-tile-title>Accesos</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'usuarios' }">
              <v-list-tile-action>
                <v-icon>group</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Usuarios</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'roles' }">
              <v-list-tile-action>
                <v-icon>manage_accounts</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Roles</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list-group>
        </template>
      </v-list>
    </v-navigation-drawer>
   <v-toolbar
      color="red darken-1"
      dark
      app
      :clipped-left="$vuetify.breakpoint.mdAndUp"
      fixed
    >
      <v-toolbar-title style="width: 300px" class="ml-0 pl-3">
        <v-toolbar-side-icon
          v-if="logueado"
          @click.stop="drawer = !drawer"
        ></v-toolbar-side-icon>
        <span v-if="logueado" class="hidden-sm-and-down">Inventarios Manaco</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <span v-if="logueado"
        ><strong>Usuario: {{ this.$store.state.usuario.usuario }}</strong></span
      >
      <v-btn @click="salir" v-if="logueado" icon>
        <v-icon>logout</v-icon>
      </v-btn>
    </v-toolbar>
    <v-content>
      <v-container fluid fill-height>
        <v-slide-y-transition mode="out-in">
          <router-view />
        </v-slide-y-transition>
      </v-container>
    </v-content>
    <v-footer dark height="auto">
      <v-layout justify-center>
        <v-flex text-xs-center>
          <v-card flat tile color="primary" class="white--text">
            <v-card-text class="white--text pt-0"
              ><strong
                >Versión 1.9.7-{{ new Date().getFullYear() }}</strong
              ></v-card-text
            >
          </v-card>
        </v-flex>
      </v-layout>
    </v-footer>
  </v-app>
</template>

<script>
export default {
  name: "App",
  data() {
    return {
      drawer: null,
    };
  },
  computed: {
    logueado() {
      return this.$store.state.usuario;
    },
    esAdministrador() {
      return (
        this.$store.state.usuario &&
        this.$store.state.usuario.rol == "ADMINISTRADOR"
      );
    },
    esAuditor() {
      return (
        this.$store.state.usuario &&
        this.$store.state.usuario.rol == "AUDITOR"
      );
    },
    esConsignatario() {
      return (
        this.$store.state.usuario &&
        this.$store.state.usuario.rol == "CONSIGNATARIO"
      );
    }
  },
  created() {
    this.$store.dispatch("autoLogin");
  },
  methods: {
    salir() {
      this.$store.dispatch("salir");
    },
  },
};
</script>
