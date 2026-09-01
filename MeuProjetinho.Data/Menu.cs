using Microsoft.EntityFrameworkCore;

namespace MeuProjetinho.Data;

public static class Menu
{
    public static void Exibir()
    {
        bool sair = false;

        while (!sair)
        {
            Console.Clear();
            Console.WriteLine(" MENU DIÁRIO ");
            Console.WriteLine("1 - Adicionar novo registro");
            Console.WriteLine("2 - Listar todos os registros");
            Console.WriteLine("3 - Buscar registro por Id");
            Console.WriteLine("4 - Atualizar registro");
            Console.WriteLine("5 - Excluir registro");
            Console.WriteLine("6 - Excluir todos os registros");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");

            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Registro.Adicionar();
                    break;
                case "2":
                    Registro.Listar();
                    break;
                case "3":
                    Registro.BuscarPorId();
                    break;
                case "4":
                    Registro.Atualizar();
                    break;
                case "5":
                    Registro.Excluir();
                    break;
                case "6":
                    Registro.ExcluirTodos();
                    break;
                case "0":
                    sair = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    Pausar();
                    break;
            }
        }
    }

    public static void Pausar()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}
