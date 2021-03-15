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
			CustomerController.RegisterACustomer();

			Console.ReadKey();
		}
	}
}
