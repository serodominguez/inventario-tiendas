<template>
  <v-layout align-center justify-center>
    <v-flex xs12 sm8 md6 lg5 xl4>
      <v-card>
        <v-toolbar dark color="red darken-1">
          <v-toolbar-title>Inicio de sesión</v-toolbar-title>
        </v-toolbar>
        <v-card-text>
          <v-text-field
            v-model="usuario"
            color="accent"
            label="Usuario"
            @keyup="uppercase"
            class="login"
            required
          ></v-text-field>
          <v-text-field
            v-model="password"
            color="accent"
            label="Contraseña"
            type="password"
            required
          ></v-text-field>
          <v-flex class="red--text font-weight-bold" v-if="error"
            ><strong>{{ error }}</strong></v-flex
          >
        </v-card-text>
        <v-card-actions class="px-3 pb-3">
          <v-flex text-xs-right>
            <v-btn @click="ingresar" color="primary">Ingresar</v-btn>
          </v-flex>
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
</template>
<script>
import axios from "axios";
export default {
  data() {
    return {
      usuario: "",
      password: "",
      error: null,
    };
  },
  methods: {
    uppercase() {
      this.usuario = this.usuario.toUpperCase();
    },
    ingresar() {
      this.error = null;
      axios
        .post("api/Usuarios/Inicio", {
          usuario: this.usuario,
          clave: this.password,
        })
        .then((respuesta) => {
          return respuesta.data;
        })
        .then((data) => {
          this.$store.dispatch("guardarToken", data.token);
          this.$router.push({ name: "home" });
        })
        .catch((err) => {
          if (err.response.status == 400) {
            this.error = "No es un usuario válido.";
          } else if (err.response.status == 404) {
            this.error = "No existe el usuario o sus datos son incorrectos.";
          }else if (err.response.status == 500) {
            this.error = "Servidor no disponible.";
          }else {
            this.error = "Ocurrio un error.";
          }
        });
    },
  },
};
</script>
<style scoped>
.code input {
  text-transform: uppercase;
}
</style>