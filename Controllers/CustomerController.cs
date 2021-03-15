using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Controllers
{
    public class CustomerController
    {
        public static void RegisterACustomer()
        {
            string customerCpf;
            Customer customer;

			Console.WriteLine("Informe os dados do cliente\n");
            Console.Write("CPF: ");
            customerCpf = Console.ReadLine();

            if (CustomerExists(customerCpf))
            {
                Console.WriteLine("\nCliente já cadastrado\n");
                Console.Write("Pressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
            else
            {
                customer = ReadingCustomerData(customerCpf);
                customer.IdCustomer = NewCustomerId();
                ConvertListForWriteFile(customer);
				Console.WriteLine("Cliente cadastrado com sucesso!!");
                Console.Write("Pressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
        }

        private static Customer ReadingCustomerData(string customerCpf) 
        {
            string cpf = customerCpf;
			Console.Write("Nome completo: ");
			string name = Console.ReadLine();
            Console.Write("Data de nascimento: ");
            string dateOfBorning = Console.ReadLine();
            Console.Write("Telefone: ");
            string telephone = Console.ReadLine();
            Console.Write("Logradouro: ");
            string publicPlace = Console.ReadLine();
            Console.Write("Bairro: ");
            string neighborhood = Console.ReadLine();
            Console.Write("Cidade: ");
            string city = Console.ReadLine();
            Console.Write("Estado (UF): ");
            string state = Console.ReadLine();
            Console.Write("CEP: ");
            string zipCode = Console.ReadLine();

            DateTime dob;

            DateTime.TryParse(dateOfBorning, out dob);

            Customer customer = new Customer
            {
                Cpf = cpf,
                Name = name,
                DateOfBorning = dob,
                Telephone = telephone,
                PublicPlace = publicPlace,
                Neighborhood = neighborhood,
                City = city,
                State = state,
                ZipCode = zipCode
            };

            return customer;
        }

        private static bool CustomerExists(string customerCpf)
        {
            FileHandler file = new FileHandler();
            file.FileName = "CLIENTE.csv";

            if (FileHandlerController.CreateDirectoryAndFile(file))
                return false;
            else
                //VERIFICAR SE O CLIENTE JÁ ESTA CADASTRADO
                //SE ESTIVER
                    //RETORNAR TRUE
                //SE NAO ESTIVER
                    //RETORNAR FALSE

            return false;
        }

        private static long NewCustomerId()
        {
            long newId;
            List<Customer> customers = ConvertFileToList();

            if (customers.Count > 0)
            {
                newId = customers.Last().IdCustomer;
                newId++;
            }
            else
            {
                newId = 1;
            }

            return newId;
        }

        private static List<Customer> ConvertFileToList()
        {
            List<Customer> customers = new List<Customer>();
            FileHandler file = new FileHandler();
            string[] fileContent;
            
            file.FileName = "CLIENTE.csv";

            fileContent = FileHandlerController.ReadFile(file);

            if(fileContent != null)
			    foreach (var lineContent in fileContent)
			    {
                    string[] lineCustomer = lineContent.Split(';');

                    Customer customer = new Customer { 
                        IdCustomer = long.Parse(lineCustomer[0]),
                        Cpf = lineCustomer[1],
                        Name = lineCustomer[2],
                        DateOfBorning = Convert.ToDateTime(lineCustomer[3]),
                        Telephone = lineCustomer[4],
                        PublicPlace = lineCustomer[5],
                        Neighborhood = lineCustomer[6],
                        City = lineCustomer[7],
                        State = lineCustomer[8],
                        ZipCode = lineCustomer[9]
                    };

                    customers.Add(customer);
                }

            return customers;
        }

        private static void ConvertListForWriteFile(Customer customer) 
        {
            List<Customer> customers = ConvertFileToList();
            FileHandler file = new FileHandler();

            file.FileName = "CLIENTE.csv";
            customers.Add(customer);

            StringBuilder customerSb = new StringBuilder();
			foreach (var sbCustomer in customers)
			{
                customerSb.Append(sbCustomer.ToStringForWriteFile());
                customerSb.AppendLine();
			}

            string[] customersForWrite = customerSb.ToString().Split('\n');

            FileHandlerController.WriteInFile(file, customersForWrite);
        }

    }
}
