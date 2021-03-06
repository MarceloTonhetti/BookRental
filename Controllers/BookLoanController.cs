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
            string auxTumbleNumber;
            long idCustomer;
            string cpfCustomer;
            BookLoan bookLoan;

            Console.Clear();
            Console.WriteLine("-x-x-x-x-  Emprestimo de Livro  -x-x-x-x-");
            Console.WriteLine("\nInforme os dados para o emprestimo\n");
            do
            {
                Console.Write("Numero do Tombo: ");
                auxTumbleNumber = Console.ReadLine();
            } while (!long.TryParse(auxTumbleNumber, out bookTumbleNumber));

            if (BookController.BookExists(bookTumbleNumber))
            {
                if (!BookAvailable(bookTumbleNumber))
                {
                    Console.WriteLine("\nLivro indisponivel\n");
                    Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
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
                        Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                        Console.ReadKey();
                    }
                    else
                    {
                        bookLoan = ReadingBookData(bookTumbleNumber, idCustomer);
                        ConvertListForWriteFile(bookLoan);
                        Console.WriteLine("\nEmprestimo realizado com sucesso!!");
                        Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                        Console.ReadKey();
                    }
                }
            }
            else
            {
                Console.WriteLine("\nLivro nao cadastrado\n");
                Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
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
            DateTime dod;
            bool dodIsPast;

            do
            {
                Console.Write("Data da devolução: ");
                string devolutionDate = Console.ReadLine();
                DateTime.TryParse(devolutionDate, out dod);
                if (DateTime.Compare(dod, DateTime.Now.Date) < 0)
                {
                    dodIsPast = true;
					Console.WriteLine("A data tem que ser igual ou posterior a data de hoje!!");
                }
                else
                    dodIsPast = false;

            } while (dod.ToString("dd/MM/yyyy") == "01/01/0001" || dodIsPast);

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

        private static void ConvertListForWriteFile(BookLoan bookLoan, bool update = false)
        {
            List<BookLoan> loanedsBooks = ConvertFileToList();
            FileHandler file = new FileHandler();
            file.FileName = "EMPRESTIMO.csv";

            if (!update)
                loanedsBooks.Add(bookLoan);
            else
            {
                for (int i = (loanedsBooks.Count - 1); i >= 0; i--)
                {
                    if (loanedsBooks[i].TumbleNumber == bookLoan.TumbleNumber)
                        if (loanedsBooks[i].LoanStatus == 1)
                        {
                            loanedsBooks[i].LoanStatus = 2;
                            break;
                        }
                }
            }

            StringBuilder bookLoanSb = new StringBuilder();
            foreach (var sbBookLoan in loanedsBooks)
            {
                bookLoanSb.Append(sbBookLoan.ToStringForWriteFile());
                bookLoanSb.AppendLine();
            }

            string[] booksLoanForWrite = bookLoanSb.ToString().Split('\n');

            FileHandlerController.WriteInFile(file, booksLoanForWrite);
        }

        public static void MakeDevolution()
        {
            long bookTumbleNumber;
            string auxTumbleNumber;
            const double delayTicket = 0.10;
            BookLoan bookLoan;
            int delayDays;

            Console.Clear();
            Console.WriteLine("-x-x-x-x-  Devoucao do Livro  -x-x-x-x-");
            Console.WriteLine("\nInforme os dados para a devolucao\n");
            do
            {
                Console.Write("Numero do Tombo: ");
                auxTumbleNumber = Console.ReadLine();
            } while (!long.TryParse(auxTumbleNumber, out bookTumbleNumber));

            if (BookController.BookExists(bookTumbleNumber))
                if (BookAvailable(bookTumbleNumber))
                {
                    Console.WriteLine("\nLivro nao encontrado para devolucao\n");
                    Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                    Console.ReadKey();
                }
                else
                {
                    bookLoan = GetBookLoan(bookTumbleNumber);
                    ConvertListForWriteFile(bookLoan, true);
                    Console.WriteLine("\nDevolucao realizada com sucesso!!");

                    delayDays = (bookLoan.DevolutionDate.Subtract(bookLoan.LoanDate)).Days;

                    if (delayDays < 0)
						Console.WriteLine("Valor da multa do cliente: R$ " + (delayTicket * (delayDays*-1)).ToString("F"));
                    
                        Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                        Console.ReadKey();
                    
                }
            else
            {
                Console.WriteLine("\nLivro nao cadastrado\n");
                Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
        }

        private static BookLoan GetBookLoan(long bookTumbleNumber)
        {
            BookLoan bookLoan = new BookLoan();

            List<BookLoan> loanedsBooks = ConvertFileToList();
            if (loanedsBooks.Count > 0)
                for (int i = (loanedsBooks.Count - 1); i >= 0; i--)
                {
                    if (loanedsBooks[i].TumbleNumber == bookTumbleNumber)
                        if (loanedsBooks[i].LoanStatus == 1)
                        {
                            bookLoan = loanedsBooks[i];
                            break;
                        }
                }

            return bookLoan;
        }

        public static void ShowReportLoan()
        {
            List<Customer> customers = CustomerController.ConvertFileToList();
            List<Book> books = BookController.ConvertFileToList();
            List<BookLoan> loanedsBooks = ConvertFileToList();

            Customer auxCustomer;
            Book auxBook;

            Console.Clear();
			Console.WriteLine("-x-x-x-x-x-x- Relatorio de Emprestimos e Devolucoes -x-x-x-x-x-x-");

            if(loanedsBooks.Count > 0)
                foreach (var loanBook in loanedsBooks)
			    {
                    auxBook = books.Find(x => x.TumbleNumber == loanBook.TumbleNumber);
                    auxCustomer = customers.Find(x => x.IdCustomer == loanBook.IdCustomer);

				    Console.WriteLine("\n-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-");
                    Console.WriteLine($"CPF: {auxCustomer.Cpf}");
                    Console.WriteLine($"Titulo: {auxBook.Title}");
                    if(loanBook.LoanStatus == 1)
                        Console.WriteLine("Status: Emprestado");
                    else
                        Console.WriteLine("Status: Devolvido");
                    Console.WriteLine($"Data Emprestimo: {loanBook.LoanDate.ToString("dd/MM/yyyy")}");
                    Console.WriteLine($"Data Devolucao: {loanBook.DevolutionDate.ToString("dd/MM/yyyy")}");
               
                }
            else
				Console.WriteLine("\nNao existe registros de livros emprestados e devolvidos");

            Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
            Console.ReadKey();
        }
    }
}
