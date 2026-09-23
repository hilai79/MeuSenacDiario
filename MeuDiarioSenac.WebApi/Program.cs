using System.Text.Json.Serialization;
using MeuDiarioSenac.Model;
using MeuDiarioSenac.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.MapGet("/", () => "boa noite");

app.MapGet("/motivacional", () => "Não desista, grandes coisas levam tempo. Continue persistindo e você alcançará seus objetivos!");

app.MapGet("/registros", () => {
    return new RegistroService().ListarTodos();
});


app.Run();
