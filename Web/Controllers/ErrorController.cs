using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;

namespace Web.Controllers;

public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        return View("404ErrorPage");
    }
}