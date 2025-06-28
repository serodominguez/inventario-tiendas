<template>
<v-layout align-start>
  <v-flex>
    <v-toolbar flat color="white">
      <v-toolbar-title>Importar Existencias</v-toolbar-title>
      <v-divider class="mx-2" inset vertical></v-divider>
      <input v-if="verNuevo == 1" id="archivoExcel" ref="fileupload" type="file" @change="subirExcel()" />
      <v-spacer></v-spacer>
      <v-text-field v-if="verNuevo == 0" class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda"
        single-line hide-details></v-text-field>
      <v-spacer></v-spacer>
      <v-btn v-if="verNuevo == 0" @click="mostrarNuevo" color="primary" dark class="mb-2">Importar</v-btn>
      <v-dialog v-model="dialog" hide-overlay persistent max-width="500">
        <v-card color="primary" dark>
          <v-card-text>
            Espere por favor
            <v-progress-linear indeterminate color="white" class="mb-0"></v-progress-linear>
          </v-card-text>
        </v-card>
      </v-dialog>
      <v-dialog v-model="verCategorias" persistent max-width="1000px">
        <v-card>
          <v-card-title>
            <span class="headline">Seleccione las categorías </span>
          </v-card-title>
          <v-card-text>
            <v-container grid-list-md>
              <v-layout wrap>
                <v-flex xs12 sm12 md12 lg12 xl12>
                  <template>
                    <v-data-table v-model="selected" :headers="cabeceraCategorias" :items="categorias" select-all
                      item-key="pK_CATEGORIA_SUP" hide-actions class="elevation-1">
                      <template slot-scope="props">
                        <tr>
                          <th>
                            <v-checkbox :input-value="props.all" :indeterminate="props.indeterminate" primary
                              hide-details @click.stop="toggleAll"></v-checkbox>
                          </th>
                        </tr>
                      </template>
                      <template slot="items" slot-scope="props">
                        <tr :active="props.selected" @click="props.selected = !props.selected">
                          <td>
                            <v-checkbox :input-value="props.selected" primary hide-details></v-checkbox>
                          </td>
                          <td>{{ props.item.descripcion }}</td>
                          <td>{{ props.item.pK_CATEGORIA_SUP }}</td>
                        </tr>
                      </template>
                      <template slot="no-data">
                        <h3>No hay categorías para mostrar.</h3>
                      </template>
                    </v-data-table>
                  </template>
                </v-flex>
              </v-layout>
            </v-container>
          </v-card-text>
          <v-card-actions>
            <div class="red--text font-weight-bold" v-for="v in marcacionMensaje" :key="v" v-text="v"></div>
            <v-spacer></v-spacer>
            <v-btn @click="guardarCategorias()" color="green darken-1" dark class="mb-2">Aceptar</v-btn>
            <v-btn @click="ocultarCategorias()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-dialog v-model="reporteModal" max-width="1000px">
        <v-card>
          <v-card-text>
            <v-btn @click="crearPDF()">
              <v-icon>print</v-icon>
            </v-btn>
            <div id="reporte">
              <header>
                <div id="datos">
                  <p id="encabezado">
                    <b>MANUFACTURA BOLIVIANA S.A.</b>
                    <br />Fecha: {{ fecha }}
                    <br />Código: {{ codigo }}
                    <br />Estado de stock inicial
                    <br />Correspondiente a la semana: {{ semana }}
                    <br />Tienda: {{ tienda }}
                    <br />Consignatario: {{ consignatario }}
                  </p>
                </div>
              </header>
              <br />
              <section>
                <div>
                  <table id="repdetalle">
                    <thead>
                      <tr id="rep">
                        <th>CATEGORIA</th>
                        <th>CANTIDAD</th>
                        <th>VALOR</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="det in existencias" :key="det.categoria">
                        <td style="text-align: center">{{ det.categoria }}</td>
                        <td style="text-align: center">{{ formatoMiles(det.total) }}</td>
                        <td style="text-align: center">{{ formatoMiles(det.monto) }}</td>
                      </tr>
                    </tbody>
                    <tfoot>
                      <tr>
                        <th style="text-align: right">Total Pares: {{formatoMiles(calcularExistenciasPares)}}</th>
                        <th style="text-align: right">Total accesorios: {{formatoMiles(calcularExistenciasAccesorios)}}</th>
                      </tr>
                      <tr>
                        <th></th>
                        <th></th>
                        <th style="text-align: right">Total: {{formatoMiles(calcularExistenciasPares + calcularExistenciasAccesorios)}}</th>
                      </tr>
                      <tr>
                        <th></th>
                        <th></th>
                        <th style="text-align: right">Total Bs: {{formatoMiles(calcularExistenciasMonto)}}</th>
                      </tr>
                    </tfoot>
                  </table>
                </div>
              </section>
              <br />
              <br />
              <footer>
                <div>
                  <p>
                    <b>Firma Consignatario &nbsp;</b>
                    <br />Declaro que toda la información contenida en este documento es verídica.
                  </p>
                </div>
              </footer>
            </div>
            <v-btn @click="ocultarReporte()" color="red" flat>Cancelar</v-btn>
          </v-card-text>
        </v-card>
      </v-dialog>
    </v-toolbar>
    <v-data-table :headers="headers" :items="importados" :search="search" class="elevation-1" v-if="verNuevo == 0"
      :rows-per-page-text="hoja">
      <template slot="items" slot-scope="props">
        <td>{{ props.item.pK_TIENDA}}</td>
        <td>{{ props.item.codigo }}</td>
        <td>{{ props.item.semana }}</td>
        <td>{{ props.item.estado }}</td>
        <td>{{ props.item.usuario }}</td>
        <td>{{ props.item.rol }}</td>
        <td>
          <v-btn class="mr-1" fab dark small color="blue darken-1" @click="imprimirReporte(props.item)">
            <v-icon dark>print</v-icon>
          </v-btn>
          <v-btn class="mr-1" fab dark small color="green darken-1" @click="iniciarMostrar(props.item)">
            <v-icon dark>check</v-icon>
          </v-btn>
          <v-btn class="mr-1" fab dark small color="red darken-1" @click="borrarMostrar(props.item)">
            <v-icon dark>block</v-icon>
          </v-btn>
        </td>
      </template>
      <template slot="no-data">
        <v-btn color="primary" @click="listar">Resetear</v-btn>
      </template>
      <template slot="no-results">
        <v-alert :value="true" color="error" outline icon="warning">
          Tu búsqueda de "{{ search }}" no encontro resultados.
        </v-alert>
      </template>
    </v-data-table>
    <v-form ref="formulario">
      <v-container grid-list-sm class="pa-4 white" v-if="verNuevo == 1">
        <span><strong>Semana: {{ this.$store.state.usuario.semana }} </strong></span>
        <v-layout row wrap>
          <v-flex v-if="$store.state.usuario.rol != 'CONSIGNATARIO'" xs4 sm4 md3 lg3 xl3>
            <v-autocomplete v-if="this.verInv == 1" v-model="pk_tienda" :items="tiendas" label="Tienda"
              onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" :rules="[rules.requerido]"
              v-on:change="encuentraTienda(pk_tienda)" no-data-text="No hay datos disponibles" readonly>
            </v-autocomplete>
            <v-autocomplete v-else v-model="pk_tienda" :items="tiendas" label="Tienda"
              onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" :rules="[rules.requerido]"
              v-on:change="encuentraTienda(pk_tienda)" no-data-text="No hay datos disponibles">
            </v-autocomplete>
          </v-flex>
          <v-flex v-if="$store.state.usuario.rol != 'CONSIGNATARIO'" xs4 sm4 md3 lg3 xl3>
            <v-text-field v-model="nombre" label="Nombre" readonly>
            </v-text-field>
          </v-flex>
          <v-flex xs8 sm4 md3 lg3 xl3>
            <v-select v-if="this.verInv == 1"
            v-model="pk_tipo" 
            :items="tipos" 
            label="Tipo"
            no-data-text="No hay datos disponibles"
            readonly>
            </v-select>
            <v-select v-else
            v-model="pk_tipo" 
            :items="tipos" 
            label="Tipo"
            no-data-text="No hay datos disponibles">
            </v-select>
          </v-flex>
          <v-flex xs4 sm4 md2 lg2 xl2>
            <div v-if="pk_tienda == ''">
              <v-btn small fab dark color="primary">
                <v-icon dark>list_alt</v-icon>
              </v-btn>
            </div>
            <div v-else>
              <v-btn @click="mostrarCategorias()" small fab dark color="primary">
                <v-icon dark>list_alt</v-icon>
              </v-btn>
            </div>
          </v-flex>
          <v-flex xs12 sm2 md2 lg2 xl2 v-if="errorArticulo">
            <div class="red--text font-weight-bold" style="font-size: 20px" v-text="errorArticulo"></div>
          </v-flex>
          <v-flex xs12 sm12 md10 lg10 xl10>
            <v-data-table :headers="cabeceraDetalles" :rows-per-page-text="pagina" :items="detalles">
              <template slot="items" slot-scope="props">
                <td>{{ props.item.pK_ARTICULO }}</td>
                <td>{{ props.item.talla }}</td>
                <td>{{ props.item.cantidad }}</td>
              </template>
              <template slot="no-data">
                <h3>No hay artículos agregados al detalle.</h3>
              </template>
            </v-data-table>
          </v-flex>
          <v-flex xs12 sm12 md10 lg10 xl10>
            <v-alert :value="true" color="red darken-1" outline v-for="v in validaMensaje" :key="v" v-text="v">
            </v-alert>
          </v-flex>
          <v-flex class="red--text font-weight-bold" v-if="errorInventario"><strong>{{ errorInventario }}</strong></v-flex>
          <v-flex xs12 sm12 md12 lg12 xl12>
            <v-btn v-if="verBoton == 1" @click="guardar()" :disabled="dialog" :loading="dialog" color="green darken-1" dark class="mb-2">Guardar</v-btn>
              <v-btn v-if="verBoton == 0" @click="generar()" :disabled="dialog" :loading="dialog" color="blue darken-2" dark class="mb-2">Generar</v-btn>
            <v-btn @click="ocultarNuevo()" :disabled="dialog" :loading="dialog" color="red darken-4" dark class="mb-2">Atrás</v-btn>
          </v-flex>
        </v-layout>
      </v-container>
    </v-form>
    <v-form ref="form">
      <v-container grid-list-sm class="pa-4 white" v-if="verNuevo == 3">
        <v-layout row wrap>
          <v-flex xs6 sm6 md6 lg2 xl2>
            <v-text-field v-model="correlativo" label="Código" readonly>
            </v-text-field>
          </v-flex>
          <v-flex xs6 sm6 md6 lg2 xl2>
            <v-text-field v-model="tienda" label="Tienda" readonly>
            </v-text-field>
          </v-flex>
          <v-flex xs5 sm5 md3 lg3 xl3>
            <v-text-field v-model="semana" label="Semana" readonly>
            </v-text-field>
          </v-flex>
          <v-flex xs12 sm12 md10 lg10 xl10>
            <v-data-table :headers="cabeceraExistencias" :items="existencias" hide-actions class="elevation-1">
              <template slot="items" slot-scope="props">
                <td>{{ props.item.categoria }}</td>
                <td>{{ props.item.total }}</td>
                <td>{{ formatoMiles(props.item.monto)}}</td>
              </template>
              <template slot="no-data">
                <td></td>
              </template>
            </v-data-table>
            <v-flex xs12 sm12 md10 lg10 xl10>
              <v-alert :value="true" color="red darken-1" outline v-for="v in validaMensaje" :key="v" v-text="v">
              </v-alert>
            </v-flex>
            <v-flex class="text-xs-left">
              <strong>Total Pares: </strong> {{existenciasPares = formatoMiles(calcularExistenciasPares)}}
            </v-flex>
            <v-flex class="text-xs-left">
              <strong>Total Accesorios: </strong> {{existenciasAccesorios = formatoMiles(calcularExistenciasAccesorios)}}
            </v-flex>
            <v-flex class="text-xs-left">
              <strong>Total: </strong> {{existenciasTotal = formatoMiles(calcularExistenciasPares + calcularExistenciasAccesorios)}}
            </v-flex>
            <v-flex class="text-xs-left">
              <strong>Total Bs: </strong> {{existenciasMonto = formatoMiles(calcularExistenciasMonto)}}
            </v-flex>
            <v-btn @click="eliminar()" color="blue darken-2" dark class="mb-2">Eliminar</v-btn>
            <v-btn @click="ocultarBorrar()" color="red darken-1" dark class="mb-2">Atrás</v-btn>
          </v-flex>
        </v-layout>
      </v-container>
    </v-form>
  </v-flex>
</v-layout>
</template>
<script>
import axios from "axios";
import readXlsFile from "read-excel-file"
import jsPDF from "jspdf";
import html2canvas from "html2canvas";
export default {
data() {
  return {
    importados: [],
    existencias: [],
    detalles: [],
    tiendas: [],
    categorias: [],
    selected: [],
    tipos: ['LECTURA', 'MANUAL'],
    dialog: false,
    rules: {
      requerido: (value) => !!value || "Requerido!",
    },
    headers: [
      { text: "Tienda", value: "pK_Tienda", sortable: false, class: "black--text font-weight-bold" },
      { text: "Código", value: "codigo", sortable: false, class: "black--text font-weight-bold" },
      { text: "Semana", value: "semana", sortable: false, class: "black--text font-weight-bold" },
      { text: "Estado", value: "estado", sortable: false, class: "black--text font-weight-bold" },
      { text: "Usuario", value: "usuario", sortable: false, class: "black--text font-weight-bold" },
      { text: "Rol", value: "rol", sortable: false, class: "black--text font-weight-bold" },
      { text: "Opciones", value: "opciones", sortable: false, class: "black--text font-weight-bold" }
    ],
    cabeceraDetalles: [
      { text: "Artículo", value: "pK_ARTICULO", sortable: false, class: "black--text font-weight-bold" },
      { text: "Talla", value: "talla", sortable: false, class: "black--text font-weight-bold" },
      { text: "Cantidad", value: "cantidad", sortable: false, class: "black--text font-weight-bold" },
    ],
    cabeceraCategorias: [
      { text: "Categoría", value: "descripcion", sortable: false, class: "black--text font-weight-bold" },
      { text: "Código", value: "pK_CATEGORIA_SUP", sortable: false, class: "black--text font-weight-bold" },
    ],
    cabeceraExistencias: [
      { text: "Categoría", value: "categoria", sortable: false, class: "black--text font-weight-bold" },
      { text: "Total", value: "total", sortable: false, class: "black--text font-weight-bold" },
      { text: "Monto", value: "monto", sortable: false, class: "black--text font-weight-bold" },
    ],
    pagina: "Artículos por página",
    hoja: "Inventarios por página",
    pk_inventario: "",
    pk_tienda: "",
    pk_tipo: "",
    tienda: "",
    consignatario: "",
    codigo: "",
    semana: "",
    search: "",
    fecha: new Date().toLocaleString(),
    errorArticulo: null,
    errorInventario: null,
    cantidadpares: 0,
    cantidadaccesorios: 0,
    marcacion: 0,
    totalcantidades: 0,
    marcacionMensaje: [],
    verCategorias: 0,
    valida: 0,
    validaMensaje: [],
    verDetalle: 0,
    verNuevo: 0,
    verBoton: 0,
    verInv: 0,
    reporteModal: 0,
    editedIndex: -1
  };
},
created() {
  if (this.$store.state.usuario.rol == 'ADMINISTRADOR'){
    this.listar();
    this.seleccionarT();
  }
  else {
    this.listarTiendas();
    this.seleccionarT();
  }
},
computed: {
  calcularExistenciasPares: function () {
    var pares = 0;
    for (var i = 0; i < this.existencias.length; i++) {
      if(this.existencias[i].categoria != "ACCESSORIES" && this.existencias[i].categoria != "PROMOTIONS" && this.existencias[i].categoria != "CLOTHING"){
          pares = parseInt(pares) + parseInt(this.existencias[i].total);
      }
    }
    return pares;
  },
  calcularExistenciasAccesorios: function () {
    var accesorios = 0;
    for (var i = 0; i < this.existencias.length; i++) {
      if(this.existencias[i].categoria == "ACCESSORIES" || this.existencias[i].categoria == "PROMOTIONS" || this.existencias[i].categoria == "CLOTHING"){
        accesorios = parseInt(accesorios) + parseInt(this.existencias[i].total);
      }
    }
    return accesorios;
  },
  calcularExistenciasMonto: function () {
    var monto = 0;
    for (var i = 0; i < this.existencias.length; i++) {
      monto = parseFloat(monto) + parseFloat(this.existencias[i].monto);
    }
    monto = monto.toFixed(2);
    return monto;
  },
},
methods: {
  formatoMiles (number) {
    var valor = 0.0;
    const exp = /(\d)(?=(\d{3})+(?!\d))/g;
    const rep = '$1,';
    let arr = number.toString().split('.');
    arr[0] = arr[0].replace(exp, rep);
    valor = arr[1] ? arr.join('.') : arr[0];
    return valor;
  },
  crearPDF() {
    var quotes = document.getElementById("reporte");
    html2canvas(quotes).then(function (canvas) {
      var imgData = canvas.toDataURL("image/png");
      var imgWidth = 210;
      var pageHeight = 295;
      var imgHeight = (canvas.height * imgWidth) / canvas.width;
      var heightLeft = imgHeight;
      var doc = new jsPDF("p", "mm", "letter");
      var position = 0;
      const pageCount = doc.internal.getNumberOfPages()

      for (var i = 1; i <= pageCount; i++) {
        doc.setPage(i);
        doc.setFontSize(10);
        doc.text('Página: ' + String(i) + ' de ' + String(pageCount), 210 - 20, 297 - 30, null, null, "right");
      }

      doc.setProperties({
        title: "Stock Inicial",
      });
      doc.addImage(imgData, "PNG", 0, position, imgWidth, imgHeight);
      window.open(doc.output("bloburl"));
    });
  },
  mostrarNuevo() {
    if (this.$store.state.usuario.rol != 'CONSIGNATARIO') {
      this.verNuevo = 1;
      this.editedIndex = -1;
    } else {
      this.pk_tienda = this.$store.state.usuario.usuario;
      this.verNuevo = 1;
      this.editedIndex = -1;
    }   
  },
  ocultarNuevo() {
    this.verNuevo = 0;
    this.verInv = 0;
    this.editedIndex = -1;
    this.limpiar();
  },
  toggleAll() {
    if (this.selected.length) this.selected = [];
    else this.selected = this.desserts.slice();
  },
  encuentraTienda(id) {
    for (var i = 0; i < this.tiendas.length; i++) {
      if (this.tiendas[i].value == id) {
        this.nombre = this.tiendas[i].text2;
      }
    }
  },
  subirExcel(){
    var articulosArray = [];
    const input = document.getElementById("archivoExcel");
    readXlsFile(input.files[0]).then((rows) => {
      this.articulosArray = rows
      for (var i = 1; i < this.articulosArray.length; i++)
      {
        this.detalles.push({
          pK_ARTICULO: String(this.articulosArray[i][0]),
          talla: String(this.articulosArray[i][2]),
          cantidad: this.articulosArray[i][1],
        });
      }
    })
    this.verBoton = 1;
  },
  exportarExcel(id) {
      axios({
        url: "api/ExistenciasT/ExportarExcel/"+ id, 
        method: "POST",
        headers: {
            Authorization: "Bearer " + this.$store.state.token 
        },
        responseType: "blob"
      })
        .then(function (response) {
          const url = window.URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute("download", "StockInicial"+ id +".xlsx");
          document.body.appendChild(link);
          link.click();
        })
        .catch(function (error) {});
    },
  listar() {
    let me = this;
    let header = { Authorization: "Bearer " + this.$store.state.token };
    let configuracion = { headers: header };
    axios
      .get("api/ExistenciasT/Listar", configuracion)
      .then(function (response) {
        me.importados = response.data;
      })
      .catch(function (error) {
        if (error.response.status == 401) {
          me.$store.dispatch("salir");
        } else {
          console.log(error);
        }
      });
  },
  guardar() {
    if (this.validar()) {
      return;
    }
    if (this.editedIndex > -1) {
      this.dialog = true;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      let me = this;
      axios.post("api/ExistenciasT/Modificar", {
      pK_INVENTARIOT: me.clave,
      pK_TIENDA: me.pk_tienda,
      existencias: me.detalles,
      categorias: me.selected
    }, configuracion)
      .then(function (response) {
        me.ocultarNuevo();
        me.limpiar();
        if (me.$store.state.usuario.rol == 'ADMINISTRADOR') {
          me.listar();
        }
        else {
          me.listarTiendas();
        }
      })
      .catch(function (error) {
        if (error.response.status == 401) {
          me.$store.dispatch("salir");
        } else {
          console.log(error);
        }
      });
    } else {
      var hoy = new Date();
      this.dialog = true;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      let me = this;
      axios.post("api/ExistenciasT/Cargar", {
        pK_TIENDA: me.pk_tienda,
        semana: me.$store.state.usuario.semana,
        pK_tipo: me.pk_tipo,
        usuario: parseInt(me.$store.state.usuario.pk_usuario),
        horainicio: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes(),
        existencias: me.detalles,
        categorias: me.selected
      }, configuracion)
        .then(function (response) {
          me.ocultarNuevo();
          me.limpiar();
          if (me.$store.state.usuario.rol == 'ADMINISTRADOR') {
            me.listar();
          }
          else {
            me.listarTiendas();
          }
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    }
  },    
  generar() {
    if (this.verificar()) {
      return;
    }
    if (this.editedIndex > -1) {
      if (this.$store.state.usuario.rol == 'ADMINISTRADOR') {
        this.errorInventario = null;
        this.dialog = true;
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios.put("api/ExistenciasT/Actualizar", {
          pK_TIENDA: me.pk_tienda,
          categorias: me.selected,
          pK_tipo: me.pk_tipo,
          pK_INVENTARIOT: me.clave
        }, configuracion)
          .then(function (response) {
            me.ocultarNuevo();
            me.limpiar();
            me.listar();
          })
          .catch(function (error) {
            if (error.response.status == 401) {
              me.$store.dispatch("salir");
            } else if (error.response.status == 404) {
              me.dialog = false;
              me.errorInventario = "El inventario ya cuenta con stock inicial!";
            } else {
              me.dialog = false;
              console.log(error);
            }
          });
      } else {
        this.errorInventario = null;
        this.dialog = true;
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios.put("api/ExistenciasT/Actualizar", {
          pK_TIENDA: parseInt(me.pk_tienda),
          categorias: me.selected,
          pK_tipo: me.pk_tipo,
          pK_INVENTARIOT: me.clave
        }, configuracion)
          .then(function (response) {
            me.ocultarNuevo();
            me.limpiar();
            me.listarTiendas();
          })
          .catch(function (error) {
            if (error.response.status == 401) {
              me.$store.dispatch("salir");
            } else if (error.response.status == 404) {
              me.dialog = false;
              me.errorInventario = "El inventario ya cuenta con stock inicial!";
            } else {
              me.dialog = false;
              console.log(error);
            }
          });
        }
    } else {
      if (this.$store.state.usuario.rol == 'ADMINISTRADOR') {
        this.errorInventario = null;
        this.dialog = true;
        var hoy = new Date();
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios.post("api/ExistenciasT/Generar", {
          pK_TIENDA: me.pk_tienda,
          categorias: me.selected,
          pK_tipo: me.pk_tipo,
          usuario: parseInt(me.$store.state.usuario.pk_usuario),
          horainicio: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes()
        }, configuracion)
          .then(function (response) {
            me.exportarExcel(response.data);
            me.ocultarNuevo();
            me.limpiar();
            me.listar();
          })
          .catch(function (error) {
            if (error.response.status == 401) {
              me.$store.dispatch("salir");
            } else {
              console.log(error);
            }
          });
      } else {
        console.log(this.$store.state.usuario.pk_usuario, this.pk_tipo, this.pk_tienda);
        this.errorInventario = null;
        this.dialog = true;
        var hoy = new Date();
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios.post("api/ExistenciasT/Generar", {
          pK_TIENDA: parseInt(me.pk_tienda),
          categorias: me.selected,
          pK_tipo: me.pk_tipo,
          usuario: parseInt(me.$store.state.usuario.pk_usuario),
          horainicio: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes()
        }, configuracion)
          .then(function (response) {
            me.exportarExcel(response.data);
            me.ocultarNuevo();
            me.limpiar();
            me.listarTiendas();
          })
          .catch(function (error) {
            if (error.response.status == 401) {
              me.$store.dispatch("salir");
            } else {
              console.log(error);
            }
          });
      }
    }
  },
  seleccionarT() {
    let me = this;
    let header = { Authorization: "Bearer " + this.$store.state.token };
    let configuracion = { headers: header };
    me.tiendas = [];
    me.nombre = "";
    var tiendaArray = [];
    axios
      .get("api/Tiendas/Seleccionar/", configuracion)
      .then(function (response) {
        tiendaArray = response.data;
        tiendaArray.map(function (x) {
          me.tiendas.push({
            text: x.pK_TIENDA,
            text2: x.nombre,
            value: x.pK_TIENDA,
          });
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
  mostrarCategorias() {
    let me = this;
    me.verCategorias = 1;
    let header = { Authorization: "Bearer " + this.$store.state.token };
    let configuracion = { headers: header };
    axios
      .get("api/Categorias/Seleccionar/", configuracion)
      .then(function (response) {
        me.categorias = response.data;
      })
      .catch(function (error) {
        if (error.response.status == 401) {
          me.$store.dispatch("salir");
        } else {
          console.log(error);
        }
      });
  },
  listarTiendas() {
    let me = this;
    let header = { Authorization: "Bearer " + this.$store.state.token };
    let configuracion = { headers: header };
    axios
      .get("api/ExistenciasT/ListarTiendas/"+ parseInt(me.$store.state.usuario.pk_usuario), configuracion)
      .then(function (response) {
        me.importados = response.data;
      })
      .catch(function (error) {
        if (error.response.status == 401) {
          me.$store.dispatch("salir");
        } else {
          console.log(error);
        }
      });
  },
  listarExistencias(id, pk) {
    let me = this;
    let header = { Authorization: "Bearer " + this.$store.state.token };
    let configuracion = { headers: header };
    axios
      .get("api/ExistenciasT/ListarImportado/" + id + "/" + pk , configuracion)
      .then(function (response) {
        me.existencias = response.data;
      })
      .catch(function (error) {
        if (error.response.status == 401) {
          me.$store.dispatch("salir");
        } else {
          console.log(error);
        }
      });
  },
  eliminar() {
    if (this.comprobar()) {
      return;
    }
    let me = this;
    let header = { Authorization: "Bearer " + this.$store.state.token };
    let configuracion = { headers: header };
    axios
      .delete("api/ExistenciasT/Eliminar/" + this.clave, configuracion)
      .then(function (response) {
        me.verNuevo = 0;
      })
      .catch(function (error) {
        if (error.response.status == 401) {
          me.$store.dispatch("salir");
        } else {
          console.log(error);
        }
      });
  },
  iniciarMostrar(item) {
    this.verNuevo = 1;
    this.verInv = 1;
    this.editedIndex = 1;
    this.pk_tipo = item.estado;
    this.pk_tienda = item.pK_TIENDA;
    this.clave = item.pK_INVENTARIOT;
    this.encuentraTienda(item.pK_TIENDA)
  },
  borrarMostrar(item) {
    this.verNuevo = 3;
    this.tienda = item.pK_TIENDA;
    this.semana = item.semana;
    this.correlativo = item.codigo;
    this.clave = item.pK_INVENTARIOT;
    this.listarExistencias(item.pK_INVENTARIOT, item.semana);
  },
  verDetalles(item) {
    this.verNuevo = 2;
    this.tienda = item.pK_TIENDA;
    this.semana = item.semana;
    this.codigo = item.codigo;
    this.listarTotales(item.pK_INVENTARIOT);
  },
  imprimirReporte(item) {
    this.pk_inventario = item.pK_INVENTARIOT,
    this.tienda = item.tienda,
    this.consignatario = item.consignatario,
    this.codigo = item.codigo,  
    this.semana = item.semana, 
    this.listarExistencias(item.pK_INVENTARIOT, item.semana);
    this.reporteModal = 1;
  },
  guardarCategorias() {
    if (this.marcar()) {
      return;
    }
    this.verCategorias = 0;
    this.marca = 0;
    this.marcaMensaje = [];
  },
  ocultarReporte() {
    this.reporteModal = 0;
  },
  ocultarCategorias() {
    this.verCategorias = 0;
    this.categorias = [];
    this.selected = [];
    this.marcacion = 0;
    this.marcacionMensaje = [];
  },
  ocultarBorrar() {
    this.verNuevo = 0;
    this.valida = 0;
    this.validaMensaje = [];
    this.existencias = [];
    this.clave = "";
    this.tienda = "";
    this.semana = "";
    this.correlativo = "";
  },
  ocultarMensaje(){
    this.dialog = false;
  },
  comprobar() {
    this.valida = 0;
    this.validaMensaje = [];
    if (this.existencias.length <= 0) {
      this.validaMensaje.push("No tiene ningún stock inicial!");
    }
    if (this.validaMensaje.length) {
      this.valida = 1;
    }
    return this.valida;
  },
  validar() {
    this.valida = 0;
    this.validaMensaje = [];
    if (this.detalles.length <= 0) {
      this.validaMensaje.push("Ingrese al menos un artículo al detalle!");
    }
    if (this.selected.length <= 0) {
      this.validaMensaje.push("Seleccione una categoría!");
    }
    if (!this.pk_tienda) {
      this.validaMensaje.push("Seleccione una tienda!");
    }
    if (this.validaMensaje.length) {
      this.valida = 1;
    }
    return this.valida;
  },
  verificar() {
    this.valida = 0;
    this.validaMensaje = [];
    if (!this.pk_tienda) {
      this.validaMensaje.push("Seleccione una tienda!");
    }
    if (!this.pk_tipo) {
      this.validaMensaje.push("Seleccione un tipo de inventario!");
    }
    if (this.selected.length <= 0) {
      this.validaMensaje.push("Seleccione una categoría!");
    }
    if (this.validaMensaje.length) {
      this.valida = 1;
    }
    return this.valida;
  },
  limpiar() {
    this.pk_tienda = "";
    this.nombre = "";
    this.pk_tipo = "";
    this.detalles = [];
    this.selected = [];
    this.categorias =[];
    this.verDetalle = 0;
    this.valida = 0;
    this.validaMensaje = [];
    this.errorArticulo = null;
    this.errorInventario = null;
    this.verCategorias = 0;
    this.marcacion = 0;
    this.marcacionMensaje = [];
    this.dialog = false;
    this.verBoton = 0;
  },
  marcar() {
    this.marcacacion = 0;
    this.marcacionMensaje = [];
    if (this.selected.length == 0) {
      this.marcacionMensaje.push("Seleccione una categoría!");
    }
    if (this.marcacionMensaje.length) {
      this.marcacion = 1;
    }
    return this.marcacion;
  },
},
};
</script>
<style>
#reporte {
  padding: 20px;
  font-family: Arial, sans-serif;
  font-size: 16px;
}

#rep {
  font-size: 18px;
  font-weight: bold;
  text-align: center;
}

#datos {
  float: left;
  margin-top: 0%;
  margin-left: 26%;
  margin-right: 2%;
}

#encabezado {
  text-align: center;
  margin-left: 10px;
  margin-right: 10px;
  font-size: 16px;
}

section {
  clear: left;
}

#rep {
  color: #ffffff;
  font-size: 14px;
}

#repdetalle {
  width: 100%;
  border-collapse: collapse;
  border-spacing: 0;
  padding: 20px;
  margin-bottom: 15px;
}

#repdetalle thead {
  padding: 20px;
  background: #ee1505;
  text-align: center;
  border-bottom: 1px solid #ffffff;
}
</style>