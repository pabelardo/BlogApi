namespace BlogApi.Models;

public class Category : Model
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public IList<Post> Posts { get; set; }
}
