<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Consignatarios</v-toolbar-title>
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
        <v-dialog v-model="dialog" persistent max-width="500px">
          <v-btn
            slot="activator"
            color="primary"
            dark
            class="mb-2"
            >Nuevo</v-btn
          >
          <v-card>
            <v-card-title>
              <span class="headline">{{ formTitle }}</span>
            </v-card-title>
            <v-form ref="form">
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="pk_tienda"
                      :rules="[rules.requerido]"
                      label="Tienda"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="tipo"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Tipo"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="nombre"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Nombre"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="ciudad"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Ciudad"
                    >
                  </v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md12>
                    <v-text-field
                      v-model="direccion"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Dirección"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="consignatario"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Consignatario"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="carnet"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Carnet"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="razon"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Razón Social"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="nit"
                      :rules="[rules.requerido]"
                      label="Nit"
                    ></v-text-field>
                  </v-flex>
                </v-layout>
              </v-container>
            </v-form>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn slot="activator"
                color="green darken-1"
                dark
                class="mb-2" 
                @click.native="guardar"
                >Guardar</v-btn
              >
              <v-btn slot="activator"
                color="red darken-4"
                dark
                class="mb-2"
                @click.native="cerrar"
                >Cancelar</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
      <v-data-table
        :headers="headers"
        :items="tiendas"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.pK_TIENDA }}</td>
          <td>{{ props.item.pK_TIPO_TDA }}</td>
          <td>{{ props.item.nombre }}</td>
          <td>{{ props.item.direccion }}</td>
          <td>{{ props.item.consignatario }}</td>
          <td>{{ props.item.carnet }}</td>
          <td>{{ props.item.razonsocial }}</td>
          <td>{{ props.item.nit }}</td>
          <td>
          <v-btn class="mr-2" fab dark small color="cyan" @click="editar(props.item)">
          <v-icon dark>edit</v-icon>
          </v-btn>  
          </td>
        </template>
        <template slot="no-data">
          <v-btn color="error" @click="listar">Resetear</v-btn>
        </template>
        <template slot="no-results">
          <v-alert :value="true" color="error" icon="warning">
            Tu búsqueda de "{{ search }}" no encontro resultados.
          </v-alert>
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
      tiendas: [],
      dialog: false,
      headers: [
        { text: "Tienda", value: "pK_TIENDA" },
        { text: "Tipo", value: "pK_TIPO_TDA" },
        { text: "Nombre", value: "nombre" },
        { text: "Dirección", value: "direccion" },
        { text: "Consignatario", value: "consignatario", sortable: false },
        { text: "Carnet", value: "carnet", sortable: false },
        { text: "Razón Social", value: "razonsocial", sortable: false },
        { text: "Nit", value: "nit", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false },
      ],
      paginas: "Consignatarios por Página",
      search: "",
      pk_tienda: "",
      tipo: "",
      nombre: "",
      direccion: "",
      ciudad: "",
      consignatario: "",
      carnet: "",
      razon: "",
      nit: "",
      editedIndex: -1,
      error: null,
      rules: {
        requerido: (value) => !!value || "Requerido!",
      },
    };
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "Nuevo Consignatario" : "Actualizar Consignatario";
    },
  },

  watch: {
    dialog(val) {
      val || this.cerrar();
    },
  },

  created() {
    this.listar();
  },
  methods: {
    uppercase() {
      this.tipo = this.tipo.toUpperCase();
      this.nombre = this.nombre.toUpperCase();
      this.ciudad = this.ciudad.toUpperCase();
      this.direccion = this.direccion.toUpperCase();
      this.consignatario = this.consignatario.toUpperCase();
      this.carnet = this.carnet.toUpperCase();
      this.razon = this.razon.toUpperCase();
    },
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Tiendas/Listar",configuracion)
        .then(function (response) {
          me.tiendas = response.data;
          
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    editar(item) {
      this.pk_tienda = item.pK_TIENDA;
      this.tipo = item.pK_TIPO_TDA;
      this.nombre = item.nombre;
      this.ciudad = item.ciudad;
      this.direccion = item.direccion;
      this.consignatario = item.consignatario;
      this.carnet = item.carnet;
      this.razon = item.razonsocial;
      this.nit = item.nit;
      this.editedIndex = 1;
      this.dialog = true;
    },
    cerrar() {
      this.dialog = false;
      this.limpiar();
      this.$refs.form.resetValidation();
    },
    limpiar() {
      this.pk_tienda = "";
      this.tipo = "";
      this.nombre = "";
      this.ciudad = "";
      this.direccion = "";
      this.consignatario = "";
      this.carnet = "";
      this.razon = "";
      this.nit = "";
      this.editedIndex = -1;
      this.error = null;
    },
    guardar() {
      if (this.$refs.form.validate()) {
        if (this.editedIndex > -1) {
          let me = this;
          let header = { Authorization: "Bearer " + this.$store.state.token };
          let configuracion = { headers: header };
          axios
            .put(
              "api/Tiendas/Actualizar",
              {
                pK_TIENDA: parseInt(me.pk_tienda),
                pK_TIPO_TDA: me.tipo,
                nombre: me.nombre,
                ciudad: me.ciudad,
                direccion: me.direccion,
                consignatario: me.consignatario,
                carnet: me.carnet,
                razonsocial: me.razon,
                nit: me.nit
              },configuracion
            )
            .then(function (response) {
              me.cerrar();
              me.listar();
              me.limpiar();
            })
            .catch(function (error) {
              console.log(error);
            });
        } else {
          let me = this;
          let header = { Authorization: "Bearer " + this.$store.state.token };
          let configuracion = { headers: header };
          axios
            .post(
              "api/Tiendas/Crear",
              {
                pK_TIENDA: parseInt(me.pk_tienda),
                pK_TIPO_TDA: me.tipo,
                nombre: me.nombre,
                ciudad: me.ciudad,
                direccion: me.direccion,
                consignatario: me.consignatario,
                carnet: me.carnet,
                razonsocial: me.razon,
                nit: me.nit
              },configuracion
            )
            .then(function (response) {
              me.cerrar();
              me.listar();
              me.limpiar();
            })
            .catch(function (error) {
              console.log(error);
            });
        }
      }
    },
  },
};
</script>
