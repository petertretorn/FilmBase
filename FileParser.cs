using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmBase
{
    public abstract class FileParser<T>
    {
        public IEnumerable<T> Parse(string path)
        {
            return File.ReadAllLines(path)
                .Select(ParseLine)
                .ToList();

        }
        public abstract T ParseLine(string line);
    }

    public class ProductParser : FileParser<Product>
    {
        public override Product ParseLine(string line)
        {
            var parts = line.Split(",");

            var keywords = parts
                .Skip(3)
                .Take(5)
                .Where(p => !string.IsNullOrWhiteSpace(p));

            var id = int.Parse(parts[0]);
            var year = int.Parse(parts[2]);
            var rating = decimal.Parse(parts[8]);
            var price = int.Parse(parts[9]);

            return new Product(id, parts[1], year, keywords.ToList(), rating, price);
        }
    }

    public class UserParser : FileParser<User>
    {
        public override User ParseLine(string line)
        {
            var parts = line.Split(",");

            var id = int.Parse(parts[0]);
            var name = parts[1] ?? string.Empty;

            var viewed = ParseSequence(parts[2]);
            var purchased = ParseSequence(parts[3]);

            return new User(id, name, viewed, purchased);
        }

        private IList<int> ParseSequence(string part) => part.Split(";").Select(int.Parse).ToList<int>();
    }

    public class CurrentSessionParser : FileParser<CurrentSession>
    {
        public override CurrentSession ParseLine(string line)
        {
            var parts = line.Split(",");
            
            var userId = int.Parse(parts[0]);
            var productId = int.Parse(parts[1]);

            return new CurrentSession(userId, productId);
        }
    }

}
