using BlogApi.Data;
using BlogApi.Extensions;
using BlogApi.Models;
using BlogApi.ViewModels;
using BlogApi.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers;

[ApiController]
[Route("v1/categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
    {
        try
        {
            var categories = await context.Categories.ToListAsync();
        
            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("05XE04 - Falha interna no servidor."));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if(category == null)
                return NotFound(new ResultViewModel<Category>("Conteúdo não encontrada."));
        
            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE05 - Falha interna no servidor."));
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

        try
        {
            var category = new Category()
            {
                Name = model.Name,
                Slug = model.Slug.ToLower()
            };

            await context.Categories.AddAsync(category);

            await context.SaveChangesAsync();
        
            return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE09 - Não foi possível incluir a categoria."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE10 - Falha interna no servidor."));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if(category == null)
                return NotFound(new ResultViewModel<Category>("Conteúdo não encontrada."));

            category.Name = model.Name;

            category.Slug = model.Slug;

            context.Categories.Update(category);

            await context.SaveChangesAsync();
        
            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE08 - Não foi possível alterar a categoria."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE11 - Falha interna no servidor."));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if(category == null)
                return NotFound(new ResultViewModel<Category>("Conteúdo não encontrada."));

            context.Categories.Remove(category);

            await context.SaveChangesAsync();
        
            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE07 - Não foi possível excluir a categoria."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE12 - Falha interna no servidor."));
        }
    }
}