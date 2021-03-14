using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Customer
    {
		public long IdCustomer { get; set; }
		public string Cpf { get; set; }
		public string Name { get; set; }
		public DateTime DateOfBorning { get; set; }
		public string Telephone { get; set; }
		public string PublicPlace { get; set; }
		public string Neighborhood { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
	}
}
