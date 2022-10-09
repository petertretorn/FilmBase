using System.Linq;

namespace FilmBase
{
    public class ApplicationRunner
    {
        private readonly FileParser<Product> productParser;
        private readonly FileParser<User> userParser;
        private readonly FileParser<CurrentSession> currentSessionParser;
        private readonly IRecommendationService recommendationService;
        private readonly IOutputter outputter;

        // Poor mans Dependency Injection!
        public ApplicationRunner() : 
            this(new ProductParser(), 
                new UserParser(), 
                new CurrentSessionParser(), 
                new RecommendationService(), 
                new ConsoleOutputter()) {}
        public ApplicationRunner(
            FileParser<Product> productParser,
            FileParser<User> userParser,
            FileParser<CurrentSession> CurrentSessionParser,
            IRecommendationService recommendationService,
            IOutputter outputter)
        {
            this.productParser = productParser;
            this.userParser = userParser;
            this.currentSessionParser = CurrentSessionParser;
            this.recommendationService = recommendationService;
            this.outputter = outputter;
        }

        public void Run()
        {
            var products = productParser.Parse(@"C:\Users\peter\source\repos\FilmBase\FilmBase\data\products.txt");
            var users = userParser.Parse(@"C:\Users\peter\source\repos\FilmBase\FilmBase\data\users.txt");
            var currentSessions = currentSessionParser.Parse(@"C:\Users\peter\source\repos\FilmBase\FilmBase\data\currentUserSession.txt");

            var higestRatedProducts = recommendationService.FindHigestRatedProducts(products);
            var mostViewedProducts = recommendationService.FindMostViewedProducts(products, users);

            outputter.OutputListing("Most Viewed Products:", mostViewedProducts, p => $"{p.name}");
            outputter.OutputListing("Higest Rated Products:", higestRatedProducts, p => $"{p.name}\tRating: {p.rating}");

            foreach (var session in currentSessions)
            {
                var recommendedProducts = recommendationService.FindSimilarProducts(products, products.Single(p => p.id == session.productId));

                outputter.OutputListing($"Recommended Products for User {session.userId}", recommendedProducts, p => $"{p.name}");
            }
            
        }
    }
}
