using System.ComponentModel.DataAnnotations;

namespace discipline.ui.blazor.wasm.Models.Users;

public class SignInDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}