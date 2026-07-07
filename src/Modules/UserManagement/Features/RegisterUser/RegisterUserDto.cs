using System.ComponentModel.DataAnnotations;

namespace SprintUp.Modules.UserManagement.Features.RegisterUser;

public class RegisterUserDto
{
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
    [Compare(nameof(Password), ErrorMessage = "As senhas não coincidem.")]
    public string? ConfirmPassword { get; set; }
}
