using System.ComponentModel.DataAnnotations;

namespace PracticaViamatica.Model
{
    public class Producto
    {
        [Key]
        public int idProducto { get; set; }
        [Required]
        public string? descripcion { get; set; }
        [Required]
        public double precio { get; set; }
        [Required]
        public int cantidadDisponible { get; set; } 
    }
}
