using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class BookLoan
	{
		public long IdCustomer { get; set; }
		public long TumbleNumber { get; set; }
		public DateTime LoanDate { get; set; }
		public DateTime DevolutionDate { get; set; }
		public int LoanStatus { get; set; }

		public string ToStringForWriteFile()
		{
			return $"{IdCustomer};{TumbleNumber};{LoanDate};{DevolutionDate};{LoanStatus}";
		}
	}
}
