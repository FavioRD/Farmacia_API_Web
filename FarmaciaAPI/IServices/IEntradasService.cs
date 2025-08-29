using FarmaciaAPI.Models;

namespace FarmaciaAPI.IServices
{
    public interface IEntradasService
    {
        Task<IEnumerable<Ingreso>> ListarIngresosAsync();
        Task<Ingreso?> ObtenerIngresoPorIdAsync(int id);
        Task RegistrarIngresoAsync(Ingreso ingreso);
        Task ActualizarIngresoAsync(Ingreso ingreso);
        Task EliminarIngresoAsync(int id);
    }
}
