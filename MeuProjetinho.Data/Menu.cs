namespace MeuProjetinho.Data;

public static class Menu
{
    public static void Exibir()
    {
        Console.WriteLine("Menu do diário em desenvolvimento.");
        Console.WriteLine("A lógica de regras e entidades foi movida para as camadas de negócio e model.");
    }

    public static void Pausar()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}
