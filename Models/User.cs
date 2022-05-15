namespace BlogApi.Models;
public class User : Model
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
    public string Slug { get; set; }
    public string GitHub { get; set; }

    public IList<Post> Posts { get; set; }
    public IList<Role> Roles { get; set; }
}