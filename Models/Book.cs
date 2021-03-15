using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Book
	{
		public long TumbleNumber { get; set; }
		public string ISBN { get; set; }
		public string Title { get; set; }
		public string Genre { get; set; }
		public DateTime PublicationDate { get; set; }
		public string Author { get; set; }

		public string ToStringForWriteFile()
		{
			return $"{TumbleNumber};{ISBN};{Title};{Genre};{PublicationDate};{Author}";
		}
	}
}
