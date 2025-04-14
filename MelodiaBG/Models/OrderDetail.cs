using MelodiaBG.Models;
using System.ComponentModel.DataAnnotations;

public class OrderDetail
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Идентификаторът на поръчката е задължителен.")]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "Идентификаторът на инструмента е задължителен.")]
    public int InstrumentId { get; set; }

    [Required(ErrorMessage = "Количеството е задължително.")]
    [Range(1, int.MaxValue, ErrorMessage = "Количеството трябва да бъде поне 1.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Цената на единица е задължителна.")]
    [Range(0.01, 100000.00, ErrorMessage = "Цената трябва да бъде между 0.01 и 100000.00.")]
    public decimal UnitPrice { get; set; }

    public Order Order { get; set; }
    public Instrument Instrument { get; set; }
}
