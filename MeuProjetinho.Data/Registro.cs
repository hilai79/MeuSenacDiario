using Microsoft.EntityFrameworkCore;

namespace MeuProjetinho.Data;

public class Registro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Conteudo { get; set; } = string.Empty;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public static void Adicionar()
    {
        var nomeUsuario = LerTexto("Nome do usuário: ");

        var registro = new Registro
        {
            Titulo = LerTexto("Título: "),
            Data = LerData("Data (dd/MM/yyyy): "),
            Conteudo = LerTexto("Texto: "),
            UsuarioId = ObterOuCriarUsuarioIdPorNome(nomeUsuario)
        };

        using var contexto = new MeuDiarioSENACContext();
        contexto.Registros.Add(registro);
        contexto.SaveChanges();
        Console.WriteLine("Registro adicionado com sucesso.");
        Menu.Pausar();
    }

    public static void Listar()
    {
        using var contexto = new MeuDiarioSENACContext();
        var registros = contexto.Registros
            .Include(r => r.Usuario)
            .AsNoTracking()
            .OrderByDescending(r => r.Data)
            .ToList();

        Console.WriteLine("\nREGISTROS");
        if (registros.Count == 0)
        {
            Console.WriteLine("Nenhum registro encontrado.");
        }

        foreach (var registro in registros)
        {
            var nomeUsuario = registro.Usuario?.Nome ?? "Desconhecido";
            Console.WriteLine($"Id: {registro.Id} | {registro.Data:dd/MM/yyyy} | {registro.Titulo} | Usuário: {nomeUsuario}");
            Console.WriteLine(registro.Conteudo);
            Console.WriteLine();
        }

        Menu.Pausar();
    }

    public static void BuscarPorId()
    {
        var id = LerInteiro("Id do registro: ");
        using var contexto = new MeuDiarioSENACContext();
        var registro = contexto.Registros
            .Include(r => r.Usuario)
            .AsNoTracking()
            .FirstOrDefault(r => r.Id == id);

        if (registro is null)
        {
            Console.WriteLine("Registro não encontrado.");
        }
        else
        {
            Exibir(registro);
        }

        Menu.Pausar();
    }

    public static void Atualizar()
    {
        var id = LerInteiro("Id do registro: ");
        using var contexto = new MeuDiarioSENACContext();
        var registro = contexto.Registros.FirstOrDefault(r => r.Id == id);

        if (registro is null)
        {
            Console.WriteLine("Registro não encontrado.");
        }
        else
        {
            registro.Titulo = LerTexto("Novo título: ");
            registro.Data = LerData("Nova data (dd/MM/yyyy): ");
            registro.Conteudo = LerTexto("Novo conteúdo: ");
            registro.UsuarioId = ObterOuCriarUsuarioIdPorNome(LerTexto("Novo nome do usuário: "));
            contexto.SaveChanges();
            Console.WriteLine("Registro atualizado com sucesso.");
        }

        Menu.Pausar();
    }

    public static void Excluir()
    {
        var id = LerInteiro("Id do registro: ");
        using var contexto = new MeuDiarioSENACContext();
        var registro = contexto.Registros.Find(id);

        if (registro is null)
        {
            Console.WriteLine("Registro não encontrado.");
        }
        else
        {
            contexto.Registros.Remove(registro);
            contexto.SaveChanges();
            Console.WriteLine("Registro excluído com sucesso.");
        }

        Menu.Pausar();
    }

    public static void ExcluirTodos()
    {
        using var contexto = new MeuDiarioSENACContext();
        contexto.Database.ExecuteSqlRaw("DELETE FROM Registros");
        contexto.Database.ExecuteSqlRaw("ALTER TABLE Registros AUTO_INCREMENT = 1");
        Console.WriteLine("Todos os registros foram excluídos. O ID foi reiniciado para 1.");
        Menu.Pausar();
    }

    private static int ObterOuCriarUsuarioIdPorNome(string nomeUsuario)
    {
        using var contexto = new MeuDiarioSENACContext();

        var usuarioExistente = contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefault(u => u.Nome == nomeUsuario);

        if (usuarioExistente is not null)
        {
            return usuarioExistente.Id;
        }

        var novoUsuario = new Usuario { Nome = nomeUsuario };
        contexto.Usuarios.Add(novoUsuario);
        contexto.SaveChanges();
        return novoUsuario.Id;
    }

    private static string LerTexto(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            var valor = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(valor))
            {
                return valor;
            }

            Console.WriteLine("O valor não pode ficar vazio.");
        }
    }

    private static int LerInteiro(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out var valor) && valor > 0)
            {
                return valor;
            }

            Console.WriteLine("Digite um número inteiro positivo.");
        }
    }

    private static DateTime LerData(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (DateTime.TryParse(Console.ReadLine(), out var valor))
            {
                return valor;
            }

            Console.WriteLine("Digite uma data válida.");
        }
    }

    private static void Exibir(Registro registro)
    {
        var nomeUsuario = registro.Usuario?.Nome ?? "Desconhecido";

        Console.WriteLine($"\nId: {registro.Id}");
        Console.WriteLine($"Título: {registro.Titulo}");
        Console.WriteLine($"Data: {registro.Data:dd/MM/yyyy}");
        Console.WriteLine($"Conteúdo: {registro.Conteudo}");
        Console.WriteLine($"Usuário: {nomeUsuario}");
    }
}
