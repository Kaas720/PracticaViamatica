using System.ComponentModel.DataAnnotations;

namespace PracticaViamatica.Model
{
    public class Persona
    {
        [Key]
        public int Idpersona { get; set; }

        [Required]
        public string nombre { get; set; }
        [Required]
        public string apellido { get; set; }
        [Required]
        public string telefono { get; set; }
        [Required]
        public string correo { get; set; }
        [Required]
        public string direccionDomicilio { get; set; }
        [Required]
        public string direccionTrabajo { get; set; }
        public string estado { get; set; }
    }
}
