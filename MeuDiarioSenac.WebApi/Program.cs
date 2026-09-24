using System.Text.Json.Serialization;
using MeuDiarioSenac.Model;
using MeuDiarioSenac.Service;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

var registrosGroup = app.MapGroup("/registros");

app.MapGet("/", () => "boa noite");

app.MapGet("/motivacional", () => "Não desista, grandes coisas levam tempo. Continue persistindo e você alcançará seus objetivos!");

registrosGroup.MapGet("/", () => {
    return new RegistroService().ListarRegistros();
});

registrosGroup.MapPost("/", ([FromBody] Registro registro) => {
    new RegistroService().CriarRegistro(
        registro.Titulo,
        registro.Conteudo,
        registro.Usuario.Nome);

    return "Registro adicionado com sucesso!";
});

registrosGroup.MapDelete("/", () => {
    var registrosApagados = new RegistroService().ApagarTodosRegistros();
    return Results.Ok($"{registrosApagados} registro(s) excluído(s) com sucesso!");
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
