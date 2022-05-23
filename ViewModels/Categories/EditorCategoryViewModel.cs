using System.ComponentModel.DataAnnotations;

namespace BlogApi.ViewModels.Categories;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "O campo '{0}' é obrigatório")]
    [StringLength(40, MinimumLength = 3, ErrorMessage = "O campo '{0}' deve conter entre {2} e {1} caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo '{0}' é obrigatório")]
    public string Slug { get; set; }
}