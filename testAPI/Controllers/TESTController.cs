using Microsoft.AspNetCore.Mvc;

namespace testAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TESTController : ControllerBase
    {
        [HttpPost]
        public testDTO Get([FromBody] testDTO dto)
        {
            return dto;
        }
    }
}