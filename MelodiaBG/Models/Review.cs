using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace MelodiaBG.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Идентификаторът на потребителя е задължителен.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Идентификаторът на инструмента е задължителен.")]
        public int InstrumentId { get; set; }

        [Required(ErrorMessage = "Текстът на отзива е задължителен.")]
        [MaxLength(2000, ErrorMessage = "Отзивът не може да надвишава 2000 символа.")]
        public string ReviewText { get; set; }

        [Required(ErrorMessage = "Оценката е задължителна.")]
        [Range(1, 5, ErrorMessage = "Оценката трябва да бъде между 1 и 5.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Датата на създаване е задължителна.")]
        public DateTime CreatedDate { get; set; } = new DateTime();

        [ValidateNever]
        public User User { get; set; }

        [ValidateNever]
        public Instrument Instrument { get; set; }
    }
}
