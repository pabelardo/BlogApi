using BlogApi.Extensions;
using BlogApi.Models;
using BlogApi.Services;
using BlogApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("v1")]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("accounts/")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User()
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login()
    {
        var tokenString = _tokenService.GenerateToken(null);

        return Ok(tokenString);
    }
}