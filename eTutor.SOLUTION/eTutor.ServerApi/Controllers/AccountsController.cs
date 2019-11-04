using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    public class AccountsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}