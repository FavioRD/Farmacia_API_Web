namespace FarmaciaWeb.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaIngreso { get; set; }

        // Relación con Producto (para mostrar el nombre en la vista)
        public string Producto { get; set; }
    }
}
