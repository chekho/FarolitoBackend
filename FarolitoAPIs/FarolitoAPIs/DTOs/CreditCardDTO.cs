using System.ComponentModel.DataAnnotations;

namespace FarolitoAPIs.DTOs
{
    public class CreditCardDTO
    {
        [Required]
        [RegularExpression(@"^[0-9]{13,16}$", ErrorMessage = "El número de tarjeta debe tener entre 13 y 16 dígitos.")]
        public string CardNumber { get; set; }
    }
}
