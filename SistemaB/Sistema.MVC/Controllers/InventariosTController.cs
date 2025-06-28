using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Sistema.MVC.Models.DetallesInventarioT;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net.Mail;

namespace Sistema.MVC.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,AUDITOR,CONSIGNATARIO")]
    [Route("api/[controller]")]
    [ApiController]

    public class InventariosTController : Controller
    {
        public readonly string _connectionString;

        public InventariosTController(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("Conexion");
        }

        [HttpGet("[action]")]
        public IEnumerable<Models.InventariosT.ListarModel> Listar()
        {
            try
            {
                List<Models.InventariosT.ListarModel> lista = new List<Models.InventariosT.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT InventariosT.PK_INVENTARIOT, InventariosT.PK_TIENDA, InventariosT.CODIGO, InventariosT.SEMANA, InventariosT.FECHAINICIO, InventariosT.ESTADO, Usuarios.USUARIO, Roles.ROL FROM InventariosT INNER JOIN Usuarios ON InventariosT.PK_USUARIO = Usuarios.PK_USUARIO INNER JOIN Roles ON Usuarios.PK_ROL = Roles.PK_ROL WHERE InventariosT.ESTADO <> 'FINALIZADO' AND InventariosT.ESTADO <> 'ANULADO' ORDER BY InventariosT.PK_INVENTARIOT DESC", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.InventariosT.ListarModel inventario = new Models.InventariosT.ListarModel();
                                DateTime inicio = Convert.ToDateTime(reader["FECHAINICIO"]);
                                inventario.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                inventario.PK_TIENDA = Convert.ToInt32(reader["PK_TIENDA"]);
                                inventario.CODIGO = reader["CODIGO"].ToString();
                                inventario.SEMANA = reader["SEMANA"].ToString();
                                inventario.FECHAINICIO = inicio.ToString("dd'/'MM'/'yyyy");
                                inventario.ESTADO = reader["ESTADO"].ToString();
                                inventario.USUARIO = reader["USUARIO"].ToString();
                                inventario.ROL = reader["ROL"].ToString();
                                lista.Add(inventario);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<Models.InventariosT.ListarModel> ListarTiendas([FromRoute] int id)
        {
            try
            {
                List<Models.InventariosT.ListarModel> lista = new List<Models.InventariosT.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT InventariosT.PK_INVENTARIOT, InventariosT.PK_TIENDA, InventariosT.CODIGO, InventariosT.SEMANA, InventariosT.FECHAINICIO, InventariosT.ESTADO, Usuarios.USUARIO, Roles.ROL FROM InventariosT INNER JOIN Usuarios ON InventariosT.PK_USUARIO = Usuarios.PK_USUARIO INNER JOIN Roles ON Usuarios.PK_ROL = Roles.PK_ROL WHERE (InventariosT.ESTADO <> 'FINALIZADO' AND InventariosT.ESTADO <> 'ANULADO') AND InventariosT.PK_USUARIO = '" + id + "'", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.InventariosT.ListarModel inventario = new Models.InventariosT.ListarModel();
                                DateTime inicio = Convert.ToDateTime(reader["FECHAINICIO"]);
                                inventario.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                inventario.PK_TIENDA = Convert.ToInt32(reader["PK_TIENDA"]);
                                inventario.CODIGO = reader["CODIGO"].ToString();
                                inventario.SEMANA = reader["SEMANA"].ToString();
                                inventario.FECHAINICIO = inicio.ToString("dd'/'MM'/'yyyy");
                                inventario.ESTADO = reader["ESTADO"].ToString();
                                inventario.USUARIO = reader["USUARIO"].ToString();
                                inventario.ROL = reader["ROL"].ToString();
                                lista.Add(inventario);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Models.InventariosT.ListarModel> Catalogar()
        {
            try
            {
                List<Models.InventariosT.ListarModel> lista = new List<Models.InventariosT.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT InventariosT.PK_INVENTARIOT, InventariosT.PK_TIENDA, InventariosT.CODIGO, InventariosT.SEMANA, InventariosT.FECHAINICIO, InventariosT.FECHAFIN, InventariosT.ESTADO, Usuarios.USUARIO, Roles.ROL FROM InventariosT INNER JOIN Usuarios ON InventariosT.PK_USUARIO = Usuarios.PK_USUARIO INNER JOIN Roles ON Usuarios.PK_ROL = Roles.PK_ROL WHERE InventariosT.ESTADO = 'FINALIZADO' OR InventariosT.ESTADO = 'ANULADO' ORDER BY InventariosT.PK_INVENTARIOT DESC", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.InventariosT.ListarModel inventario = new Models.InventariosT.ListarModel();
                                DateTime inicio = Convert.ToDateTime(reader["FECHAINICIO"]);
                                DateTime fin = Convert.ToDateTime(reader["FECHAFIN"]);
                                inventario.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                inventario.PK_TIENDA = Convert.ToInt32(reader["PK_TIENDA"]);
                                inventario.CODIGO = reader["CODIGO"].ToString();
                                inventario.SEMANA = reader["SEMANA"].ToString();
                                inventario.FECHAINICIO = inicio.ToString("dd'/'MM'/'yyyy");
                                inventario.FECHAFIN = fin.ToString("dd'/'MM'/'yyyy");
                                inventario.ESTADO = reader["ESTADO"].ToString();
                                inventario.USUARIO = reader["USUARIO"].ToString();
                                inventario.ROL = reader["ROL"].ToString();
                                lista.Add(inventario);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<Models.InventariosT.ListarModel> CatalogarTiendas([FromRoute] int id)
        {
            try
            {
                List<Models.InventariosT.ListarModel> lista = new List<Models.InventariosT.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT InventariosT.PK_INVENTARIOT, InventariosT.PK_TIENDA, InventariosT.CODIGO, InventariosT.SEMANA, InventariosT.FECHAINICIO, InventariosT.FECHAFIN, InventariosT.ESTADO, Usuarios.USUARIO, Roles.ROL FROM InventariosT INNER JOIN Usuarios ON InventariosT.PK_USUARIO = Usuarios.PK_USUARIO INNER JOIN Roles ON Usuarios.PK_ROL = Roles.PK_ROL WHERE (InventariosT.ESTADO = 'FINALIZADO' OR InventariosT.ESTADO = 'ANULADO') AND InventariosT.PK_USUARIO = '" + id + "'", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime hoy = DateTime.Now;
                                DateTime inicio = Convert.ToDateTime(reader["FECHAINICIO"]);
                                DateTime fin = Convert.ToDateTime(reader["FECHAFIN"]);
                                TimeSpan difFechas = hoy - fin;
                                int dias = difFechas.Days;
                                if (dias < 30)
                                {
                                    Models.InventariosT.ListarModel inventario = new Models.InventariosT.ListarModel();
                                    inventario.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                    inventario.PK_TIENDA = Convert.ToInt32(reader["PK_TIENDA"]);
                                    inventario.CODIGO = reader["CODIGO"].ToString();
                                    inventario.SEMANA = reader["SEMANA"].ToString();
                                    inventario.FECHAINICIO = inicio.ToString("dd'/'MM'/'yyyy");
                                    inventario.FECHAFIN = fin.ToString("dd'/'MM'/'yyyy");
                                    inventario.ESTADO = reader["ESTADO"].ToString();
                                    inventario.USUARIO = reader["USUARIO"].ToString();
                                    inventario.ROL = reader["ROL"].ToString();
                                    lista.Add(inventario);
                                }
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.ModificarModel> Buscar([FromRoute] int id, string pk)
        {
            try
            {
                List<Models.DetallesInventarioT.ModificarModel> lista = new List<Models.DetallesInventarioT.ModificarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var command = new OracleCommand(@"SELECT PK_INVENTARIOT, PK_ARTICULO, CANAL, TALLA, CANTIDAD, CALIDAD, PLANES FROM DetalleInventariosT WHERE PK_INVENTARIOT = '" + id + "' AND ESTANTE = '" + pk + "'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.ModificarModel articulo = new Models.DetallesInventarioT.ModificarModel();
                                articulo.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                articulo.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                articulo.CANAL = reader["CANAL"].ToString();
                                articulo.TALLA = reader["TALLA"].ToString();
                                articulo.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"].ToString());
                                articulo.CALIDAD = reader["CALIDAD"].ToString();
                                articulo.PLANES = reader["PLANES"].ToString();
                                lista.Add(articulo);
                            }
                        }
                    }
                    connection.Close();
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("[action]")]
        public IActionResult Crear([FromBody] Models.InventariosT.RegistrarModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                List<Models.DetallesInventarioT.ListarModel> excluidos = new List<Models.DetallesInventarioT.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    foreach (var lista in model.detalles)
                    {
                        if (Validar(lista.PK_ARTICULO, lista.TALLA))
                        {
                            using (var command = new OracleCommand(@"SELECT COUNT(*) FROM InventariosT WHERE PK_INVENTARIOT = '" + model.PK_INVENTARIOT + "'", connection))
                            {
                                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                                {
                                    command.CommandText = @"INSERT INTO DetalleInventariosT (PK_INVENTARIOT, PK_ARTICULO, CANAL, TALLA, CANTIDAD, CALIDAD, PLANES, HORAINICIO, HORAFIN, ESTANTE, USUARIO) VALUES ('" + model.PK_INVENTARIOT + "', '" + lista.PK_ARTICULO + "', '" + lista.CANAL + "','" + lista.TALLA + "', '" + Convert.ToInt32(lista.CANTIDAD) + "', '" + lista.CALIDAD + "', '" + lista.PLANES + "', '" + lista.HORAINICIO + "', '" + lista.HORAFIN + "', '" + model.ESTANTE + "', '" + model.USUARIO + "')";
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            Models.DetallesInventarioT.ListarModel lectura = new Models.DetallesInventarioT.ListarModel();
                            lectura.PK_ARTICULO = lista.PK_ARTICULO;
                            lectura.TALLA = lista.TALLA;
                            lectura.CONTADOS = lista.CANTIDAD.ToString();
                            excluidos.Add(lectura);
                        }
                    }
                    connection.Close();
                }
                return Ok(excluidos);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpPost("[action]")]
        public IActionResult Registrar([FromBody] Models.InventariosT.ManualModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string categoria = "";
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM InventariosT WHERE PK_INVENTARIOT = '" + model.PK_INVENTARIOT + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT DESCRIPCION FROM CategoriaSuperior WHERE PK_CATEGORIA_SUP ='" + model.ESTANTE + "'";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    categoria = reader["DESCRIPCION"].ToString();
                                }

                            }

                            foreach (var lista in model.productos)
                            {
                                if (lista.CANTIDAD != "")
                                {
                                    command.CommandText = @"INSERT INTO DetalleInventariosT (PK_INVENTARIOT, PK_ARTICULO, CANAL, TALLA, CANTIDAD, CALIDAD, PLANES, HORAINICIO, HORAFIN, ESTANTE, USUARIO) VALUES ('" + model.PK_INVENTARIOT + "', '" + lista.PK_ARTICULO + "', '" + "1" + "','" + lista.TALLA + "', '" + Convert.ToInt32(lista.CANTIDAD) + "', '" + "1" + "', '" + "1234" + "', '" + model.HORAINICIO + "', '" + model.HORAFIN + "', '" + categoria + "', '" + model.USUARIO + "')";
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("[action]")]
        public IActionResult Guardar([FromBody] Models.InventariosT.ManualModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM InventariosT WHERE PK_INVENTARIOT = '" + model.PK_INVENTARIOT + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            foreach (var lista in model.productos)
                            {
                                if (lista.CANTIDAD != "")
                                {
                                    command.CommandText = @"INSERT INTO DetalleInventariosT (PK_INVENTARIOT, PK_ARTICULO, CANAL, TALLA, CANTIDAD, CALIDAD, PLANES, HORAINICIO, HORAFIN, ESTANTE, USUARIO) VALUES ('" + model.PK_INVENTARIOT + "', '" + lista.PK_ARTICULO + "', '" + "1" + "','" + lista.TALLA + "', '" + Convert.ToInt32(lista.CANTIDAD) + "', '" + "1" + "', '" + "1234" + "', '" + model.HORAINICIO + "', '" + model.HORAFIN + "', '" + model.ESTANTE + "', '" + model.USUARIO + "')";
                                    command.ExecuteNonQuery();
                                }
                            }

                        }

                    }
                    connection.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("[action]")]
        public IActionResult Agregar([FromBody] Models.InventariosT.ManualModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM InventariosT WHERE PK_INVENTARIOT = '" + model.PK_INVENTARIOT + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            foreach (var lista in model.productos)
                            {
                                if (lista.CANTIDAD != "")
                                {
                                    command.CommandText = @"INSERT INTO DetalleInventariosT (PK_INVENTARIOT, PK_ARTICULO, CANAL, TALLA, CANTIDAD, CALIDAD, PLANES, HORAINICIO, HORAFIN, ESTANTE, USUARIO) VALUES ('" + model.PK_INVENTARIOT + "', '" + lista.PK_ARTICULO + "', '" + "1" + "','" + lista.TALLA + "', '" + Convert.ToInt32(lista.CANTIDAD) + "', '" + "1" + "', '" + "1234" + "', '" + model.HORAINICIO + "', '" + model.HORAFIN + "', '" + model.ESTANTE + "', '" + model.USUARIO + "')";
                                    command.ExecuteNonQuery();
                                }
                            }

                        }

                    }
                    connection.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("[action]")]
        public IActionResult Actualizar([FromBody] Models.DetallesInventarioT.ModificarModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"UPDATE DetalleInventariosT SET CANAL = '" + model.CANAL + "', TALLA = '" + model.TALLA + "', CANTIDAD = '" + Convert.ToInt32(model.CANTIDAD) + "', CALIDAD = '" + model.CALIDAD + "', PLANES = '" + model.PLANES + "' WHERE PK_INVENTARIOT  = '" + model.PK_INVENTARIOT + "' AND ESTANTE ='" + model.ESTANTE + "' AND PK_ARTICULO = '" + model.PK_ARTICULO + "' AND TALLA = '" + model.TALLA + "'", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("[action]")]
        public IActionResult Eliminar([FromBody] Models.DetallesInventarioT.ModificarModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"DELETE FROM DetalleInventariosT WHERE PK_INVENTARIOT = '" + model.PK_INVENTARIOT + "' AND PK_ARTICULO = '" + model.PK_ARTICULO + "' AND ESTANTE = '" + model.ESTANTE + "' AND TALLA = '" + model.TALLA + "' AND PLANES = '" + model.PLANES + "'", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("[action]/{id}/{pk}")]
        public IActionResult Suprimir([FromRoute] int id, string pk)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM DetalleInventariosT WHERE PK_INVENTARIOT = '" + id + "' AND ESTANTE = '" + pk + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"DELETE FROM DetalleInventariosT WHERE PK_INVENTARIOT = '" + id + "' AND ESTANTE = '" + pk + "'";
                            command.ExecuteNonQuery();
                        }
                    }
                    connection.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IActionResult Correlativo(int id, string pk)
        {
            try
            {
                var estante = "";
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM DetalleInventariosT WHERE PK_INVENTARIOT = '" + id + "' AND USUARIO = '" + pk + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT ESTANTE FROM (SELECT ESTANTE FROM DETALLEINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "' AND USUARIO = '" + pk + "' ORDER BY TO_NUMBER(SUBSTR(ESTANTE,2,4)) DESC) WHERE ROWNUM = 1";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    estante = reader["ESTANTE"].ToString();
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                return Ok(estante);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        [HttpGet("[action]/{codigo}")]
        public IEnumerable<Models.InventariosT.ListarModel> SeleccionarVer(string codigo)
        {
            try
            {
                List<Models.InventariosT.ListarModel> lista = new List<Models.InventariosT.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT PK_INVENTARIOT, CODIGO FROM InventariosT WHERE (ESTADO <> 'FINALIZADO' AND ESTADO <> 'ANULADO') AND PK_INVENTARIOT = '" + codigo + "'", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.InventariosT.ListarModel inventario = new Models.InventariosT.ListarModel();
                                inventario.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                inventario.CODIGO = reader["CODIGO"].ToString();
                                lista.Add(inventario);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<Models.DetallesInventarioT.MostrarModel> ListarArticulos([FromRoute] int id)
        {
            try
            {
                List<Models.DetallesInventarioT.MostrarModel> lista = new List<Models.DetallesInventarioT.MostrarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT PK_ARTICULO, TALLA FROM ExistenciasT WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.MostrarModel articulo = new Models.DetallesInventarioT.MostrarModel();
                                articulo.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                articulo.TALLA = reader["TALLA"].ToString();
                                lista.Add(articulo);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("[action]/{id}/{pk}")]
        public IActionResult Finalizar([FromRoute] int id, int pk)
        {
            if (id <= 0 && pk <= 0)
            {
                return BadRequest();
            }

            try
            {
                int inventario = 0;
                int semana = 0;
                int tienda = 0;
                string fecha = DateTime.Now.ToString("yyyy-MM-dd");
                string hora = DateTime.Now.ToString("HH:mm");
                string estado = "FINALIZADO";
                int usuario = Convert.ToInt32(pk);
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM DETALLEINVENTARIOST WHERE PK_INVENTARIOT = '"+ id +"'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT PK_INVENTARIOT, SEMANA, PK_TIENDA FROM INVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    inventario = Convert.ToInt32(reader["PK_INVENTARIOT"].ToString());
                                    semana = Convert.ToInt32(reader["SEMANA"].ToString());
                                    tienda = Convert.ToInt32(reader["PK_TIENDA"].ToString());
                                }
                            }

                            if(Copiar(inventario, semana))
                            {
                                Generar(inventario, semana, tienda);
                                command.CommandText = @"UPDATE InventariosT SET FECHAFIN = '" + fecha + "', HORAFIN = '" + hora + "', ESTADO = '" + estado + "', PK_USUARIO = '" + usuario + "' WHERE PK_INVENTARIOT = '" + id + "'";
                                command.ExecuteNonQuery();
                            }

                        }
                    }
                    connection.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("[action]/{id}/{pk}")]
        public IActionResult Anular([FromRoute] int id, int pk)
        {
            if (id <= 0 && pk <= 0)
            {
                return BadRequest();
            }

            try
            {
                string fecha = DateTime.Now.ToString("yyyy-MM-dd");
                string estado = "ANULADO";
                int usuario = Convert.ToInt32(pk);

                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"UPDATE InventariosT SET  FECHAFIN = '" + fecha + "', ESTADO = '" + estado + "', PK_USUARIO = '" + usuario + "' WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("[action]/{texto}")]
        public IEnumerable<Models.DetallesInventarioT.ListarModel> ListarTamanos([FromRoute] string texto)
        {
            try
            {
                List<Models.DetallesInventarioT.ListarModel> articulos = new List<Models.DetallesInventarioT.ListarModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT PK_ARTICULO, TALLA, Tallas.DESCRIPCION AS NUMERACION, Articulos.DESCRIPCION AS DESCRIPCION FROM Tallas, Articulos WHERE OPCION = OPCION_TALLA AND SUBSTR(PK_ARTICULO,1,1) = GRUPO AND PK_ARTICULO = '" + texto + "'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.ListarModel tamanos = new Models.DetallesInventarioT.ListarModel();
                                tamanos.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                tamanos.TALLA = reader["TALLA"].ToString();
                                tamanos.CANTIDAD = "0";
                                articulos.Add(tamanos);
                            }
                        }
                        connection.Close();
                    }
                    return articulos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}/{texto}")]
        public IEnumerable<Models.DetallesInventarioT.ListarModel> ListarProducto([FromRoute] int id, string pk, string texto)
        {
            string categoria = "";

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (OracleCommand command = new OracleCommand(@"SELECT DESCRIPCION FROM CategoriaSuperior WHERE PK_CATEGORIA_SUP = '" + texto + "'", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categoria = reader["DESCRIPCION"].ToString();
                        }
                    }
                }
                connection.Close();
            }

            try
            {
                List<Models.DetallesInventarioT.ListarModel> lista = new List<Models.DetallesInventarioT.ListarModel>();
                List<Models.DetallesInventarioT.ListarModel> articulos = new List<Models.DetallesInventarioT.ListarModel>();
                List<Models.DetallesInventarioT.ListarModel> tallas = new List<Models.DetallesInventarioT.ListarModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM DetalleInventariosT WHERE PK_INVENTARIOT = '" + id + "' AND ESTANTE = '" + categoria + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                        {
                            command.CommandText = @"SELECT ExistenciasT.PK_ARTICULO, SUM(ExistenciasT.CANTIDAD) AS CANTIDAD, ExistenciasT.TALLA, PrecioSemana.VENTA FROM  Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                        WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND CategoriaSuperior.PK_CATEGORIA_SUP = '" + texto + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY ExistenciasT.PK_ARTICULO, ExistenciasT.TALLA, PrecioSemana.VENTA";

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.ListarModel existencias = new Models.DetallesInventarioT.ListarModel();
                                    existencias.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                    existencias.PRECIO = Convert.ToDecimal(reader["VENTA"]);
                                    existencias.CANTIDAD = reader["CANTIDAD"].ToString();
                                    existencias.TALLA = reader["TALLA"].ToString();
                                    articulos.Add(existencias);
                                }
                            }

                            foreach (var a in articulos)
                            {
                                command.CommandText = @"SELECT PK_ARTICULO, TALLA, Tallas.DESCRIPCION AS NUMERACION, Articulos.DESCRIPCION AS DESCRIPCION FROM Tallas, Articulos WHERE OPCION = OPCION_TALLA AND SUBSTR(PK_ARTICULO,1,1) = GRUPO AND PK_ARTICULO = '" + a.PK_ARTICULO + "'";
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Models.DetallesInventarioT.ListarModel tamanos = new Models.DetallesInventarioT.ListarModel();
                                        tamanos.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                        tamanos.PRECIO = a.PRECIO; ;
                                        tamanos.TALLA = reader["TALLA"].ToString();
                                        tamanos.CANTIDAD = "0";
                                        tallas.Add(tamanos);
                                    }
                                }
                            }
                            connection.Close();
                        }
                    }

                    var unionList = articulos.Union(tallas, new ListComparer());
                    foreach (var item in unionList)
                    {
                        Models.DetallesInventarioT.ListarModel union = new Models.DetallesInventarioT.ListarModel();
                        union.PK_ARTICULO = item.PK_ARTICULO;
                        union.PRECIO = item.PRECIO;
                        union.TALLA = item.TALLA;
                        union.CANTIDAD = item.CANTIDAD;
                        lista.Add(union);
                    }

                    return lista.OrderBy(l => l.PK_ARTICULO).ThenBy(l => l.TALLA);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{texto}")]
        public IEnumerable<Models.DetallesInventarioT.TallasModel> ListarTalla([FromRoute] string texto)
        {
            try
            {
                List<Models.DetallesInventarioT.TallasModel> lista = new List<Models.DetallesInventarioT.TallasModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT PK_ARTICULO, TALLA, Tallas.DESCRIPCION AS NUMERACION, Articulos.DESCRIPCION AS DESCRIPCION FROM Tallas, Articulos WHERE OPCION = OPCION_TALLA AND SUBSTR(PK_ARTICULO,1,1) = GRUPO AND PK_ARTICULO = '" + texto + "'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.TallasModel tallas = new Models.DetallesInventarioT.TallasModel();
                                tallas.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                tallas.TALLA = reader["TALLA"].ToString();
                                tallas.NUMERACION = reader["NUMERACION"].ToString();
                                tallas.DESCRIPCION = reader["DESCRIPCION"].ToString();
                                lista.Add(tallas);
                            }
                        }
                        connection.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.ExistenciasT.InformeModel> ListarStock([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.ExistenciasT.InformeModel> lista = new List<Models.ExistenciasT.InformeModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT * FROM (SELECT CategoriaSuperior.DESCRIPCION AS CATEGORIAI, SUM(ExistenciasT.CANTIDAD) AS CANTIDADI, SUM(ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS VALORI FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                                        WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY CategoriaSuperior.DESCRIPCION ) T1 FULL OUTER JOIN (SELECT CategoriaSuperior.DESCRIPCION AS CATEGORIAF, SUM(DetalleInventariosT.CANTIDAD) AS CANTIDADF, SUM(DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS VALORF FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "'  AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY CategoriaSuperior.DESCRIPCION) T2 ON T1.CATEGORIAI = T2.CATEGORIAF", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.ExistenciasT.InformeModel existencia = new Models.ExistenciasT.InformeModel();
                                existencia.CATEGORIAI = reader["CATEGORIAI"].ToString();
                                if (reader["CANTIDADI"] != DBNull.Value)
                                    existencia.TOTALI = Convert.ToDecimal(reader["CANTIDADI"]);
                                if (reader["VALORI"] != DBNull.Value)
                                    existencia.MONTOI = Convert.ToDecimal(reader["VALORI"]);

                                existencia.CATEGORIAF = reader["CATEGORIAF"].ToString();
                                if (reader["CANTIDADF"] != DBNull.Value)
                                {
                                    existencia.TOTALF = Convert.ToDecimal(reader["CANTIDADF"]);
                                }
                                else
                                {
                                    existencia.TOTALF = 0;
                                }
                                if (reader["VALORF"] != DBNull.Value)
                                {
                                    existencia.MONTOF = Convert.ToDecimal(reader["VALORF"]);
                                }
                                else
                                {
                                    existencia.MONTOF = 0;
                                }

                                lista.Add(existencia);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.ExistenciasT.InformeModel> ListarStockFinal([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.ExistenciasT.InformeModel> lista = new List<Models.ExistenciasT.InformeModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {

                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {

                            command.CommandText = @"SELECT * FROM (SELECT CategoriaSuperior.DESCRIPCION AS CATEGORIAI, SUM(ExistenciasT.CANTIDAD) AS CANTIDADI, SUM(ExistenciasT.CANTIDAD * PreciosInventariosT.VENTA) AS VALORI FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                                        WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' GROUP BY CategoriaSuperior.DESCRIPCION ) T1 FULL OUTER JOIN (SELECT CategoriaSuperior.DESCRIPCION AS CATEGORIAF, SUM(DetalleInventariosT.CANTIDAD) AS CANTIDADF, SUM(DetalleInventariosT.CANTIDAD * PreciosInventariosT.VENTA) AS VALORF FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "'  AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' GROUP BY CategoriaSuperior.DESCRIPCION) T2 ON T1.CATEGORIAI = T2.CATEGORIAF";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.ExistenciasT.InformeModel existencia = new Models.ExistenciasT.InformeModel();
                                    existencia.CATEGORIAI = reader["CATEGORIAI"].ToString();
                                    if (reader["CANTIDADI"] != DBNull.Value)
                                        existencia.TOTALI = Convert.ToDecimal(reader["CANTIDADI"]);
                                    if (reader["VALORI"] != DBNull.Value)
                                        existencia.MONTOI = Convert.ToDecimal(reader["VALORI"]);

                                    existencia.CATEGORIAF = reader["CATEGORIAF"].ToString();
                                    if (reader["CANTIDADF"] != DBNull.Value)
                                    {
                                        existencia.TOTALF = Convert.ToDecimal(reader["CANTIDADF"]);
                                    }
                                    else
                                    {
                                        existencia.TOTALF = 0;
                                    }
                                    if (reader["VALORF"] != DBNull.Value)
                                    {
                                        existencia.MONTOF = Convert.ToDecimal(reader["VALORF"]);
                                    }
                                    else
                                    {
                                        existencia.MONTOF = 0;
                                    }

                                    lista.Add(existencia);
                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT * FROM (SELECT CategoriaSuperior.DESCRIPCION AS CATEGORIAI, SUM(ExistenciasT.CANTIDAD) AS CANTIDADI, SUM(ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS VALORI FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                                        WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY CategoriaSuperior.DESCRIPCION ) T1 FULL OUTER JOIN (SELECT CategoriaSuperior.DESCRIPCION AS CATEGORIAF, SUM(DetalleInventariosT.CANTIDAD) AS CANTIDADF, SUM(DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS VALORF FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "'  AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY CategoriaSuperior.DESCRIPCION) T2 ON T1.CATEGORIAI = T2.CATEGORIAF";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.ExistenciasT.InformeModel existencia = new Models.ExistenciasT.InformeModel();
                                    existencia.CATEGORIAI = reader["CATEGORIAI"].ToString();
                                    if (reader["CANTIDADI"] != DBNull.Value)
                                        existencia.TOTALI = Convert.ToDecimal(reader["CANTIDADI"]);
                                    if (reader["VALORI"] != DBNull.Value)
                                        existencia.MONTOI = Convert.ToDecimal(reader["VALORI"]);

                                    existencia.CATEGORIAF = reader["CATEGORIAF"].ToString();
                                    if (reader["CANTIDADF"] != DBNull.Value)
                                    {
                                        existencia.TOTALF = Convert.ToDecimal(reader["CANTIDADF"]);
                                    }
                                    else
                                    {
                                        existencia.TOTALF = 0;
                                    }
                                    if (reader["VALORF"] != DBNull.Value)
                                    {
                                        existencia.MONTOF = Convert.ToDecimal(reader["VALORF"]);
                                    }
                                    else
                                    {
                                        existencia.MONTOF = 0;
                                    }

                                    lista.Add(existencia);
                                }
                            }
                        }

                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.ListarModel> ListarDiferencias([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.ListarModel> lista = new List<Models.DetallesInventarioT.ListarModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT PK_ARTICULO, CATEGORIA, SUM(EXISTENCIAS) AS EXISTENCIAS, SUM(CONTADOS) AS CONTADOS, (SUM(EXISTENCIAS) - SUM(CONTADOS)) * -1 AS DIFERENCIAS, PRECIO, (PRECIO * SUM(EXISTENCIAS-CONTADOS)) * -1 AS TOTAL FROM ( SELECT DetalleInventariosT.PK_ARTICULO, 0 AS EXISTENCIAS, DetalleInventariosT.CANTIDAD AS CONTADOS, PrecioSemana.VENTA AS PRECIO, CategoriaSuperior.DESCRIPCION AS CATEGORIA FROM DetalleInventariosT INNER JOIN Articulos ON DetalleInventariosT.PK_ARTICULO = Articulos.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' UNION ALL SELECT ExistenciasT.PK_ARTICULO, ExistenciasT.CANTIDAD AS EXISTENCIAS, 0 AS CONTADOS, PrecioSemana.VENTA AS PRECIO, CategoriaSuperior.DESCRIPCION AS CATEGORIA FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a GROUP BY PK_ARTICULO, CATEGORIA, PRECIO ORDER BY CATEGORIA", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.ListarModel detalle = new Models.DetallesInventarioT.ListarModel();
                                int diferencia = Convert.ToInt32(reader["DIFERENCIAS"]);
                                if (diferencia != 0)
                                {
                                    detalle.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                    detalle.CATEGORIA = reader["CATEGORIA"].ToString();
                                    detalle.EXISTENCIAS = reader["EXISTENCIAS"].ToString();
                                    detalle.CONTADOS = reader["CONTADOS"].ToString();
                                    detalle.DIFERENCIAS = reader["DIFERENCIAS"].ToString();
                                    detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                    detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                    lista.Add(detalle);
                                }

                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.ListarModel> ListarDiferenciasFinal([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.ListarModel> lista = new List<Models.DetallesInventarioT.ListarModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {

                            command.CommandText = @"SELECT PK_ARTICULO, CATEGORIA, SUM(EXISTENCIAS) AS EXISTENCIAS, SUM(CONTADOS) AS CONTADOS, (SUM(EXISTENCIAS) - SUM(CONTADOS)) * -1 AS DIFERENCIAS, PRECIO, (PRECIO * SUM(EXISTENCIAS-CONTADOS)) * -1 AS TOTAL FROM ( SELECT DetalleInventariosT.PK_ARTICULO, 0 AS EXISTENCIAS, DetalleInventariosT.CANTIDAD AS CONTADOS, PreciosInventariosT.VENTA AS PRECIO, CategoriaSuperior.DESCRIPCION AS CATEGORIA FROM DetalleInventariosT INNER JOIN Articulos ON DetalleInventariosT.PK_ARTICULO = Articulos.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' UNION ALL SELECT ExistenciasT.PK_ARTICULO, ExistenciasT.CANTIDAD AS EXISTENCIAS, 0 AS CONTADOS, PreciosInventariosT.VENTA AS PRECIO, CategoriaSuperior.DESCRIPCION AS CATEGORIA FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "') a GROUP BY PK_ARTICULO, CATEGORIA, PRECIO ORDER BY CATEGORIA";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.ListarModel detalle = new Models.DetallesInventarioT.ListarModel();
                                    int diferencia = Convert.ToInt32(reader["DIFERENCIAS"]);
                                    if (diferencia != 0)
                                    {
                                        detalle.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                        detalle.CATEGORIA = reader["CATEGORIA"].ToString();
                                        detalle.EXISTENCIAS = reader["EXISTENCIAS"].ToString();
                                        detalle.CONTADOS = reader["CONTADOS"].ToString();
                                        detalle.DIFERENCIAS = reader["DIFERENCIAS"].ToString();
                                        detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                        detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                        lista.Add(detalle);
                                    }

                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT PK_ARTICULO, CATEGORIA, SUM(EXISTENCIAS) AS EXISTENCIAS, SUM(CONTADOS) AS CONTADOS, (SUM(EXISTENCIAS) - SUM(CONTADOS)) * -1 AS DIFERENCIAS, PRECIO, (PRECIO * SUM(EXISTENCIAS-CONTADOS)) * -1 AS TOTAL FROM ( SELECT DetalleInventariosT.PK_ARTICULO, 0 AS EXISTENCIAS, DetalleInventariosT.CANTIDAD AS CONTADOS, PrecioSemana.VENTA AS PRECIO, CategoriaSuperior.DESCRIPCION AS CATEGORIA FROM DetalleInventariosT INNER JOIN Articulos ON DetalleInventariosT.PK_ARTICULO = Articulos.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' UNION ALL SELECT ExistenciasT.PK_ARTICULO, ExistenciasT.CANTIDAD AS EXISTENCIAS, 0 AS CONTADOS, PrecioSemana.VENTA AS PRECIO, CategoriaSuperior.DESCRIPCION AS CATEGORIA FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a GROUP BY PK_ARTICULO, CATEGORIA, PRECIO";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.ListarModel detalle = new Models.DetallesInventarioT.ListarModel();
                                    int diferencia = Convert.ToInt32(reader["DIFERENCIAS"]);
                                    if (diferencia != 0)
                                    {
                                        detalle.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                        detalle.CATEGORIA = reader["CATEGORIA"].ToString();
                                        detalle.EXISTENCIAS = reader["EXISTENCIAS"].ToString();
                                        detalle.CONTADOS = reader["CONTADOS"].ToString();
                                        detalle.DIFERENCIAS = reader["DIFERENCIAS"].ToString();
                                        detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                        detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                        lista.Add(detalle);
                                    }

                                }
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.StockModel> ListarExistencias([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.StockModel> lista = new List<Models.DetallesInventarioT.StockModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUBSTR(ESTANTE,2,5) AS ESTANTE FROM DETALLEINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "' ORDER BY SUBSTR(ESTANTE,2,5) FETCH FIRST 1 ROWS ONLY", connection))
                    {
                        using (var read = command.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                string estante = read["ESTANTE"].ToString();
                                bool isNumeric = estante.All(char.IsDigit);
                                if (isNumeric == true)
                                {
                                    command.CommandText = @"SELECT DetalleInventariosT.ESTANTE,SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD,SUM(PrecioSemana.VENTA) AS PRECIO, DetalleInventariosT.USUARIO, MIN(DetalleInventariosT.HORAINICIO) AS HORAINICIO, MAX(DetalleInventariosT.HORAFIN) AS HORAFIN
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                         DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO ORDER BY SUBSTR(ESTANTE,1,1), CAST(SUBSTR(ESTANTE,2,5)AS INTEGER)";
                                    using (var reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                            detalle.ESTANTE = reader["ESTANTE"].ToString();
                                            detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                            detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                            detalle.USUARIO = reader["USUARIO"].ToString();
                                            detalle.INICIO = reader["HORAINICIO"].ToString();
                                            detalle.FIN = reader["HORAFIN"].ToString();
                                            lista.Add(detalle);

                                        }
                                    }
                                }
                                else
                                {
                                    command.CommandText = @"SELECT DetalleInventariosT.ESTANTE,SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD,SUM(PrecioSemana.VENTA) AS PRECIO, DetalleInventariosT.USUARIO, MIN(DetalleInventariosT.HORAINICIO) AS HORAINICIO, MAX(DetalleInventariosT.HORAFIN) AS HORAFIN
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                         DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO ORDER BY DetalleInventariosT.ESTANTE";
                                    using (var reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                            detalle.ESTANTE = reader["ESTANTE"].ToString();
                                            detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                            detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                            detalle.USUARIO = reader["USUARIO"].ToString();
                                            detalle.INICIO = reader["HORAINICIO"].ToString();
                                            detalle.FIN = reader["HORAFIN"].ToString();
                                            lista.Add(detalle);

                                        }
                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.StockModel> ListarExistenciasFinal([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.StockModel> lista = new List<Models.DetallesInventarioT.StockModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {

                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT SUBSTR(ESTANTE,2,5) AS ESTANTE FROM DETALLEINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "' ORDER BY SUBSTR(ESTANTE,2,5) FETCH FIRST 1 ROWS ONLY";
                            using (var read = command.ExecuteReader())
                            {
                                while (read.Read())
                                {
                                    string estante = read["ESTANTE"].ToString();
                                    bool isNumeric = estante.All(char.IsDigit);
                                    if (isNumeric == true)
                                    {
                                        command.CommandText = @"SELECT DetalleInventariosT.ESTANTE,SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD,SUM(PreciosInventariosT.VENTA) AS PRECIO, DetalleInventariosT.USUARIO, MIN(DetalleInventariosT.HORAINICIO) AS HORAINICIO, MAX(DetalleInventariosT.HORAFIN) AS HORAFIN
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                         DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' GROUP BY DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO ORDER BY SUBSTR(ESTANTE,1,1), CAST(SUBSTR(ESTANTE,2,5)AS INTEGER)";
                                        using (var reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                                detalle.ESTANTE = reader["ESTANTE"].ToString();
                                                detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                                detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                                detalle.USUARIO = reader["USUARIO"].ToString();
                                                detalle.INICIO = reader["HORAINICIO"].ToString();
                                                detalle.FIN = reader["HORAFIN"].ToString();
                                                lista.Add(detalle);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        command.CommandText = @"SELECT DetalleInventariosT.ESTANTE,SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD,SUM(PreciosInventariosT.VENTA) AS PRECIO, DetalleInventariosT.USUARIO, MIN(DetalleInventariosT.HORAINICIO) AS HORAINICIO, MAX(DetalleInventariosT.HORAFIN) AS HORAFIN
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                         DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIO = '" + id + "' GROUP BY DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO ORDER BY DetalleInventariosT.ESTANTE";
                                        using (var reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                                detalle.ESTANTE = reader["ESTANTE"].ToString();
                                                detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                                detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                                detalle.USUARIO = reader["USUARIO"].ToString();
                                                detalle.INICIO = reader["HORAINICIO"].ToString();
                                                detalle.FIN = reader["HORAFIN"].ToString();
                                                lista.Add(detalle);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT SUBSTR(ESTANTE,2,5) AS ESTANTE FROM DETALLEINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "' ORDER BY SUBSTR(ESTANTE,2,5) FETCH FIRST 1 ROWS ONLY";
                            using (var read = command.ExecuteReader())
                            {
                                while (read.Read())
                                {
                                    string estante = read["ESTANTE"].ToString();
                                    bool isNumeric = estante.All(char.IsDigit);
                                    if (isNumeric == true)
                                    {
                                        command.CommandText = @"SELECT DetalleInventariosT.ESTANTE,SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD,SUM(PrecioSemana.VENTA) AS PRECIO, DetalleInventariosT.USUARIO, MIN(DetalleInventariosT.HORAINICIO) AS HORAINICIO, MAX(DetalleInventariosT.HORAFIN) AS HORAFIN
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                         DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO ORDER BY SUBSTR(ESTANTE,1,1), CAST(SUBSTR(ESTANTE,2,5)AS INTEGER)";
                                        using (var reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                                detalle.ESTANTE = reader["ESTANTE"].ToString();
                                                detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                                detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                                detalle.USUARIO = reader["USUARIO"].ToString();
                                                detalle.INICIO = reader["HORAINICIO"].ToString();
                                                detalle.FIN = reader["HORAFIN"].ToString();
                                                lista.Add(detalle);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        command.CommandText = @"SELECT DetalleInventariosT.ESTANTE,SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD,SUM(PrecioSemana.VENTA) AS PRECIO, DetalleInventariosT.USUARIO, MIN(DetalleInventariosT.HORAINICIO) AS HORAINICIO, MAX(DetalleInventariosT.HORAFIN) AS HORAFIN
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                         DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO ORDER BY DetalleInventariosT.ESTANTE";
                                        using (var reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                                detalle.ESTANTE = reader["ESTANTE"].ToString();
                                                detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                                detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                                detalle.USUARIO = reader["USUARIO"].ToString();
                                                detalle.INICIO = reader["HORAINICIO"].ToString();
                                                detalle.FIN = reader["HORAFIN"].ToString();
                                                lista.Add(detalle);

                                            }
                                        }
                                    }
                                }
                            }
                        }

                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.StockModel> ListarLecturas([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.StockModel> lista = new List<Models.DetallesInventarioT.StockModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT DetalleInventariosT.PK_ARTICULO, CategoriaSuperior.DESCRIPCION AS CATEGORIA,PrecioSemana.VENTA AS PRECIO, SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS TOTAL, DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO, DetalleInventariosT.HORAINICIO, DetalleInventariosT.HORAFIN
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY DetalleInventariosT.PK_ARTICULO, CategoriaSuperior.DESCRIPCION, PrecioSemana.VENTA, DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO, DetalleInventariosT.HORAINICIO, DetalleInventariosT.HORAFIN ORDER BY ESTANTE, PK_ARTICULO", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                detalle.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                detalle.CATEGORIA = reader["CATEGORIA"].ToString();
                                detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                detalle.ESTANTE = reader["ESTANTE"].ToString();
                                detalle.USUARIO = reader["USUARIO"].ToString();
                                detalle.INICIO = reader["HORAINICIO"].ToString();
                                detalle.FIN = reader["HORAFIN"].ToString();
                                lista.Add(detalle);

                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.StockModel> ListarLecturasFinal([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.StockModel> lista = new List<Models.DetallesInventarioT.StockModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT DetalleInventariosT.PK_ARTICULO, CategoriaSuperior.DESCRIPCION AS CATEGORIA,PreciosInventariosT.VENTA AS PRECIO, SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PreciosInventariosT.VENTA) AS TOTAL, DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO, DetalleInventariosT.HORAINICIO, DetalleInventariosT.HORAFIN
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' GROUP BY DetalleInventariosT.PK_ARTICULO, CategoriaSuperior.DESCRIPCION, PreciosInventariosT.VENTA, DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO, DetalleInventariosT.HORAINICIO, DetalleInventariosT.HORAFIN ORDER BY ESTANTE, PK_ARTICULO";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                    detalle.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                    detalle.CATEGORIA = reader["CATEGORIA"].ToString();
                                    detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                    detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                    detalle.ESTANTE = reader["ESTANTE"].ToString();
                                    detalle.USUARIO = reader["USUARIO"].ToString();
                                    detalle.INICIO = reader["HORAINICIO"].ToString();
                                    detalle.FIN = reader["HORAFIN"].ToString();
                                    lista.Add(detalle);
                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT DetalleInventariosT.PK_ARTICULO, CategoriaSuperior.DESCRIPCION AS CATEGORIA,PrecioSemana.VENTA AS PRECIO, SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS TOTAL, DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO, DetalleInventariosT.HORAINICIO, DetalleInventariosT.HORAFIN
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' GROUP BY DetalleInventariosT.PK_ARTICULO, CategoriaSuperior.DESCRIPCION, PrecioSemana.VENTA, DetalleInventariosT.ESTANTE, DetalleInventariosT.USUARIO, DetalleInventariosT.HORAINICIO, DetalleInventariosT.HORAFIN ORDER BY ESTANTE, PK_ARTICULO";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.StockModel detalle = new Models.DetallesInventarioT.StockModel();
                                    detalle.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                    detalle.CATEGORIA = reader["CATEGORIA"].ToString();
                                    detalle.PRECIO = Convert.ToDecimal(reader["PRECIO"]);
                                    detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                    detalle.ESTANTE = reader["ESTANTE"].ToString();
                                    detalle.USUARIO = reader["USUARIO"].ToString();
                                    detalle.INICIO = reader["HORAINICIO"].ToString();
                                    detalle.FIN = reader["HORAFIN"].ToString();
                                    lista.Add(detalle);
                                }
                            }
                        }

                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalPares([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS TOTAL
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                if (reader[reader.GetOrdinal("TOTAL")] != DBNull.Value)
                                {
                                    detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                }
                                else
                                {
                                    detalle.TOTAL = 0;
                                }

                                if (reader[reader.GetOrdinal("CANTIDAD")] != DBNull.Value)
                                {
                                    detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                }
                                else
                                {
                                    detalle.CANTIDAD = 0;
                                }

                                lista.Add(detalle);

                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalParesFinal([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PreciosInventariosT.VENTA) AS TOTAL
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I'";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                    if (reader[reader.GetOrdinal("TOTAL")] != DBNull.Value)
                                    {
                                        detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                    }
                                    else
                                    {
                                        detalle.TOTAL = 0;
                                    }

                                    if (reader[reader.GetOrdinal("CANTIDAD")] != DBNull.Value)
                                    {
                                        detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                    }
                                    else
                                    {
                                        detalle.CANTIDAD = 0;
                                    }

                                    lista.Add(detalle);

                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS TOTAL
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I'";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                    if (reader[reader.GetOrdinal("TOTAL")] != DBNull.Value)
                                    {
                                        detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                    }
                                    else
                                    {
                                        detalle.TOTAL = 0;
                                    }

                                    if (reader[reader.GetOrdinal("CANTIDAD")] != DBNull.Value)
                                    {
                                        detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                    }
                                    else
                                    {
                                        detalle.CANTIDAD = 0;
                                    }

                                    lista.Add(detalle);

                                }
                            }
                        }

                          
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalAccesorios([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS TOTAL
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J'OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                if (reader[reader.GetOrdinal("TOTAL")] != DBNull.Value)
                                {
                                    detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                }
                                else
                                {
                                    detalle.TOTAL = 0;
                                }

                                if (reader[reader.GetOrdinal("CANTIDAD")] != DBNull.Value)
                                {
                                    detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                }
                                else
                                {
                                    detalle.CANTIDAD = 0;
                                }
                                lista.Add(detalle);

                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalAccesoriosFinal([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PreciosInventariosT.VENTA) AS TOTAL
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J'OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "'";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                    if (reader[reader.GetOrdinal("TOTAL")] != DBNull.Value)
                                    {
                                        detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                    }
                                    else
                                    {
                                        detalle.TOTAL = 0;
                                    }

                                    if (reader[reader.GetOrdinal("CANTIDAD")] != DBNull.Value)
                                    {
                                        detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                    }
                                    else
                                    {
                                        detalle.CANTIDAD = 0;
                                    }
                                    lista.Add(detalle);

                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT SUM(DetalleInventariosT.CANTIDAD) AS CANTIDAD, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS TOTAL
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J'OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "'";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                    if (reader[reader.GetOrdinal("TOTAL")] != DBNull.Value)
                                    {
                                        detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                    }
                                    else
                                    {
                                        detalle.TOTAL = 0;
                                    }

                                    if (reader[reader.GetOrdinal("CANTIDAD")] != DBNull.Value)
                                    {
                                        detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                    }
                                    else
                                    {
                                        detalle.CANTIDAD = 0;
                                    }
                                    lista.Add(detalle);

                                }
                            }
                        }

                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.ExistenciasT.IngresadoModel> IngresadoPares([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.ExistenciasT.IngresadoModel> lista = new List<Models.ExistenciasT.IngresadoModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUM(ExistenciasT.CANTIDAD) AS CANTIDAD, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS TOTAL
                                                FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.ExistenciasT.IngresadoModel detalle = new Models.ExistenciasT.IngresadoModel();
                                if (reader[reader.GetOrdinal("TOTAL")] != DBNull.Value)
                                {
                                    detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                }
                                else
                                {
                                    detalle.TOTAL = 0;
                                }

                                if (reader[reader.GetOrdinal("CANTIDAD")] != DBNull.Value)
                                {
                                    detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                }
                                else
                                {
                                    detalle.CANTIDAD = 0;
                                }

                                lista.Add(detalle);

                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.ExistenciasT.IngresadoModel> IngresadoAccesorios([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.ExistenciasT.IngresadoModel> lista = new List<Models.ExistenciasT.IngresadoModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUM(ExistenciasT.CANTIDAD) AS CANTIDAD, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS TOTAL FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.ExistenciasT.IngresadoModel detalle = new Models.ExistenciasT.IngresadoModel();
                                if (reader[reader.GetOrdinal("TOTAL")] != DBNull.Value)
                                {
                                    detalle.TOTAL = Convert.ToDecimal(reader["TOTAL"]);
                                }
                                else
                                {
                                    detalle.TOTAL = 0;
                                }

                                if (reader[reader.GetOrdinal("CANTIDAD")] != DBNull.Value)
                                {
                                    detalle.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                }
                                else
                                {
                                    detalle.CANTIDAD = 0;
                                }
                                lista.Add(detalle);

                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalImportado([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(ExistenciasT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(ExistenciasT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                detalle.ACCESORIOS = Convert.ToInt32(reader["ACCESORIOS"]);
                                detalle.PARES = Convert.ToInt32(reader["PARES"]);
                                detalle.PRECIOA = Convert.ToDecimal(reader["PRECIOA"]);
                                detalle.PRECIOP = Convert.ToDecimal(reader["PRECIOP"]);
                                lista.Add(detalle);

                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalImportadoFinal([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '"+ id +"'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {

                            command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(ExistenciasT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (ExistenciasT.CANTIDAD * PreciosInventariosT.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                    WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(ExistenciasT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (ExistenciasT.CANTIDAD * PreciosInventariosT.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT= '" + id + "') a";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                    detalle.ACCESORIOS = Convert.ToInt32(reader["ACCESORIOS"]);
                                    detalle.PARES = Convert.ToInt32(reader["PARES"]);
                                    detalle.PRECIOA = Convert.ToDecimal(reader["PRECIOA"]);
                                    detalle.PRECIOP = Convert.ToDecimal(reader["PRECIOP"]);
                                    lista.Add(detalle);

                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(ExistenciasT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(ExistenciasT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                    detalle.ACCESORIOS = Convert.ToInt32(reader["ACCESORIOS"]);
                                    detalle.PARES = Convert.ToInt32(reader["PARES"]);
                                    detalle.PRECIOA = Convert.ToDecimal(reader["PRECIOA"]);
                                    detalle.PRECIOP = Convert.ToDecimal(reader["PRECIOP"]);
                                    lista.Add(detalle);

                                }
                            }
                        }

                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalLectura([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(DetalleInventariosT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(DetalleInventariosT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                detalle.ACCESORIOS = Convert.ToInt32(reader["ACCESORIOS"]);
                                detalle.PARES = Convert.ToInt32(reader["PARES"]);
                                detalle.PRECIOA = Convert.ToDecimal(reader["PRECIOA"]);
                                detalle.PRECIOP = Convert.ToDecimal(reader["PRECIOP"]);
                                lista.Add(detalle);

                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalLecturaFinal([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(DetalleInventariosT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (DetalleInventariosT.CANTIDAD * PreciosInventariosT.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(DetalleInventariosT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (DetalleInventariosT.CANTIDAD * PreciosInventariosT.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "') a";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                    detalle.ACCESORIOS = Convert.ToInt32(reader["ACCESORIOS"]);
                                    detalle.PARES = Convert.ToInt32(reader["PARES"]);
                                    detalle.PRECIOA = Convert.ToDecimal(reader["PRECIOA"]);
                                    detalle.PRECIOP = Convert.ToDecimal(reader["PRECIOP"]);
                                    lista.Add(detalle);

                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(DetalleInventariosT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(DetalleInventariosT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                                    detalle.ACCESORIOS = Convert.ToInt32(reader["ACCESORIOS"]);
                                    detalle.PARES = Convert.ToInt32(reader["PARES"]);
                                    detalle.PRECIOA = Convert.ToDecimal(reader["PRECIOA"]);
                                    detalle.PRECIOP = Convert.ToDecimal(reader["PRECIOP"]);
                                    lista.Add(detalle);

                                }
                            }
                        }

                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalDiferencia([FromRoute] int id, int pk)
        {
            int AccesorioImportado = 0;
            int ParesImportado = 0;
            decimal PrecioAImportado = 0;
            decimal PrecioPImportado = 0;
            int AccesorioLectura = 0;
            int ParesLectura = 0;
            decimal PrecioALectura = 0;
            decimal PrecioPLectura = 0;

            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(ExistenciasT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(ExistenciasT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a", connection))
                    {
                        command.Connection = connection;
                        using (var reader1 = command.ExecuteReader())
                        {
                            while (reader1.Read())
                            {
                                AccesorioImportado = Convert.ToInt32(reader1["ACCESORIOS"]);
                                ParesImportado = Convert.ToInt32(reader1["PARES"]);
                                PrecioAImportado = Convert.ToDecimal(reader1["PRECIOA"]);
                                PrecioPImportado = Convert.ToDecimal(reader1["PRECIOP"]);

                            }
                        }

                        command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(DetalleInventariosT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(DetalleInventariosT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a";
                        command.Connection = connection;
                        using (var reader2 = command.ExecuteReader())
                        {
                            while (reader2.Read())
                            {
                                AccesorioLectura = Convert.ToInt32(reader2["ACCESORIOS"]);
                                ParesLectura = Convert.ToInt32(reader2["PARES"]);
                                PrecioALectura = Convert.ToDecimal(reader2["PRECIOA"]);
                                PrecioPLectura = Convert.ToDecimal(reader2["PRECIOP"]);
                            }
                        }
                        connection.Close();
                        Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                        detalle.ACCESORIOS = (AccesorioImportado - AccesorioLectura) * -1;
                        detalle.PARES = (ParesImportado - ParesLectura) * -1;
                        detalle.PRECIOA = (PrecioAImportado - PrecioALectura) * -1;
                        detalle.PRECIOP = (PrecioPImportado - PrecioPLectura) * -1;
                        lista.Add(detalle);
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.DetallesInventarioT.TotalModel> TotalDiferenciaFinal([FromRoute] int id, int pk)
        {
            int AccesorioImportado = 0;
            int ParesImportado = 0;
            decimal PrecioAImportado = 0;
            decimal PrecioPImportado = 0;
            int AccesorioLectura = 0;
            int ParesLectura = 0;
            decimal PrecioALectura = 0;
            decimal PrecioPLectura = 0;

            try
            {
                List<Models.DetallesInventarioT.TotalModel> lista = new List<Models.DetallesInventarioT.TotalModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(ExistenciasT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (ExistenciasT.CANTIDAD * PreciosInventariosT.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                    WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(ExistenciasT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (ExistenciasT.CANTIDAD * PreciosInventariosT.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "') a";
                            using (var reader1 = command.ExecuteReader())
                            {
                                while (reader1.Read())
                                {
                                    AccesorioImportado = Convert.ToInt32(reader1["ACCESORIOS"]);
                                    ParesImportado = Convert.ToInt32(reader1["PARES"]);
                                    PrecioAImportado = Convert.ToDecimal(reader1["PRECIOA"]);
                                    PrecioPImportado = Convert.ToDecimal(reader1["PRECIOP"]);

                                }
                            }

                            command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(DetalleInventariosT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (DetalleInventariosT.CANTIDAD * PreciosInventariosT.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(DetalleInventariosT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (DetalleInventariosT.CANTIDAD * PreciosInventariosT.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PreciosInventariosT ON Articulos.PK_ARTICULO = PreciosInventariosT.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PreciosInventariosT.PK_INVENTARIOT = '" + id + "') a";

                            using (var reader2 = command.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    AccesorioLectura = Convert.ToInt32(reader2["ACCESORIOS"]);
                                    ParesLectura = Convert.ToInt32(reader2["PARES"]);
                                    PrecioALectura = Convert.ToDecimal(reader2["PRECIOA"]);
                                    PrecioPLectura = Convert.ToDecimal(reader2["PRECIOP"]);
                                }
                            }
                        }
                        else
                        {
                            command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(ExistenciasT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(ExistenciasT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a";
                            using (var reader1 = command.ExecuteReader())
                            {
                                while (reader1.Read())
                                {
                                    AccesorioImportado = Convert.ToInt32(reader1["ACCESORIOS"]);
                                    ParesImportado = Convert.ToInt32(reader1["PARES"]);
                                    PrecioAImportado = Convert.ToDecimal(reader1["PRECIOA"]);
                                    PrecioPImportado = Convert.ToDecimal(reader1["PRECIOP"]);

                                }
                            }

                            command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(DetalleInventariosT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(DetalleInventariosT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a";

                            using (var reader2 = command.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    AccesorioLectura = Convert.ToInt32(reader2["ACCESORIOS"]);
                                    ParesLectura = Convert.ToInt32(reader2["PARES"]);
                                    PrecioALectura = Convert.ToDecimal(reader2["PRECIOA"]);
                                    PrecioPLectura = Convert.ToDecimal(reader2["PRECIOP"]);
                                }
                            }
                        }

                        connection.Close();
                        Models.DetallesInventarioT.TotalModel detalle = new Models.DetallesInventarioT.TotalModel();
                        detalle.ACCESORIOS = (AccesorioImportado - AccesorioLectura) * -1;
                        detalle.PARES = (ParesImportado - ParesLectura) * -1;
                        detalle.PRECIOA = (PrecioAImportado - PrecioALectura) * -1;
                        detalle.PRECIOP = (PrecioPImportado - PrecioPLectura) * -1;
                        lista.Add(detalle);
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<Models.DetallesInventarioT.PlanillaModel> PlanillaManual([FromRoute] int id)
        {
            try
            {
                EliminarManual(id);
                List<Models.DetallesInventarioT.PlanillaModel> lista = new List<Models.DetallesInventarioT.PlanillaModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT ExistenciasT.PK_INVENTARIOT, ExistenciasT.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, ExistenciasT.TALLA, ExistenciasT.CANTIDAD FROM Articulos INNER JOIN
                         Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int pk = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                string sup = reader["PK_CATEGORIA_SUP"].ToString();
                                string articulo = reader["PK_ARTICULO"].ToString();
                                int cantidad = Convert.ToInt32(reader["CANTIDAD"]);
                                string talla = reader["TALLA"].ToString();
                                bool isNumeric = talla.All(char.IsDigit);
                                if (isNumeric == false)
                                {
                                    talla = "1";
                                }

                                command.CommandText = @"INSERT INTO PLANILLAMANUAL (PK_INVENTARIOT, PK_ARTICULO, PK_CATEGORIA_SUP, TALLA, CANTIDAD) VALUES ('" + pk + "', '" + articulo + "', '" + sup + "', '" + talla + "', '" + cantidad + "')";
                                command.ExecuteNonQuery();
                            }
                        }
                        command.CommandText = @"UPDATE PLANILLAMANUAL SET TALLA = 1 WHERE (TALLA = 0 OR PK_CATEGORIA_SUP = 'H' OR PK_CATEGORIA_SUP = 'I') AND PK_INVENTARIOT = '" + id + "'";
                        command.ExecuteNonQuery();

                        command.CommandText = @"SELECT PK_ARTICULO, (SUM(TAM1) + SUM(TAM2) + SUM(TAM3) + SUM(TAM4) + SUM(TAM5) + SUM(TAM6) + SUM(TAM7) + SUM(TAM8) + SUM(TAM9)) AS TOTAL, SUM(TAM1) TAM1, SUM(TAM2) TAM2, SUM(TAM3) TAM3, SUM(TAM4) TAM4, SUM(TAM5) TAM5, SUM(TAM6) TAM6, SUM(TAM7) TAM7, SUM(TAM8) TAM8, SUM(TAM9) TAM9 FROM (SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, SUM(PLANILLAMANUAL.CANTIDAD) TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 1 GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA UNION SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, 0 TAM1, SUM(PLANILLAMANUAL.CANTIDAD) TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 2 GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA UNION SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, 0 TAM1,0 TAM2, SUM(PLANILLAMANUAL.CANTIDAD) TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 3 GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA UNION SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, 0 TAM1,0 TAM2,0 TAM3, SUM(PLANILLAMANUAL.CANTIDAD) TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 4 GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA UNION SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, 0 TAM1,0 TAM2,0 TAM3,0 TAM4, SUM(PLANILLAMANUAL.CANTIDAD) TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 5 GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA UNION SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5, SUM(PLANILLAMANUAL.CANTIDAD) TAM6,0 TAM7,0 TAM8,0 TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 6 GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA UNION SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6, SUM(PLANILLAMANUAL.CANTIDAD) TAM7,0 TAM8,0 TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 7 GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA UNION SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7, SUM(PLANILLAMANUAL.CANTIDAD) TAM8,0 TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 8 GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA UNION SELECT PLANILLAMANUAL.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, Categorias.PK_CATEGORIA, Articulos.PK_SUBCATEGORIA, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8, SUM(PLANILLAMANUAL.CANTIDAD) TAM9 FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN PLANILLAMANUAL ON Articulos.PK_ARTICULO = PLANILLAMANUAL.PK_ARTICULO WHERE PLANILLAMANUAL.PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PLANILLAMANUAL.PK_ARTICULO,1,1), TALLA) = 9GROUP BY PLANILLAMANUAL.PK_ARTICULO,CategoriaSuperior.PK_CATEGORIA_SUP,Categorias.PK_CATEGORIA,Articulos.PK_SUBCATEGORIA) GROUP BY PK_ARTICULO,PK_CATEGORIA_SUP,PK_CATEGORIA,PK_SUBCATEGORIA ORDER BY PK_CATEGORIA_SUP, PK_CATEGORIA, PK_SUBCATEGORIA, TO_NUMBER(SUBSTR(PK_ARTICULO, 6, 3)), TO_NUMBER(SUBSTR(PK_ARTICULO, 4, 2)), TO_NUMBER(SUBSTR(PK_ARTICULO, 1, 1))";
                        using (var read = command.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                Models.DetallesInventarioT.PlanillaModel tamanos = new Models.DetallesInventarioT.PlanillaModel();
                                tamanos.PK_ARTICULO = read["PK_ARTICULO"].ToString();
                                tamanos.TOTAL = read["TOTAL"].ToString();
                                tamanos.TAM1 = read["TAM1"].ToString();
                                tamanos.TAM2 = read["TAM2"].ToString();
                                tamanos.TAM3 = read["TAM3"].ToString();
                                tamanos.TAM4 = read["TAM4"].ToString();
                                tamanos.TAM5 = read["TAM5"].ToString();
                                tamanos.TAM6 = read["TAM6"].ToString();
                                tamanos.TAM7 = read["TAM7"].ToString();
                                tamanos.TAM8 = read["TAM8"].ToString();
                                tamanos.TAM9 = read["TAM9"].ToString();
                                lista.Add(tamanos);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<Models.DetallesInventarioT.PlanillaModel> PlanillaInicial([FromRoute] int id)
        {
            try
            {

                EliminarInicial(id);
                List<Models.DetallesInventarioT.PlanillaModel> lista = new List<Models.DetallesInventarioT.PlanillaModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT ExistenciasT.PK_INVENTARIOT, ExistenciasT.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, ExistenciasT.TALLA, ExistenciasT.CANTIDAD FROM Articulos INNER JOIN
                         Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int pk = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                string sup = reader["PK_CATEGORIA_SUP"].ToString();
                                string articulo = reader["PK_ARTICULO"].ToString();
                                string talla = reader["TALLA"].ToString();
                                int cantidad = Convert.ToInt32(reader["CANTIDAD"]);
                                bool isNumeric = talla.All(char.IsDigit);
                                if (isNumeric == false)
                                {
                                    talla = "1";
                                }

                                command.CommandText = @"INSERT INTO PLANILLAINICIAL (PK_INVENTARIOT, PK_ARTICULO, PK_CATEGORIA_SUP, TALLA, CANTIDAD) VALUES ('" + pk + "', '" + articulo + "', '" + sup + "', '" + talla + "', '" + cantidad + "')";
                                command.ExecuteNonQuery();

                            }
                        }

                        command.CommandText = @"UPDATE PLANILLAINICIAL SET TALLA = 1 WHERE (TALLA = 0 OR PK_CATEGORIA_SUP = 'H' OR PK_CATEGORIA_SUP = 'I') AND PK_INVENTARIOT = '" + id + "'";
                        command.ExecuteNonQuery();

                        command.CommandText = @"SELECT PK_ARTICULO, (SUM(TAM1) + SUM(TAM2) + SUM(TAM3) + SUM(TAM4) + SUM(TAM5) + SUM(TAM6) + SUM(TAM7) + SUM(TAM8) + SUM(TAM9)) AS TOTAL, SUM(TAM1) TAM1, SUM(TAM2) TAM2, SUM(TAM3) TAM3, SUM(TAM4) TAM4, SUM(TAM5) TAM5, SUM(TAM6) TAM6, SUM(TAM7) TAM7, SUM(TAM8) TAM8, SUM(TAM9) TAM9 FROM (SELECT PK_ARTICULO, SUM(CANTIDAD) TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 1 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,SUM(CANTIDAD) TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 2 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,SUM(CANTIDAD) TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 3 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,SUM(CANTIDAD) TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 4 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,SUM(CANTIDAD) TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 5 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,SUM(CANTIDAD) TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 6 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,SUM(CANTIDAD) TAM7,0 TAM8,0 TAM9 FROM  PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 7 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,SUM(CANTIDAD) TAM8,0 TAM9 FROM PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 8 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,SUM(CANTIDAD) TAM9 FROM PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 9 GROUP BY PK_ARTICULO) GROUP BY PK_ARTICULO ORDER BY PK_ARTICULO";
                        using (var read = command.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                Models.DetallesInventarioT.PlanillaModel tamanos = new Models.DetallesInventarioT.PlanillaModel();
                                tamanos.PK_ARTICULO = read["PK_ARTICULO"].ToString();
                                tamanos.TOTAL = read["TOTAL"].ToString();
                                tamanos.TAM1 = read["TAM1"].ToString();
                                tamanos.TAM2 = read["TAM2"].ToString();
                                tamanos.TAM3 = read["TAM3"].ToString();
                                tamanos.TAM4 = read["TAM4"].ToString();
                                tamanos.TAM5 = read["TAM5"].ToString();
                                tamanos.TAM6 = read["TAM6"].ToString();
                                tamanos.TAM7 = read["TAM7"].ToString();
                                tamanos.TAM8 = read["TAM8"].ToString();
                                tamanos.TAM9 = read["TAM9"].ToString();
                                lista.Add(tamanos);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<Models.DetallesInventarioT.PlanillaModel> PlanillaFinal([FromRoute] int id)
        {
            try
            {

                EliminarFinal(id);
                List<Models.DetallesInventarioT.PlanillaModel> lista = new List<Models.DetallesInventarioT.PlanillaModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT DetalleInventariosT.PK_INVENTARIOT, DetalleInventariosT.PK_ARTICULO, CategoriaSuperior.PK_CATEGORIA_SUP, DetalleInventariosT.TALLA, DetalleInventariosT.CANTIDAD FROM Articulos INNER JOIN
                         Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int pk = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                string sup = reader["PK_CATEGORIA_SUP"].ToString();
                                string articulo = reader["PK_ARTICULO"].ToString();
                                string talla = reader["TALLA"].ToString();
                                int cantidad = Convert.ToInt32(reader["CANTIDAD"]);
                                bool isNumeric = talla.All(char.IsDigit);
                                if (isNumeric == false)
                                {
                                    talla = "1";
                                }

                                command.CommandText = @"INSERT INTO PLANILLAFINAL(PK_INVENTARIOT, PK_ARTICULO, PK_CATEGORIA_SUP, TALLA, CANTIDAD) VALUES ('" + pk + "', '" + articulo + "', '" + sup + "', '" + talla + "', '" + cantidad + "')";
                                command.ExecuteNonQuery();

                            }
                        }

                        command.CommandText = @"UPDATE PLANILLAFINAL SET TALLA = 1 WHERE (TALLA = 0 OR PK_CATEGORIA_SUP = 'H' OR PK_CATEGORIA_SUP = 'I') AND PK_INVENTARIOT = '" + id + "'";
                        command.ExecuteNonQuery();

                        command.CommandText = @"SELECT PK_ARTICULO, (SUM(TAM1) + SUM(TAM2) + SUM(TAM3) + SUM(TAM4) + SUM(TAM5) + SUM(TAM6) + SUM(TAM7) + SUM(TAM8) + SUM(TAM9)) AS TOTAL, SUM(TAM1) TAM1, SUM(TAM2) TAM2, SUM(TAM3) TAM3, SUM(TAM4) TAM4, SUM(TAM5) TAM5, SUM(TAM6) TAM6, SUM(TAM7) TAM7, SUM(TAM8) TAM8, SUM(TAM9) TAM9 FROM (SELECT PK_ARTICULO, SUM(CANTIDAD) TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 1 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,SUM(CANTIDAD) TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 2 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,SUM(CANTIDAD) TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 3 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,SUM(CANTIDAD) TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 4 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,SUM(CANTIDAD) TAM5,0 TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 5 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,SUM(CANTIDAD) TAM6,0 TAM7,0 TAM8,0 TAM9 FROM  PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 6 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,SUM(CANTIDAD) TAM7,0 TAM8,0 TAM9 FROM  PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 7 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,SUM(CANTIDAD) TAM8,0 TAM9 FROM PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 8 GROUP BY PK_ARTICULO UNION SELECT PK_ARTICULO, 0 TAM1,0 TAM2,0 TAM3,0 TAM4,0 TAM5,0 TAM6,0 TAM7,0 TAM8,SUM(CANTIDAD) TAM9 FROM PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "' AND FANUM_OBTENER_TAMANO(SUBSTR(PK_ARTICULO,1,1), TALLA) = 9 GROUP BY PK_ARTICULO ) GROUP BY PK_ARTICULO ORDER BY PK_ARTICULO";
                        using (var read = command.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                Models.DetallesInventarioT.PlanillaModel tamanos = new Models.DetallesInventarioT.PlanillaModel();
                                tamanos.PK_ARTICULO = read["PK_ARTICULO"].ToString();
                                tamanos.TOTAL = read["TOTAL"].ToString();
                                tamanos.TAM1 = read["TAM1"].ToString();
                                tamanos.TAM2 = read["TAM2"].ToString();
                                tamanos.TAM3 = read["TAM3"].ToString();
                                tamanos.TAM4 = read["TAM4"].ToString();
                                tamanos.TAM5 = read["TAM5"].ToString();
                                tamanos.TAM6 = read["TAM6"].ToString();
                                tamanos.TAM7 = read["TAM7"].ToString();
                                tamanos.TAM8 = read["TAM8"].ToString();
                                tamanos.TAM9 = read["TAM9"].ToString();
                                lista.Add(tamanos);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<Models.InventariosT.ActaModel> ActaInventario([FromRoute] int id)
        {
            try
            {
                List<Models.InventariosT.ActaModel> lista = new List<Models.InventariosT.ActaModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT Tiendas.PK_TIENDA, Tiendas.CIUDAD, Tiendas.DIRECCION, InventariosT.FECHAINICIO, InventariosT.HORAINICIO, InventariosT.HORAFIN, InventariosT.FECHAFIN, Tiendas.CONSIGNATARIO, Tiendas.CARNET, Tiendas.RAZONSOCIAL, Tiendas.NIT, Usuarios.NOMBRES, Usuarios.APELLIDOS, Usuarios.CARNET AS CI
                                                    FROM DetalleInventariosT INNER JOIN InventariosT ON DetalleInventariosT.PK_INVENTARIOT = InventariosT.PK_INVENTARIOT INNER JOIN Tiendas ON InventariosT.PK_TIENDA = Tiendas.PK_TIENDA INNER JOIN Usuarios ON DetalleInventariosT.USUARIO = Usuarios.USUARIO
						                            WHERE InventariosT.PK_INVENTARIOT = '" + id + "' GROUP BY  Tiendas.PK_TIENDA, Tiendas.CIUDAD, Tiendas.DIRECCION, InventariosT.FECHAINICIO, InventariosT.HORAINICIO, InventariosT.FECHAFIN, InventariosT.HORAFIN, Tiendas.CONSIGNATARIO, Tiendas.CARNET, Tiendas.RAZONSOCIAL, Tiendas.NIT, Usuarios.NOMBRES, Usuarios.APELLIDOS, Usuarios.CARNET", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.InventariosT.ActaModel acta = new Models.InventariosT.ActaModel();
                                DateTime inicio = Convert.ToDateTime(reader["FECHAINICIO"]);
                                DateTime final = Convert.ToDateTime(reader["FECHAFIN"]);
                                acta.HORAINICIO = reader["HORAINICIO"].ToString();
                                acta.HORAFIN = reader["HORAFIN"].ToString();
                                acta.PK_TIENDA = reader["PK_TIENDA"].ToString();
                                acta.CIUDAD = reader["CIUDAD"].ToString();
                                acta.DIRECCION = reader["DIRECCION"].ToString();
                                acta.FECHAINICIO = inicio.ToString("yyyy'-'MM'-'dd");
                                acta.FECHAFIN = final.ToString("yyyy'-'MM'-'dd");
                                acta.CONSIGNATARIO = reader["CONSIGNATARIO"].ToString();
                                acta.CARNET = reader["CARNET"].ToString();
                                acta.RAZONSOCIAL = reader["RAZONSOCIAL"].ToString();
                                acta.NIT = reader["NIT"].ToString();
                                acta.NOMBRES = reader["NOMBRES"].ToString();
                                acta.APELLIDOS = reader["APELLIDOS"].ToString();
                                acta.CI = reader["CI"].ToString();
                                lista.Add(acta);
                            }
                        }
                        connection.Close();
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("[action]")]
        public IActionResult ExportarExcel([FromBody] Models.DetallesInventarioT.ExportarModel model)
        {
            try
            {
                List<Models.DetallesInventarioT.ExportarModel> lista = new List<Models.DetallesInventarioT.ExportarModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT INVENTARIOST.PK_INVENTARIOT, INVENTARIOST.PK_TIENDA, INVENTARIOST.CODIGO, INVENTARIOST.SEMANA, INVENTARIOST.FECHAINICIO, INVENTARIOST.FECHAFIN, INVENTARIOST.ESTADO, INVENTARIOST.HORAINICIO, INVENTARIOST.HORAFIN, USUARIOS.USUARIO FROM INVENTARIOST INNER JOIN USUARIOS ON INVENTARIOST.PK_USUARIO = USUARIOS.PK_USUARIO WHERE FECHAINICIO BETWEEN '"+ model.FECHAINICIO +"' AND '"+ model.FECHAFIN +"' AND INVENTARIOST.ESTADO = 'FINALIZADO'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.DetallesInventarioT.ExportarModel excel = new Models.DetallesInventarioT.ExportarModel();
                                excel.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                excel.PK_TIENDA = reader["PK_TIENDA"].ToString();
                                excel.CODIGO = reader["CODIGO"].ToString();
                                excel.SEMANA = reader["SEMANA"].ToString();
                                excel.FECHAINICIO = reader["FECHAINICIO"].ToString();
                                excel.FECHAFIN = reader["FECHAFIN"].ToString();
                                excel.HORAINICIO = reader["HORAINICIO"].ToString();
                                excel.HORAFIN = reader["HORAFIN"].ToString();
                                excel.ESTADO = reader["ESTADO"].ToString();
                                excel.USUARIO = reader["USUARIO"].ToString();

                                command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(ExistenciasT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE ExistenciasT.PK_INVENTARIOT = '" + excel.PK_INVENTARIOT + "' AND PrecioSemana.PK_SEMANA = '" + excel.SEMANA + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(ExistenciasT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND ExistenciasT.PK_INVENTARIOT = '" + excel.PK_INVENTARIOT + "' AND PrecioSemana.PK_SEMANA = '" + excel.SEMANA + "') a";
                                using (var read = command.ExecuteReader())
                                {
                                    while (read.Read())
                                    {
                                        excel.ACCESORIOSINICIAL = Convert.ToInt32(read["ACCESORIOS"]);
                                        excel.PARESINICIAL = Convert.ToInt32(read["PARES"]);
                                        excel.PRECIOINICIALA = Convert.ToDecimal(read["PRECIOA"]);
                                        excel.PRECIOINICIALP = Convert.ToDecimal(read["PRECIOP"]);
                                    }
                                }

                                command.CommandText = @"SELECT SUM(ACCESORIOS) AS ACCESORIOS, SUM(PARES) AS PARES, SUM(PRECIOA) AS PRECIOA, SUM(PRECIOP) AS PRECIOP FROM 
                                                ( SELECT 0 AS ACCESORIOS, SUM(DetalleInventariosT.CANTIDAD) AS PARES, 0 AS PRECIOA, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOP
                                                    FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN
                                                     DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO
                                                    WHERE DetalleInventariosT.PK_INVENTARIOT = '" + excel.PK_INVENTARIOT + "' AND PrecioSemana.PK_SEMANA = '" + excel.SEMANA + "' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'H' AND CategoriaSuperior.PK_CATEGORIA_SUP != 'J'AND CategoriaSuperior.PK_CATEGORIA_SUP != 'I' UNION ALL SELECT SUM(DetalleInventariosT.CANTIDAD) AS ACCESORIOS, 0 AS PARES, SUM (DetalleInventariosT.CANTIDAD * PrecioSemana.VENTA) AS PRECIOA, 0 AS PRECIOP FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE (CategoriaSuperior.PK_CATEGORIA_SUP = 'H' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'J' OR CategoriaSuperior.PK_CATEGORIA_SUP = 'I') AND DetalleInventariosT.PK_INVENTARIOT = '" + excel.PK_INVENTARIOT + "' AND PrecioSemana.PK_SEMANA = '" + excel.SEMANA + "') a";
                                using (var read = command.ExecuteReader())
                                {
                                    while (read.Read())
                                    {
                                        excel.ACCESORIOSLECTURA = Convert.ToInt32(read["ACCESORIOS"]);
                                        excel.PARESLECTURA = Convert.ToInt32(read["PARES"]);
                                        excel.PRECIOLECTURAA = Convert.ToDecimal(read["PRECIOA"]);
                                        excel.PRECIOLECTURAP = Convert.ToDecimal(read["PRECIOP"]);
                                    }
                                }

                                excel.ACCESORIOSDIFERENCIA = (excel.ACCESORIOSINICIAL - excel.ACCESORIOSLECTURA) * -1;
                                excel.PARESDIFERENCIA = (excel.PARESINICIAL - excel.PARESLECTURA) * -1;
                                excel.PRECIODIFERENCIAA = (excel.PRECIOINICIALA - excel.PRECIOLECTURAA) * -1;
                                excel.PRECIODIFERENCIAP = (excel.PRECIOINICIALP - excel.PRECIOLECTURAP) * -1;

                                excel.TOTALINICIALU = excel.PARESINICIAL + excel.ACCESORIOSINICIAL;
                                excel.TOTALINICIALP = excel.PRECIOINICIALP + excel.PRECIOINICIALA;
                                excel.TOTALLECTURAU = excel.PARESLECTURA + excel.ACCESORIOSLECTURA;
                                excel.TOTALLECTURAP = excel.PRECIOLECTURAP + excel.PRECIOLECTURAA;
                                excel.TOTALDIFERENCIAU = excel.PARESDIFERENCIA + excel.ACCESORIOSDIFERENCIA;
                                excel.TOTALDIFERENCIAP = excel.PRECIODIFERENCIAP + excel.PRECIODIFERENCIAA;
                                lista.Add(excel);
                            }
                        }
                    }
                    connection.Close();
                }

                using (var libro = new XLWorkbook())
                {
                    var hoja = libro.Worksheets.Add("Inventario");
                    var currentRow = 1;
                    hoja.Cell(currentRow, 1).Value = "TIENDA";
                    hoja.Cell(currentRow, 2).Value = "CODIGO";
                    hoja.Cell(currentRow, 3).Value = "SEMANA";
                    hoja.Cell(currentRow, 4).Value = "FECHA INICIO";
                    hoja.Cell(currentRow, 5).Value = "FECHA FIN";
                    hoja.Cell(currentRow, 6).Value = "HORA INICIO";
                    hoja.Cell(currentRow, 7).Value = "HORA FIN";
                    hoja.Cell(currentRow, 8).Value = "USUARIO";
                    hoja.Cell(currentRow, 9).Value = "CANTIDAD INICIAL A";
                    hoja.Cell(currentRow, 10).Value = "CANTIDAD INICIAL P";
                    hoja.Cell(currentRow, 11).Value = "TOTAL INICIAL U";
                    hoja.Cell(currentRow, 12).Value = "PRECIO INICIAL A";
                    hoja.Cell(currentRow, 13).Value = "PRECIO INICIAL P";
                    hoja.Cell(currentRow, 14).Value = "TOTAL INICIAL PR";
                    hoja.Cell(currentRow, 15).Value = "CANTIDAD LECTURA A";
                    hoja.Cell(currentRow, 16).Value = "CANTIDAD LECTURA P";
                    hoja.Cell(currentRow, 17).Value = "TOTAL LECTURA U";
                    hoja.Cell(currentRow, 18).Value = "PRECIO LECTURA A";
                    hoja.Cell(currentRow, 19).Value = "PRECIO LECTURA P";
                    hoja.Cell(currentRow, 20).Value = "TOTAL LECTURA PR";
                    hoja.Cell(currentRow, 21).Value = "CANTIDAD DIFERENCIA A";
                    hoja.Cell(currentRow, 22).Value = "CANTIDAD DIFERENCIA P";
                    hoja.Cell(currentRow, 23).Value = "TOTAL DIFERENCIA U";
                    hoja.Cell(currentRow, 24).Value = "PRECIO DIFERENCIA A";
                    hoja.Cell(currentRow, 25).Value = "PRECIO DIFERENCIA P";
                    hoja.Cell(currentRow, 26).Value = "TOTAL DIFERENCIA PR";
                    hoja.ColumnsUsed().AdjustToContents();

                    foreach (var l in lista)
                    {

                        currentRow++;
                        hoja.Cell(currentRow, 1).SetValue(l.PK_TIENDA.ToString());
                        hoja.Cell(currentRow, 2).SetValue(l.CODIGO.ToString());
                        hoja.Cell(currentRow, 3).SetValue(l.SEMANA.ToString());
                        hoja.Cell(currentRow, 4).SetValue(l.FECHAINICIO.ToString());
                        hoja.Cell(currentRow, 5).SetValue(l.FECHAFIN.ToString());
                        hoja.Cell(currentRow, 6).SetValue(l.HORAINICIO.ToString());
                        hoja.Cell(currentRow, 7).SetValue(l.HORAFIN.ToString());
                        hoja.Cell(currentRow, 8).SetValue(l.USUARIO.ToString());
                        hoja.Cell(currentRow, 9).Value = l.ACCESORIOSINICIAL;
                        hoja.Cell(currentRow, 10).Value = l.PARESINICIAL;
                        hoja.Cell(currentRow, 11).Value = l.TOTALINICIALU;
                        hoja.Cell(currentRow, 12).Value = l.PRECIOINICIALA;
                        hoja.Cell(currentRow, 13).Value = l.PRECIOINICIALP;
                        hoja.Cell(currentRow, 14).Value = l.TOTALINICIALP;
                        hoja.Cell(currentRow, 15).Value = l.ACCESORIOSLECTURA;
                        hoja.Cell(currentRow, 16).Value = l.PARESLECTURA;
                        hoja.Cell(currentRow, 17).Value = l.TOTALLECTURAU;
                        hoja.Cell(currentRow, 18).Value = l.PRECIOLECTURAA;
                        hoja.Cell(currentRow, 19).Value = l.PRECIOLECTURAP;
                        hoja.Cell(currentRow, 20).Value = l.TOTALLECTURAP;
                        hoja.Cell(currentRow, 21).Value = l.ACCESORIOSDIFERENCIA;
                        hoja.Cell(currentRow, 22).Value = l.PARESDIFERENCIA;
                        hoja.Cell(currentRow, 23).Value = l.TOTALDIFERENCIAU;
                        hoja.Cell(currentRow, 24).Value = l.PRECIODIFERENCIAA;
                        hoja.Cell(currentRow, 25).Value = l.PRECIODIFERENCIAP;
                        hoja.Cell(currentRow, 26).Value = l.TOTALDIFERENCIAP;
                    }

                    using (var memoria = new MemoryStream())
                    {
                        libro.SaveAs(memoria);
                        var content = memoria.ToArray();
                        var nombreExcel = string.Concat("Inventarios", ".xlsx");

                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Generar(int id, int pk, int td)
        {
            try
            {
                string dia = DateTime.Now.ToString("yyyy-MM-dd");
                string fecha = DateTime.Now.ToString("yyyyMMdd");
                string hora = DateTime.Now.ToString("HHmmss");
                string hoy = fecha + hora;
                var lista = new List<Models.DetallesInventarioT.EnviarModel>();

                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT SUBSTR(CODIGO,(INSTR(CODIGO,'-') +1)) FROM InventariosT WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        string numero = Convert.ToString(command.ExecuteScalar());
                        numero = numero.PadLeft(4, '0');
                        string linea1 = "<Header line_count=" + '"' + "" + 1 + "" + '"' + " download_id=" + '"' + "INV_DOC_" + hoy + "" + '_' + "" + td + "" + '"' + " target_org_node=" + '"' + "STORE:" + td + "" + '"' + " deployment_name=" + '"' + "INV_DOC_" + hoy + "" + '_' + "" + td + "" + '"' + " download_time=" + '"' + "IMMEDIATE" + '"' + " apply_immediately=" + '"' + "true " + '"' + " />";
                        string linea2 = "INSERT|INV_DOC|RECEIVING|REPLENISHMENT|" + td + "|006807000" + numero + "||OPEN|" + dia + "|||00680|FABRICA MANACO|HOME_OFFICE|||||||||||||||||||||||||||||||7000" + numero + "";
                        string ruta1 = ("D:\\Interfaces\\INV_DOC_" + hoy + "_" + td + ".MNT");
                        using (StreamWriter sw1 = new StreamWriter(ruta1))
                        {
                            sw1.WriteLine(linea1);
                            sw1.WriteLine(linea2);
                        }

                        //string path1 = ("\\\\10.0.0.10\\inventarios\\INV_DOC_" + hoy + "_" + td + ".MNT");
                        //var text1 = new FileInfo(ruta1);
                        //text1.CopyTo(path1);

                        string ruta2 = ("D:\\Interfaces\\CARTON_" + hoy + "_" + td + ".MNT");
                        string linea3 = "<Header line_count=" + '"' + "" + 1 + "" + '"' + " download_id=" + '"' + "CARTON_" + hoy + "" + '_' + "" + td + "" + '"' + " target_org_node=" + '"' + "STORE:" + td + "" + '"' + " deployment_name=" + '"' + "CARTON_" + hoy + "" + '_' + "" + td + "" + '"' + " download_time=" + '"' + "IMMEDIATE" + '"' + " apply_immediately=" + '"' + "true " + '"' + " />";
                        string linea4 = "INSERT|CARTON|RECEIVING||" + td + "|006807000" + numero + "|006807000" + numero + "|OPEN|HOME_OFFICE|";
                        using (StreamWriter sw2 = new StreamWriter(ruta2))
                        {
                            sw2.WriteLine(linea3);
                            sw2.WriteLine(linea4);
                        }

                        //string path2 = ("\\\\10.0.0.10\\inventarios\\CARTON_" + hoy + "_" + td + ".MNT");
                        //var text2 = new FileInfo(ruta2);
                        //text2.CopyTo(path2);

                        command.CommandText = @"SELECT PK_ARTICULO, OPCION_TALLA, TALLA, COSTO, (SUM(EXISTENCIAS) - SUM(CONTADOS)) * -1 AS CANTIDAD FROM (SELECT DetalleInventariosT.PK_ARTICULO, Articulos.OPCION_TALLA, DetalleInventariosT.TALLA, 0 AS EXISTENCIAS, DetalleInventariosT.CANTIDAD AS CONTADOS, PrecioSemana.COSTO FROM  Articulos INNER JOIN DetalleInventariosT ON Articulos.PK_ARTICULO = DetalleInventariosT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE DetalleInventariosT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "' UNION ALL SELECT ExistenciasT.PK_ARTICULO, Articulos.OPCION_TALLA, ExistenciasT.TALLA, ExistenciasT.CANTIDAD AS EXISTENCIAS, 0 AS CONTADOS, PrecioSemana.COSTO FROM  Articulos INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE ExistenciasT.PK_INVENTARIOT = '" + id + "' AND PrecioSemana.PK_SEMANA = '" + pk + "') a GROUP BY PK_ARTICULO, OPCION_TALLA, TALLA, COSTO ORDER BY PK_ARTICULO";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int diferencia = Convert.ToInt32(reader["CANTIDAD"]);
                                if (diferencia != 0)
                                {
                                    command.CommandText = @"SELECT POSICION FROM TALLAS WHERE GRUPO = '" + Convert.ToInt32(reader["PK_ARTICULO"].ToString().Substring(0, 1)) + "' AND TALLA = '" + reader["TALLA"].ToString() + "' AND OPCION = '" + reader["OPCION_TALLA"].ToString() + "'";
                                    using (var read = command.ExecuteReader())
                                    {
                                        while (read.Read())
                                        {
                                            Models.DetallesInventarioT.EnviarModel articulos = new Models.DetallesInventarioT.EnviarModel();
                                            articulos.PK_ARTICULO = reader["PK_ARTICULO"].ToString() + "0" + read["POSICION"].ToString();
                                            articulos.TALLA = reader["TALLA"].ToString();
                                            articulos.VENTA = Convert.ToDecimal(reader["COSTO"]);
                                            articulos.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                            lista.Add(articulos);
                                        }
                                    }
                                }
                            }
                        }

                        int contador = lista.Count;
                        string ruta3 = ("D:\\Interfaces\\INV_DOC_LINE_ITEM_" + hoy + "_" + td + ".MNT");
                        string linea5 = "<Header line_count=" + '"' + "" + contador + "" + '"' + " download_id=" + '"' + "INV_DOC_LINE_ITEM_" + hoy + "" + '_' + "" + td + "" + '"' + " target_org_node=" + '"' + "STORE:" + td + "" + '"' + " deployment_name=" + '"' + "INV_DOC_LINE_ITEM_" + hoy + "" + '_' + "" + td + "" + '"' + " download_time=" + '"' + "IMMEDIATE" + '"' + " apply_immediately=" + '"' + "true " + '"' + " />";
                        using (StreamWriter sw3 = new StreamWriter(ruta3))
                        {
                            int i = 1;
                            sw3.WriteLine(linea5);
                            foreach (var l in lista)
                            {
                                string linea6 = "INSERT|INV_DOC_LINE_ITEM|RECEIVING||006807000" + numero + "|" + td + "|006807000" + numero + "|" + i + "|OPEN|" + l.PK_ARTICULO + "|" + l.CANTIDAD + "|||||||HOME_OFFICE|ITEM|||||||" + l.VENTA + "||";
                                sw3.WriteLine(linea6);
                                i++;
                            }
                        }

                        //string path3 = ("\\\\10.0.0.10\\inventarios\\INV_DOC_LINE_ITEM_" + hoy + "_" + td + ".MNT");
                        //var text3 = new FileInfo(ruta3);
                        //text3.CopyTo(path3);

                        string origen = "sistemas.correos23@gmail.com";
                        //string destino = "sergio.dominguez@agencia.com.bo";
                        string destino = "hugo.oropeza@bata.com";
                        //string oculto = "david.arce@bata.com";
                        string clave = "mkfoitsgeusyrxnf";
                        string titulo = "Interfaces" + " " + td;
                        string cuerpo = "Generado Automaticamente en fecha:" + " " + dia;
                        string dir1 = ruta1;
                        string dir2 = ruta2;
                        string dir3 = ruta3;

                        MailMessage oMailMessage = new MailMessage();
                        oMailMessage.From = new MailAddress(origen);
                        oMailMessage.To.Add(destino);
                        //oMailMessage.Bcc.Add(oculto);
                        oMailMessage.Subject = titulo;
                        oMailMessage.Body = cuerpo;
                        oMailMessage.Attachments.Add(new Attachment(dir1));
                        oMailMessage.Attachments.Add(new Attachment(dir2));
                        oMailMessage.Attachments.Add(new Attachment(dir3));
                        oMailMessage.IsBodyHtml = true;
                        SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
                        oSmtpClient.EnableSsl = true;
                        oSmtpClient.UseDefaultCredentials = false;
                        oSmtpClient.Port = 587;
                        oSmtpClient.Credentials = new System.Net.NetworkCredential(origen, clave);

                        oSmtpClient.Send(oMailMessage);
                        oSmtpClient.Dispose();

                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Copiar(int id, int pk)
        {
            try
            {
                bool valido;
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT COUNT(*) FROM PRECIOSINVENTARIOST WHERE PK_INVENTARIOT ='"+ id +"'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                        {
                            command.CommandText = @"SELECT PK_INVENTARIOT, PK_ARTICULO, PK_SEMANA, VENTA  FROM (SELECT EXISTENCIAST.PK_INVENTARIOT, EXISTENCIAST.PK_ARTICULO, PRECIOSEMANA.PK_SEMANA, PRECIOSEMANA.VENTA FROM ARTICULOS INNER JOIN EXISTENCIAST ON ARTICULOS.PK_ARTICULO = EXISTENCIAST.PK_ARTICULO INNER JOIN PRECIOSEMANA ON Articulos.PK_ARTICULO = PRECIOSEMANA.PK_ARTICULO WHERE EXISTENCIAST.PK_INVENTARIOT = '" + id + "' AND PRECIOSEMANA.PK_SEMANA = '" + pk + "' UNION ALL SELECT DETALLEINVENTARIOST.PK_INVENTARIOT, DETALLEINVENTARIOST.PK_ARTICULO, PRECIOSEMANA.PK_SEMANA, PRECIOSEMANA.VENTA FROM ARTICULOS INNER JOIN DETALLEINVENTARIOST ON ARTICULOS.PK_ARTICULO = DETALLEINVENTARIOST.PK_ARTICULO INNER JOIN PRECIOSEMANA ON Articulos.PK_ARTICULO = PRECIOSEMANA.PK_ARTICULO WHERE DETALLEINVENTARIOST.PK_INVENTARIOT = '" + id + "' AND PRECIOSEMANA.PK_SEMANA = '" + pk + "') a GROUP BY PK_INVENTARIOT, PK_ARTICULO, PK_SEMANA, VENTA ORDER BY PK_ARTICULO";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    command.CommandText = @"INSERT INTO PRECIOSINVENTARIOST (PK_INVENTARIOT, PK_ARTICULO, PK_SEMANA, VENTA) VALUES ('" + reader["PK_INVENTARIOT"].ToString() + "', '" + reader["PK_ARTICULO"].ToString() + "', '" + reader["PK_SEMANA"].ToString() + "', '" + reader["VENTA"].ToString() + "')";
                                    command.ExecuteNonQuery();
                                }
                            }
                            valido = true;
                        }
                        else
                        {
                            valido = false;
                        }
                    }
                    connection.Close();
                }
                return valido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EliminarInicial(int id)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (var command = new OracleCommand(@"DELETE FROM PLANILLAINICIAL WHERE PK_INVENTARIOT = '" + id + "'", connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private void EliminarFinal(int id)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (var command = new OracleCommand(@"DELETE FROM PLANILLAFINAL WHERE PK_INVENTARIOT = '" + id + "'", connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private void EliminarManual(int id)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (var command = new OracleCommand(@"DELETE FROM PLANILLAMANUAL WHERE PK_INVENTARIOT = '" + id + "'", connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private bool Validar(string articulos, string tallas)
        {
            try
            {
                bool valido;
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM Tallas, Articulos WHERE OPCION = OPCION_TALLA AND SUBSTR(PK_ARTICULO,1,1) = GRUPO AND PK_ARTICULO = '" + articulos + "' AND TALLA = '" + tallas + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            valido = true;
                        }
                        else
                        {
                            valido = false;
                        }

                    }
                }
                return valido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public class ListComparer : IEqualityComparer<ListarModel>
        {
            public bool Equals(ListarModel item1, ListarModel item2)
            {
                return item1.PK_ARTICULO == item2.PK_ARTICULO && item1.TALLA == item2.TALLA;
            }

            public int GetHashCode(ListarModel item)
            {
                int hcode = item.PK_ARTICULO.Length;
                return hcode.GetHashCode();
            }
        }

    }
}
