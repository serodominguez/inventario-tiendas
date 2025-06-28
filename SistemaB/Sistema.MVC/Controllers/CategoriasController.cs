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
    [Authorize(Roles = "ADMINISTRADOR,AUDITOR,CONSIGNATARIO")]
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriasController : Controller
    {
        public readonly string _connectionString;
        public CategoriasController(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("Conexion");
        }

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR, CONSIGNATARIO")]
        [HttpGet("[action]")]
        public IEnumerable<Models.CategoriaSuperior.SeleccionarModel> Seleccionar()
        {
            try
            {
                List<Models.CategoriaSuperior.SeleccionarModel> lista = new List<Models.CategoriaSuperior.SeleccionarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT PK_CATEGORIA_SUP, DESCRIPCION FROM CategoriaSuperior WHERE PK_CATEGORIA_SUP <> '0' AND PK_CATEGORIA_SUP <> '-1'", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.CategoriaSuperior.SeleccionarModel superior = new Models.CategoriaSuperior.SeleccionarModel();
                                superior.PK_CATEGORIA_SUP = reader["PK_CATEGORIA_SUP"].ToString();
                                superior.DESCRIPCION = reader["DESCRIPCION"].ToString();
                                lista.Add(superior);
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
