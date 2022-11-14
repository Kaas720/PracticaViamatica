using System.ComponentModel.DataAnnotations;

namespace PracticaViamatica.Model
{
    public class Persona
    {
        [Key]
        public int Idpersona { get; set; }

        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string direccionDomicilio { get; set; }
        public string direccionTrabajo { get; set; }
        public string estado { get; set; }
    }
}
