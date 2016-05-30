using Microsoft.AspNet.Mvc;
using PaperParser;

namespace WarAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var dataLoader = new DataLoader();

            var data = dataLoader.GetData("data.txt");

            var parsed = new DataParser().Parse(data);

            return View("Analysis", parsed);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
