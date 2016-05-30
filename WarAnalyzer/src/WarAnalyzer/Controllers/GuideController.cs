using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WarAnalyzer.Controllers
{
    public class GuideController : Controller
    {
        // GET: /<controller>/
        public IActionResult ChainMitigation()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult PumpingStacking()
        {
            return View();
        }
    }
}
