<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Roles</v-toolbar-title>
        <v-divider class="mx-2" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-text-field
          class="text-xs-center"
          v-model="search"
          append-icon="search"
          label="Búsqueda"
          single-line
          hide-details
        ></v-text-field>
        <v-spacer></v-spacer>
      </v-toolbar>
      <v-data-table
        :headers="headers"
        :items="roles"
        :rows-per-page-text="paginas"
        :search="search"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.rol }}</td>
          <td>
            <div v-if="props.item.estado == 'ACTIVO'">
              <span class="green--text">{{ props.item.estado }}</span>
            </div>
            <div v-else>
              <span class="red--text">{{ props.item.estado }}</span>
            </div>
          </td>
        </template>
        <template slot="no-data">
          <v-btn color="error" @click="listar">Resetear</v-btn>
        </template>
      </v-data-table>
    </v-flex>
  </v-layout>
</template>
<script>
import axios from "axios";
export default {
  data() {
    return {
      roles: [],
      paginas: "Roles por Página",
      dialog: false,
      headers: [
        { text: "Roles", value: "rol" },
        { text: "Estado", value: "estado", sortable: false },
      ],
      search: "",
      paginas: "Roles por Página",
    };
  },
  computed: {},
  watch: {},

  created() {
    this.listar();
  },
  methods: {
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Roles/Listar",configuracion)
        .then(function (response) {
          me.roles = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
  },
};
</script>
