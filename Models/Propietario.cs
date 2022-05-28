using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zanche_Martin_InmobiliariaULP.Models;

public class Propietario
{
  [Display(Name= "Código")]
  public int Id { get ;set; }
  	[Required]
  public string? Nombre { get ;set; }
	[Required]
   public string? Apellido { get ;set; }
    [Display(Name= "DNI")]
    [Required]
  public string? Dni { get ;set; }
 [Display(Name= "Teléfono")]
  public string? Telefono { get ;set; }
  	[Required, EmailAddress]
   public string? Email { get ;set; }

		[DataType(DataType.Password)]
		public string? Clave { get; set; }
public string? Avatar { get; set; }
		[NotMapped]//Para EF
		public IFormFile? AvatarFile { get; set; }

}
