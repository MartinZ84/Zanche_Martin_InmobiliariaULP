using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zanche_Martin_InmobiliariaULP.Models;

public class InmuebleView
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

    	public string? Foto { get; set; } 
     
   
    public InmuebleView(Inmueble inm)
    { 
      this.Id=inm.Id;
      this.Direccion=inm.Direccion;
      this.Ambientes=inm.Ambientes;
      this.Tipo=inm.Tipo;
      this.Uso=inm.Uso;
      this.Precio=inm.Precio;
      this.Superficie=inm.Superficie;
      this.Latitud=inm.Latitud;
      this.Longitud=inm.Longitud;
      this.Estado=inm.Estado;
      this.Foto=inm.Foto;
    }
  

}
