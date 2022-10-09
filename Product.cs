using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmBase
{
    public record Product(int id, string name, int year, IList<string> keyWords, decimal rating, int price);
}
