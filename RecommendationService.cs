using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmBase
{
    public interface IRecommendationService
    {
        IEnumerable<Product> FindHigestRatedProducts(IEnumerable<Product> products, int length = 3);
        IEnumerable<Product> FindMostViewedProducts(IEnumerable<Product> products, IEnumerable<User> users, int length = 3);
        IEnumerable<Product> FindSimilarProducts(IEnumerable<Product> products, Product product, int length = 3);
    }

    public class RecommendationService : IRecommendationService
    {
        public IEnumerable<Product> FindMostViewedProducts(IEnumerable<Product> products, IEnumerable<User> users, int length = 3)
        {
            return RankProductsBySelector(products, Selector);

            Dictionary<Product, int> Selector(Dictionary<Product, int> dict, Product product)
            {
                int count = users.Where(u => u.purchased.Contains(product.id)).Count();
                dict.Add(product, count);
                return dict;
            }
        }

        public IEnumerable<Product> FindHigestRatedProducts(IEnumerable<Product> products, int length = 3) => products.OrderBy(p => p.rating).Reverse().Take(length);

        public IEnumerable<Product> FindSimilarProducts(IEnumerable<Product> products, Product product, int length = 3)
        {
            return RankProductsBySelector(products, Selector);

            Dictionary<Product, int> Selector(Dictionary<Product, int> dict, Product current)
            {
                if (product.id == current.id)
                {
                    return dict;
                }

                int count = product.keyWords
                    .Intersect(current.keyWords)
                    .Count();

                dict.Add(current, count);

                return dict;
            }
        }

        private IEnumerable<Product> RankProductsBySelector(IEnumerable<Product> products,
            Func<Dictionary<Product, int>, Product, Dictionary<Product, int>> selector,
            int length = 3) =>

                products
                .Aggregate(new Dictionary<Product, int>(), selector)
                .OrderBy(entry => entry.Value)
                .Reverse()
                .Take(length)
                .Select(kv => kv.Key);
    }
}
