using System.Text;
using BlogApi;
using BlogApi.Data;
using BlogApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

builder.Services.AddAuthentication(auth =>  // Aqui está apenas dizendo como fará a authenticação para o asp.net
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.TokenValidationParameters = new TokenValidationParameters // Aqui você está informando como ele irá desencriptar esse token
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
}); 

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddDbContext<BlogDataContext>();

builder.Services.AddTransient<TokenService>(); //Também conhecido como life time, ou seja, o tempo de vida do serviço. No caso do transient, ele sempre cria um novo.
//builder.Services.AddScoped(); -> A duração dele é por requisição.
//builder.Services.AddSingleton(); -> 1 por App. Sempre fica na memória da aplicação.

var app = builder.Build();

LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("SmtpConfiguration").Bind(smtp);
    Configuration.Smtp = smtp;
}
