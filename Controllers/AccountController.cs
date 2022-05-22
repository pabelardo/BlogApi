using BlogApi.Data;
using BlogApi.Extensions;
using BlogApi.Models;
using BlogApi.Services;
using BlogApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BlogApi.Controllers;

[ApiController]
[Route("v1/accounts")]
public class AccountController : ControllerBase
{
    private readonly EmailService _emailService;
    private readonly TokenService _tokenService;
    private readonly BlogDataContext _context;

    public AccountController(TokenService tokenService, BlogDataContext context, EmailService emailService)
    {
        _tokenService = tokenService;
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
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

        var password = PasswordGenerator.Generate(25);

        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _emailService.Send(
                user.Name,
                user.Email,
                subject: "Bem vindo ao blog!",
                body:$"Sua senha é <strong>{password}</strong>");

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05X99 - Este E-mail já está cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await _context
            .Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if(user == null)
            return StatusCode(401, new ResultViewModel<string>("05X100 - Usuário ou senha inválidos"));

        if(!PasswordHasher.Verify(user.PasswordHash, model.Password)) //PasswordHash é o Hash que está salvo no banco e o Password vem do client
            return StatusCode(401, new ResultViewModel<string>("05X101 - Usuário ou senha inválidos"));

        try
        {
            var token = _tokenService.GenerateToken(user);

            return Ok(new ResultViewModel<string>(token, null));
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }
}