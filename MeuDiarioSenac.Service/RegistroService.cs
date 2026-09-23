using MeuDiarioSenac.Business;
using MeuDiarioSenac.Model;
using MeuProjetinho.Data;
using Microsoft.EntityFrameworkCore;

namespace MeuDiarioSenac.Service;

public class RegistroService
{
    public ResultadoValidacao ValidarRegras()
    {
        var registroBusiness = new RegistroBusiness();
        var usuarioBusiness = new UsuarioBusiness();
        const string nomeUsuario = "Exemplo";

        return new ResultadoValidacao(
            registroBusiness.TituloObrigatorio(),
            registroBusiness.TituloMax(),
            registroBusiness.ConteudoMax(),
            registroBusiness.DataDeAgora(DateTime.Now),
            usuarioBusiness.NomeObrigatorio(nomeUsuario),
            usuarioBusiness.NomeMax(nomeUsuario));
    }

    public Registro CriarRegistro(string titulo, string conteudo, string nomeUsuario)
    {
        if (string.IsNullOrWhiteSpace(nomeUsuario))
        {
            throw new ArgumentException("O nome do usuário é obrigatório.", nameof(nomeUsuario));
        }

        using var contexto = new MeuDiarioSENACContext();
        var usuario = contexto.Usuarios
            .FirstOrDefault(usuario => usuario.Nome == nomeUsuario);

        if (usuario is null)
        {
            usuario = new Usuario { Nome = nomeUsuario };
            contexto.Usuarios.Add(usuario);
        }

        var registro = new RegistroBusiness().CriarRegistro(titulo, conteudo, usuario.Id);
        registro.Usuario = usuario;
        contexto.Registros.Add(registro);
        contexto.SaveChanges();

        return registro;
    }

    public List<Registro> ListarRegistros()
    {
        using var contexto = new MeuDiarioSENACContext();

        return contexto.Registros
            .Include(registro => registro.Usuario)
            .AsNoTracking()
            .OrderBy(registro => registro.Id)
            .ToList();
    }

    public List<RegistroDAO> ListarTodos()
    {
        using var contexto = new MeuDiarioSENACContext();

        return contexto.Registros
            .AsNoTracking()
            .OrderBy(registro => registro.Id)
            .Select(registro => new RegistroDAO(
                registro.Id,
                registro.Titulo,
                registro.Data,
                registro.Conteudo,
                registro.UsuarioId,
                registro.Usuario.Nome))
            .ToList();
    }

    public void ApagarTodosRegistros()
    {
        using var contexto = new MeuDiarioSENACContext();
        contexto.Registros.ExecuteDelete();
        contexto.Database.ExecuteSqlRaw("ALTER TABLE Registros AUTO_INCREMENT = 1");
    }
}

public record ResultadoValidacao(
    bool TituloObrigatorio,
    bool TituloMaximo,
    bool ConteudoMaximo,
    bool DataDeAgora,
    bool NomeObrigatorio,
    bool NomeMaximo);