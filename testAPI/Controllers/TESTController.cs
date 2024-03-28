using Microsoft.AspNetCore.Mvc;

namespace testAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TESTController : ControllerBase
    {
        [HttpPost]
        public TestDTO Get([FromBody] TestDTO dto)
        {
            return dto;
        }
    }
}