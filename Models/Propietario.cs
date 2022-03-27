using System.ComponentModel.DataAnnotations;

namespace Zanche_Martin_InmobiliariaULP.Models;

public class Propietario
{
  [Display(Name= "Código")]
  public int Id { get ;set; }
  public string? Nombre { get ;set; }
   public string? Apellido { get ;set; }
    [Display(Name= "DNI")]
  public string? Dni { get ;set; }
 [Display(Name= "Teléfono")]
  public string? Telefono { get ;set; }
   public string? Email { get ;set; }



}
