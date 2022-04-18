using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zanche_Martin_InmobiliariaULP.Models;

public class Inmueble
{
  [Key]
  [Display(Name= "Código")]
  [Required]
  public int Id { get ;set; }
		public string? Direccion { get; set; }
		[Required]
		public int Ambientes { get; set; }
		[Required]
    public string ? Tipo {get; set;}

    public string ? Uso {get; set;}
    public int Precio { get; set; }
		public decimal Superficie { get; set; }
		public decimal Latitud { get; set; }
     public decimal Longitud { get; set; }
     
    public string ? Estado {get; set;}

     [Display(Name = "Dueño")]
     public int PropietarioId { get; set; }
     
    [ForeignKey(nameof(PropietarioId))]
    public Propietario?  Duenio { get; set; }

}
