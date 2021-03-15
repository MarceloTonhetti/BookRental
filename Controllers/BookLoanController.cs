using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Controllers
{
	public class BookLoanController
	{
        public static void RegisterALoan()
        {
            long bookTumbleNumber;
            long idCustomer;
            string cpfCustomer;
            BookLoan bookLoan;

            Console.Clear();
            Console.WriteLine("-=-=-=-=-  Emprestimo de Livro  -=-=-=-=-");
            Console.WriteLine("\nInforme os dados para o emprestimo\n");
            Console.Write("Numero do Tombo: ");
            bookTumbleNumber = long.Parse(Console.ReadLine());
            if (BookController.BookExists(bookTumbleNumber))
                if (!BookAvailable(bookTumbleNumber))
                {
                    Console.WriteLine("\nLivro indisponivel\n");
                    Console.Write("Pressione qualquer tecla para voltar ao menu princial...");
                    Console.ReadKey();
                }
                else
                {
                    Console.Write("CPF do Cliente: ");
                    cpfCustomer = Console.ReadLine();

                    idCustomer = CustomerController.CustomerExistsAndReturnId(cpfCustomer);
                    if (idCustomer == 0)
                    {
                        Console.WriteLine("\nCliente nao cadastrado\n");
                        Console.Write("Pressione qualquer tecla para voltar ao menu princial...");
                        Console.ReadKey();
                    }
                    else
                    {
                        bookLoan = ReadingBookData(bookTumbleNumber, idCustomer);
                        ConvertListForWriteFile(bookLoan);
                        Console.WriteLine("\nLivro cadastrado com sucesso!!");
                        Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                        Console.ReadKey();
                    }
                }
            else
            {
                Console.WriteLine("\nLivro nao cadastrado\n");
                Console.Write("Pressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
        }

        private static bool BookAvailable(long bookTumbleNumber)
        {
            bool available = true;

            FileHandler file = new FileHandler();
            file.FileName = "EMPRESTIMO.csv";

            if (FileHandlerController.CreateDirectoryAndFile(file))
                available = true;
            else
            {
                List<BookLoan> loanedsBooks = ConvertFileToList();
                if(loanedsBooks.Count > 0)
                    for (int i = (loanedsBooks.Count - 1); i >= 0; i--)
                    {
                        if (loanedsBooks[i].TumbleNumber == bookTumbleNumber)
                            if(loanedsBooks[i].LoanStatus == 1)
                            {
                                available = false;
                                break;
                            }
                    }
            }

            return available;
        }

        private static List<BookLoan> ConvertFileToList()
        {
            List<BookLoan> loanedsBooks = new List<BookLoan>();
            FileHandler file = new FileHandler();
            string[] fileContent;

            file.FileName = "EMPRESTIMO.csv";

            fileContent = FileHandlerController.ReadFile(file);

            if (fileContent != null)
                foreach (var lineContent in fileContent)
                {
                    string[] lineLoan = lineContent.Split(';');

                    BookLoan bookLoan = new BookLoan
                    {
                        IdCustomer = long.Parse(lineLoan[0]),
                        TumbleNumber = long.Parse(lineLoan[1]),
                        LoanDate = Convert.ToDateTime(lineLoan[2]),
                        DevolutionDate = Convert.ToDateTime(lineLoan[3]),
                        LoanStatus = int.Parse(lineLoan[4])
                    };

                    loanedsBooks.Add(bookLoan);
                }

            return loanedsBooks;
        }

        private static BookLoan ReadingBookData(long bookTumbleNumber, long idCustomer)
        {
            Console.Write("Data da devolução: ");
            string devolutionDate = Console.ReadLine();

            DateTime dod;

            DateTime.TryParse(devolutionDate, out dod);

            BookLoan bookLoan = new BookLoan
            {
                IdCustomer = idCustomer,
                TumbleNumber = bookTumbleNumber,
                LoanDate = DateTime.Now,
                DevolutionDate = dod,
                LoanStatus = 1
            };

            return bookLoan;
        }

        private static void ConvertListForWriteFile(BookLoan bookLoan)
        {
            List<BookLoan> loanedsBooks = ConvertFileToList();
            FileHandler file = new FileHandler();

            file.FileName = "EMPRESTIMO.csv";
            loanedsBooks.Add(bookLoan);

            StringBuilder bookLoanSb = new StringBuilder();
            foreach (var sbBookLoan in loanedsBooks)
            {
                bookLoanSb.Append(sbBookLoan.ToStringForWriteFile());
                bookLoanSb.AppendLine();
            }

            string[] booksLoanForWrite = bookLoanSb.ToString().Split('\n');

            FileHandlerController.WriteInFile(file, booksLoanForWrite);
        }


    }
}
