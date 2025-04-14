using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Order
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Идентификаторът на потребителя е задължителен.")]
    [ForeignKey("User")]
    public string UserId { get; set; }  // Change Guid to string

    public User User { get; set; }  // Navigation property

    [Required(ErrorMessage = "Датата на поръчката е задължителна.")]
    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "Общата сума е задължителна.")]
    [Range(0.01, 1000000.00, ErrorMessage = "Сумата трябва да бъде между 0.01 и 1000000.00.")]
    public decimal TotalAmount { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
}
