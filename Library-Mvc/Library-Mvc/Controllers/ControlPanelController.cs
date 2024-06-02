using Microsoft.AspNetCore.Mvc;

namespace Library_Mvc.Controllers
{
    public class ControlPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
