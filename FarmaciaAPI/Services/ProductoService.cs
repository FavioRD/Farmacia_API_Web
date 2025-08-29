using System.Data;
using FarmaciaAPI.IServices;
using FarmaciaAPI.Models;
using Microsoft.Data.SqlClient;

namespace FarmaciaAPI.Services
{
    public class ProductoService : IProductoService
    {
        private readonly string _connectionString;

        public ProductoService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("FarmaciaConnection");
        }


        // Listar productos
        public async Task<List<Producto>> Listar()
        {
            var lista = new List<Producto>();
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("USP_ListarProductos", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lista.Add(new Producto
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        Precio = reader.GetDecimal(3),
                        Stock = reader.GetInt32(4)
                    });

                }
            }
            return lista;
        }

        // Obtener producto por ID
        public async Task<Producto?> ObtenerPorId(int id)
        {
            Producto? producto = null;
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("USP_ObtenerProductoPorId", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    producto = new Producto
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        Precio = reader.GetDecimal(3),
                        Stock = reader.GetInt32(4),
                        FechaExpiracion = reader.GetDateTime(5)
                    };
                }
            }
            return producto;
        }

        // Insertar producto
        public async Task Insertar(Producto p)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("USP_InsertarProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", p.Descripcion);
            cmd.Parameters.AddWithValue("@Precio", p.Precio);
            cmd.Parameters.AddWithValue("@Stock", p.Stock);
            cmd.Parameters.AddWithValue("@FechaExpiracion", p.FechaExpiracion);


            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // Actualizar producto
        public async Task<bool> Actualizar(int id, Producto p)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("USP_ActualizarProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", p.Descripcion);
            cmd.Parameters.AddWithValue("@Precio", p.Precio);
            cmd.Parameters.AddWithValue("@Stock", p.Stock);
            cmd.Parameters.AddWithValue("@FechaExpiracion", p.FechaExpiracion);


            await con.OpenAsync();
            var filasAfectadas = await cmd.ExecuteNonQueryAsync();
            return filasAfectadas > 0;
        }

        // Eliminar producto
        public async Task<bool> Eliminar(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("USP_EliminarProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            await con.OpenAsync();
            var filasAfectadas = await cmd.ExecuteNonQueryAsync();
            return filasAfectadas > 0;
        }
    }
}
