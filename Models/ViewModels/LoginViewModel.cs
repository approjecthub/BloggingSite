using System.ComponentModel.DataAnnotations;

namespace mvc1.Models.ViewModels
{
    public class LoginViewModel
    {
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    }
}