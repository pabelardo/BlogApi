using System.ComponentModel.DataAnnotations;

namespace BlogApi.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O atributo '{0}' é obrigatório")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O atributo '{0}' é obrigatório")]
    [EmailAddress(ErrorMessage = "O atributo '{0}' é inválido")]
    public string Email { get; set; }
}