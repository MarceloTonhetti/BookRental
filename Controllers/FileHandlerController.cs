using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Controllers
{
	public class FileHandlerController
	{
        public static void CreateDirectoryAndFile(FileHandler file)
        {
            if (!Directory.Exists(file.GetPath()))
                Directory.CreateDirectory(file.GetPath());
          
            if (!File.Exists($@"{file.GetPath()}\{file.FileName}"))
                File.Create($@"{file.GetPath()}\{file.FileName}");
        }

        public static void WriteInFile(FileHandler file, string[] content)
        {
            using (StreamWriter streamWriter = File.CreateText($@"{file.GetPath()}\{file.FileName}"))
            {
                foreach (var lineContent in content)
                {
                    streamWriter.Write(lineContent);
                }
            }
        }

        public static string[] ReadFile(FileHandler file)
        {
            return File.ReadAllLines($@"{file.GetPath()}\{file.FileName}");
        }
    }
}
