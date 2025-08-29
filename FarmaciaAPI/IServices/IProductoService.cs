using FarmaciaAPI.Models;

namespace FarmaciaAPI.IServices
{
    public interface IProductoService
    {
        Task<List<Producto>> Listar();
        Task<Producto?> ObtenerPorId(int id);
        Task Insertar(Producto p);
        Task<bool> Actualizar(int id, Producto p);
        Task<bool> Eliminar(int id);
    }
}
