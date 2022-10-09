using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmBase
{
    public interface IOutputter
    {
        void OutputListing<T>(string header, IEnumerable<T> productCollection, Func<T, string> makeLine);
    }

    public class ConsoleOutputter : IOutputter
    {
        public void OutputListing<T>(string header, IEnumerable<T> productCollection, Func<T, string> makeLine)
        {
            Console.WriteLine(header);

            foreach (var p in productCollection)
            {
                Console.WriteLine(makeLine(p));
            }

            Console.WriteLine();
        }
    }
}
