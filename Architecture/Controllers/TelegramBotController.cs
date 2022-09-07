using Microsoft.AspNetCore.Mvc;

namespace Architecture.Controllers
{
    [Route("api/telegram")]
    [ApiController]
    public class TelegramBotController : Controller
    {

        /// <summary>
        /// Request telegram data by webhook
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        #if DEBUG
                [ApiExplorerSettings(IgnoreApi = false)]
        #else
                [ApiExplorerSettings(IgnoreApi = true)]
        #endif  
        [HttpPost("webhook")]
        public IActionResult Bot([FromBody] object update)
        {
            return Ok(update.ToString());
        }
    }
}
