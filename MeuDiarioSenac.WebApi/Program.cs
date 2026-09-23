using MeuDiarioSenac.Model;
using MeuDiarioSenac.Service;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "boa noite");

app.MapGet("/motivacional", () => "Não desista, grandes coisas levam tempo. Continue persistindo e você alcançará seus objetivos!");

app.MapGet("/registros", () => {
    List<Registro> registros = new RegistroService().ListarRegistros();
    return registros;
});


app.Run();
