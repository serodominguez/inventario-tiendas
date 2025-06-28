<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Usuarios</v-toolbar-title>
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
                      v-model="nombres"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Nombres"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="apellidos"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Apellidos"
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
                      v-model="usuario"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Usuario"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-select
                      v-model="pk_rol"
                      :items="roles"
                      :rules="[rules.requerido]"
                      label="Rol"
                    >
                    </v-select>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      type="password"
                      v-model="password"
                      :rules="[rules.requerido]"
                      label="Contraseña"
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
        <v-dialog v-model="adModal" max-width="290px">
          <v-card>
            <v-card-title class="headline" v-if="adAccion == 1"
              >Activar Item?</v-card-title
            >
            <v-card-title class="headline" v-if="adAccion == 2"
              >Desactivar Item?</v-card-title
            >
            <v-card-text>
              Estás a punto de
              <span v-if="adAccion == 1">Activar</span>
              <span v-if="adAccion == 2">Desactivar</span>
              el ítem {{ adUsuario }}
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn            
                slot="activator"
                color="red darken-4"
                dark
                class="mb-2"
                @click="activardesactivarCerrar"
                >Cancelar</v-btn
              >
              <v-btn
                v-if="adAccion == 1"
                slot="activator"
                color="green darken-1"
                dark
                class="mb-2"
                @click="activar"
                >Activar</v-btn
              >
              <v-btn
                v-if="adAccion == 2"
                slot="activator"
                color="orange darken-1"
                dark
                class="mb-2"
                @click="desactivar"
                >Desactivar</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
      <v-data-table
        :headers="headers"
        :items="usuarios"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.nombres }}</td>
          <td>{{ props.item.apellidos }}</td>
          <td>{{ props.item.carnet }}</td>
          <td>{{ props.item.rol }}</td>
          <td>{{ props.item.usuario }}</td>
          <td>
            <div v-if="props.item.estado == 'ACTIVO'">
              <span class="green--text">{{ props.item.estado }}</span>
            </div>
            <div v-else>
              <span class="red--text">{{ props.item.estado }}</span>
            </div>
          </td>
          <td>
          <v-btn class="mr-2" fab dark small color="cyan" @click="editar(props.item)">
          <v-icon dark>edit</v-icon>
          </v-btn>  
            <template v-if="props.item.estado == 'ACTIVO'">
              <v-btn class="mr-2" fab dark small color="red" @click="activardesactivarMostrar(2, props.item)">
              <v-icon dark>block</v-icon>
              </v-btn>
            </template>
            <template v-else>
            <v-btn class="mr-2" fab dark small color="green" @click="activardesactivarMostrar(1, props.item)">
              <v-icon dark>check</v-icon>
              </v-btn>
            </template>
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
      usuarios: [],
      dialog: false,
      headers: [
        { text: "Nombres", value: "nombres" },
        { text: "Apellidos", value: "apellidos" },
        { text: "Carnet", value: "carnet" },
        { text: "Rol", value: "rol" },
        { text: "Usuario", value: "usuario", sortable: false },
        { text: "Estado", value: "estado", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false },
      ],
      paginas: "Usuarios por Página",
      search: "",
      editedIndex: -1,
      pk: "",
      pk_rol: "",
      pk_usuario: "",
      roles: [],
      nombres:"",
      apellidos: "",
      carnet: "",
      usuario: "",
      password: "",
      actualizarPassword: false,
      anteriorPassword: "",
      adModal: 0,
      adAccion: 0,
      adUsuario: "",
      adId: "",
      error: null,
      rules: {
        requerido: (value) => !!value || "Requerido!",
      },
    };
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "Nuevo usuario" : "Actualizar usuario";
    },
  },

  watch: {
    dialog(val) {
      val || this.cerrar();
    },
  },

  created() {
    this.listar();
    this.seleccionarRol();
  },
  methods: {
    uppercase() {
      this.usuario = this.usuario.toUpperCase();
      this.nombres = this.nombres.toUpperCase();
      this.apellidos = this.apellidos.toUpperCase();
    },
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Usuarios/Listar",configuracion)
        .then(function (response) {
          me.usuarios = response.data;
          
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    seleccionarRol() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var rolesArray = [];
      axios
        .get("api/Roles/Seleccionar",configuracion)
        .then(function (response) {
          rolesArray = response.data;
          rolesArray.map(function (x) {
            me.roles.push({ text: x.rol, value: x.pK_ROL});
          });
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
      this.pk = item.pK_USUARIO;
      this.pk_rol = item.pK_ROL;
      this.usuario = item.usuario;
      this.nombres = item.nombres,
      this.apellidos = item.apellidos,
      this.carnet = item.carnet,
      this.password = item.password;
      this.anteriorPassword = item.password;
      this.editedIndex = 1;
      this.dialog = true;
    },
    cerrar() {
      this.dialog = false;
      this.limpiar();
      this.$refs.form.resetValidation();
    },
    limpiar() {
      this.pk = "";
      this.pk_rol = "";
      this.usuario = "";
      this.nombres = "";
      this.apellidos = "";
      this.carnet = "";
      this.password = "";
      this.anteriorPassword = "";
      this.actualizarPassword = false;
      this.editedIndex = -1;
      this.error = null;
    },
    guardar() {
      if (this.$refs.form.validate()) {
        if (this.editedIndex > -1) {
          let me = this;
          let header = { Authorization: "Bearer " + this.$store.state.token };
          let configuracion = { headers: header };
          if (me.password != me.anteriorPassword) {
            me.actualizarPassword = true;
          }
          axios
            .put(
              "api/Usuarios/Actualizar",
              {
                pK_USUARIO: me.pk,
                pK_ROL: me.pk_rol,
                usuario: me.usuario,
                nombres: me.nombres,
                apellidos: me.apellidos,
                carnet: me.carnet,
                clave: me.password,
                actualizarPassword: me.actualizarPassword,
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
              "api/Usuarios/Crear",
              {
                pK_ROL: me.pk_rol,
                usuario: me.usuario,
                nombres: me.nombres,
                apellidos: me.apellidos,
                carnet: me.carnet,
                clave: me.password,
                estado: "ACTIVO",
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
    activardesactivarMostrar(accion, item) {
      (this.adModal = 1), (this.adUsuario = item.usuario);
      this.adId = item.pK_USUARIO;
      if (accion == 1) {
        this.adAccion = 1;
      } else if (accion == 2) {
        this.adAccion = 2;
      } else {
        this.adModal = 0;
      }
    },
    activardesactivarCerrar() {
      this.adModal = 0;
    },
    activar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .put("api/Usuarios/Activar/" + this.adId, {},configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adUsuario = "";
          me.adId = "";
          me.listar();
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    desactivar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .put("api/Usuarios/Desactivar/" + this.adId, {},configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adUsuario = "";
          me.adId = "";
          me.listar();
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
