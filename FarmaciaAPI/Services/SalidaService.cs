using System.Data;
using FarmaciaAPI.IServices;
using FarmaciaAPI.Models;
using Microsoft.Data.SqlClient;

namespace FarmaciaAPI.Services
{
    public class SalidaService : ISalidaService
    {
        private readonly string _connectionString;
        private readonly ILogger<SalidaService> _logger;

        public SalidaService(IConfiguration config, ILogger<SalidaService> logger)
        {
            _connectionString = config.GetConnectionString("FarmaciaConnection");
            _logger = logger;
        }

        // Listar 
        public async Task<List<Salida>> Listar()
        {
            try
            {


                var lista = new List<Salida>();
                using (var con = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("USP_ListarSalidas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Salida
                        {
                            Id = reader.GetInt32(0),
                            ProductoId = reader.GetInt32(1),
                            Cantidad = reader.GetInt32(2),
                            FechaSalida = reader.GetDateTime(3)
                        });
                    }
                }

                return lista;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Obtener por Id
        public async Task<Salida?> ObtenerPorId(int id)
        {
            Salida? salida = null;
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("USP_ObtenerSalidaPorId", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    salida = new Salida
                    {
                        Id = reader.GetInt32(0),
                        ProductoId = reader.GetInt32(1),
                        Cantidad = reader.GetInt32(2),
                        FechaSalida = reader.GetDateTime(3)
                    };
                }
            }
            return salida;
        }

        // Registrar 
        public async Task Registrar(int productoId, int cantidad)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("USP_RegistrarSalida", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductoId", productoId);
            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // Actualizar salida
        public async Task<bool> Actualizar(int id, Salida salida)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("USP_ActualizarSalida", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@ProductoId", salida.ProductoId);
            cmd.Parameters.AddWithValue("@Cantidad", salida.Cantidad);
            cmd.Parameters.AddWithValue("@FechaSalida", salida.FechaSalida);

            await con.OpenAsync();
            var filasAfectadas = await cmd.ExecuteNonQueryAsync();
            return filasAfectadas > 0;
        }

        // Eliminar salida
        public async Task<bool> Eliminar(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("USP_EliminarSalida", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            await con.OpenAsync();
            var filasAfectadas = await cmd.ExecuteNonQueryAsync();
            return filasAfectadas > 0;
        }
    }
}
