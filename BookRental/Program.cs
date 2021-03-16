using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental
{
	class Program
	{
		static void Main(string[] args)
		{
			int option;

			do
			{
				option = MainMenu();
				switch (option)
				{
					case 1:
						CustomerController.RegisterACustomer();
						break;
					case 2:
						BookController.RegisterABook();
						break;
					case 3:
						BookLoanController.RegisterALoan();
						break;
					case 4:
						BookLoanController.MakeDevolution();
						break;
					case 5:
						BookLoanController.ShowReportLoan();
						break;
				}
			} while (option != 0);

			Console.Write("Pressione qualquer tecla para encerrar o sistema...");
			Console.ReadKey();
		}

		public static int MainMenu()
		{
			bool isInt = false;
			string aux;
			int option;
			Console.Clear();
			
				Console.WriteLine("============= Biblioteca Municipal =============");
				Console.WriteLine("================ Menu Principal ================");
				Console.WriteLine("1 - Cadastro de Cliente");
				Console.WriteLine("2 - Cadastro de Livro");
				Console.WriteLine("3 - Emprestimo de Livro");
				Console.WriteLine("4 - Devolucao do Livro");
				Console.WriteLine("5 - Relatorio de Emprestimos e Devolucoes");
				Console.WriteLine("0 - Fechar Sistema");
			do
			{
				Console.Write("Escolha uma opcao: ");
				aux = Console.ReadLine();
				if (!int.TryParse(aux, out option) || (option < 0) || (option > 5))
					Console.WriteLine("Opcao invalida, escolha entre os numeros de 0 a 5");
				else
					isInt = true;
			} while (!isInt);


			return option;
		}
	}
}
