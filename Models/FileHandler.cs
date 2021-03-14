using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class FileHandler
	{
		private string FilePath = "C:\\BookRentalFiles";
		public string FileName { get; set; }

		public string GetPath() 
		{
			return this.FilePath;
		}
	}
}
