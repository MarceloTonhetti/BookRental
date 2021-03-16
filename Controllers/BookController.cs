using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Controllers
{
	public class BookController
	{
        public static void RegisterABook()
        {
            string bookISBN;
            Book book;

            Console.Clear();
			Console.WriteLine("-=-=-=-=-  Cadastro de Livro  -=-=-=-=-");
            Console.WriteLine("\nInforme os dados do livro\n");
            Console.Write("ISBN: ");
            bookISBN = Console.ReadLine();

            if (BookExists(bookISBN))
            {
                Console.WriteLine("\nLivro já cadastrado\n");
                Console.Write("Pressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
            else
            {
                book = ReadingBookData(bookISBN);
                book.TumbleNumber = NewTumbleNumber();
                ConvertListForWriteFile(book);
                Console.WriteLine("\nLivro cadastrado com sucesso!!");
                Console.WriteLine("\n>>> Numero do tombo: {0}", book.TumbleNumber);
                Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
        }

        private static bool BookExists(string bookISBN)
        {
            bool exists = false;

            FileHandler file = new FileHandler();
            file.FileName = "LIVRO.csv";

            if (FileHandlerController.CreateDirectoryAndFile(file))
                exists = false;
            else
            {
                List<Book> books = ConvertFileToList();
                for (int i = 0; i < books.Count; i++)
                {
                    if (books[i].ISBN.Equals(bookISBN))
                    {
                        exists = true;
                        break;
                    }
                }
            }

            return exists;
        }

        public static List<Book> ConvertFileToList()
        {
            List<Book> books = new List<Book>();
            FileHandler file = new FileHandler();
            string[] fileContent;

            file.FileName = "LIVRO.csv";

            fileContent = FileHandlerController.ReadFile(file);

            if (fileContent != null)
                foreach (var lineContent in fileContent)
                {
                    string[] lineBook = lineContent.Split(';');

                    Book book = new Book
                    {
                        TumbleNumber = long.Parse(lineBook[0]),
                        ISBN = lineBook[1],
                        Title = lineBook[2],
                        Genre = lineBook[3],
                        PublicationDate =  Convert.ToDateTime(lineBook[4]),
                        Author = lineBook[5]
                    };

                    books.Add(book);
                }

            return books;
        }

        private static Book ReadingBookData(string bookISBN)
        {
            string isbn = bookISBN;
            Console.Write("Titulo: ");
            string title = Console.ReadLine();
            Console.Write("Genero: ");
            string genre = Console.ReadLine();
            Console.Write("Data da Publicação: ");
            string publicationDate = Console.ReadLine();
            Console.Write("Autor: ");
            string author = Console.ReadLine();

            DateTime dop;

            DateTime.TryParse(publicationDate, out dop);

            Book book = new Book
            {
                ISBN = isbn,
                Title = title,
                Genre = genre,
                PublicationDate = dop,
                Author = author
            };

            return book;
        }

        private static long NewTumbleNumber()
        {
            long newTumbleNumber;
            List<Book> books = ConvertFileToList();

            if (books.Count > 0)
            {
                newTumbleNumber = books.Last().TumbleNumber;
                newTumbleNumber++;
            }
            else
                newTumbleNumber = 1;

            return newTumbleNumber;
        }

        private static void ConvertListForWriteFile(Book book)
        {
            List<Book> books = ConvertFileToList();
            FileHandler file = new FileHandler();

            file.FileName = "LIVRO.csv";
            books.Add(book);

            StringBuilder bookSb = new StringBuilder();
            foreach (var sbBook in books)
            {
                bookSb.Append(sbBook.ToStringForWriteFile());
                bookSb.AppendLine();
            }

            string[] booksForWrite = bookSb.ToString().Split('\n');

            FileHandlerController.WriteInFile(file, booksForWrite);
        }

        public static bool BookExists(long bookTumbleNumber)
        {
            bool exists = false;

            FileHandler file = new FileHandler();
            file.FileName = "LIVRO.csv";

            if (FileHandlerController.CreateDirectoryAndFile(file))
                exists = false;
            else
            {
                List<Book> books = ConvertFileToList();
                for (int i = 0; i < books.Count; i++)
                {
                    if (books[i].TumbleNumber == bookTumbleNumber)
                    {
                        exists = true;
                        break;
                    }
                }
            }

            return exists;
        }
    }
}
