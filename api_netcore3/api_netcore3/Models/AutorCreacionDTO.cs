using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_netcore3.Models
{
    public class AutorCreacionDTO
    {
        [Required]
        //[PrimeraLetraMayuscula]
        [StringLength(10, ErrorMessage = "El campo Nombre debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
