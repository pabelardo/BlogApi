namespace BlogApi.Models;

public class Role : Model
{
    public string Name { get; set; }
    public string Slug { get; set; }

    public IList<User> Users { get; set; }
}