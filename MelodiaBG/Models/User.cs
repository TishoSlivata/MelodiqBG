using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    [Required(ErrorMessage = "Първото име е задължително.")]
    [MaxLength(50, ErrorMessage = "Първото име не може да надвишава 50 символа.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Фамилията е задължителна.")]
    [MaxLength(50, ErrorMessage = "Фамилията не може да надвишава 50 символа.")]
    public string LastName { get; set; }
}
