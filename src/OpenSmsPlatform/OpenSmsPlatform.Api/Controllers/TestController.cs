using Microsoft.AspNetCore.Mvc;

namespace OpenSmsPlatform.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpGet]
        public string Index()
        {
            return "Hello World !";
        }
    }
}
