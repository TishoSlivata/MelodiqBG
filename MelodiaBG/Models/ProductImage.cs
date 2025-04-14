using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MelodiaBG.Models
{
    public class ProductImage
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(500)]
        public string UrlP { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Instrument Instrument { get; set; }
    }
}
