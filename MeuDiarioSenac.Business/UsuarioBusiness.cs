using MeuProjetinho.Data;


namespace MeuDiarioSenac.Business;

public class UsuarioBusiness
{
    public bool NomeObrigatorio(string nome)
    {
        return !string.IsNullOrWhiteSpace(nome);
    }

    public bool NomeMax(string nome)
    {
        try
        {
            using var contexto = new MeuDiarioSENACContext();
            return nome.Length <= 50;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar nomes máximos: {ex.Message}");
            return false;
        }
    }
}