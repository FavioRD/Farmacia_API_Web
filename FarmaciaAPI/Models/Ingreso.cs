namespace FarmaciaAPI.Models
{
    public class Ingreso
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string Producto { get; set; } // Para mostrar el nombre
        public int Cantidad { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
