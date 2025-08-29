using System.Data;
using Dapper;
using FarmaciaAPI.IServices;
using FarmaciaAPI.Models;
using Microsoft.Data.SqlClient;

namespace FarmaciaAPI.Services
{
    public class EntradasService : IEntradasService
    {
        private readonly string _connectionString;

        public EntradasService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FarmaciaConnection");
        }

        public async Task<IEnumerable<Ingreso>> ListarIngresosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Ingreso>(
                "USP_ListarIngresos",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Ingreso?> ObtenerIngresoPorIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Ingreso>(
                "USP_ObtenerIngresoPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task RegistrarIngresoAsync(Ingreso ingreso)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "USP_RegistrarIngreso",
                new { ingreso.ProductoId, ingreso.Cantidad },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ActualizarIngresoAsync(Ingreso ingreso)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "USP_ActualizarIngreso",
                new { ingreso.Id, ingreso.ProductoId, ingreso.Cantidad, ingreso.FechaIngreso },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task EliminarIngresoAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "USP_EliminarIngreso",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
