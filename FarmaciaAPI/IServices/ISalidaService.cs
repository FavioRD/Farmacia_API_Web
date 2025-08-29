using FarmaciaAPI.Models;

namespace FarmaciaAPI.IServices
{
    public interface ISalidaService
    {
        Task<List<Salida>> Listar();
        Task<Salida?> ObtenerPorId(int id);
        Task Registrar(int productoId, int cantidad);
        Task<bool> Actualizar(int id, Salida salida);
        Task<bool> Eliminar(int id);
    }
}
