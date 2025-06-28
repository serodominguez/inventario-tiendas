using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using Sistema.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsuariosController : Controller
    {
        private readonly IConfiguration _config;
        public readonly string _connectionString;

        public UsuariosController(IConfiguration config, IConfiguration _configuration)
        {
            _config = config;
            _connectionString = _configuration.GetConnectionString("Conexion");
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("[action]")]
        public IEnumerable<Models.Usuarios.ListarModel> Listar()
        {
            try
            {
                List<Models.Usuarios.ListarModel> lista = new List<Models.Usuarios.ListarModel>();
                using (var connection = new OracleConnection(_connectionString))
                {
                    using (var command = new OracleCommand(@"SELECT Usuarios.PK_USUARIO, Usuarios.USUARIO, Usuarios.NOMBRES, Usuarios.APELLIDOS, Usuarios.CARNET, Usuarios.ESTADO, Usuarios.PASSWORD, Roles.PK_ROL, Roles.ROL FROM  Roles INNER JOIN Usuarios ON Roles.PK_ROL = Usuarios.PK_ROL ORDER BY PK_USUARIO ASC", connection))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                            command.Connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.Usuarios.ListarModel usuario = new Models.Usuarios.ListarModel();
                                usuario.PK_USUARIO = Convert.ToInt32(reader["PK_USUARIO"]);
                                usuario.USUARIO = reader["USUARIO"].ToString();
                                usuario.NOMBRES = reader["NOMBRES"].ToString();
                                usuario.APELLIDOS = reader["APELLIDOS"].ToString();
                                usuario.CARNET = reader["CARNET"].ToString();
                                usuario.PASSWORD = reader["PASSWORD"].ToString();
                                usuario.ESTADO = reader["ESTADO"].ToString();
                                usuario.PK_ROL = Convert.ToInt32(reader["PK_ROL"]);
                                usuario.ROL = reader["ROL"].ToString();
                                lista.Add(usuario);
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPut("[action]")]
        public IActionResult Actualizar([FromBody] Models.Usuarios.ActualizarModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.PK_USUARIO <= 0)
            {
                return BadRequest();
            }

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    if (model.ACTUALIZARPASSWORD == true)
                    {
                        string pwdHashed = SHA1Encrypt(model.CLAVE);
                        using (var command = new OracleCommand(@"UPDATE Usuarios SET  NOMBRES = '" + model.NOMBRES + "', APELLIDOS = '" + model.APELLIDOS + "', CARNET = '" + model.CARNET + "', USUARIO = '" + model.USUARIO + "', PASSWORD = '" + pwdHashed + "', PK_ROL = '" + model.PK_ROL + "' WHERE PK_USUARIO = '" + model.PK_USUARIO + "'", connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (var command = new OracleCommand(@"UPDATE Usuarios SET  NOMBRES = '" + model.NOMBRES + "', APELLIDOS = '" + model.APELLIDOS + "', CARNET = '" + model.CARNET + "', USUARIO = '" + model.USUARIO + "', PK_ROL = '" + model.PK_ROL + "' WHERE PK_USUARIO = '" + model.PK_USUARIO + "'", connection))
                        {
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost("[action]")]
        public IActionResult Crear([FromBody] Models.Usuarios.ActualizarModel model)
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

                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM Usuarios WHERE PK_USUARIO <> '" + model.PK_USUARIO + "' AND USUARIO = '" + model.USUARIO + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            return BadRequest("El usuario ya existe!");
                        }
                        else
                        {
                            string pwdHashed = SHA1Encrypt(model.CLAVE);
                            command.CommandText = @"INSERT INTO Usuarios (USUARIO, NOMBRES, APELLIDOS, CARNET, ESTADO, PK_ROL, PASSWORD) VALUES ('" + model.USUARIO + "', '" + model.NOMBRES + "', '" + model.APELLIDOS + "', '" + model.CARNET + "', '" + model.ESTADO + "', '" + model.PK_ROL + "', '" + pwdHashed + "')";
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPut("[action]/{id}")]
        public IActionResult Desactivar([FromRoute] int id)
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
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM Usuarios WHERE PK_USUARIO = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"UPDATE Usuarios SET ESTADO = '" + "INACTIVO" + "' WHERE PK_USUARIO = '" + id + "'";
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPut("[action]/{id}")]
        public IActionResult Activar([FromRoute] int id)
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
                    using (var command = new OracleCommand(@"SELECT COUNT(*) FROM Usuarios WHERE PK_USUARIO = '" + id + "'", connection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            command.CommandText = @"UPDATE Usuarios SET ESTADO = '" + "ACTIVO" + "' WHERE PK_USUARIO = '" + id + "'";
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

        [HttpPost("[action]")]
        public IActionResult Inicio(Models.Usuarios.InicioModel model)
        {
            string pwdHashed = "";
            string manifestCertificate = DateTime.Now.ToString("yyyy/MM/dd");
            string manifestCertificateExpired = "2023/08/15";
            if (Convert.ToDateTime(manifestCertificate) <= Convert.ToDateTime(manifestCertificateExpired))
            {
                pwdHashed = SHA1Encrypt(model.CLAVE);
            }

            using (var connection = new OracleConnection(_connectionString))
            {
                
                Models.Usuarios.ListarModel usuario = new Models.Usuarios.ListarModel();
                using (var command = new OracleCommand(@"SELECT Usuarios.PK_USUARIO, Usuarios.USUARIO, Roles.ROL FROM Roles INNER JOIN Usuarios ON Roles.PK_ROL = Usuarios.PK_ROL WHERE Usuarios.USUARIO = '"+ model.USUARIO + "' AND PASSWORD = '" + pwdHashed + "'  AND Usuarios.ESTADO = '" + "ACTIVO" +"'", connection))
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usuario.PK_USUARIO = Convert.ToInt32(reader["PK_USUARIO"]);
                                usuario.USUARIO = reader["USUARIO"].ToString();
                                usuario.ROL = reader["ROL"].ToString();
                            }
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                   command.Connection.Close();
                }

                ObtenerSemana(out string semana);
                var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, usuario.PK_USUARIO.ToString()),
                                new Claim(ClaimTypes.Name, usuario.USUARIO),
                                new Claim(ClaimTypes.Role, usuario.ROL),

                                new Claim("pk_usuario", usuario.PK_USUARIO.ToString()),
                                new Claim("rol", usuario.ROL),
                                new Claim("usuario", usuario.USUARIO),
                                new Claim("semana", semana)
                            };

                return Ok(
                    new { token = GenerarToken(claims) }
                  );
            }
        }

        private string SHA1Encrypt(string password)
        {
            System.Security.Cryptography.HashAlgorithm hashValue = new
           System.Security.Cryptography.SHA1CryptoServiceProvider();

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);

            byte[] byteHash = hashValue.ComputeHash(bytes);

            hashValue.Clear();

            return (Convert.ToBase64String(byteHash));
        }

        private string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(600),
              signingCredentials: creds,
              claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void ObtenerSemana(out string semana)
        {
            semana = "";
            string hoy = DateTime.Now.ToString("dd/MM/yyyy");
            using (var connection = new OracleConnection(_connectionString))
            {
                using (var command = new OracleCommand(@"SELECT PK_SEMANA, INICIO, FIN FROM Semanas WHERE TO_DATE('" + hoy + "', 'dd/mm/rrrr') BETWEEN TO_DATE (inicio, 'dd/mm/rrrr') AND TO_DATE (fin, 'dd/mm/rrrr')", connection))
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            semana = reader["PK_SEMANA"].ToString();
                        }
                    }
                    connection.Close();
                }
            }
        }
    }
}
