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

            Console.Clear();
            Console.WriteLine("-x-x-x-x-  Cadastro de Cliente  -x-x-x-x-");
            Console.WriteLine("Informe os dados do cliente\n");
            do
            {
                Console.Write("CPF: ");
                customerCpf = Console.ReadLine();
                customerCpf = customerCpf.Trim();
            } while (customerCpf == "");

            if (CustomerExists(customerCpf))
            {
                Console.WriteLine("\nCliente já cadastrado\n");
                Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
            else
            {
                customer = ReadingCustomerData(customerCpf);
                customer.IdCustomer = NewCustomerId();
                ConvertListForWriteFile(customer);
				Console.WriteLine("\nCliente cadastrado com sucesso!!");
                Console.Write("\nPressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
        }

        private static Customer ReadingCustomerData(string customerCpf) 
        {
            DateTime dob;
            string dateOfBorning;
            string cpf = customerCpf;
			Console.Write("Nome completo: ");
			string name = Console.ReadLine();

            do
            {
                Console.Write("Data de nascimento: ");
                dateOfBorning = Console.ReadLine();
                DateTime.TryParse(dateOfBorning, out dob);
            } while (dob.ToString("dd/MM/yyyy") == "01/01/0001");

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
            bool exists = false;

            FileHandler file = new FileHandler();
            file.FileName = "CLIENTE.csv";

            if (FileHandlerController.CreateDirectoryAndFile(file))
                exists = false;
            else
            {
                List<Customer> customers = ConvertFileToList();
                for (int i = 0; i < customers.Count; i++)
				{
                    if (customers[i].Cpf.Equals(customerCpf))
                    {
                        exists = true;
                        break;
                    }
				}
            } 

            return exists;
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

        public static List<Customer> ConvertFileToList()
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

        public static long CustomerExistsAndReturnId(string customerCpf)
        {
            long idCustomer = 0;

            FileHandler file = new FileHandler();
            file.FileName = "CLIENTE.csv";

            if (FileHandlerController.CreateDirectoryAndFile(file))
                idCustomer = 0;
            else
            {
                List<Customer> customers = ConvertFileToList();
                for (int i = 0; i < customers.Count; i++)
                {
                    if (customers[i].Cpf.Equals(customerCpf))
                    {
                        idCustomer = customers[i].IdCustomer;
                        break;
                    }
                }
            }

            return idCustomer;
        }

    }
}
