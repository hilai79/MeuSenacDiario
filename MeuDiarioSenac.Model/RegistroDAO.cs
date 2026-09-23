namespace MeuDiarioSenac.Model;

public record RegistroDAO(
    int Id,
    string Titulo,
    DateTime Data,
    string Conteudo,
    int UsuarioId,
    string UsuarioNome);