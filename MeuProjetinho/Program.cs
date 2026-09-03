using MeuDiarioSenac.Business;
using MeuDiarioSenac.Model;

var registroBusiness = new RegistroBusiness();
var usuarioBusiness = new UsuarioBusiness();

var hoje = DateTime.Today;
var registro = new Registro
{
    Titulo = "Primeiro registro",
    Conteudo = "Conteúdo de teste",
    Data = hoje,
    Usuario = new Usuario { Nome = "joao" }
};

Console.WriteLine($"Data atual: {registro.Data:dd/MM/yyyy}");
Console.WriteLine($"Título obrigatório: {registroBusiness.TituloObrigatorio()}");
Console.WriteLine($"Nome obrigatório: {usuarioBusiness.NomeObrigatorio(registro.Usuario.Nome)}");
Console.WriteLine($"Nome máximo: {usuarioBusiness.NomeMax(registro.Usuario.Nome)}");
Console.WriteLine($"Data de hoje: {registroBusiness.DataDeAgora(registro.Data)}");
