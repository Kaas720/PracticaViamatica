using System.ComponentModel.DataAnnotations;

namespace PracticaViamatica.Model
{
    public class Producto
    {
        [Key]
        public int idProducto { get; set; }
        public string? descripcion { get; set; }
        public double precio { get; set; }
        [Required]
        public int cantidadDisponible { get; set; }
        public string url { get; set; }
    }
}
