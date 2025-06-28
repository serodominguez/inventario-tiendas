using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ExistenciasTController : Controller
    {
        public readonly string _connectionString;
        public readonly string _connectionStrings;

        public ExistenciasTController(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("Conexion");
            _connectionStrings = _configuration.GetConnectionString("Connection");
        }
        
        private string semana;
        private string codigo;
        private int tienda;

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("[action]")]
        public  IEnumerable<Models.ExistenciasT.ListarModel> Listar()
        {
            try
            {
                List<Models.ExistenciasT.ListarModel> lista = new List<Models.ExistenciasT.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT InventariosT.PK_INVENTARIOT, InventariosT.PK_TIENDA, Tiendas.NOMBRE, Tiendas.CONSIGNATARIO, InventariosT.CODIGO, InventariosT.SEMANA, InventariosT.ESTADO, Usuarios.USUARIO, Roles.ROL FROM InventariosT INNER JOIN Tiendas ON InventariosT.PK_TIENDA = Tiendas.PK_TIENDA INNER JOIN Usuarios ON InventariosT.PK_USUARIO = Usuarios.PK_USUARIO INNER JOIN Roles ON Usuarios.PK_ROL = Roles.PK_ROL WHERE InventariosT.ESTADO = 'MANUAL' OR InventariosT.ESTADO = 'LECTURA' ORDER BY PK_INVENTARIOT DESC", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.ExistenciasT.ListarModel existencia = new Models.ExistenciasT.ListarModel();
                                existencia.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                existencia.PK_TIENDA = Convert.ToInt32(reader["PK_TIENDA"]);
                                existencia.TIENDA = reader["PK_TIENDA"].ToString() + " " + reader["NOMBRE"].ToString();
                                existencia.CONSIGNATARIO = reader["CONSIGNATARIO"].ToString();
                                existencia.USUARIO = reader["USUARIO"].ToString();
                                existencia.CODIGO = reader["CODIGO"].ToString();
                                existencia.SEMANA = reader["SEMANA"].ToString();
                                existencia.ESTADO = reader["ESTADO"].ToString();
                                existencia.ROL = reader["ROL"].ToString();
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

        [Authorize(Roles = "AUDITOR, CONSIGNATARIO")]
        [HttpGet("[action]/{id}")]
        public IEnumerable<Models.ExistenciasT.ListarModel> ListarTiendas([FromRoute] int id)
        {
            try
            {
                List<Models.ExistenciasT.ListarModel> lista = new List<Models.ExistenciasT.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT InventariosT.PK_INVENTARIOT, InventariosT.PK_TIENDA, Tiendas.NOMBRE, Tiendas.CONSIGNATARIO, InventariosT.CODIGO, InventariosT.SEMANA, InventariosT.ESTADO, Usuarios.USUARIO, Roles.ROL FROM InventariosT INNER JOIN Tiendas ON InventariosT.PK_TIENDA = Tiendas.PK_TIENDA INNER JOIN Usuarios ON InventariosT.PK_USUARIO = Usuarios.PK_USUARIO INNER JOIN Roles ON Usuarios.PK_ROL = Roles.PK_ROL WHERE (InventariosT.ESTADO = 'MANUAL' OR InventariosT.ESTADO = 'LECTURA') AND InventariosT.PK_USUARIO = '" + id + "' ORDER BY InventariosT.CODIGO DESC", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.ExistenciasT.ListarModel existencia = new Models.ExistenciasT.ListarModel();
                                existencia.PK_INVENTARIOT = Convert.ToInt32(reader["PK_INVENTARIOT"]);
                                existencia.PK_TIENDA = Convert.ToInt32(reader["PK_TIENDA"]);
                                existencia.TIENDA = reader["PK_TIENDA"].ToString() + " " + reader["NOMBRE"].ToString();
                                existencia.CONSIGNATARIO = reader["CONSIGNATARIO"].ToString();
                                existencia.USUARIO = reader["USUARIO"].ToString();
                                existencia.CODIGO = reader["CODIGO"].ToString();
                                existencia.SEMANA = reader["SEMANA"].ToString();
                                existencia.ESTADO = reader["ESTADO"].ToString();
                                existencia.ROL = reader["ROL"].ToString();
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

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR, CONSIGNATARIO")]
        [HttpGet("[action]/{id}/{pk}")]
        public IEnumerable<Models.ExistenciasT.ReporteModel> ListarImportado([FromRoute] int id, int pk)
        {
            try
            {
                List<Models.ExistenciasT.ReporteModel> lista = new List<Models.ExistenciasT.ReporteModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        using (OracleCommand command = new OracleCommand(@"SELECT CategoriaSuperior.DESCRIPCION AS CATEGORIA, SUM(ExistenciasT.CANTIDAD) AS CANTIDAD, SUM(ExistenciasT.CANTIDAD * PrecioSemana.VENTA) AS VALOR FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP INNER JOIN ExistenciasT ON Articulos.PK_ARTICULO = ExistenciasT.PK_ARTICULO INNER JOIN PrecioSemana ON Articulos.PK_ARTICULO = PrecioSemana.PK_ARTICULO WHERE ExistenciasT.PK_INVENTARIOT = '"+ id +"' AND PrecioSemana.PK_SEMANA = '"+ pk +"' GROUP BY CategoriaSuperior.DESCRIPCION", connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.ExistenciasT.ReporteModel existencia = new Models.ExistenciasT.ReporteModel();
                                    existencia.CATEGORIA = reader["CATEGORIA"].ToString();
                                    existencia.TOTAL = Convert.ToDecimal(reader["CANTIDAD"]);
                                    existencia.MONTO = Convert.ToDecimal(reader["VALOR"]);
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

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR, CONSIGNATARIO")]
        [HttpPost("[action]")]
        public IActionResult Generar(Models.ExistenciasT.RegistrarModel model)
        {
            var id = "";
            if (model.categorias.Count > 10)
            {
                tienda = model.PK_TIENDA;
                ObtenerSemana();
                ReadXML();
                Correlativo(model.PK_TIENDA);
                string hoy = DateTime.Now.ToString("yyyy-MM-dd");
                using (var connectionNova = new OracleConnection(_connectionStrings))
                {
                    using (var commandNova = new OracleCommand(@"SELECT SARTICULO, SCANTIDAD, STALLA FROM STOCK_POS WHERE PK_SEMANA = '" + semana + "' AND STIENDA = '" + model.PK_TIENDA + "'", connectionNova))
                    {
                        if (connectionNova.State == ConnectionState.Closed)
                            connectionNova.Open();
                        OracleDataReader reader = commandNova.ExecuteReader();

                        using (var connection = new OracleConnection(_connectionString))
                        {
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();

                            using (var command = new OracleCommand(@"SELECT COUNT(*) FROM InventariosT WHERE PK_TIENDA = '" + model.PK_TIENDA + "' AND FECHAINICIO = '" + hoy + "'", connection))
                            {
                                if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                                {
                                    ObtenerSecuencia(out int secuencia);
                                    id = secuencia.ToString();
                                    command.CommandText = @"INSERT INTO InventariosT (PK_INVENTARIOT, PK_TIENDA, CODIGO, SEMANA, FECHAINICIO, HORAINICIO, ESTADO, PK_USUARIO) VALUES ('" + secuencia + "', '" + model.PK_TIENDA + "', '" + codigo + "', '" + semana + "', '" + hoy + "', '" + model.HORAINICIO + "', '" + model.PK_TIPO + "', '" + model.USUARIO + "')";
                                   command.ExecuteNonQuery();

                                    while (reader.Read())
                                    {
                                        try
                                        {
                                            command.CommandText = @"INSERT INTO ExistenciasT (PK_ARTICULO, CANTIDAD, TALLA, PK_INVENTARIOT) VALUES ('" + reader["SARTICULO"].ToString() + "', '" + Convert.ToInt32(reader["SCANTIDAD"]) + "', '" + reader["STALLA"].ToString() + "', '" + secuencia + "')";
                                            command.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                            }
                            connection.Close();
                        }
                    }
                    connectionNova.Close();
                }
            }
            else
            {
                tienda = model.PK_TIENDA;
                ObtenerSemana();
                ReadXML();
                Correlativo(model.PK_TIENDA);
                string hoy = DateTime.Now.ToString("yyyy-MM-dd");
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM InventariosT WHERE PK_TIENDA = '" + model.PK_TIENDA + "' AND FECHAINICIO = '" + hoy + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                        {
                            ObtenerSecuencia(out int secuencia);
                            command.CommandText = @"INSERT INTO InventariosT (PK_INVENTARIOT, PK_TIENDA, CODIGO, SEMANA, FECHAINICIO, HORAINICIO, ESTADO, PK_USUARIO) VALUES ('" + secuencia + "', '" + model.PK_TIENDA + "', '" + codigo + "', '" + semana + "', '" + hoy + "', '" + model.HORAINICIO + "', '" + model.PK_TIPO + "', '" + model.USUARIO + "')";
                            command.ExecuteNonQuery();

                            using (var connectionNova = new OracleConnection(_connectionStrings))
                            {
                                foreach (var lista in model.categorias)
                                {
                                    using (var oracleCommand = new OracleCommand(@"SELECT SARTICULO, SCANTIDAD, STALLA FROM STOCK_POS WHERE PK_SEMANA = '" + semana + "' AND STIENDA = '" + model.PK_TIENDA + "'", connectionNova))
                                    {
                                        if (connectionNova.State == ConnectionState.Closed)
                                            connectionNova.Open();
                                        OracleDataReader reader = oracleCommand.ExecuteReader();
                                        while (reader.Read())
                                        {
                                            try
                                            {
                                                command.CommandText = @"SELECT COUNT(Articulos.PK_ARTICULO) FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP
						                                                 WHERE Articulos.PK_ARTICULO = '" + reader["SARTICULO"].ToString() + "' AND CategoriaSuperior.PK_CATEGORIA_SUP = '" + lista.PK_CATEGORIA_SUP + "'";

                                                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                                                {

                                                    command.CommandText = @"INSERT INTO ExistenciasT (PK_ARTICULO, CANTIDAD, TALLA, PK_INVENTARIOT) VALUES ('" + reader["SARTICULO"].ToString() + "', '" + Convert.ToInt32(reader["SCANTIDAD"]) + "', '" + reader["STALLA"].ToString() + "', '" + secuencia + "')";
                                                    command.ExecuteNonQuery();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                                connectionNova.Close();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            return Ok(id);
        }

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR, CONSIGNATARIO")]
        [HttpPut("[action]")]
        public IActionResult Actualizar(Models.ExistenciasT.ActualizarModel model)
        {
            if (Validar(model.PK_INVENTARIOT))
            {
                return NotFound();
            }
            else
            {
                if (model.categorias.Count > 10)
                {
                    tienda = model.PK_TIENDA;
                    ObtenerSemana();
                    ReadXML();
                    using (var connectionNova = new OracleConnection(_connectionStrings))
                    {
                        using (var commandNova = new OracleCommand(@"SELECT SARTICULO, SCANTIDAD, STALLA FROM STOCK_POS WHERE PK_SEMANA = '" + semana + "' AND STIENDA = '" + model.PK_TIENDA + "'", connectionNova))
                        {
                            if (connectionNova.State == ConnectionState.Closed)
                                connectionNova.Open();
                            OracleDataReader reader = commandNova.ExecuteReader();
                            using (var connection = new OracleConnection(_connectionString))
                            {
                                if (connection.State == ConnectionState.Closed)
                                    connection.Open();

                                    while (reader.Read())
                                    {
                                        try
                                        {
                                            using (var command = new OracleCommand(@"INSERT INTO ExistenciasT (PK_ARTICULO, CANTIDAD, TALLA, PK_INVENTARIOT) VALUES ('" + reader["SARTICULO"].ToString() + "', '" + Convert.ToInt32(reader["SCANTIDAD"]) + "', '" + reader["STALLA"].ToString() + "', '" + model.PK_INVENTARIOT + "')", connection))
                                            {
                                                command.ExecuteNonQuery();
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                connection.Close();
                            }
                        }
                        connectionNova.Close();
                    }
                }
                else
                {
                    tienda = model.PK_TIENDA;
                    ObtenerSemana();
                    ReadXML();
                    using (var connection = new OracleConnection(_connectionString))
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        using (var command = new OracleCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.Text;

                            using (OracleConnection connectionNova = new OracleConnection(_connectionStrings))
                            {
                                foreach (var lista in model.categorias)
                                {
                                    using (OracleCommand commnadNova = new OracleCommand(@"SELECT SARTICULO, SCANTIDAD, STALLA FROM STOCK_POS WHERE PK_SEMANA = '" + semana + "' AND STIENDA = '" + model.PK_TIENDA + "'", connectionNova))
                                    {
                                        if (connectionNova.State == ConnectionState.Closed)
                                            connectionNova.Open();
                                        OracleDataReader reader = commnadNova.ExecuteReader();
                                        while (reader.Read())
                                        {
                                            try
                                            {
                                                command.CommandText = @"SELECT COUNT(Articulos.PK_ARTICULO) FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA INNER JOIN CategoriaSuperior ON Categorias.PK_CATEGORIA_SUP = CategoriaSuperior.PK_CATEGORIA_SUP
						                                                 WHERE Articulos.PK_ARTICULO = '" + reader["SARTICULO"].ToString() + "' AND CategoriaSuperior.PK_CATEGORIA_SUP = '" + lista.PK_CATEGORIA_SUP + "'";

                                                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                                                {

                                                    command.CommandText = @"INSERT INTO ExistenciasT (PK_ARTICULO, CANTIDAD, TALLA, PK_INVENTARIOT) VALUES ('" + reader["SARTICULO"].ToString() + "', '" + Convert.ToInt32(reader["SCANTIDAD"]) + "', '" + reader["STALLA"].ToString() + "', '" + model.PK_INVENTARIOT + "')";
                                                    command.ExecuteNonQuery();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                                connectionNova.Close();
                            }
                        }
                        connection.Close();
                    }
                }
            }
            return Ok();
        }

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR, CONSIGNATARIO")]
        [HttpPost("[action]")]
        public IActionResult Cargar([FromBody] Models.ExistenciasT.CargarModel model)
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
                    using (var command = new OracleCommand())
                    {
                        ObtenerSemana();
                        Correlativo(model.PK_TIENDA);
                        ObtenerSecuencia(out int secuencia);
                        string hoy = DateTime.Now.ToString("yyyy-MM-dd");
                        command.Connection = connection;
                        command.CommandText = @"INSERT INTO InventariosT (PK_INVENTARIOT, PK_TIENDA, CODIGO, SEMANA, FECHAINICIO, HORAINICIO, ESTADO, PK_USUARIO) VALUES ('" + secuencia + "', '" + model.PK_TIENDA + "', '" + codigo + "', '" + semana + "', '" + hoy + "', '" + model.HORAINICIO + "', '" + model.PK_TIPO + "', '" + model.USUARIO + "')";
                        command.ExecuteNonQuery();
                        foreach (var lista in model.categorias)
                        {
                            foreach (var detalle in model.existencias)
                            {
                                command.CommandText = @"SELECT COUNT(*) FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA WHERE Articulos.PK_ARTICULO = '"+ detalle.PK_ARTICULO +"' AND  Categorias.PK_CATEGORIA_SUP = '"+ lista.PK_CATEGORIA_SUP +"'";
                                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                                {
                                    command.CommandText = @"INSERT INTO ExistenciasT (PK_ARTICULO, CANTIDAD, TALLA, PK_INVENTARIOT) VALUES ('" + detalle.PK_ARTICULO + "', '" + detalle.CANTIDAD + "', '" + detalle.TALLA + "', '" + secuencia + "')";
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

        [Authorize(Roles = "ADMINISTRADOR, AUDITOR, CONSIGNATARIO")]
        [HttpPost("[action]")]
        public IActionResult Modificar([FromBody] Models.ExistenciasT.ModificarModel model)
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
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        foreach (var lista in model.categorias)
                        {
                            foreach (var detalle in model.existencias)
                            {
                                command.CommandText = @"SELECT COUNT(*) FROM Articulos INNER JOIN Categorias ON Articulos.PK_CATEGORIA = Categorias.PK_CATEGORIA WHERE Articulos.PK_ARTICULO = '" + detalle.PK_ARTICULO + "' AND  Categorias.PK_CATEGORIA_SUP = '" + lista.PK_CATEGORIA_SUP + "'";
                                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                                {
                                    command.CommandText = @"INSERT INTO ExistenciasT (PK_ARTICULO, CANTIDAD, TALLA, PK_INVENTARIOT) VALUES ('" + detalle.PK_ARTICULO + "', '" + detalle.CANTIDAD + "', '" + detalle.TALLA + "', '" + model.PK_INVENTARIOT + "')";
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


        [Authorize(Roles = "ADMINISTRADOR, AUDITOR, CONSIGNATARIO")]
        [HttpDelete("[action]/{id}")]
        public IActionResult Eliminar([FromRoute] int id)
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
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM ExistenciasT WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"DELETE FROM ExistenciasT WHERE PK_INVENTARIOT = '" + id + "'";
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
        private void Correlativo(int tienda)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                if (tienda == 23318 || tienda == 26203)
                {
                    using (var command = new OracleCommand(@"SELECT SUBSTR(CODIGO,10,4) CODIGO FROM (SELECT CODIGO FROM INVENTARIOST WHERE PK_TIENDA = '"+ tienda +"' ORDER BY TO_NUMBER(SUBSTR(CODIGO,10,4)) DESC) WHERE ROWNUM = 1", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) < 15)
                        {
                            codigo = "INV" + tienda + "-" + 15;
                        }
                        else
                        {
                            command.CommandText = @"SELECT SUBSTR(CODIGO,10,4) + 1 CODIGO FROM (SELECT CODIGO FROM INVENTARIOST WHERE PK_TIENDA = '" + tienda + "' ORDER BY TO_NUMBER(SUBSTR(CODIGO,10,4)) DESC) WHERE ROWNUM = 1";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    codigo = "INV" + tienda + "-" + Convert.ToString(reader[reader.GetOrdinal("CODIGO")]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (var command = new OracleCommand(@"SELECT SUBSTR(CODIGO,10,4) + 1 CODIGO FROM (SELECT CODIGO FROM INVENTARIOST WHERE PK_TIENDA = '" + tienda + "' ORDER BY TO_NUMBER(SUBSTR(CODIGO,10,4)) DESC) WHERE ROWNUM = 1", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                codigo = "INV" + tienda + "-" + Convert.ToString(reader[reader.GetOrdinal("CODIGO")]);
                            }
                        }

                    }
                }
                connection.Close();
            }
        }

        private void ObtenerSemana()
        {
            string hoy = DateTime.Now.ToString("dd/MM/yyyy");
            using (OracleConnection connection = new OracleConnection(_connectionStrings))
            {
                using (OracleCommand oracleCommand = new OracleCommand())
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    oracleCommand.BindByName = true;
                    oracleCommand.CommandText = @"SELECT PK_SEMANA, INICIO, FIN FROM Semanas WHERE TO_DATE('" + hoy + "', 'dd/mm/rrrr') BETWEEN TO_DATE (inicio, 'dd/mm/rrrr') AND TO_DATE (fin, 'dd/mm/rrrr')";
                    oracleCommand.Connection = connection;
                    OracleDataReader reader = oracleCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        semana = reader["PK_SEMANA"].ToString();
                    }
                    connection.Close();
                }
            }
        }

        private void ReadXML()
        {
            using (OracleConnection connection = new OracleConnection(_connectionStrings))
            {
                try
                {
                    string fecha = DateTime.Now.ToString("yyyy-MM-dd");
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    OracleCommand command = new OracleCommand("XSTORE_PL.READ_XML_ALL", connection);
                    command.Parameters.Add("SYSDATE", OracleDbType.Varchar2).Value = fecha;
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    connection.Close();
                    InsertData();
                }
                catch (Exception ex)
                {
                    InsertData();
                }

            }
        }

        private void InsertData()
        {
            using (OracleConnection connection = new OracleConnection(_connectionStrings))
            {
                try
                {
                    string fecha = DateTime.Now.ToString("yyyy-MM-dd");
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    OracleCommand command = new OracleCommand("XSTORE_PL.PR_INSERT_DATA_RETAIL", connection);
                    command.Parameters.Add("SYSDATE", OracleDbType.Varchar2).Value = fecha;
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    connection.Close();
                    Calcular();
                }
                catch (Exception ex)
                {
                    Calcular();
                }

            }
        }
        private void Calcular()
        {
            using (OracleConnection connection = new OracleConnection(_connectionStrings))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    OracleCommand oracleCommand = new OracleCommand("XSTORE_PL.PR_CALCULAR_STOCK_XSTORE", connection);
                    oracleCommand.Parameters.Add("SEMANA", OracleDbType.Varchar2).Value = semana;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.ExecuteNonQuery();
                    connection.Close();
                    Migrar();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
        }
        private void Migrar()
        {
            using (OracleConnection connection = new OracleConnection(_connectionStrings))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    OracleCommand oracleCommand = new OracleCommand("PA_MIGRAR_STOCK_XSTORE_POS", connection);
                    oracleCommand.Parameters.Add("SEMANA", OracleDbType.Varchar2).Value = semana;
                    oracleCommand.Parameters.Add("TIENDA", OracleDbType.Varchar2).Value = tienda;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
        }

        private void ObtenerSecuencia(out int secuencia)
        {
            secuencia = 0;
            using (var connection = new OracleConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var command = new OracleCommand(@"SELECT INVENTARIOS.ISEQ$$_77537.nextval FROM dual", connection))
                //using (var command = new OracleCommand(@"SELECT INVENTARIOS.ISEQ$$_87596.nextval FROM dual", connection))
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        secuencia = Convert.ToInt32(reader[reader.GetOrdinal("NEXTVAL")]);
                    }
                }
                connection.Close();
            }
        }

        private bool Validar(int pk)
        {
            try
            {
                bool valido;
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM ExistenciasT WHERE PK_INVENTARIOT = '" + pk + "'", connection))
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
                    connection.Close();
                }
                return valido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("[action]/{id}")]
        public IActionResult ExportarExcel([FromRoute] int id)
        {
            try
            {
                List<Models.ExistenciasT.ImportarModel> lista = new List<Models.ExistenciasT.ImportarModel>();
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (OracleCommand command = new OracleCommand(@"SELECT PK_ARTICULO, TALLA, CANTIDAD FROM EXISTENCIAST WHERE PK_INVENTARIOT = '" + id + "'", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.ExistenciasT.ImportarModel stock = new Models.ExistenciasT.ImportarModel();
                                stock.PK_ARTICULO = reader["PK_ARTICULO"].ToString();
                                stock.TALLA = reader["TALLA"].ToString();
                                stock.CANTIDAD = Convert.ToInt32(reader["CANTIDAD"]);
                                lista.Add(stock);
                            }
                        }
                    }
                    connection.Close();
                }

                using (var libro = new XLWorkbook())
                {
                    var hoja = libro.Worksheets.Add("Inventario");
                    var currentRow = 1;
                    hoja.Cell(currentRow, 1).Value = "pK_ARTICULO";
                    hoja.Cell(currentRow, 2).Value = "cantidad";
                    hoja.Cell(currentRow, 3).Value = "talla";
                    hoja.ColumnsUsed().AdjustToContents();

                    foreach (var l in lista)
                    {

                        currentRow++;
                        hoja.Cell(currentRow, 1).SetValue(l.PK_ARTICULO.ToString());
                        hoja.Cell(currentRow, 2).Value = l.CANTIDAD;
                        hoja.Cell(currentRow, 3).Value = l.TALLA; 
                    }

                    using (var memoria = new MemoryStream())
                    {
                        libro.SaveAs(memoria);
                        var content = memoria.ToArray();
                        var nombreExcel = string.Concat("StockInicial", ".xlsx");

                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
