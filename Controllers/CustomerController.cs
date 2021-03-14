using System;
using System.Collections.Generic;
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

            Console.Write("Informe o CPF do cliente: ");
            customerCpf = Console.ReadLine();

            if (CustomerExists(customerCpf))
            {
                Console.WriteLine("Cliente já cadastrado");
                Console.Write("Pressione qualquer tecla para voltar ao menu princial...");
                Console.ReadKey();
            }
            else
            {
                customer = ReadingCustomerData(customerCpf);

                //GRAVAR DADOS DO CLIENTE NO ARQUIVO
            }
        }

        private static Customer ReadingCustomerData(string customerCpf) 
        {
            Customer customer = new Customer();

            //LER DADOS DO CLIENTE
            return customer;
        }

        private static bool CustomerExists(string customerCpf) 
        {
            //VERIFICAR SE O ARQUIVO EXISTE
            //SE NAO EXISTIR
                //CRIAR O ARQUIVO
                //RETORNAR QUE O CLIENTE NAO EXISTE
            //SE EXISTIR
                //VERIFICAR SE O CLIENTE JÁ ESTA CADASTRADO
                //SE ESTIVER
                    //RETORNAR TRUE
                //SE NAO ESTIVER
                    //RETORNAR FALSE

            return true;
        }
    }
}
