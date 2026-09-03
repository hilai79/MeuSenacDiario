namespace MeuDiarioSenac.Model;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ICollection<Registro> Registros { get; set; } = new List<Registro>();
}