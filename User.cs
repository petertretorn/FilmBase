using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmBase
{
    public record User(int id, string name, IList<int> viewed, IList<int> purchased);
}
