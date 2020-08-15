using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_netcore3.Models
{
    public class AutorDTO: Recurso
    {
        public int Id { get; set; }

        [Required]
        //[PrimeraLetraMayuscula]
        [StringLength(10, ErrorMessage = "El campo Nombre debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        //[Range(18, 20)]
        //public int Edad { get; set; }

        //[CreditCard]
        //public string CreditCard { get; set; }

        //[Url]
        //public string Url { get; set; }

        public List<LibroDTO> Libros { get; set; }
    }
}
