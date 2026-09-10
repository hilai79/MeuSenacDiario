using MeuDiarioSenac.Service;

var registroService = new RegistroService();

while (true)
{
	Console.Clear();
	Console.WriteLine("=== Meu Diário SENAC ===");
	Console.WriteLine("1 - Validar regras");
	Console.WriteLine("2 - Criar registro");
	Console.WriteLine("3 - Mostrar registros");
	Console.WriteLine("4 - Apagar todos e resetar IDs");
	Console.WriteLine("0 - Sair");
	Console.Write("Escolha uma opção: ");

	try
	{
		switch (Console.ReadLine())
		{
			case "1":
				MostrarValidacoes(registroService.ValidarRegras());
				Pausar();
				break;
			case "2":
				CriarRegistro(registroService);
				Pausar();
				break;
			case "3":
				MostrarRegistros(registroService.ListarRegistros());
				Pausar();
				break;
			case "4":
				ApagarTodosRegistros(registroService);
				Pausar();
				break;
			case "0":
				return;
			default:
				Console.WriteLine("Opção inválida.");
				Pausar();
				break;
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Não foi possível concluir a operação: {ex.Message}");
		Pausar();
	}
}

static void MostrarValidacoes(ResultadoValidacao validacao)
{
	Console.WriteLine($"Título Obrigatório: {validacao.TituloObrigatorio}");
	Console.WriteLine($"Título Máximo: {validacao.TituloMaximo}");
	Console.WriteLine($"Conteúdo Máximo: {validacao.ConteudoMaximo}");
	Console.WriteLine($"Data de Agora: {validacao.DataDeAgora}");
	Console.WriteLine($"Nome Obrigatório: {validacao.NomeObrigatorio}");
	Console.WriteLine($"Nome Máximo: {validacao.NomeMaximo}");
}

static void CriarRegistro(RegistroService registroService)
{
	Console.Write("Título: ");
	string titulo = Console.ReadLine() ?? string.Empty;

	Console.Write("Conteúdo: ");
	string conteudo = Console.ReadLine() ?? string.Empty;

	Console.Write("Nome do usuário: ");
	string nomeUsuario = Console.ReadLine() ?? string.Empty;

	var registro = registroService.CriarRegistro(titulo, conteudo, nomeUsuario);
	Console.WriteLine("Registro salvo com sucesso:");
	Console.WriteLine($"Id do registro: {registro.Id}");
	Console.WriteLine($"Título: {registro.Titulo}");
	Console.WriteLine($"Conteúdo: {registro.Conteudo}");
	Console.WriteLine($"Data: {registro.Data:dd/MM/yyyy HH:mm}");
	Console.WriteLine($"Usuário: {registro.Usuario.Nome}");
}

static void MostrarRegistros(List<MeuDiarioSenac.Model.Registro> registros)
{
	if (registros.Count == 0)
	{
		Console.WriteLine("Nenhum registro encontrado.");
		return;
	}

	Console.WriteLine("=== Registros ===");
	foreach (var registro in registros)
	{
		Console.WriteLine($"Id do registro: {registro.Id}");
		Console.WriteLine($"Título: {registro.Titulo}");
		Console.WriteLine($"Conteúdo: {registro.Conteudo}");
		Console.WriteLine($"Data: {registro.Data:dd/MM/yyyy HH:mm}");
		Console.WriteLine($"Usuário: {registro.Usuario.Nome}");
		Console.WriteLine(new string('-', 30));
	}
}

static void ApagarTodosRegistros(RegistroService registroService)
{
	Console.Write("Digite APAGAR para confirmar: ");
	if (!string.Equals(Console.ReadLine(), "APAGAR", StringComparison.OrdinalIgnoreCase))
	{
		Console.WriteLine("Operação cancelada.");
		return;
	}

	registroService.ApagarTodosRegistros();
	Console.WriteLine("Todos os registros foram apagados e o contador de IDs foi resetado.");
}

static void Pausar()
{
	Console.WriteLine();
	Console.WriteLine("Pressione uma tecla para continuar...");
	Console.ReadKey();
}
