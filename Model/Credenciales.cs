using System.ComponentModel.DataAnnotations;

namespace PracticaViamatica.Model
{
    public class Credenciales
    {
        [Key]
        public int idCredenciales { get; set; }
        [Required]
        public string? usuario { get; set; }
        [Required]
        public string? password { get; set; }
        public Persona? idPerson { get; set; }
        public string? estado   { get; set; }

    }
}
