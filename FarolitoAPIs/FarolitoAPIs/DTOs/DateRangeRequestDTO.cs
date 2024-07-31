using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FarolitoAPIs.DTOs
{
    public class DateRangeRequestDTO
    {
        [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = "El formato de la fecha debe ser yyyy-MM-dd.")]
        public string? StartDate { get; set; }

        [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = "El formato de la fecha debe ser yyyy-MM-dd.")]
        public string? EndDate { get; set; }
    }
}
