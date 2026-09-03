using MeuDiarioSenac.Model;
using MeuProjetinho.Data;

namespace MeuDiarioSenac.Business;

public class RegistroBusiness
{
    public bool TituloObrigatorio()
    {
        try
        {
            using var contexto = new MeuDiarioSENACContext();
            return contexto.Registros.All(r => !string.IsNullOrWhiteSpace(r.Titulo));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar títulos obrigatórios: {ex.Message}");
            return false;
        }
    }

    public bool TituloMax()
    {
        try
        {
            using var contexto = new MeuDiarioSENACContext();
            return contexto.Registros.All(r => r.Titulo.Length <= 50);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar títulos máximos: {ex.Message}");
            return false;
        }
    }

    public bool ConteudoMax()
    {
        try
        {
            using var contexto = new MeuDiarioSENACContext();
            return contexto.Registros.All(r => r.Conteudo.Length <= 3000);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar conteúdos máximos: {ex.Message}");
            return false;
        }
    }

    public bool DataDeAgora(DateTime data)
    {
        return data.Date == DateTime.Today;
    }

    public Registro CriarRegistro(string titulo, string conteudo, int usuarioId)
    {
        return new Registro
        {
            Titulo = titulo,
            Conteudo = conteudo,
            UsuarioId = usuarioId,
            Data = DateTime.Now
        };
    }
}
