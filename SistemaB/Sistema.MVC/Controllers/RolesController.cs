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
    [Authorize(Roles = "ADMINISTRADOR")]
    [Route("api/[controller]")]
    [ApiController]

    public class RolesController : Controller
    {
        public readonly string _connectionString;

        public RolesController(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("Conexion");
        }

        [HttpGet("[action]")]
        public IEnumerable<Models.Roles.ListarModel> Listar()
        {
            try
            {
                List<Models.Roles.ListarModel> lista = new List<Models.Roles.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT PK_ROL, ROL, ESTADO FROM Roles ORDER BY PK_ROL ", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.Roles.ListarModel rol = new Models.Roles.ListarModel();
                                rol.PK_ROL = Convert.ToInt32(reader["PK_ROL"]);
                                rol.ROL = reader["ROL"].ToString();
                                rol.ESTADO = reader["ESTADO"].ToString();
                                lista.Add(rol);
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

        [HttpGet("[action]")]
        public IEnumerable<Models.Roles.SeleccionarModel> Seleccionar()
        {
            try
            {
                List<Models.Roles.SeleccionarModel> lista = new List<Models.Roles.SeleccionarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT PK_ROL, ROL FROM Roles WHERE ESTADO = '" + "ACTIVO" +"'", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.Roles.SeleccionarModel rol = new Models.Roles.SeleccionarModel();
                                rol.PK_ROL = Convert.ToInt32(reader["PK_ROL"]);
                                rol.ROL = reader["ROL"].ToString();
                                lista.Add(rol);
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
    }
}
