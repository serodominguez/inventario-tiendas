<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Inventario</v-toolbar-title>
        <v-divider class="mx-2" inset vertical></v-divider>
        <input v-if="verNuevo == 1" id="archivoExcel" ref="fileupload" type="file" @change="subirExcel()" />
        <v-btn v-if="verNuevo == 2" @click="ocultarDetalle" color="primary" dark class="mb-2">Atrás</v-btn>
        <v-btn v-if="verNuevo == 4" @click="ocultarManual" color="primary" dark class="mb-2">Atrás</v-btn>
        <v-spacer></v-spacer>
        <v-text-field v-if="verNuevo == 0" class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda"
          single-line hide-details></v-text-field>
        <v-spacer></v-spacer>
        <v-dialog v-model="waitModal" max-width="500px" persistent>
          <v-card color="primary" dark>
            <v-card-text>
              Espere por favor
              <v-progress-linear indeterminate color="white" class="mb-0"></v-progress-linear>
            </v-card-text>
          </v-card>
        </v-dialog>
        <v-dialog v-model="adAceptar" max-width="290px">
          <v-card>
            <v-card-text>
              Los artículos se guardaron correctamente
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="red darken-1" flat="flat" @click="aceptarCerrar">Aceptar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="adEliminar" max-width="290px">
          <v-card>
            <v-card-text>
              Los artículos se eliminaron correctamente
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="red darken-1" flat="flat" @click="suprimirCerrar">Aceptar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="adFin" max-width="290px">
          <v-card>
            <v-card-title class="headline" v-if="adAccion == 3">Finalizar el Inventario?</v-card-title>
            <v-card-text>
              Estás a punto de
              <span v-if="adAccion == 3">Finalizar</span>
              el ítem {{ adInventario}}
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn v-if="wait == 0" color="green darken-1" flat="flat" @click="finalizarCerrar">Cancelar</v-btn>
              <v-btn v-if="adAccion == 3 && wait == 0" color="orange darken-4" flat="flat" @click="finalizar">Finalizar</v-btn>
              <v-progress-circular v-if="wait == 1" :size="40" indeterminate color="orange" ></v-progress-circular>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="adAnular" max-width="290px">
          <v-card>
            <v-card-title class="headline" v-if="adAccion == 4">Anular el Inventario?</v-card-title>
            <v-card-text>
              Estás a punto de
              <span v-if="adAccion == 3">Anular</span>
              el ítem {{ adInventario}}
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="green darken-1" flat="flat" @click="anularCerrar">Cancelar</v-btn>
              <v-btn v-if="adAccion == 4" color="orange darken-4" flat="flat" @click="anular">Anular</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialog" max-width="500px" persistent>
          <v-card>
            <v-card-title>
              <span class="headline">Editar</span>
            </v-card-title>
            <v-card-text>
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs12 sm6 md4>
                    <v-text-field v-model="editedItem.canal" label="Canal"></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field v-model="editedItem.pK_ARTICULO" label="Artículo"></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field v-model="editedItem.talla" @keyup="uppercase" label="Talla"></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field v-model.number="editedItem.cantidad" label="Cantidad"></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field v-model="editedItem.calidad" label="Calidad"></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field v-model="editedItem.planes" label="Plan"></v-text-field>
                  </v-flex>
                </v-layout>
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="red darken-4" flat @click="close">Cancelar</v-btn>
              <v-btn color="green darken-1" flat @click="save">Guardar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="modificarModal" max-width="500px" persistent>
          <v-card>
            <v-card-title>
              <span class="headline">Modificar</span>
            </v-card-title>
            <v-card-text>
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs12 sm6 md4>
                    <v-text-field 
                    v-model="canal" 
                    label="Canal"
                    >
                  </v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field 
                    v-model="pk_articulo" 
                    label="Artículo"
                    readonly
                    >
                  </v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field 
                    v-model="talla" 
                    @keyup="uppercase"
                    label="Talla"
                    readonly
                    >
                  </v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field 
                    v-model.number="cantidad" 
                    label="Cantidad"
                    >
                  </v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field 
                    v-model="calidad" 
                    label="Calidad"
                    >
                  </v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field 
                    v-model="planes" 
                    label="Plan"
                    >
                  </v-text-field>
                  </v-flex>
                </v-layout>
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="red darken-4" flat @click="cancelar">Cancelar</v-btn>
              <v-btn color="green darken-1" flat @click="actualizar">Guardar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="agregarModal" max-width="400px" persistent>
          <v-card>
            <v-card-title>
              <span class="headline">Agregar</span>
            </v-card-title>
            <v-card-text>
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs12 sm6 md6>
                    <v-text-field 
                    v-model="savedItem.pK_ARTICULO" 
                    label="Artículo"
                    >
                  </v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field 
                    v-model="savedItem.talla" 
                    @keyup="uppercase"
                    label="Tallas"
                    >
                  </v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md12>
                    <v-text-field 
                    v-model="savedItem.cantidad" 
                    label="Cantidad"
                    type="number"
                    >
                  </v-text-field>
                  </v-flex>
                </v-layout>
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="red darken-4" flat @click="cancel">Cancelar</v-btn>
              <v-btn color="green darken-1" flat @click="add">Guardar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="verTallas" persistent max-width="1000px">
          <v-card>
            <v-card-title>
              <span class="headline">Lista de tallas</span>
            </v-card-title>
            <v-card-text>
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs8 sm8 md8 lg10 xl10>
                    <v-text-field
                      append-icon="search"
                      class="text-xs-center"
                      v-model="texto"
                      label="Ingrese el código a buscar"
                      @keyup.enter="buscarTalla()"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs4 sm4 md4 lg2 xl2>
                    <v-btn
                @click="buscarTalla()"
                fab
                dark
                color="teal"
              >
                <v-icon dark>list</v-icon>
              </v-btn>
                  </v-flex>
                  <v-flex xs12 sm12 md12 lg12 xl12>
                    <template>
                      <v-data-table
                        :headers="cabeceraTallas"
                        :items="tallas"
                        :rows-per-page-text="paginas"
                        class="elevation-1"
                      >
                        <template slot="items" slot-scope="props">
                          <td>{{ props.item.pK_ARTICULO }}</td>
                          <td>{{ props.item.talla }}</td>
                          <td>{{ props.item.numeracion }}</td>
                          <td>{{ props.item.descripcion}}</td>
                        </template>
                        <template slot="no-data">
                          <h3>No hay tallas para mostrar.</h3>
                        </template>
                      </v-data-table>
                    </template>
                  </v-flex>
                  <v-flex xs12 sm12 md10 lg10 xl10 v-if="valida > 0">
                    <v-alert :value="true" color="red darken-1" outline v-text="validaTalla">
                    </v-alert>
                  </v-flex>
                </v-layout>
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn @click="ocultarTallas()" color="red darken-4" dark class="mb-2">Cerrar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="eliminarModal" max-width="290px">
          <v-card>
            <v-card-title class="headline"
              >Eliminar Item?</v-card-title
            >
            <v-card-text>
              Estás a punto deverTallas
              <span>eliminar</span>
              el ítem {{ pk_articulo }}
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="green darken-1"
                flat="flat"
                @click="eliminarCerrar"
                >Cancelar</v-btn
              >
              <v-btn
                color="orange darken-4"
                flat="flat"
                @click="eliminar"
                >Eliminar</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>        
        <v-dialog v-model="importadoModal" max-width="1000px">
          <v-card>
            <v-card-text>
              <v-btn @click="crearImportado()">
                <v-icon>print</v-icon>
              </v-btn>
              <div id="importado">
                <header>
                  <div id="datos">
                    <p id="encabezado">
                      <b>MANUFACTURA BOLIVIANA S.A.</b>
                      <br />Fecha: {{ fechareporte }}
                      <br />Tienda: {{ tiendareporte }}
                      <br />Estado de stock inicial
                      <br />Correspondiente a la semana: {{ semana }}
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
                        <tr v-for="det in importados" :key="det.categoria">
                          <td style="text-align: center">{{ formatoMiles(det.categoria) }}</td>
                          <td style="text-align: center">{{ formatoMiles(det.total) }}</td>
                          <td style="text-align: center">{{ formatoMiles(det.monto) }}</td>
                        </tr>
                      </tbody>
                      <tfoot>
                        <tr>
                          <th></th>
                          <th></th>
                          <th style="text-align: right">Total: {{total = (calcularTotalImportado)}}</th>
                        </tr>
                        <tr>
                          <th></th>
                          <th></th>
                          <th style="text-align: right">Total Bs: {{total = (calcularImportado)}}</th>
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
              <v-btn @click="ocultarImportado()" color="red" flat>Cancelar</v-btn>
            </v-card-text>
          </v-card>
        </v-dialog>
        <v-dialog v-model="stockModal" max-width="1000px">
          <v-card>
            <v-card-text>
              <v-btn @click="crearStock()">
                <v-icon>print</v-icon>
              </v-btn>
              <div id="stock">
                <header>
                  <div id="datos">
                    <p id="encabezado">
                      <b>MANUFACTURA BOLIVIANA S.A.</b>
                      <br />Fecha: {{ fechareporte }}
                      <br />Estado de stock final
                      <br />Tienda: {{ tiendareporte }}
                      <br />Correspondiente a la semana: {{ semanareporte }}
                    </p>
                  </div>
                </header>
                <br />
                <section>
                  <div>
                    <br />Stock inicial a  semana: {{ semanareporte }}
                    <table id="repdetalle">
                      <thead>
                        <tr id="rep">
                          <th>CATEGORIA</th>
                          <th>CANTIDAD</th>
                          <th>VALOR</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="det, index in stocks" :key="index">
                          <td style="text-align: center">{{ det.categoriai }}</td>
                          <td style="text-align: center">{{ formatoMiles(det.totali) }}</td>
                          <td style="text-align: center">{{ formatoMiles(det.montoi) }}</td>
                        </tr>
                      </tbody>
                      <tfoot>
                        <tr>
                          <th style="text-align: right">Total Artículos: {{totalInicialArticulo = (calcularTotalExistencia)}}</th>
                          <th style="text-align: right">Total Importe: {{totalInicialImporte = (calcularExistencia)}}</th>
                        </tr>
                      </tfoot>
                    </table>
                    <br />Stock final a  semana: {{ semanareporte }}
                    <table id="repdetalle">
                      <thead>
                        <tr id="rep">
                          <th>CATEGORIA</th>
                          <th>CANTIDAD</th>
                          <th>VALOR</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="det, index in stocks" :key="index">
                          <td v-if="det.categoriaf != ''" style="text-align: center">{{ det.categoriaf }}</td>
                          <td v-if="det.totalf != ''" style="text-align: center">{{ formatoMiles(det.totalf) }}</td>
                          <td v-if="det.montof != ''" style="text-align: center">{{ formatoMiles(det.montof) }}</td>
                        </tr>
                      </tbody>
                      <tfoot>
                        <tr>
                          <th style="text-align: right">Total Artículos: {{totalFinalArticulo = (calcularTotalStock)}}</th>
                          <th style="text-align: right">Total Importe: {{totalFinalImporte = (calcularStock)}}</th>
                        </tr>
                      </tfoot>
                    </table>
                    <tr>
                      <th></th>
                      <th></th>
                      <th style="text-align: left">Diferencia Artículos: {{totalStokArticulo = (calcularDiferenciaArticulo)}}</th>
                    </tr>
                    <tr>
                      <th></th>
                      <th></th>
                      <th style="text-align: left">Diferencia Importe: {{totalStockImporte = (calcularDiferenciaImporte)}}</th>
                    </tr>
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
              <v-btn @click="ocultarStock()" color="red" flat>Cancelar</v-btn>
            </v-card-text>
          </v-card>
        </v-dialog>
      </v-toolbar>
      <v-data-table :headers="headers" :items="inventarios" :search="search" class="elevation-1" v-if="verNuevo == 0"
        :rows-per-page-text="pagina">
        <template slot="items" slot-scope="props">
          <td>{{ props.item.pK_TIENDA}}</td>
          <td>{{ props.item.codigo }}</td>
          <td>{{ props.item.semana }}</td>
          <td>{{ props.item.fechainicio }}</td>
          <td>{{ props.item.estado }}</td>
          <td>{{ props.item.usuario }}</td>
          <td>{{ props.item.rol }}</td>
          <td>
            <template>
              <v-btn v-if="props.item.estado != 'MANUAL'" class="mr-1" fab dark small color="pink" @click="verInventario(props.item)">
                <v-icon dark>open_in_new</v-icon>
              </v-btn>
              <v-btn class="mr-1" fab dark small color="purple" @click="verManual(props.item)">
                <v-icon dark>description</v-icon>
              </v-btn>
              <v-btn  class="mr-1" fab dark small color="cyan" @click="verIngresar(props.item)">
                <v-icon dark>add</v-icon>
              </v-btn>
              <v-btn  class="mr-1" fab dark small color="orange" @click="verModificar(props.item)">
                <v-icon dark>edit</v-icon>
              </v-btn>
              <v-btn v-if="props.item.estado != 'MANUAL'"  class="mr-1" fab dark small color="cyan" @click="verDetalles(props.item)">
                <v-icon dark>tab</v-icon>
              </v-btn>
              <v-btn class="mr-1" fab dark small color="green" @click="finalizarMostrar(3,props.item)">
              <v-icon dark>done_all</v-icon>
            </v-btn>
            <v-btn v-if="$store.state.usuario.rol != 'CONSIGNATARIO'"  class="mr-1" fab dark small color="red darken-1" @click="anularMostrar(4,props.item)">
              <v-icon dark>block</v-icon>
            </v-btn>
            </template>
          </td>
        </template>
        <template slot="no-data">
          <v-btn color="primary" @click="listar">Resetear</v-btn>
        </template>
        <template slot="no-results">
          <v-alert :value="true" color="red darken-1" outline icon="warning">
            Tu búsqueda de "{{ search }}" no encontro resultados.
          </v-alert>
        </template>
      </v-data-table>
      <v-form ref="form">
        <v-container grid-list-sm class="pa-4 white" v-if="verNuevo == 1">
          <v-layout row wrap>
            <v-flex xs6 sm6 md2 lg2 xl2>
              <v-autocomplete  
              v-if="this.verInv == 1" 
              v-model="pk_tienda" 
              :items="tiendas" 
              label="Tienda"
              onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" :rules="[rules.requerido]"
              v-on:change="encuentraTienda(pk_tienda)" 
              no-data-text="No hay datos disponibles"
              readonly>
              </v-autocomplete>
            </v-flex>
            <v-flex xs6 sm6 md2 lg2 xl2>
              <v-text-field v-model="nombre" label="Nombre" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs12 sm12 md2 lg2 xl2>
              <v-select 
              v-if="this.verInv == 1" 
              v-model="pk_inventario" 
              :items="numeros" 
              :rules="[rules.requerido]"
              no-data-text="No hay datos disponibles" 
              label="Número"
              readonly>
              </v-select>
            </v-flex>
            <v-flex xs6 sm6 md2 lg2 xl2>
              <v-text-field v-model="estante" :rules="[rules.requerido]" @keyup="uppercase" label="Estante">
              </v-text-field>
            </v-flex>
            <v-flex xs10 sm10 md8 lg8 xl8>
              <v-text-field v-model="codigo" label="Código de Barras"
                @keyup.enter="buscarCodigo()">
              </v-text-field>
            </v-flex>
            <v-flex xs2 sm2 md4 lg2 xl2>
              <div>
              <v-btn @click="buscarCodigo()" small fab dark color="primary">
                <v-icon dark>login</v-icon>
              </v-btn>
            </div>
            </v-flex>
            <v-flex xs12 sm2 md2 lg2 xl2 v-if="errorArticulo">
              <div class="red--text font-weight-bold" style="font-size: 20px" v-text="errorArticulo"></div>
            </v-flex>
            <v-flex xs12 sm12 md10 lg10 xl10>
              <v-flex class="text-xs-left" style="font-size: 18px">
              <strong>Total Lecturas: </strong>
                {{ (totalLecturas = calcularTotalLecturas) }}
              </v-flex>
              <v-data-table :headers="cabeceraDetalles" :items="detalles" hide-actions class="elevation-1">
                <template slot="items" slot-scope="props">
                  <td>{{ props.index  + 1}}</td>
                  <td>{{ props.item.canal }}</td>
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.talla }}</td>
                  <td>{{ props.item.cantidad }}</td>
                  <td>{{ props.item.calidad }}</td>
                  <td>{{ props.item.planes }}</td>
                  <td>{{ props.item.horainicio }}</td>
                  <td>{{ props.item.horafin }}</td>
                  <td class="justify-center layout px-0">
                    <v-icon small class="mr-2" @click="editItem(props.item)">
                      edit
                    </v-icon>
                    <v-icon small class="mr-2" @click="eliminarDetalle(detalles, props.item)">
                      delete
                    </v-icon>
                  </td>
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
            <v-flex xs12 sm12 md12 lg12 xl12>
              <li class="red--text font-weight-bold" v-for="(excluido, index) in excluidos" :key="index">
              {{ excluido.pK_ARTICULO }}/{{ excluido.talla }}/{{ excluido.contados }}
            </li>
          </v-flex>
            <v-flex xs12 sm12 md12 lg12 xl12>
              <v-btn v-if="verDetalle == 0" @click="guardar()" :disabled="waitModal" :loading="waitModal" color="green darken-1" dark class="mb-2">Guardar</v-btn>
              <v-btn @click="exportarDetalleExcel()" color="blue darken-1" dark class="mb-2">Exportar</v-btn>
              <v-btn @click="mostrarTallas()" color="orange darken-1" dark class="mb-2">Tallas</v-btn>
              <v-btn @click="ocultarNuevo()" color="red darken-1" dark class="mb-2">Atrás</v-btn>
            </v-flex>
          </v-layout>
        </v-container>
      </v-form>
      <v-form ref="form">
        <v-container grid-list-sm class="pa-4 white" v-if="verNuevo == 2">
          <v-layout row wrap>
            <v-flex xs6 sm6 md6 lg4 xl4>
              <v-text-field 
              v-model="correlativo" label="Código" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs3 sm3 md3 lg3 xl3>
              <v-text-field v-model="tienda" label="Tienda" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs3 sm3 md3 lg3 xl3>
              <v-text-field v-model="semana" label="Semana" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs8 sm8 md8 lg8 xl8>
              <v-select v-model="reporte" :items="reportes" label="Reportes de Inventario"></v-select>
            </v-flex>
            <v-flex xs4 sm4 md4 lg4 xl4>
              <v-btn @click="verReporte()" color="blue darken-4" dark class="mb-2" >GENERAR</v-btn>
            </v-flex>
            <v-flex v-if="verLista == 1" xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraDiferencias" :items="diferencias" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.categoria }}</td>
                  <td>{{ props.item.existencias }}</td>
                  <td>{{ props.item.contados }}</td>
                  <td>{{ props.item.diferencias }}</td>
                  <td>{{ props.item.precio }}</td>
                  <td>{{ formatoMiles(props.item.total) }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{totalPares = calcularDiferenciaPares}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{totalAccesorios = calcularDiferenciaAccesorios}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{totalDiferenciasPares = formatoMiles(calcularDiferenciaImportePares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{totalDiferenciasAccesorios = formatoMiles(calcularDiferenciaImporteAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalDiferenciasImporte = (calcularDiferenciaTotalImporte)}}
              </v-flex>
              <v-btn @click="crearDiferencia()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="exportarDiferenciaExcel()" color="green darken-4" dark class="mb-2">Exportar</v-btn>
              <v-btn @click="ocultarDiferencia()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
            <v-flex v-if="verLista == 2" xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraRecapitulado" :items="recapitulado" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.estante }}</td>
                  <td>{{ props.item.cantidad }}</td>
                  <td>{{ props.item.precio }}</td>
                  <td>{{ props.item.usuario }}</td>
                  <td>{{ props.item.inicio }}</td>
                  <td>{{ props.item.fin }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{recapituladoPares = formatoMiles(cantidadPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{recapituladoAccesorios = formatoMiles(cantidadAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{importeRecapituladoPares = formatoMiles(importePares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{importeRecapituladoAccesorios = formatoMiles(importeAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalRecapituladoImporte = (calcularRecapituladoTotalImporte)}}
              </v-flex>
              <v-btn @click="crearRecapitulado()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="exportarRecapituladoExcel()" color="green darken-4" dark class="mb-2">Exportar</v-btn>
              <v-btn @click="ocultarRecapitulado()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
            <v-flex v-if="verLista == 3" xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraLectura" :items="lecturas" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.cantidad }}</td>
                  <td>{{ props.item.precio }}</td>
                  <td>{{ props.item.estante }}</td>
                  <td>{{ props.item.usuario }}</td>
                  <td>{{ props.item.inicio }}</td>
                  <td>{{ props.item.fin }}</td>
                  <td>{{ props.item.categoria }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{recapituladoPares = formatoMiles(cantidadPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{recapituladoAccesorios = formatoMiles(cantidadAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{importeRecapituladoPares = formatoMiles(importePares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{importeRecapituladoAccesorios = formatoMiles(importeAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalRecapituladoImporte = (calcularRecapituladoTotalImporte)}}
              </v-flex>
              <v-btn @click="crearLectura()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="exportarLecturaExcel()" color="green darken-4" dark class="mb-2">Exportar</v-btn>
              <v-btn @click="ocultarLectura()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
            <v-flex v-if="verLista == 4" xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraInicial" :items="iniciales" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.total }}</td>
                  <td>{{ props.item.taM1 }}</td>
                  <td>{{ props.item.taM2 }}</td>
                  <td>{{ props.item.taM3 }}</td>
                  <td>{{ props.item.taM4 }}</td>
                  <td>{{ props.item.taM5 }}</td>
                  <td>{{ props.item.taM6 }}</td>
                  <td>{{ props.item.taM7 }}</td>
                  <td>{{ props.item.taM8 }}</td>
                  <td>{{ props.item.taM9 }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="red--text font-weight-bold" v-if="errorTalla"><strong>{{ errorTalla }}</strong></v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{inicialPares = formatoMiles(numeroPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{inicialAccesorios = formatoMiles(numeroAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{importeInicialPares = formatoMiles(costoPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{importeInicialAccesorios = formatoMiles(costoAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalInicialImporte = (calcularInicialTotalImporte)}}
              </v-flex>
              <v-btn @click="crearInicial()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="ocultarInicial()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
            <v-flex v-if="verLista == 5" xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraFinal" :items="finales" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.total }}</td>
                  <td>{{ props.item.taM1 }}</td>
                  <td>{{ props.item.taM2 }}</td>
                  <td>{{ props.item.taM3 }}</td>
                  <td>{{ props.item.taM4 }}</td>
                  <td>{{ props.item.taM5 }}</td>
                  <td>{{ props.item.taM6 }}</td>
                  <td>{{ props.item.taM7 }}</td>
                  <td>{{ props.item.taM8 }}</td>
                  <td>{{ props.item.taM9 }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="red--text font-weight-bold" v-if="errorTalla"><strong>{{ errorTalla }}</strong></v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{finalPares = formatoMiles(cantidadPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{finalAccesorios = formatoMiles(cantidadAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{importeFinalPares = formatoMiles(importePares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{importeFinalAccesorios = formatoMiles(importeAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalFinalImporte = (calcularFinalTotalImporte)}}
              </v-flex>
              <v-btn @click="crearFinal()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="ocultarFinal()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
          </v-layout>
        </v-container>
      </v-form>
      <v-form ref="form">
        <v-container grid-list-sm class="pa-4 white" v-if="verNuevo == 3">
          <v-layout row wrap>
            <v-flex xs6 sm6 md6 lg2 xl2>
              <v-text-field 
              v-model="correlativo" label="Código" readonly>
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
            <v-flex xs5 sm5 md3 lg2 xl2>
              <v-text-field v-model="estante" @keyup="uppercase" label="Estante">
              </v-text-field>
            </v-flex>
            <v-flex xs2 sm2 md2 lg2 xl2>
              <v-btn
                @click="buscarEstante()"
                fab
                dark
                color="teal"
              >
                <v-icon dark>list</v-icon>
              </v-btn>
            </v-flex>
            <v-flex xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraEstante" :items="estantes" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.canal }}</td>
                  <td>{{ props.item.talla }}</td>
                  <td>{{ props.item.cantidad }}</td>
                  <td>{{ props.item.calidad }}</td>
                  <td>{{ props.item.planes }}</td>
                  <td class="justify-center layout px-0">
                    <v-icon small class="mr-2" @click="modificar(props.item)">
                      edit
                    </v-icon>
                    <v-icon small class="mr-2" @click="mostrarEliminar(props.item)">
                      delete
                    </v-icon>
                  </td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex xs12 sm12 md10 lg10 xl10 v-if="valida > 0">
              <v-alert :value="true" color="red darken-1" outline v-text="validaEstante">
              </v-alert>
            </v-flex>
            <v-flex class="text-xs-left" style="font-size: 18px">
              <strong>Total Lecturas: </strong>
                {{ (totalEstantes = calcularTotalEstantes) }}
              </v-flex>
              <v-btn @click="eliminarModificar(clave,estante)" color="blue darken-2" dark class="mb-2">Eliminar</v-btn>
              <v-btn @click="ocultarModificar()" color="red darken-1" dark class="mb-2">Atrás</v-btn>
            </v-flex>
          </v-layout>
        </v-container>
      </v-form>
      <v-form ref="form">
        <v-container grid-list-sm class="pa-4 white" v-if="verNuevo == 4">
          <v-layout row wrap>
            <v-flex v-if="verCampo == 1" xs12 sm12 md12 lg12 xl12>
              <input id="archivoExcel" ref="fileupload" type="file" @change="subirIngresar()" />
            </v-flex>
            <v-flex xs6 sm6 md3 lg3 xl3>
              <v-text-field 
              v-model="correlativo" label="Código" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs3 sm3 md3 lg3 xl3>
              <v-text-field v-model="tienda" label="Tienda" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs3 sm3 md3 lg3 xl3>
              <v-text-field v-model="semana" label="Semana" readonly>
              </v-text-field>
            </v-flex>
            <v-flex v-if="estado == 'MANUAL'" xs8 sm8 md8 lg8 xl8>
              <v-select v-model="manual" :items="manuales" label="Inventario Manual"></v-select>
            </v-flex>
            <v-flex v-if="estado == 'MANUAL'" xs4 sm4 md4 lg4 xl4>
              <v-btn @click="generarManual()" color="blue darken-4" dark class="mb-2" >GENERAR</v-btn>
            </v-flex>
            <v-flex v-if="estado != 'MANUAL'" xs8 sm8 md8 lg8 xl8>
              <v-select v-model="hibrido" :items="hibridos" label="Inventario Manual"></v-select>
            </v-flex>
            <v-flex v-if="estado != 'MANUAL'" xs4 sm4 md4 lg4 xl4>
              <v-btn @click="generarHibrido()" color="blue darken-4" dark class="mb-2" >GENERAR</v-btn>
            </v-flex>
            <v-flex v-if="verTabla == 1" xs12 sm12 md10 lg10 xl10>
              <v-data-table :headers="cabeceraInicial" :items="iniciales" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.total }}</td>
                  <td>{{ props.item.taM1 }}</td>
                  <td>{{ props.item.taM2 }}</td>
                  <td>{{ props.item.taM3 }}</td>
                  <td>{{ props.item.taM4 }}</td>
                  <td>{{ props.item.taM5 }}</td>
                  <td>{{ props.item.taM6 }}</td>
                  <td>{{ props.item.taM7 }}</td>
                  <td>{{ props.item.taM8 }}</td>
                  <td>{{ props.item.taM9 }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="red--text font-weight-bold" v-if="errorTalla"><strong>{{ errorTalla }}</strong></v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{inicialPares = formatoMiles(numeroPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{inicialAccesorios = formatoMiles(numeroAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{importeInicialPares = formatoMiles(costoPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{importeInicialAccesorios = formatoMiles(costoAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalInicialImporte = (calcularInicialTotalImporte)}}
              </v-flex>
              <v-btn @click="crearManual()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="ocultarPlanilla()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
            <v-flex v-if="verCampo == 1" xs8 sm8 md8 lg8 xl8>
              <v-select 
              v-model="pk_categoria" 
              :items="categorias" 
              label="Categorías"
              no-data-text="No hay datos disponibles">
              </v-select>
            </v-flex>
            <v-flex v-if="verCampo == 1" xs4 sm4 md4 lg4 xl4>
              <v-btn @click="buscarProducto()" fab dark color="teal">
              <v-icon dark>list</v-icon>
              </v-btn>
            </v-flex>
            <v-flex xs12 sm12 md12 lg12 xl12 class="red--text font-weight-bold" v-if="errorCategoria"><strong>{{ errorCategoria }}</strong></v-flex>
            <v-flex v-if="verCampo == 1 && $store.state.usuario.rol != 'CONSIGNATARIO'" xs12 sm12 md4 lg4 xl4>
              <v-text-field v-model="estante" :rules="[rules.requerido]" @keyup="uppercase" label="Estante">
              </v-text-field>
            </v-flex>
            <v-flex v-if="verCampo == 1" xs12 sm12 md2 lg2 xl2>
              <v-text-field v-model="hora1" label="Hora Inicio" type="time"></v-text-field>
            </v-flex>
            <v-flex v-if="verCampo == 1" xs12 sm12 md2 lg2 xl2>
              <v-text-field v-model="hora2" label="Hora Final" type="time"></v-text-field>
            </v-flex>
<!--             <v-flex v-if="verCampo == 1" xs4 sm4 md4 lg4 xl4>
              <v-btn @click="verAgregar()" fab dark color="blue">
              <v-icon dark>add</v-icon>
              </v-btn>
            </v-flex> -->
            <v-flex v-if="verCampo == 1" xs12 sm12 md10 lg9 xl9>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraProductos" :items="productos" :search="search" :pagination.sync="pagination" hide-actions class="elevation-1">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.precio }}</td>
                  <td>{{ props.item.talla }}</td>
                  <td><v-text-field onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" v-model="props.item.cantidad"></v-text-field></td>
                </template>
                <template slot="no-data">
                  <h3>No hay artículo para mostrar.</h3>
                </template>
              </v-data-table>
              <div class="text-xs-center pt-2">
                  <v-pagination v-model="pagination.page" :length="pages"></v-pagination>
              </div>
              <v-flex class="text-xs-left"  style="font-size: 18px">
                <strong>Total Artículos: </strong>{{ total = calcularTotalArticulos }}
              </v-flex>
              <v-alert :value="true" color="red darken-1" outline v-for="v in validaMensaje" :key="v" v-text="v">
              </v-alert>
              <v-btn @click="registrar()" :disabled="waitModal" :loading="waitModal" color="green darken-4" dark class="mb-2">Guardar</v-btn>
              <v-btn @click="ocultarRegistrar()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
            <v-flex v-if="verLista == 1" xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraDiferencias" :items="diferencias" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.categoria }}</td>
                  <td>{{ props.item.existencias }}</td>
                  <td>{{ props.item.contados }}</td>
                  <td>{{ props.item.diferencias }}</td>
                  <td>{{ props.item.precio }}</td>
                  <td>{{ formatoMiles(props.item.total) }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{totalPares = calcularDiferenciaPares}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{totalAccesorios = calcularDiferenciaAccesorios}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{totalDiferenciasPares = formatoMiles(calcularDiferenciaImportePares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{totalDiferenciasAccesorios = formatoMiles(calcularDiferenciaImporteAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalDiferenciasImporte = (calcularDiferenciaTotalImporte)}}
              </v-flex>
              <v-btn @click="crearDiferencia()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="exportarDiferenciaExcel()" color="green darken-4" dark class="mb-2">Exportar</v-btn>
              <v-btn @click="ocultarDiferencia()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
            <v-flex v-if="verLista == 4" xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraInicial" :items="iniciales" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.total }}</td>
                  <td>{{ props.item.taM1 }}</td>
                  <td>{{ props.item.taM2 }}</td>
                  <td>{{ props.item.taM3 }}</td>
                  <td>{{ props.item.taM4 }}</td>
                  <td>{{ props.item.taM5 }}</td>
                  <td>{{ props.item.taM6 }}</td>
                  <td>{{ props.item.taM7 }}</td>
                  <td>{{ props.item.taM8 }}</td>
                  <td>{{ props.item.taM9 }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="red--text font-weight-bold" v-if="errorTalla"><strong>{{ errorTalla }}</strong></v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{inicialPares = formatoMiles(numeroPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{inicialAccesorios = formatoMiles(numeroAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{importeInicialPares = formatoMiles(costoPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{importeInicialAccesorios = formatoMiles(costoAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalInicialImporte = (calcularInicialTotalImporte)}}
              </v-flex>
              <v-btn @click="crearInicial()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="ocultarInicial()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
            <v-flex v-if="verLista == 5" xs12 sm12 md10 lg10 xl10>
              <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details>
              </v-text-field>
              <v-data-table :headers="cabeceraFinal" :items="finales" :search="search" :rows-per-page-text="paginas">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.total }}</td>
                  <td>{{ props.item.taM1 }}</td>
                  <td>{{ props.item.taM2 }}</td>
                  <td>{{ props.item.taM3 }}</td>
                  <td>{{ props.item.taM4 }}</td>
                  <td>{{ props.item.taM5 }}</td>
                  <td>{{ props.item.taM6 }}</td>
                  <td>{{ props.item.taM7 }}</td>
                  <td>{{ props.item.taM8 }}</td>
                  <td>{{ props.item.taM9 }}</td>
                </template>
                <template slot="no-data">
                  <td></td>
                </template>
                <template slot="no-results">
                  <v-alert :value="true" color="red darken-1" outline icon="warning">
                    Tu búsqueda de "{{ search }}" no encontro resultados.
                  </v-alert>
                </template>
              </v-data-table>
              <v-flex class="red--text font-weight-bold" v-if="errorTalla"><strong>{{ errorTalla }}</strong></v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Pares: </strong> {{finalPares = formatoMiles(cantidadPares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Accesorios: </strong> {{finalAccesorios = formatoMiles(cantidadAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Pares: </strong> {{importeFinalPares = formatoMiles(importePares)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Importe Accesorios: </strong> {{importeFinalAccesorios = formatoMiles(importeAccesorios)}}
              </v-flex>
              <v-flex class="text-xs-left">
                <strong>Total Importe: </strong> {{totalFinalImporte = (calcularFinalTotalImporte)}}
              </v-flex>
              <v-btn @click="crearFinal()" color="blue darken-4" dark class="mb-2">Imprimir</v-btn>
              <v-btn @click="ocultarFinal()" color="red darken-4" dark class="mb-2">Cancelar</v-btn>
            </v-flex>
          </v-layout>
        </v-container>
      </v-form>
      <v-form ref="form">
        <v-container grid-list-sm class="pa-4 white" v-if="verNuevo == 5">
          <v-layout row wrap>
            <v-flex xs6 sm6 md3 lg2 xl2>
              <v-text-field 
              v-model="correlativo" label="Código" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs3 sm3 md3 lg2 xl2>
              <v-text-field v-model="tienda" label="Tienda" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs3 sm3 md3 lg2 xl2>
              <v-text-field v-model="semana" label="Semana" readonly>
              </v-text-field>
            </v-flex>
            <v-flex xs5 sm5 md4 lg2 xl2>
              <v-text-field v-model="estante" @keyup="uppercase" label="Estante">
              </v-text-field>
            </v-flex>
            <v-flex xs5 sm5 md4 lg6 xl6>
              <v-text-field v-model="articulo" label="Código del Artículo"
                @keyup.enter="buscarArticulo()">
              </v-text-field>
            </v-flex>
            <v-flex xs2 sm2 md4 lg2 xl2>
              <div>
              <v-btn @click="buscarArticulo()" small fab dark color="primary">
                <v-icon dark>login</v-icon>
              </v-btn>
            </div>
            </v-flex>
            <v-flex xs12 sm12 md8 lg8 xl8>
              <v-data-table :headers="cabeceraArticulos" :items="articulos" hide-actions class="elevation-1">
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.pK_ARTICULO }}</td>
                  <td>{{ props.item.talla }}</td>
                  <td><v-text-field onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" v-model="props.item.cantidad"></v-text-field></td>
                </template>
                <template slot="no-data">
                  <h3>No hay artículo para mostrar.</h3>
                </template>
              </v-data-table>
              <v-alert :value="true" color="red darken-1" outline v-for="v in validaMensaje" :key="v" v-text="v">
              </v-alert>
              <v-btn @click="ingresar()" color="green darken-4" dark class="mb-2">Guardar</v-btn>
              <v-btn @click="ocultarIngresar()" color="red darken-1" dark class="mb-2">Atrás</v-btn>
            </v-flex>
          </v-layout>
        </v-container>
      </v-form>
    </v-flex>
  </v-layout>
</template>
<script>
import axios from "axios";
import jsPDF from "jspdf";
import html2canvas from "html2canvas";
import autoTable from "jspdf-autotable";
import readXlsFile from "read-excel-file"
import XLSX from "xlsx";
export default {
  data() {
    return {
      pagination: {},
      enableDisable: false,
      articulos: [],
      estantes: [],
      inventarios: [],
      detalles: [],
      tiendas: [],
      numeros: [],
      categorias: [],
      diferencias: [],
      recapitulado: [],
      lecturas: [],
      iniciales: [],
      finales: [],
      excluidos: [],
      importados: [],
      productos: [],
      stocks: [],
      tallas: [],
      hibridos: ['Planilla Inventario', 'Registrar Inventario'],
      reportes: ['Stock Final', 'Planilla Inicial', 'Planilla Diferencias', 'Planilla Recapitulado', 'Planilla Lecturas', 'Planilla Final'],
      manuales: ['Planilla Inventario', 'Registrar Inventario', 'Stock Final', 'Planilla Inicial', 'Planilla Diferencias', 'Planilla Final'],
      waitModal: false,
      modificarModal: false,
      eliminarModal: false,
      dialog: false,
      rules: {
        requerido: (value) => !!value || "Requerido!",
      },
      headers: [
        { text: "Tienda", value: "pK_TIENDA", sortable: false, class: "black--text font-weight-bold" },
        { text: "Código", value: "codigo", sortable: false, class: "black--text font-weight-bold" },
        { text: "Semana", value: "semana", sortable: false, class: "black--text font-weight-bold" },
        { text: "Fecha Inicio", value: "fechainicio", sortable: false, class: "black--text font-weight-bold" },
        { text: "Estado", value: "estado", sortable: false, class: "black--text font-weight-bold" },
        { text: "Usuario", value: "usuario", sortable: false, class: "black--text font-weight-bold" },
        { text: "Rol", value: "rol", sortable: false, class: "black--text font-weight-bold" },
        { text: "Opciones", value: "opciones", sortable: false, class: "black--text font-weight-bold" }
      ],
      cabeceraDetalles: [
        { text: "#", value: "index", class: "black--text font-weight-bold" },
        { text: "Canal", value: "canal", sortable: false, class: "black--text font-weight-bold" },
        { text: "Artículo", value: "pK_ARTICULO", sortable: false, class: "black--text font-weight-bold" },
        { text: "Talla", value: "talla", sortable: false, class: "black--text font-weight-bold" },
        { text: "Cantidad", value: "cantidad", sortable: false, class: "black--text font-weight-bold" },
        { text: "Calidad", value: "calidad", sortable: false, class: "black--text font-weight-bold" },
        { text: "Plan", value: "planes", sortable: false, class: "black--text font-weight-bold" },
        { text: "Hora I", value: "horainicio", sortable: false, class: "black--text font-weight-bold" },
        { text: "Hora F", value: "horafin", sortable: false, class: "black--text font-weight-bold" },
        { text: "Opciones", value: "opciones", sortable: false, class: "black--text font-weight-bold" }
      ],
      cabeceraEstante: [
        { text: "Artículo", value: "pK_ARTICULO", sortable: false, class: "black--text font-weight-bold" },
        { text: "Canal", value: "canal", sortable: false, class: "black--text font-weight-bold" },
        { text: "Talla", value: "talla", sortable: false, class: "black--text font-weight-bold" },
        { text: "Cantidad", value: "cantidad", sortable: false, class: "black--text font-weight-bold" },
        { text: "Calidad", value: "calidad", sortable: false, class: "black--text font-weight-bold" },
        { text: "Plan", value: "planes", sortable: false, class: "black--text font-weight-bold" },
        { text: "Opciones", value: "opciones", sortable: false, class: "black--text font-weight-bold" }
      ],
      cabeceraDiferencias: [
        { text: "Artículo", value: "pK_ARTICULO", sortable: false, class: "black--text font-weight-bold" },
        { text: "Categoría", value: "categoria", sortable: false, class: "black--text font-weight-bold" },
        { text: "Existencias", value: "existencias", sortable: false, class: "black--text font-weight-bold" },
        { text: "Contados", value: "contados", sortable: false, class: "black--text font-weight-bold" },
        { text: "Diferencias", value: "diferencia", sortable: false, class: "black--text font-weight-bold" },
        { text: "Precio", value: "precio", sortable: false, class: "black--text font-weight-bold" },
        { text: "Total", value: "total", sortable: false, class: "black--text font-weight-bold" },
      ],
      cabeceraRecapitulado: [
        { text: "Estante", value: "estante", sortable: false, class: "black--text font-weight-bold" },
        { text: "Total", value: "cantidad", sortable: false, class: "black--text font-weight-bold" },
        { text: "Importe", value: "precio", sortable: false, class: "black--text font-weight-bold" },
        { text: "Usuario", value: "usuario", sortable: false, class: "black--text font-weight-bold" },
        { text: "Hora Inicio", value: "inicio", sortable: false, class: "black--text font-weight-bold" },
        { text: "Hora Final", value: "fin", sortable: false, class: "black--text font-weight-bold" },
      ],
      cabeceraLectura: [
        { text: "Artículo", value: "pK_ARTICULO", sortable: false, class: "black--text font-weight-bold" },
        { text: "Total", value: "cantidad", sortable: false, class: "black--text font-weight-bold" },
        { text: "Importe", value: "precio", sortable: false, class: "black--text font-weight-bold" },
        { text: "Estante", value: "estante", sortable: false, class: "black--text font-weight-bold" },
        { text: "Usuario", value: "usuario", sortable: false, class: "black--text font-weight-bold" },
        { text: "Hora Inicio", value: "inicio", sortable: false, class: "black--text font-weight-bold" },
        { text: "Hora Final", value: "fin", sortable: false, class: "black--text font-weight-bold" },
        { text: "Categoría", value: "categoria", sortable: false, class: "black--text font-weight-bold" },
      ],
      cabeceraInicial: [
        { text: "Artículo", value: "pK_ARTICULO", sortable: false, class: "black--text font-weight-bold" },
        { text: "Total", value: "total", sortable: false, class: "black--text font-weight-bold" },
        { text: "N1", value: "taM1", sortable: false, class: "black--text font-weight-bold" },
        { text: "N2", value: "taM2", sortable: false, class: "black--text font-weight-bold" },
        { text: "N3", value: "taM3", sortable: false, class: "black--text font-weight-bold" },
        { text: "N4", value: "taM4", sortable: false, class: "black--text font-weight-bold" },
        { text: "N5", value: "taM5", sortable: false, class: "black--text font-weight-bold" },
        { text: "N6", value: "taM6", sortable: false, class: "black--text font-weight-bold" },
        { text: "N7", value: "taM7", sortable: false, class: "black--text font-weight-bold" },
        { text: "N8", value: "taM8", sortable: false, class: "black--text font-weight-bold" },
        { text: "N9", value: "taM9", sortable: false, class: "black--text font-weight-bold" },
      ],
      cabeceraFinal: [
        { text: "Artículo", value: "pK_ARTICULO", sortable: false, class: "black--text font-weight-bold" },
        { text: "Total", value: "total", sortable: false, class: "black--text font-weight-bold" },
        { text: "N1", value: "taM1", sortable: false, class: "black--text font-weight-bold" },
        { text: "N2", value: "taM2", sortable: false, class: "black--text font-weight-bold" },
        { text: "N3", value: "taM3", sortable: false, class: "black--text font-weight-bold" },
        { text: "N4", value: "taM4", sortable: false, class: "black--text font-weight-bold" },
        { text: "N5", value: "taM5", sortable: false, class: "black--text font-weight-bold" },
        { text: "N6", value: "taM6", sortable: false, class: "black--text font-weight-bold" },
        { text: "N7", value: "taM7", sortable: false, class: "black--text font-weight-bold" },
        { text: "N8", value: "taM8", sortable: false, class: "black--text font-weight-bold" },
        { text: "N9", value: "taM9", sortable: false, class: "black--text font-weight-bold" },
      ],
      cabeceraTallas: [
        { text: "Artículo", value: "pK_ARTICULO", sortable: false },
        { text: "Talla", value: "talla", sortable: false },
        { text: "Numeración", value: "numeracion", sortable: false },
        { text: "Descripción", value: "descripcion", sortable: false },
      ],
      cabeceraProductos: [
        { text: "Artículo", value: "pK_ARTICULO", sortable: false },
        { text: "Precio", value: "precio", sortable: false },
        { text: "Talla", value: "talla", sortable: false },
        { text: "Cantidad", value: "cantidad", sortable: false },
      ],
      cabeceraArticulos: [
        { text: "Artículo", value: "pK_ARTICULO", sortable: false },
        { text: "Talla", value: "talla", sortable: false },
        { text: "Cantidad", value: "cantidad", sortable: false },
      ],
      a: "",
      id: "",
      texto: "",
      clave: 0,
      search: "",
      horainicio: "",
      horafin: "",
      hora1: "",
      excel: 0,
      hora2: "",
      medida: "",
      estado: "",
      pk_tienda: "",
      pk_inventario: "",
      pk_categoria: "",
      articulo: "",
      nombre:"",
      codigo: "",
      numero:"",
      semana: "",
      tienda: "",
      pk_articulo: "",
      canal: "",
      talla: "",
      cantidad: 0,
      calidad: "",
      planes: "",
      consignatario: "",
      tiendareporte: "",
      codigoreporte: "",
      semanareporte: "",
      fechareporte: "",
      totalLecturas: 0,
      totalDiferenciasPares: 0,
      totalDiferenciasAccesorios: 0,
      totalDiferenciasImporte: 0,
      totalAccesorios: 0,
      totalPares: 0,
      cantidadPares: 0,
      cantidadAccesorios: 0,
      numeroPares: 0,
      numeroAccesorios: 0,
      costoPares: 0,
      costoAccesorios: 0,
      importePares: 0,
      importeAccesorios: 0,
      recapituladoPares: 0,
      recapituladoAccesorios: 0,
      importeRecapituladoPares: 0,
      importeRecapituladoAccesorios: 0,
      totalRecapituladoImporte: 0,
      inicialPares: 0,
      inicialAccesorios: 0,
      importeInicialPares: 0,
      importeInicialAccesorios: 0,
      finalPares: 0,
      finalAccesorios: 0,
      importeFinalPares: 0,
      importeFinalAccesorios: 0,
      diferenciaImportePares: 0,
      DiferenciaImporteAccesorios: 0,
      estante: "",
      fecha: "",
      reporte: "",
      manual: "",
      hibrido: "",
      paginas: "Items por página",
      pagina: "Inventarios por página",
      verNuevo: 0,
      adInventario: "",
      adId: "",
      adPk: "",
      adFin: 0,
      adAnular: 0,
      wait: 0,
      adAceptar: false,
      adEliminar: false,
      importadoModal: 0,
      agregarModal: 0,
      stockModal: 0,
      adAccion: 0,
      errorTalla: null,
      errorArticulo: null,
      errorCategoria: null,
      valida: 0,
      validaMensaje: [],
      validaEstante: "",
      validaTalla: "",
      verInv: 0,
      verDetalle: 0,
      verLista: 0,
      verTallas: 0,
      verTabla: 0,
      verCampo: 0,
      editedItem: {
      canal: '',
      pK_ARTICULO: '',
      talla: '',
      cantidad: 0,
      calidad: '',
      planes: '',
    },
    savedItem: {
      pK_ARTICULO: '',
      precio: 0,
      talla: '',
      cantidad: '',
    },
    starItem: {
      pK_ARTICULO: '',
      precio: 0,
      talla: '',
      cantidad: '',
    }
    };
  },
  computed: {
    pages () {
        if (this.pagination.rowsPerPage == null ||
          this.pagination.totalItems == null
        ) return 0
        this.pagination.page = 1;
        this.pagination.rowsPerPage = 9;
        this.pagination.totalItems = this.productos.length;
        return Math.ceil(this.pagination.totalItems / this.pagination.rowsPerPage)
      },
    calcularTotalArticulos: function () {
      var resultado = 0;
      for (var i = 0; i < this.productos.length; i++) {
        resultado = resultado + parseInt(this.productos[i].cantidad);
      }
      return resultado;
    },
    calcularTotalLecturas: function () {
      var resultado = 0;
      for (var i = 0; i < this.detalles.length; i++) {
        resultado = parseInt(resultado) + parseInt(this.detalles[i].cantidad);
      }
      return resultado;
    },
    calcularTotalEstantes: function () {
      var resultado = 0;
      for (var i = 0; i < this.estantes.length; i++) {
        resultado = parseInt(resultado) + parseInt(this.estantes[i].cantidad);
      }
      return resultado;
    },
    calcularImportado: function () {
      var resultado = 0.0;
      for (var i = 0; i < this.importados.length; i++) {
        resultado = resultado + parseFloat(this.importados[i].monto);
      }
      var total = 0;
      total = resultado.toFixed(2);
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = total.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      total =  arr[1] ? arr.join('.') : arr[0];
      return total;
    },
    calcularTotalExistencia: function () {
      var resultado = 0;
      for (var i = 0; i < this.stocks.length; i++) {
        resultado = resultado + parseInt(this.stocks[i].totali);
      }
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
    calcularExistencia: function () {
      var resultado = 0.0;
      for (var i = 0; i < this.stocks.length; i++) {
        resultado = resultado + parseFloat(this.stocks[i].montoi);
      }
      var total = 0;
      total = resultado.toFixed(2);
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = total.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      total =  arr[1] ? arr.join('.') : arr[0];
      return total;
    },
    calcularTotalImportado: function () {
      var resultado = 0;
      for (var i = 0; i < this.importados.length; i++) {
        resultado = resultado + parseInt(this.importados[i].total);
      }
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
    calcularStock: function () {
      var resultado = 0.0;
      for (var i = 0; i < this.stocks.length; i++) {
        resultado = resultado + parseFloat(this.stocks[i].montof);
      }
      resultado = resultado.toFixed(2);
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
    calcularTotalStock: function () {
      var resultado = 0;
      for (var i = 0; i < this.stocks.length; i++) {
        resultado = resultado + parseInt(this.stocks[i].totalf);
      }
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
    calcularDiferenciaArticulo: function () {
      var a = 0;
      var b = 0;
      var resultado = 0;
      for (var i = 0; i < this.stocks.length; i++) {
        a = a + parseInt(this.stocks[i].totali);
      }

      for (var i = 0; i < this.stocks.length; i++) {
       b = b + parseInt(this.stocks[i].totalf);
      }
      resultado = b - a
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
    calcularDiferenciaImporte: function () {
      var a = 0;
      var b = 0;
      var resultado = 0;
      for (var i = 0; i < this.stocks.length; i++) {
        a = a + parseFloat(this.stocks[i].montoi);
      }

      for (var i = 0; i < this.stocks.length; i++) {
        b = b + parseFloat(this.stocks[i].montof);
      }
      resultado = b - a;
      resultado = resultado.toFixed(2);
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado; 
    },
    calcularDiferenciaImportePares: function () {
      var resultado = 0.0;
      for (var i = 0; i < this.diferencias.length; i++) {
        if(this.diferencias[i].categoria != "ACCESSORIES" && this.diferencias[i].categoria != "PROMOTIONS" && this.diferencias[i].categoria != "CLOTHING"){
          resultado = resultado + parseFloat(this.diferencias[i].total);
        }
      };
      this.diferenciaImportePares = resultado;
      return resultado.toFixed(2);
    },
    calcularDiferenciaImporteAccesorios: function () {
      var resultado = 0.0;
      for (var i = 0; i < this.diferencias.length; i++) {
        if(this.diferencias[i].categoria == "ACCESSORIES" || this.diferencias[i].categoria == "PROMOTIONS" || this.diferencias[i].categoria == "CLOTHING"){
          resultado = resultado + parseFloat(this.diferencias[i].total);
        }
      }
      this.diferenciaImporteAccesorios = resultado;
      return resultado.toFixed(2);
    },
    calcularDiferenciaTotalImporte: function () {
      var resultado = 0.0;
      var a = 0;
      var b = 0
      a = this.diferenciaImportePares;
      b = this.diferenciaImporteAccesorios;

      resultado = a + b;  
      resultado = resultado.toFixed(2);
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
    calcularDiferenciaPares: function () {
      var pares = 0;
      for (var i = 0; i < this.diferencias.length; i++) {
        if(this.diferencias[i].categoria != "ACCESSORIES" && this.diferencias[i].categoria != "PROMOTIONS" && this.diferencias[i].categoria != "CLOTHING"){
            pares = parseInt(pares) + parseInt(this.diferencias[i].diferencias);
        }
      }
      return pares;
    },
    calcularDiferenciaAccesorios: function () {
      var accesorios = 0;
      for (var i = 0; i < this.diferencias.length; i++) {
        if(this.diferencias[i].categoria == "ACCESSORIES" || this.diferencias[i].categoria == "PROMOTIONS" || this.diferencias[i].categoria == "CLOTHING"){
          accesorios = parseInt(accesorios) + parseInt(this.diferencias[i].diferencias);
        }
      }
      return accesorios;
    },
    calcularRecapituladoTotalImporte: function () {
      var resultado = 0.0;
      var a = 0;
      var b = 0
      a = parseFloat(this.importePares);
      b = parseFloat(this.importeAccesorios);
      resultado = a + b;
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
    calcularInicialTotalImporte: function () {
      var resultado = 0.0;
      var a = 0;
      var b = 0
      a = parseFloat(this.costoPares);
      b = parseFloat(this.costoAccesorios);
      resultado = a + b;
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
    calcularFinalTotalImporte: function () {
      var resultado = 0.0;
      var a = 0;
      var b = 0
      a = parseFloat(this.importePares);
      b = parseFloat(this.importeAccesorios);
      resultado = a + b;
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = resultado.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      resultado =  arr[1] ? arr.join('.') : arr[0];
      return resultado;
    },
  },
  watch: {
    dialog(val) {
      val || this.close();
    },
  },
  created() {
    if (this.$store.state.usuario.rol == 'AUDITOR' || this.$store.state.usuario.rol == 'CONSIGNATARIO'){
      this.listarTiendas();
      this.seleccionarT();
    }
    else {
      this.listar();
      this.seleccionarT();
    }
  },
  methods: {
    subirExcel(){
      var articulosArray = [];
      const input = document.getElementById("archivoExcel");
      readXlsFile(input.files[0]).then((rows) => {
        this.articulosArray = rows
        for (var i = 1; i < this.articulosArray.length; i++)
        {
          this.detalles.push({
            pK_ARTICULO: String(this.articulosArray[i][0]),
            canal: this.articulosArray[i][1],
            talla: String(this.articulosArray[i][2]),
            cantidad: this.articulosArray[i][3],
            calidad: this.articulosArray[i][4],
            planes: this.articulosArray[i][5],
            horainicio: this.articulosArray[i][6],
            horafin: this.articulosArray[i][7],
          });
        }
      })
    },
    subirIngresar(){
      var productosArray = [];
      const input = document.getElementById("archivoExcel");
      this.excel = 1;
      readXlsFile(input.files[0]).then((rows) => {
        this.productosArray = rows
        for (var i = 1; i < this.productosArray.length; i++)
        {
          this.productos.push({
            pK_ARTICULO: String(this.productosArray[i][0]),
            precio: this.productosArray[i][3],
            talla: String(this.productosArray[i][1]),
            cantidad: this.productosArray[i][2],
          });
        }
      });
    },
    exportarDetalleExcel(){
      if (this.validar()) {
        return;
      }
      const items = this.diferencias;
      const data = XLSX.utils.json_to_sheet(this.detalles)
      const workbook = XLSX.utils.book_new()
      const filename = 'Lecturas estante'+this.estante
      XLSX.utils.book_append_sheet(workbook, data, filename)
      XLSX.writeFile(workbook, `${filename}.xlsx`)
      this.detalles = [];
    },
    exportarDiferenciaExcel(){
      const items = this.diferencias;
      items.forEach(o=>delete o.talla)
      items.forEach(o=>delete o.cantidad)
      const data = XLSX.utils.json_to_sheet(this.diferencias)
      const workbook = XLSX.utils.book_new()
      const filename = 'Planilla diferencias ' + this.tienda
      XLSX.utils.book_append_sheet(workbook, data, filename)
      XLSX.writeFile(workbook, `${filename}.xlsx`)
    },
    exportarRecapituladoExcel(){
      const items = this.recapitulado;
      items.forEach(o=>delete o.pK_ARTICULO)
      items.forEach(o=>delete o.categoria)
      const data = XLSX.utils.json_to_sheet(this.recapitulado)
      const workbook = XLSX.utils.book_new()
      const filename = 'Planilla recapitulado ' + this.tienda
      XLSX.utils.book_append_sheet(workbook, data, filename)
      XLSX.writeFile(workbook, `${filename}.xlsx`)
    },
    exportarLecturaExcel(){
      const data = XLSX.utils.json_to_sheet(this.lecturas)
      const workbook = XLSX.utils.book_new()
      const filename = 'Planilla lecturas ' + this.tienda
      XLSX.utils.book_append_sheet(workbook, data, filename)
      XLSX.writeFile(workbook, `${filename}.xlsx`)
    },
    exportarIngresarExcel(){
      const items = this.productos;
      items.forEach(o=>delete o.categoria)
      items.forEach(o=>delete o.existencias)
      items.forEach(o=>delete o.contados)
      items.forEach(o=>delete o.diferencias)
      items.forEach(o=>delete o.total)
      const data = XLSX.utils.json_to_sheet(this.productos)
      const workbook = XLSX.utils.book_new()
      const filename = 'Artículos'+ this.pk_categoria
      XLSX.utils.book_append_sheet(workbook, data, filename)
      XLSX.writeFile(workbook, `${filename}.xlsx`)
    },
    formatoMiles (number) {
      var valor = 0.0;
      const exp = /(\d)(?=(\d{3})+(?!\d))/g;
      const rep = '$1,';
      let arr = number.toString().split('.');
      arr[0] = arr[0].replace(exp, rep);
      valor = arr[1] ? arr.join('.') : arr[0];
      return valor;
    },
    crearImportado() {
      var quotes = document.getElementById("importado");
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
    crearStock() {
      var quotes = document.getElementById("stock");
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
          title: "Stock Final",
        });
        doc.addImage(imgData, "PNG", 0, position, imgWidth, imgHeight);
        window.open(doc.output("bloburl"));
      });
    },
    crearDiferencia() {
      this.fecha = new Date().toLocaleString();
      var doc = new jsPDF('p', 'pt', 'letter');
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("MANUFACTURA BOLIVIANA S.A.", 200, 35);
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("Diferencia de inventario", 225, 50);
      doc.setFontSize(10);
      doc.setFont(undefined, 'normal');
      doc.text("Tienda: " + this.tienda, 60, 65);
      doc.text("Correspondiente a la semana: " + this.semana, 150, 65);
      doc.text("Fecha: " + this.fecha, 340, 65);
      var rows = [];
      this.diferencias.map(function (x) {
        rows.push({ pK_ARTICULO:x.pK_ARTICULO,categoria:x.categoria,existencias:x.existencias,contados:x.contados,diferencias:x.diferencias,precio:x.precio,total:x.total });
      });
      doc.autoTable({
        columns: [
          { title: "Artículo", dataKey: "pK_ARTICULO" },
          { title: "Categoría", dataKey: "categoria" },
          { title: "Existencias", dataKey: "existencias" },
          { title: "Contados", dataKey: "contados" },
          { title: "Diferencias", dataKey: "diferencias" },
          { title: "Precio", dataKey: "precio" },
          { title: "Total", dataKey: "total" },
        ],
        body: rows,
        startY: 70,
        foot:[
          ['Total Pares: ' + this.totalPares,'  '],
          ['Total Accesorios: ' + this.totalAccesorios,'  '],
          ['Importe Pares: ' + this.totalDiferenciasPares,'  '],
          ['Importe Accesorios: ' + this.totalDiferenciasAccesorios,'  '],
          ['Total Importe: ' + this.totalDiferenciasImporte,'  '],
        ],
        margin: { horizontal: 10 },
        styles: { overflow: "linebreak" },
        bodyStyles: { valign: "top" },
        showHead: "everyPage",
        headStyles :{
          fillColor : [229, 57, 53],
        },
        footStyles :{
          fillColor : [255, 255, 255],
          textColor: [33, 33, 33],
          halign : 'left'
        },
        styles : { halign : 'center'},
        showFoot: "lastPage",
        didDrawPage: function (data) {
        let str = "" + doc.internal.getNumberOfPages();
        doc.setFontSize(10);
        let pageSize = doc.internal.pageSize;
        let pageHeight = pageSize.height
          ? pageSize.height
          : pageSize.getHeight();
        doc.text(550, 760, "Página: " + str);
      }
      })
        doc.setProperties({
        title: "Diferencias de Inventario",
      });
      window.open(doc.output('bloburl'));
    },
    crearRecapitulado() {
      this.fecha = new Date().toLocaleString();
      var doc = new jsPDF('p', 'pt','letter');
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("MANUFACTURA BOLIVIANA S.A.", 200, 35);
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("Planilla de inventario recapitulado", 197, 50);
      doc.setFontSize(10);
      doc.setFont(undefined, 'normal');
      doc.text("Tienda: " + this.tienda, 60, 65);
      doc.text("Correspondiente a la semana: " + this.semana, 150, 65);
      doc.text("Fecha: " + this.fecha, 340, 65);
      var rows = [];
      this.recapitulado.map(function (x) {
        rows.push({ estante:x.estante,cantidad:x.cantidad,precio:x.precio,usuario:x.usuario,inicio:x.inicio,fin:x.fin });
      });
      doc.autoTable({
        columns: [
          { title: "Estante", dataKey: "estante" },
          { title: "Total", dataKey: "cantidad" },
          { title: "Importe", dataKey: "precio" },
          { title: "Usuario", dataKey: "usuario" },
          { title: "Hora Inicio", dataKey: "inicio" },
          { title: "Hora Final", dataKey: "fin" }
        ],
        body: rows,
        startY: 70,
        foot:[
          ['Total Pares: ' + this.recapituladoPares,'  '],
          ['Total Accesorios: ' + this.recapituladoAccesorios,'  '],
          ['Importe Pares: ' + this.importeRecapituladoPares,'  '],
          ['Importe Accesorios: ' + this.importeRecapituladoAccesorios,'  '],
          ['Total Importe: ' + this.totalRecapituladoImporte,'  '],
        ],
        margin: { horizontal: 10 },
        styles: { overflow: "linebreak" },
        bodyStyles: { valign: "top" },
        showHead: "everyPage",
        headStyles :{
          fillColor : [229, 57, 53],
        },
        footStyles :{
          fillColor : [255, 255, 255],
          textColor: [33, 33, 33],
          halign : 'left'
        },
        styles : { halign : 'center'},
        showFoot: "lastPage",
        didDrawPage: function (data) {
        let str = "" + doc.internal.getNumberOfPages();
        doc.setFontSize(10);
        let pageSize = doc.internal.pageSize;
        let pageHeight = pageSize.height
          ? pageSize.height
          : pageSize.getHeight();
        doc.text(550, 760, "Página: " + str);
      }
      })
        doc.setProperties({
        title: "Planilla de inventario recapitulado",
      });
      window.open(doc.output('bloburl'));
    },
    crearLectura() {
      this.fecha = new Date().toLocaleString();
      var doc = new jsPDF('p', 'pt','letter');
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("MANUFACTURA BOLIVIANA S.A.", 200, 35);
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("Planilla de inventario lecturas", 197, 50);
      doc.setFontSize(10);
      doc.setFont(undefined, 'normal');
      doc.text("Tienda: " + this.tienda, 60, 65);
      doc.text("Correspondiente a la semana: " + this.semana, 150, 65);
      doc.text("Fecha: " + this.fecha, 340, 65);
      var rows = [];
      this.lecturas.map(function (x) {
        rows.push({ pK_ARTICULO:x.pK_ARTICULO,cantidad:x.cantidad,precio:x.precio,estante:x.estante,usuario:x.usuario,inicio:x.inicio,fin:x.fin,categoria:x.categoria });
      });
      doc.autoTable({
        columns: [
          { title: "Artículo", dataKey: "pK_ARTICULO" },
          { title: "Total", dataKey: "cantidad" },
          { title: "Importe", dataKey: "precio" },
          { title: "Estante", dataKey: "estante" },
          { title: "Usuario", dataKey: "usuario" },
          { title: "Hora Inicio", dataKey: "inicio" },
          { title: "Hora Final", dataKey: "fin" },
          { title: "Categoría", dataKey: "categoria" }
        ],
        body: rows,
        startY: 70,
        foot:[
          ['Total Pares: ' + this.recapituladoPares,'  '],
          ['Total Accesorios: ' + this.recapituladoAccesorios,'  '],
          ['Importe Pares: ' + this.importeRecapituladoPares,'  '],
          ['Importe Accesorios: ' + this.importeRecapituladoAccesorios,'  '],
          ['Total Importe: ' + this.totalRecapituladoImporte,'  '],
        ],
        margin: { horizontal: 10 },
        styles: { overflow: "linebreak" },
        bodyStyles: { valign: "top" },
        showHead: "everyPage",
        headStyles :{
          fillColor : [229, 57, 53],
        },
        footStyles :{
          fillColor : [255, 255, 255],
          textColor: [33, 33, 33],
          halign : 'left'
        },
        styles : { halign : 'center'},
        showFoot: "lastPage",
        didDrawPage: function (data) {
        let str = "" + doc.internal.getNumberOfPages();
        doc.setFontSize(10);
        let pageSize = doc.internal.pageSize;
        let pageHeight = pageSize.height
          ? pageSize.height
          : pageSize.getHeight();
        doc.text(550, 760, "Página: " + str);
      }
      })
        doc.setProperties({
        title: "Planilla de inventario lecturas",
      });
      window.open(doc.output('bloburl'));
    },
    crearInicial() {
      this.fecha = new Date().toLocaleString();
      var doc = new jsPDF('p', 'pt','letter');
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("MANUFACTURA BOLIVIANA S.A.", 200, 35);
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("Planilla de inventario inicial", 212, 50);
      doc.setFontSize(10);
      doc.setFont(undefined, 'normal');
      doc.text("Tienda: " + this.tienda, 60, 65);
      doc.text("Correspondiente a la semana: " + this.semana, 150, 65);
      doc.text("Fecha: " + this.fecha, 340, 65);
      var rows = [];
      this.iniciales.map(function (x) {
        rows.push({ pK_ARTICULO:x.pK_ARTICULO,total:x.total,taM1:x.taM1,taM2:x.taM2,taM3:x.taM3,taM4:x.taM4,taM5:x.taM5,taM6:x.taM6,taM7:x.taM7,taM8:x.taM8,taM9:x.taM9 });
      });
      doc.autoTable({
        columns: [
          { title: "Artículo", dataKey: "pK_ARTICULO" },
          { title: "Total", dataKey: "total" },
          { title: "N1", dataKey: "taM1" },
          { title: "N2", dataKey: "taM2"},
          { title: "N3", dataKey: "taM3" },
          { title: "N4", dataKey: "taM4" },
          { title: "N5", dataKey: "taM5" },
          { title: "N6", dataKey: "taM6" },
          { title: "N7", dataKey: "taM7" },
          { title: "N8", dataKey: "taM8" },
          { title: "N9", dataKey: "taM9" }
        ],
        body: rows,
        startY: 70,
        foot:[
          ['Total Pares: ' + this.inicialPares,'  '],
          ['Total Accesorios: ' + this.inicialAccesorios,'  '],
          ['Importe Pares: ' + this.importeInicialPares,'  '],
          ['Importe Accesorios: ' + this.importeInicialAccesorios,'  '],
          ['Total Importe: ' + this.totalInicialImporte,'  '],
        ],
        margin: { horizontal: 10 },
        styles: { overflow: "linebreak" },
        bodyStyles: { valign: "top"},
        showHead: "everyPage",
        headStyles :{
          fillColor : [229, 57, 53],
        },
        footStyles :{
          fillColor : [255, 255, 255],
          textColor: [33, 33, 33],
          halign : 'left'
        },
        styles : { halign : 'center'},
        showFoot: "lastPage",
        didDrawPage: function (data) {
        let str = "" + doc.internal.getNumberOfPages();
        doc.setFontSize(10);
        let pageSize = doc.internal.pageSize;
        let pageHeight = pageSize.height
          ? pageSize.height
          : pageSize.getHeight();
        doc.text(550, 760, "Página: " + str);
      }
      })
        doc.setProperties({
        title: "Planilla de inventario inicial",
      });
      window.open(doc.output('bloburl'));
    },
    crearFinal() {
      this.fecha = new Date().toLocaleString();
      var doc = new jsPDF('p', 'pt','letter');
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("MANUFACTURA BOLIVIANA S.A.", 200, 35);
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("Planilla de inventario final", 212, 50);
      doc.setFontSize(10);
      doc.setFont(undefined, 'normal');
      doc.text("Tienda: " + this.tienda, 60, 65);
      doc.text("Correspondiente a la semana: " + this.semana, 150, 65);
      doc.text("Fecha: " + this.fecha, 340, 65);
      var rows = [];
      this.finales.map(function (x) {
        rows.push({ pK_ARTICULO:x.pK_ARTICULO,total:x.total,taM1:x.taM1,taM2:x.taM2,taM3:x.taM3,taM4:x.taM4,taM5:x.taM5,taM6:x.taM6,taM7:x.taM7,taM8:x.taM8,taM9:x.taM9 });
      });
      doc.autoTable({
        columns: [
          { title: "Artículo", dataKey: "pK_ARTICULO" },
          { title: "Total", dataKey: "total" },
          { title: "N1", dataKey: "taM1"},
          { title: "N2", dataKey: "taM2"},
          { title: "N3", dataKey: "taM3" },
          { title: "N4", dataKey: "taM4" },
          { title: "N5", dataKey: "taM5" },
          { title: "N6", dataKey: "taM6" },
          { title: "N7", dataKey: "taM7" },
          { title: "N8", dataKey: "taM8" },
          { title: "N9", dataKey: "taM9" }
        ],
        body: rows,
        startY: 70,
        foot:[
          ['Total Pares: ' + this.finalPares,'  '],
          ['Total Accesorios: ' + this.finalAccesorios,'  '],
          ['Importe Pares: ' + this.importeFinalPares,'  '],
          ['Importe Accesorios: ' + this.importeFinalAccesorios,'  '],
          ['Total Importe: ' + this.totalFinalImporte,'  '],
        ],
        margin: { horizontal: 10 },
        styles: { overflow: "linebreak" },
        bodyStyles: { valign: "top"},
        showHead: "everyPage",
        headStyles :{
          fillColor : [229, 57, 53],
        },
        footStyles :{
          fillColor : [255, 255, 255],
          textColor: [33, 33, 33],
          halign : 'left'
        },
        styles : { halign : 'center'},
        showFoot: "lastPage",
        didDrawPage: function (data) {
        let str = "" + doc.internal.getNumberOfPages();
        doc.setFontSize(10);
        let pageSize = doc.internal.pageSize;
        let pageHeight = pageSize.height
          ? pageSize.height
          : pageSize.getHeight();
        doc.text(550, 760, "Página: " + str);
      }
      })
        doc.setProperties({
        title: "Planilla de inventario final",
      });
      window.open(doc.output('bloburl'));
    },
    crearManual() {
      this.fecha = new Date().toLocaleString();
      var doc = new jsPDF('p', 'pt','letter');
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("MANUFACTURA BOLIVIANA S.A.", 200, 35);
      doc.setFontSize(10);
      doc.setFont(undefined, 'bold');
      doc.text("Planilla de inventario manual", 212, 50);
      doc.setFontSize(10);
      doc.setFont(undefined, 'normal');
      doc.text("Tienda: " + this.tienda, 60, 65);
      doc.text("Correspondiente a la semana: " + this.semana, 150, 65);
      doc.text("Fecha: " + this.fecha, 340, 65);
      var rows = [];
      this.iniciales.map(function (x) {
        rows.push({ pK_ARTICULO:x.pK_ARTICULO,total:x.total,taM1:x.taM1+":.....",taM2:x.taM2+":.....",taM3:x.taM3+":.....",taM4:x.taM4+":.....",taM5:x.taM5+":.....",taM6:x.taM6+":.....",taM7:x.taM7+":.....",taM8:x.taM8+":.....",taM9:x.taM9+":....." });
      });
      doc.autoTable({
        columns: [
          { title: "Artículo", dataKey: "pK_ARTICULO" },
          { title: "Total", dataKey: "total" },
          { title: "N1", dataKey: "taM1" },
          { title: "N2", dataKey: "taM2"},
          { title: "N3", dataKey: "taM3" },
          { title: "N4", dataKey: "taM4" },
          { title: "N5", dataKey: "taM5" },
          { title: "N6", dataKey: "taM6" },
          { title: "N7", dataKey: "taM7" },
          { title: "N8", dataKey: "taM8" },
          { title: "N9", dataKey: "taM9" }
        ],
        body: rows,
        startY: 70,
        head:[
          ['Estante:..........','',14,15,16,17,18,19,20,21],
          ['Hora Inicio:..........','','','','','',22,23,24,25],
          ['Hora Fin:..........','',26,27,28,29,30,31,32,33],
          ['','',33,34,35,36,37,38,39,40],
          ["Artículo","Total",37,38,39,40,41,42,43,44],
        ],
        foot:[
          ['Total Pares: ' + this.inicialPares,'  '],
          ['Total Accesorios: ' + this.inicialAccesorios,'  '],
          ['Importe Pares: ' + this.importeInicialPares,'  '],
          ['Importe Accesorios: ' + this.importeInicialAccesorios,'  '],
          ['Total Importe: ' + this.totalInicialImporte,'  '],
        ],
        margin: { horizontal: 10 },
        styles: { overflow: "linebreak" },
        bodyStyles: { valign: "top"},
        showHead: "everyPage",
        headStyles :{
          fillColor : [255, 255, 255],
          textColor: [33, 33, 33],
          halign : 'center'
        },
        footStyles :{
          fillColor : [255, 255, 255],
          textColor: [33, 33, 33],
          halign : 'left'
        },
        styles : { halign : 'center'},
        showFoot: "lastPage",
      })

      const pageCount = doc.internal.getNumberOfPages();
      doc.setFontSize(10);

      for (var i = 1; i <= pageCount; i++) {
        doc.setPage(i);
        doc.text('Página ' + String(i) + ' de ' + String(pageCount), 525, 760);
      }

      doc.setProperties({
        title: "Planilla de inventario manual",
      });
      window.open(doc.output('bloburl'));
    },
    obtenerhora(){
      var hoy = new Date();
      this.hora = ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes();
    },
    uppercase() {
      this.estante = this.estante.toUpperCase();
      this.talla = this.talla.toUpperCase();
      this.editedItem.talla = this.editedItem.talla.toUpperCase();
      this.savedItem.talla = this.savedItem.talla.toUpperCase();
    },
    ocultarNuevo() {
      this.verNuevo = 0;
      this.limpiar();
    },
    buscarCodigo() {
      var hoy = new Date();
      let code = this.codigo.replace(/ /g, "");;
      const str =  code;
      this.errorArticulo = null;
      if (code != "") {
        if (code.length == 15){
        this.canalbar = str[0];  
        this.articulobar = str[1]+''+str[2]+''+str[3]+''+str[4]+''+str[5]+''+str[6]+''+str[7]+''+ 0;
        this.calidadbar = str[8];
        console.log(str[9]+''+str[10]);
        if (str[9]+''+str[10] == 11) {
            this.tallabar = '1'
          } else {
            this.tallabar = str[9]+''+str[10];
          }
        this.tallabar = str[9]+''+str[10];
        this.planbar = str[11]+''+str[12]+''+str[13]+''+str[14];
          if (!this.encuentra(this.articulobar, this.tallabar, this.planbar)) {
            this.detalles.unshift({
              pK_ARTICULO: this.articulobar,
              canal: this.canalbar,
              talla: this.tallabar,
              cantidad: 1,
              calidad: this.calidadbar,
              planes: this.planbar,
              horainicio: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes(),
              horafin: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes()
            });
            this.codigo = "";
            code = "";
          }
        } else if (code.length == 16){
        this.canalbar = str[0];  
        this.articulobar = str[1]+''+str[2]+''+str[3]+''+str[4]+''+str[5]+''+str[6]+''+str[7]+''+str[8];
        this.calidadbar = str[9];
          if (str[10]+''+str[11] == '11') {
            this.tallabar = '1'
          } else {
            this.tallabar = str[10]+''+str[11];
          }
        this.planbar = str[12]+''+str[13]+''+str[14]+''+str[15];
          if (!this.encuentra(this.articulobar, this.tallabar, this.planbar)) {
            this.detalles.unshift({
              pK_ARTICULO: this.articulobar,
              canal: this.canalbar,
              talla: this.tallabar,
              cantidad: 1,
              calidad: this.calidadbar,
              planes: this.planbar,
              horainicio: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes(),
              horafin: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes()
            });
            this.codigo = "";
            code = "";
          }
        } else if (code.length == 17){
        this.canalbar = str[0];  
        this.articulobar = str[1]+''+str[2]+''+str[3]+''+str[4]+''+str[5]+''+str[6]+''+str[7]+''+str[8];
        this.calidadbar = str[9];
        this.tallabar = str[10]+''+str[11]+''+str[12];
        this.planbar = str[13]+''+str[14]+''+str[15]+''+str[16];
          if (!this.encuentra(this.articulobar, this.tallabar, this.planbar)) {
            this.detalles.unshift({
              pK_ARTICULO: this.articulobar,
              canal: this.canalbar,
              talla: this.tallabar,
              cantidad: 1,
              calidad: this.calidadbar,
              planes: this.planbar,
              horainicio: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes(),
              horafin: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes()
            });
            this.codigo = "";
            code = "";
          }
        } else {
          this.errorArticulo = "El código de barras no es valido!";
          this.codigo = "";
          code = "";
        }
      }
    },
    encuentra(id, medida, proyecto) {
      var sw = 0;
      for (var i = 0; i < this.detalles.length; i++) {
        if (this.detalles[i].pK_ARTICULO == id && this.detalles[i].talla == medida && this.detalles[i].planes == proyecto)  {
            this.detalles[i].cantidad += 1;
            sw = 1;
        }
      }
      this.codigo = "";
      return sw;
    },
    encuentraTienda(id) {
      for (var i = 0; i < this.tiendas.length; i++) {
        if (this.tiendas[i].value == id) {
          this.nombre = this.tiendas[i].text2;
        }
      }
    },
    eliminarDetalle(arr, item) {
      var i = arr.indexOf(item);
      if (i !== -1) {
        arr.splice(i, 1);
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
    seleccionarC() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      me.categorias = [];
      var categoriaArray = [];
      axios
        .get("api/Categorias/Seleccionar/", configuracion)
        .then(function (response) {
          categoriaArray = response.data;
          categoriaArray.map(function (x) {
            me.categorias.push({
              text: x.descripcion,
              value: x.pK_CATEGORIA_SUP,
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
    seleccionarV(pk_inventario) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      me.numeros = [];
      var numeroArray = [];
      axios
        .get(
          "api/InventariosT/SeleccionarVer/" + pk_inventario,
          configuracion
        )
        .then(function (response) {
          numeroArray = response.data;
          numeroArray.map(function (x) {
            me.numeros.push({ text: x.codigo, value: x.pK_INVENTARIOT });
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
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get(
          "api/InventariosT/Listar/", configuracion
        )
        .then(function (response) {
          me.inventarios = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    guardar(){
      if (this.validar()) {
        return;
      }
      if (this.$refs.form.validate()) {
        this.waitModal = true;
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios.post("api/InventariosT/Crear",{
          pK_INVENTARIOT: me.pk_inventario,
          usuario: me.$store.state.usuario.usuario,
          estante: me.estante,
          detalles: me.detalles
        }, configuracion)
          .then(function(response) {
            if(response.data.length > 0) {
              me.excluidos = response.data;
              me.detalles = [];
              me.waitModal = false;
            } else{
              me.ocultarNuevo();
              me.limpiar();
              if (me.$store.state.usuario.rol != 'ADMINISTRADOR') {
                me.listarTiendas();
              }
              else {
                me.listar();
              }
            }
          })
          .catch(function(error) {
            console.log(error);
            me.waitModal = false;
          });
      }
    },
    ingresar() {
      if (this.ratificar()) {
        return;
      }
        var hoy = new Date();
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios.post("api/InventariosT/Agregar", {
          pK_INVENTARIOT: me.clave,
          usuario: me.$store.state.usuario.usuario,
          estante: me.estante,
          horainicio: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes(),
          horafin: ((hoy.getHours() < 10) ? "0" : "") + hoy.getHours() + ':' + ((hoy.getMinutes() < 10) ? "0" : "") + hoy.getMinutes(),
          productos: me.articulos
        }, configuracion)
          .then(function (response) {
            me.articulo = "";
            me.articulos = [];
            me.validaMensaje = [];
            me.adAceptar = true;
          })
          .catch(function (error) {
            console.log(error);
          });
    },
    registrar() {
      if (this.$store.state.usuario.rol == "CONSIGNATARIO") {
        if (this.comprobar()) {
          return;
        }
        if(this.excel == 0){
          this.exportarIngresarExcel();
        }
        this.waitModal = true;
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios.post("api/InventariosT/Registrar", {
          pK_INVENTARIOT: me.clave,
          usuario: me.$store.state.usuario.usuario,
          estante: me.pk_categoria,
          horainicio: me.hora1,
          horafin: me.hora2,
          productos: me.productos
        }, configuracion)
          .then(function (response) {
            me.waitModal = false;
            me.borrar();
            me.adAceptar = true;
          })
          .catch(function (error) {
            console.log(error);
            me.waitModal = false;
          });
      } else {
        if (this.verificar()) {
          return;
        }
        this.waitModal = true;
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios.post("api/InventariosT/Guardar", {
          pK_INVENTARIOT: me.clave,
          usuario: me.$store.state.usuario.usuario,
          estante: me.estante,
          horainicio: me.hora1,
          horafin: me.hora2,
          productos: me.productos
        }, configuracion)
          .then(function (response) {
            me.waitModal = false;
            me.borrar();
          })
          .catch(function (error) {
            console.log(error);
            me.waitModal = false;
          });
      }
    },
    actualizar(){
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      let me = this;
      axios.put("api/InventariosT/Actualizar", {
        pK_INVENTARIOT: me.clave,
        pK_ARTICULO: me.pk_articulo,
        canal: me.canal,
        talla: me.talla,
        cantidad: me.cantidad,
        calidad: me.calidad,
        planes: me.planes,
        estante: me.estante
      }, configuracion)
        .then(function (response) {
          me.cancelar();
          me.buscarEstante()
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    finalizar() {
      this.wait = 1;
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .put("api/InventariosT/Finalizar/" + this.adId + "/" + this.adPk, {}, configuracion)
        .then(function (response) {
          me.adFin = 0;
          me.adAccion = 0;
          me.adInventario = "";
          me.adId = "";
          me.adPk = "";
          me.wait = 0;
          if (me.$store.state.usuario.rol != 'ADMINISTRADOR') {
            me.listarTiendas();
          }
          else {
            me.listar();
          }
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    anular() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .put("api/InventariosT/Anular/" + this.adId + "/" + this.adPk, {}, configuracion)
        .then(function (response) {
          me.adAnular = 0;
          me.adAccion = 0;
          me.adInventario = "";
          me.adId = "";
          me.adPk = "";
          if (me.$store.state.usuario.rol != 'ADMINISTRADOR') {
            me.listarTiendas();
          }
          else {
            me.listar();
          }
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    buscarEstante() {
      if (this.validarEstante()) {
        return;
      }
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get(
          "api/InventariosT/Buscar/" + this.clave + "/" + this.estante , configuracion
        )
        .then(function (response) {
          me.estantes = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    buscarTalla() {
      if (this.validarTalla()) {
        return;
      }
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/ListarTalla/" + me.texto, configuracion)
        .then(function(response) {
          me.tallas = response.data;
        })
        .catch(function(error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    buscarProducto() {
      this.errorCategoria = null;
      this.waitModal = true;
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/ListarProducto/" + me.clave + "/" + me.semana + "/" + me.pk_categoria, configuracion)
        .then(function (response) {
          if (response.data.length > 0) {
            me.productos = response.data;
            me.verTabla = 2;
            me.waitModal = false;
          } else {
            me.waitModal = false;
            me.productos = [];
            me.errorCategoria = "La categoría ya se guardo o no cuenta con stock!";
          }
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
            me.waitModal = false;
          }
        });
    },
    buscarArticulo() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get(
          "api/InventariosT/ListarTamanos/" + this.articulo, configuracion
        )
        .then(function (response) {
          me.articulos = response.data;
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
        .get(
          "api/InventariosT/ListarTiendas/"+ parseInt(me.$store.state.usuario.pk_usuario), configuracion
        )
        .then(function (response) {
          me.inventarios = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    listarImportado(id, pk) {
    let me = this;
    let header = { Authorization: "Bearer " + this.$store.state.token };
    let configuracion = { headers: header };
    axios
      .get("api/ExistenciasT/ListarImportado/" + id + "/" + pk , configuracion)
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
    listarStock(id, pk) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/ListarStock/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.stocks = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    listarDiferencia(id, pk) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/ListarDiferencias/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.diferencias = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    listarRecapitulado(id, pk) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/ListarExistencias/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.recapitulado = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    listarLectura(id, pk) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/ListarLecturas/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.lecturas = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    listarInicial(id) {
      this.errorTalla = null;
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/PlanillaInicial/"+ id , configuracion)
        .then(function (response) {
          me.iniciales = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else if (error.response.status == 500) {
            me.errorTalla = "Tallas no validas!";
          } else {
            console.log(error);
          }
        });
    },
    listarFinal(id) {
      this.error = null;
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/PlanillaFinal/"+ id , configuracion)
        .then(function (response) {
          me.finales = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else if (error.response.status == 500) {
            me.errorTalla = "Tallas no validas!";
          } else {
            console.log(error);
          }
        });
    },
    listarManual(id) {
      this.errorTalla = null;
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/PlanillaManual/"+ id , configuracion)
        .then(function (response) {
          me.iniciales = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else if (error.response.status == 500) {
            me.errorTalla = "Tallas no validas!";
          } else {
            console.log(error);
          }
        });
    },
    obtenerCorrelativo(id, pk){
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/Correlativo/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.estante = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    sumoPares(id, pk) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/IngresadoPares/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.obtenerPares(response.data);
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    sumoAccesorios(id, pk) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/IngresadoAccesorios/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.obtenerAccesorios(response.data);
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    sumaPares(id, pk) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/TotalPares/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.mostrarPares(response.data);
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    sumaAccesorios(id, pk) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/InventariosT/TotalAccesorios/" + id + "/" + pk , configuracion)
        .then(function (response) {
          me.mostrarAccesorios(response.data);
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
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .put("api/InventariosT/Eliminar", {
          pK_INVENTARIOT: me.pk_inventario,
          pK_ARTICULO: me.pk_articulo,
          canal: me.canal,
          talla: me.talla,
          cantidad: me.cantidad,
          calidad: me.calidad,
          planes: me.planes,
          estante: me.estante
        }, configuracion)
        .then(function (response) {
          me.buscarEstante();
          me.eliminarCerrar();
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    eliminarModificar(id, pk){
      if (this.validarCategoria()) {
        return;
      }
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .delete("api/InventariosT/Suprimir/" + id + "/" + pk, configuracion)
        .then(function (response) {
         me.estantes = [];
         me.estante = "";
         me.adEliminar = true;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    finalizarMostrar(accion, item) {
      (this.adFin = 1), (this.adInventario = item.codigo);
      this.adId = item.pK_INVENTARIOT;
      this.adPk = this.$store.state.usuario.pk_usuario;
      if (accion == 3) {
        this.adAccion = 3;
      } else {
        this.adFin = 0;
      }
    },
    anularMostrar(accion, item) {
      (this.adAnular = 1), (this.adInventario = item.codigo);
      this.adId = item.pK_INVENTARIOT;
      this.adPk = this.$store.state.usuario.pk_usuario;
      if (accion == 4) {
        this.adAccion = 4;
      } else {
        this.adAnular = 0;
      }
    },
    finalizarCerrar() {
      this.adFin = 0;
    },
    anularCerrar() {
      this.adAnular = 0;
    },
    eliminarCerrar() {
      this.eliminarModal = 0;
    },
    aceptarCerrar() {
      this.adAceptar = false;
    },
    suprimirCerrar() {
      this.adEliminar = false;
    },
    cancelar() {
      this.modificarModal = false;
      this.pk_articulo = "";
      this.canal = "";
      this.talla = "";
      this.cantidad = 0;
      this.calidad = "";
      this.planes = "";
    },
    modificar(item) {
      this.canal = item.canal;
      this.pk_articulo = item.pK_ARTICULO;
      this.talla = item.talla;
      this.cantidad = item.cantidad;
      this.calidad = item.calidad;
      this.planes = item.planes;
      this.modificarModal= true;
    },
    verReporte() {
      switch (this.reporte){
        case "Stock Inicial":
        this.verImportado();
        break;
        case "Stock Final":
        this.verStock();
        break;
        case "Planilla Inicial":
        this.verInicial();
        break;
        case "Planilla Diferencias":
        this.verDiferencia();
        break;
        case "Planilla Recapitulado":
        this.verRecapitulado();
        break;
        case "Planilla Lecturas":
        this.verLectura();
        break;
        case "Planilla Final":
        this.verFinal();
        break;
      }
    },
    generarManual() {
      switch (this.manual){
        case "Planilla Inventario":
        this.errorCategoria = null;
        this.verLista = 0;
        this.productos = [];
        this.verPlanilla();
        break;
        case "Registrar Inventario":
        this.search = "";
        this.errorCategoria = null;
        this.verLista = 0;
        this.productos = [];
        this.verProducto();
        this.seleccionarC();
        break;
        case "Stock Inicial":
        this.errorCategoria = null;
        this.verLista = 0;
        this.verTabla = 0;
        this.verCampo = 0;
        this.productos = [];
        this.verImportado();
        break;
        case "Stock Final":
        this.errorCategoria = null;
        this.verLista = 0;
        this.verTabla = 0;
        this.verCampo = 0;
        this.productos = [];
        this.verStock();
        break;
        case "Planilla Inicial":
        this.search = "";
        this.errorCategoria = null;
        this.verTabla = 0;
        this.verCampo = 0;
        this.productos = [];
        this.verInicial();
        break;
        case "Planilla Diferencias":
        this.search = "";
        this.errorCategoria = null;
        this.verTabla = 0;
        this.verCampo = 0;
        this.productos = [];
        this.verDiferencia();
        break;
        case "Planilla Final":
        this.search = "";
        this.errorCategoria = null;
        this.verTabla = 0;
        this.verCampo = 0;
        this.productos = [];
        this.verFinal();
        break;
      }
    },
    generarHibrido() {
      switch (this.hibrido){
        case "Planilla Inventario":
        this.verPlanilla();
        break;
        case "Registrar Inventario":
        this.verProducto();
        this.seleccionarC();
        break;
      }
    },
    verAgregar(){
      this.agregarModal = true;
    },
    verProducto(){
      this.verCampo = 1;
      this.verTabla = 0;
      this.pk_categoria = "";
    },
    verDetalles(item) {
      this.verNuevo = 2;
      this.tienda = item.pK_TIENDA;
      this.semana = item.semana;
      this.correlativo = item.codigo;
      this.clave = item.pK_INVENTARIOT;
    },
    verModificar(item) {
      this.verNuevo = 3;
      this.search = "";
      this.tienda = item.pK_TIENDA;
      this.semana = item.semana;
      this.correlativo = item.codigo;
      this.clave = item.pK_INVENTARIOT;
    },
    verManual(item) {
      this.verNuevo = 4;
      this.search = "";
      this.tienda = item.pK_TIENDA;
      this.semana = item.semana;
      this.correlativo = item.codigo;
      this.clave = item.pK_INVENTARIOT;
      this.estado = item.estado;
    },
    verIngresar(item) {
      this.verNuevo = 5;
      this.tienda = item.pK_TIENDA;
      this.semana = item.semana;
      this.correlativo = item.codigo;
      this.clave = item.pK_INVENTARIOT;
    },
    verPlanilla(){
      this.verCampo = 0;
      this.verTabla = 1;
      this.listarManual(this.clave);
      this.sumoPares(this.clave, this.semana);
      this.sumoAccesorios(this.clave, this.semana);
    },
    verDiferencia(){
      this.search = "";
      this.verLista = 1;
      this.listarDiferencia(this.clave,this.semana);
    },
    verRecapitulado(){
      this.search = "";
      this.verLista = 2;
      this.listarRecapitulado(this.clave,this.semana);
      this.sumaPares(this.clave, this.semana);
      this.sumaAccesorios(this.clave, this.semana);
    },
    verLectura(){
      this.search = "";
      this.verLista = 3;
      this.listarLectura(this.clave,this.semana);
      this.sumaPares(this.clave, this.semana);
      this.sumaAccesorios(this.clave, this.semana);
    },
    verInicial(){
      this.search = "";
      this.verLista = 4;
      this.listarInicial(this.clave);
      this.sumoPares(this.clave, this.semana);
      this.sumoAccesorios(this.clave, this.semana);
    },
    verFinal(){
      this.search = "";
      this.verLista = 5;
      this.listarFinal(this.clave);
      this.sumaPares(this.clave, this.semana);
      this.sumaAccesorios(this.clave, this.semana);
    },
    verImportado(){
      this.importadoModal = 1;
      this.fechareporte = new Date().toLocaleString(),
      this.tiendareporte = this.tienda,
      this.codigoreporte = this.correlativo,
      this.semanareporte = this.semana,
      this.listarImportado(this.clave, this.semana);
    },
    verStock() {
      this.tiendareporte = this.tienda,
      this.fechareporte = new Date().toLocaleString(),
      this.codigoreporte = this.correlativo,
      this.semanareporte = this.semana,
      this.listarStock(this.clave, this.semana);
      this.stockModal = 1;
    },
    verInventario(item) {
      this.limpiar();
      this.verNuevo = 1;
      this.verInv = 1;
      this.pk_tienda = item.pK_TIENDA;
      this.pk_inventario = item.pK_INVENTARIOT;
      this.encuentraTienda(item.pK_TIENDA)
      this.seleccionarV(item.pK_INVENTARIOT);
      this.obtenerCorrelativo(item.pK_INVENTARIOT,this.$store.state.usuario.usuario)
    },
    ocultarDetalle() {
      this.verNuevo = 0;
      this.correlativo = "";
      this.semana = "";
      this.tienda = "";
      this.reporte = "";
      this.ocultarDiferencia();
      this.ocultarRecapitulado();
      this.ocultarLectura();
      this.ocultarInicial();
      this.ocultarFinal();
    },
    ocultarModificar() {
      this.verNuevo = 0;
      this.estante = "";
      this.search = "";
      this.estantes = [];
      this.valida = 0;
    },
    ocultarManual() {
      this.verNuevo = 0;
      this.verTabla = 0;
      this.verCampo = 0;
      this.verLista = 0;
      this.search = "";
      this.manual = "";
      this.hibrido = "";
      this.estante = "";
      this.hora1 = "";
      this.hora2 = "";
      this.pk_categoria = "";
      this.iniciales = [];
      this.productos = [];
      this.cantidadPares = 0;
      this.cantidadAccesorios = 0;
      this.importePares = 0;
      this.importeAccesorios = 0;
      this.errorCategoria = null;
    },
    ocultarImportado() {
      this.importadoModal = 0;
      this.fechareporte = "";
      this.tiendareporte = "";
      this.codigoreporte = "";
      this.semanareporte = "";
      this.importados = [];
    },
    ocultarStock() {
      this.stockModal = 0;
      this.fechareporte = "";
      this.tiendareporte = "";
      this.codigoreporte = "";
      this.semanareporte = "";
      this.stocks = [];
    },
    ocultarDiferencia() {
      this.verLista = 0;
      this.search = "";
      this.diferencias = [];
      this.totalDiferenciasPares = 0;
      this.totalDiferenciasAccesorios = 0;
      this.totalAccesorios = 0;
      this.totalPares = 0;
    },
    ocultarRecapitulado() {
      this.verLista = 0;
      this.search = "";
      this.recapitulado = [];
      this.cantidadPares = 0;
      this.cantidadAccesorios = 0;
      this.importePares = 0;
      this.importeAccesorios = 0;
    },
    ocultarLectura() {
      this.verLista = 0;
      this.search = "";
      this.lecturas = [];
      this.cantidadPares = 0;
      this.cantidadAccesorios = 0;
      this.importePares = 0;
      this.importeAccesorios = 0;
    },
    ocultarInicial() {
      this.verLista = 0;
      this.search = "";
      this.iniciales = [];
      this.cantidadPares = 0;
      this.cantidadAccesorios = 0;
      this.importePares = 0;
      this.importeAccesorios = 0;
    },
    ocultarFinal() {
      this.verLista = 0;
      this.search = "";
      this.finales = [];
      this.cantidadPares = 0;
      this.cantidadAccesorios = 0;
      this.importePares = 0;
      this.importeAccesorios = 0;
    },
    ocultarPlanilla() {
      this.verTabla = 0;
      this.manual = "";
      this.iniciales = [];
      this.cantidadPares = 0;
      this.cantidadAccesorios = 0;
      this.importePares = 0;
      this.importeAccesorios = 0;
    },
    ocultarRegistrar(){
      this.verTabla = 0;
      this.verCampo = 0;
      this.search = "";
      this.manual = "";
      this.estante = "";
      this.hora1 = "";
      this.hora2 = "";
      this.pk_categoria = "";
      this.errorCategoria = null;
      this.productos = [];
    },
    ocultarIngresar(){
      this.verNuevo = 0;
      this.tienda = "";
      this.semana = "";
      this.correlativo = "";
      this.clave = "";
      this.articulo = "";
      this.estante = "";
      this.articulos = [];
      this.validaMensaje = [];
    },
    ocultarMensaje(){
      this.waitModal = false;
    },
    mostrarEliminar(item)
    {
      this.eliminarModal = true;
      this.pk_inventario = item.pK_INVENTARIOT;
      this.pk_articulo = item.pK_ARTICULO;
      this.canal = item.canal;
      this.talla = item.talla;
      this.cantidad = item.cantidad;
      this.calidad = item.calidad;
      this.planes = item.planes;
    },
    obtenerPares(data = []){
      for (var i = 0; i < data.length; i++) {
          this.numeroPares = data[i].cantidad,
          this.costoPares = data[i].total
      }
    },
    obtenerAccesorios(data = []){
      for (var i = 0; i < data.length; i++) {
          this.numeroAccesorios = data[i].cantidad,
          this.costoAccesorios = data[i].total
      }
    },
    mostrarPares(data = []){
      for (var i = 0; i < data.length; i++) {
          this.cantidadPares = data[i].cantidad,
          this.importePares = data[i].total
      }
    },
    mostrarAccesorios(data = []){
      for (var i = 0; i < data.length; i++) {
          this.cantidadAccesorios = data[i].cantidad,
          this.importeAccesorios = data[i].total
      }
    },
    mostrarTallas() {
      this.verTallas = 1;
    },
    ocultarTallas() {
      this.verTallas = 0;
      this.texto = "";
      this.tallas = [];
      this.validaTalla = "";
      this.valida = 0;
    },
    verificar() {
      this.valida = 0;
      this.validaMensaje = [];
      if (this.productos.length <= 0) {
        this.validaMensaje.push("Ingrese al menos un artículo al detalle!");
      }
      if (!this.hora1) {
        this.validaMensaje.push("Ingrese una hora de inicio!");
      }
      if (!this.hora2) {
        this.validaMensaje.push("Ingrese una hora de final!");
      }
      if (!this.estante) {
        this.validaMensaje.push("Ingrese un número de estante");
      }
      if (this.validaMensaje.length) {
        this.valida = 1;
      }

      return this.valida;
    },
    comprobar() {
      this.valida = 0;
      this.validaMensaje = [];
        if (this.productos.length <= 0) {
          this.validaMensaje.push("Ingrese al menos un artículo al detalle!");
        }
        if (!this.hora1) {
          this.validaMensaje.push("Ingrese una hora de inicio!");
        }
        if (!this.hora2) {
          this.validaMensaje.push("Ingrese una hora de final!");
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
      if (!this.pk_tienda) {
        this.validaMensaje.push("Seleccione una Tienda!");
      }
      if (!this.pk_inventario) {
        this.validaMensaje.push("Seleccione un inventario!");
      }
      if (!this.estante) {
        this.validaMensaje.push("Ingrese un número de estante");
      }
      if (this.validaMensaje.length) {
        this.valida = 1;
      }
      return this.valida;
    },
    ratificar() {
      this.valida = 0;
      this.validaMensaje = [];
        if (this.articulos.length <= 0) {
          this.validaMensaje.push("Ingrese al menos un artículo al detalle!");
        }
        if (!this.estante) {
        this.validaMensaje.push("Ingrese un número de estante o una categoría");
      }
        if (this.validaMensaje.length) {
        this.valida = 1;
      }

      return this.valida;
    },
    validarEstante() {
      this.valida = 0;
      this.validaEstante = "";
      if (!this.estante) {
        this.validaEstante = ("Ingrese un número de estante o una categoría");
      }
      if (this.validaEstante.length) {
        this.valida = 1;
      }
      return this.valida;
    },
    validarTalla() {
      this.valida = 0;
      this.validaTalla = "";
      if (!this.texto) {
        this.validaTalla = ("Ingrese un código de artículo");
      }
      if (this.validaTalla.length) {
        this.valida = 1;
      }
      return this.valida;
    },
    validarCategoria() {
      this.valida = 0;
      this.validaEstante = "";
      if (this.estantes.length <= 0) {
        this.validaEstante = ("Sin artículos el detalle!");
      }
      if (this.validaEstante.length) {
        this.valida = 1;
      }
      return this.valida;
    },
    borrar(){
      this.texto ="";
      this.pk_categoria = "";
      this.productos = [];
      this.hora1 = ""; 
      this.hora2 = "";
      this.excel = 0;
      const input = document.getElementById("archivoExcel");
      input.value = "";
    },
    limpiar() {
      this.pk_tienda = "";
      this.pk_inventario = "";
      this.nombre = "";
      this.codigo = "";
      this.estante = "";
      this.cantidad = 1;
      this.detalles = [];
      this.numeros = [];
      this.excluidos = [];
      this.verDetalle = 0;
      this.valida = 0;
      this.verInv = 0;
      this.validaMensaje = [];
      this.errorArticulo = null;
      this.enableDisable = false;
      this.waitModal = false;
    },
    editItem(item) {
      this.editedIndex = this.detalles.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialog = true
    },
    close() {
      this.dialog = false
      setTimeout(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      }, 300)
    },
    save() {
      if (this.detalles[this.editedIndex].talla == this.editedItem.talla) {
        Object.assign(this.detalles[this.editedIndex], this.editedItem)
      } else {
        this.detalles.push(this.editedItem)
      }
      this.close()
    },
    add () {
        this.productos.unshift(this.savedItem);
        this.cancel()
    },
    cancel () {
      this.agregarModal = false;
      this.savedItem = Object.assign({}, this.starItem)
    }
  },
};
</script>
<style>

#importado {
  padding: 20px;
  font-family: Arial, sans-serif;
  font-size: 16px;
}

#stock {
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

#firmas {
text-align: center;
}
</style>
