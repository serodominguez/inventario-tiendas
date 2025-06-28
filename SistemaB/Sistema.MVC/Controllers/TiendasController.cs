using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Sistema.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TiendasController : Controller
    {
        public readonly string _connectionString;

        public TiendasController( IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("Conexion");
        }

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR")]
        [HttpGet("[action]")]
        public IEnumerable<Models.Tiendas.ActualizarModel> Listar()
        {
            try
            {
                List<Models.Tiendas.ActualizarModel> lista = new List<Models.Tiendas.ActualizarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT PK_TIENDA, PK_TIPO_TDA, NOMBRE, DIRECCION, CIUDAD, CONSIGNATARIO, CARNET, RAZONSOCIAL, NIT FROM Tiendas", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.Tiendas.ActualizarModel tienda = new Models.Tiendas.ActualizarModel();
                                tienda.PK_TIENDA = Convert.ToInt32(reader["PK_TIENDA"]);
                                tienda.PK_TIPO_TDA = reader["PK_TIPO_TDA"].ToString();
                                tienda.NOMBRE = reader["NOMBRE"].ToString();
                                tienda.DIRECCION = reader["DIRECCION"].ToString();
                                tienda.CIUDAD = reader["CIUDAD"].ToString();
                                tienda.CONSIGNATARIO = reader["CONSIGNATARIO"].ToString();
                                tienda.CARNET = reader["CARNET"].ToString();
                                tienda.RAZONSOCIAL = reader["RAZONSOCIAL"].ToString();
                                tienda.NIT = reader["NIT"].ToString();
                                lista.Add(tienda);
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
        public IEnumerable<Models.Tiendas.SeleccionarModel> Seleccionar()
        {
            try
            {
                List<Models.Tiendas.SeleccionarModel> lista = new List<Models.Tiendas.SeleccionarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT PK_TIENDA, NOMBRE FROM Tiendas WHERE ACTIVO = 'S' ORDER BY PK_TIENDA ASC", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.Tiendas.SeleccionarModel tienda = new Models.Tiendas.SeleccionarModel();
                                tienda.PK_TIENDA = Convert.ToInt32(reader["PK_TIENDA"]);
                                tienda.NOMBRE = reader["NOMBRE"].ToString();
                                lista.Add(tienda);
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

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR")]
        [HttpPost("[action]")]
        public IActionResult Crear([FromBody] Models.Tiendas.ActualizarModel model)
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
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM Tiendas WHERE PK_TIENDA = '" + model.PK_TIENDA + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            return BadRequest("La tienda ya existe!");
                        }
                        else
                        {
                            command.CommandText = @"INSERT INTO Tiendas (PK_TIENDA, PK_TIPO_TDA, NOMBRE, DIRECCION, CIUDAD, CONSIGNATARIO, CARNET, RAZONSOCIAL, NIT, ACTIVO) VALUES ('" + model.PK_TIENDA + "', '" + model.PK_TIPO_TDA + "', '" + model.NOMBRE + "', '" + model.DIRECCION + "', '" + model.CIUDAD + "', '" + model.CONSIGNATARIO + "', '" + model.CARNET + "', '" + model.RAZONSOCIAL + "', '" + model.NIT + "', '" + "S" + "')";
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

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR")]
        [HttpPut("[action]")]
        public IActionResult Actualizar([FromBody] Models.Tiendas.ActualizarModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.PK_TIENDA <= 0)
            {
                return BadRequest();
            }

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM Tiendas WHERE PK_TIENDA = '" + model.PK_TIENDA + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"UPDATE Tiendas SET PK_TIPO_TDA = '" + model.PK_TIPO_TDA + "', NOMBRE = '" + model.NOMBRE + "', DIRECCION = '" + model.DIRECCION + "', CIUDAD = '" + model.CIUDAD + "', CONSIGNATARIO = '" + model.CONSIGNATARIO + "', CARNET = '" + model.CARNET + "', RAZONSOCIAL = '" + model.RAZONSOCIAL + "', NIT = '" + model.NIT+ "' WHERE PK_TIENDA = '" + model.PK_TIENDA + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            return NotFound();
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
    }
}
