using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MelodiaBG.Models.ViewModels
{
    public class EditInstrumentViewModel
    {
        public int InstrumentId { get; set; }

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

        [ValidateNever]
        public string ImageUrl { get; set; } // Запазваме текущото изображение

        public IFormFile? Image { get; set; } // Не е задължително, само ако качват ново изображение
    }
}
