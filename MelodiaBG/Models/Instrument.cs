using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelodiaBG.Models
{
    public class Instrument
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на инструмента е задължително.")]
        [MaxLength(100, ErrorMessage = "Името на инструмента не може да надвишава 100 символа.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Описанието не може да надвишава 500 символа.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Цената е задължителна.")]
        [Range(0.01, 100000.00, ErrorMessage = "Цената трябва да бъде между 0.01 и 100000.00.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Категорията е задължителна.")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")] // Explicitly define the foreign key
        public Category Category { get; set; } // Navigation property

        [Url(ErrorMessage = "Моля, въведете валиден URL за изображението.")]
        [MaxLength(200, ErrorMessage = "URL адресът не може да надвишава 200 символа.")]
        public string ImageUrl { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
