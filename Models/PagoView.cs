using System.ComponentModel.DataAnnotations;

namespace Zanche_Martin_InmobiliariaULP.Models
{
  public class PagoView{

  [Display(Name= "Código")]     
  public int Id { get; set; }

    [Display(Name= "Numero de pago")]     
    public int NroPago { get; set; }

   [Required,Display(Name="Fecha pago")]
    [DataType(DataType.Date)]    
    public DateTime FechaDePago { get; set; }
   [Required]
    public Decimal Importe {get;set;}

    public int ContratoId { get; set; }

   
    
    public PagoView(Pago pago)
    {
      this.Id=pago.Id;
      this.NroPago=pago.NroPago;
      this.FechaDePago=pago.FechaPago;
      this.Importe=pago.Importe;    
      this.ContratoId=pago.ContratoId; 
    }
  }
}