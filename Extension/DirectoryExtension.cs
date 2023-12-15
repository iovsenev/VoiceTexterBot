using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceTexterBot.Extension
{
    public class DirectoryExtension
    {
        /// <summary>
        /// Получаем путь до каталога с .sln файлом
        /// </summary>
        public static string GetSolutionRoot()
        {
            var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            var fullname = Directory.GetParent(dir).FullName;
            //Console.WriteLine(fullname);
            var projectRoot = fullname.Substring(0, fullname.Length - 4);
            //Console.WriteLine(projectRoot);
            return projectRoot;//Directory.GetParent(projectRoot)?.FullName;
        }
    }
}
