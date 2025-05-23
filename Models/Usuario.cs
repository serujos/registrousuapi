using System.ComponentModel.DataAnnotations;

namespace registrousuapi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PaisCode { get; set; }
    }
}