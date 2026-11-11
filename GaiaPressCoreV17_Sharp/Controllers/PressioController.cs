using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace GaiaPressCoreV17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PressioController : ControllerBase
    {
        [HttpGet]
        public string GetValue()
        {
            return "GaiaPress API is running!";
        }
    }
}