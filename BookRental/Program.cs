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
				}
			} while (option != 0);

			Console.Write("Pressione qualquer tecla para encerrar o sistema...");
			Console.ReadKey();
		}

		public static int MainMenu()
		{
			int option;
			Console.Clear();
			Console.WriteLine("============= Biblioteca Municipal =============");
			Console.WriteLine("================ Menu Principal ================");
			Console.WriteLine("1 - Cadastro de Cliente");
			Console.WriteLine("2 - Cadastro de Livro");
			Console.WriteLine("3 - Emprestimo de Livro");
			Console.WriteLine("0 - Fechar Sistema");

			return option = int.Parse(Console.ReadLine());
		}
	}
}
