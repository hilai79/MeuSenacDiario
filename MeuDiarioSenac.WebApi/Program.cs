using System.Text;
using System.Text.Json.Serialization;
using MeuDiarioSenac.Model;
using MeuDiarioSenac.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

 var jwtSettings = builder.Configuration.GetSection("JWTSettings");
var chaveSecretaBytes = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
}).AddJwtBearer("JwtBearer", options =>
{
   options.RequireHttpsMetadata = false;
   options.SaveToken = true;
   options.TokenValidationParameters = new TokenValidationParameters
   {
         ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(chaveSecretaBytes),
          ClockSkew = TimeSpan.Zero
   };

});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login/auth", ([FromBody] Usuario usuario1) =>
{
   const string usuarioMock = "Joao Silva";
   TokenService authService = new TokenService(builder.Configuration);
   return authService.GerarToken(usuario1.Nome);
});

var registrosGroup = app.MapGroup("/registros").RequireAuthorization();

app.MapGet("/", () => "boa noite");


registrosGroup.MapGet("/", () => {
    return new RegistroService().ListarRegistros();
});

registrosGroup.MapPost("/", ([FromBody] CreateRegistroRequest registro) => {
    var nomeUsuario = registro.Usuario?.Nome;
    if (string.IsNullOrWhiteSpace(nomeUsuario))
    {
        return Results.BadRequest("Informe o nome do usuário em 'usuario.nome'.");
    }

    var registroCriado = new RegistroService().CriarRegistro(
        registro.Titulo,
        registro.Conteudo,
        nomeUsuario);

    return Results.Ok(registroCriado);
});

registrosGroup.MapDelete("/", () => {
    var registrosApagados = new RegistroService().ApagarTodosRegistros();
    return Results.Ok($"{registrosApagados} registro(s) excluído(s) com sucesso!");
});

registrosGroup.MapDelete("/{id:int}", (int id) => {
    var apagado = new RegistroService().ApagarRegistro(id);

    return apagado
        ? Results.Ok($"Registro com id {id} excluído com sucesso!")
        : Results.NotFound($"Registro com id {id} não encontrado.");
});

registrosGroup.MapPut("/{id:int}", (int id, [FromBody] Registro registro) => {
    var registroAtualizado = new RegistroService().EditarRegistro(
        id,
        registro.Titulo,
        registro.Conteudo,
        registro.Usuario.Nome);

    if (registroAtualizado is null)
    {
        return Results.NotFound($"Registro com id {id} não encontrado.");
    }

    return Results.Ok(registroAtualizado);
});


app.Run();

public sealed record CreateRegistroRequest(
    string Titulo,
    string Conteudo,
    CreateUsuarioRequest? Usuario);

public sealed record CreateUsuarioRequest(string? Nome);
