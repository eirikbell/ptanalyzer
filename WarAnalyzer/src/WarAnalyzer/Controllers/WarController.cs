using Microsoft.AspNet.Mvc;
using PaperParser;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WarAnalyzer.Controllers
{
    public class WarController : Controller
    {
        // GET: /<controller>/
        public IActionResult War5_7()
        {
            var file = "war5_7.txt";

            var dataLoader = new DataLoader();

            var data = dataLoader.GetData(file);

            var parsed = new DataParser().Parse(data);

            return View("Analysis", parsed);
        }

        // GET: /<controller>/
        public IActionResult War6_10()
        {
            var file = "war6_10.txt";

            var dataLoader = new DataLoader();

            var data = dataLoader.GetData(file);

            var parsed = new DataParser().Parse(data);

            return View("Analysis", parsed);
        }

        // GET: /<controller>/
        public IActionResult War2_4()
        {
            var file = "war2_4.txt";

            var dataLoader = new DataLoader();

            var data = dataLoader.GetData(file);

            var parsed = new DataParser().Parse(data);

            return View("Analysis", parsed);
        }

        // GET: /<controller>/
        public IActionResult War6_13()
        {
            var file = "war6_13.txt";

            var dataLoader = new DataLoader();

            var data = dataLoader.GetData(file);

            var parsed = new DataParser().Parse(data);

            return View("Analysis", parsed);
        }
    }
}
