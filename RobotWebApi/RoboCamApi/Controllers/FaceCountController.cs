using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static ApiTest.Extensions.FaceCountExtensions;
using ApiTest.Models;

namespace ApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceCountController : ControllerBase
    {
        private readonly ILogger<FaceCountController> logger;

        public FaceCountController(ILogger<FaceCountController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public FaceCount Get()
        {
            FaceCount faceCount = GetFaceCountFromJson();
            logger.LogInformation($"Get(FaceCount) was called => {faceCount}");
            return faceCount;
        }

        [HttpPost]
        public void Post([FromBody]FaceCount faceCount)
        {
            faceCount.PostFaceCountToJson();
            logger.LogInformation($"Post(FaceCount) was called => {faceCount}");
        }
    }
}
