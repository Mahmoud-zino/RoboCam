using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RobotWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceCountController : ControllerBase
    {
        private readonly ILogger<FaceCountController> logger;
        int faceCount = 0;

        public FaceCountController(ILogger<FaceCountController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public int Get()
        {
            logger.LogInformation($"Get(FaceCount) was called => {this.faceCount}");
            return this.faceCount;
        }

        [HttpPost]
        public void Post(int faceCount)
        {
            this.faceCount = faceCount;
            logger.LogInformation($"Post(FaceCount) was called => {faceCount}");
        }
    }
}
