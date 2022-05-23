using System.ComponentModel.DataAnnotations;

namespace BlogApi.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o e-mail")]
        [EmailAddress(ErrorMessage = "'{0}' inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public string Password { get; set; }
    }
}
