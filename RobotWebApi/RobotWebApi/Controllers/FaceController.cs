using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotWebApi.Models;
using static RobotWebApi.Extensions.FaceExtensions;

namespace RobotWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceController : ControllerBase
    {
        private readonly ILogger<FaceController> logger;

        public FaceController(ILogger<FaceController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public Face Get()
        {
            Face face = GetFaceFromJson();
            logger.LogInformation($"Get(Face) was called => {face}");
            return face;
        }

        [HttpPost]
        public void Post([FromBody]Face face)
        {
            face.PostFaceToJson();
            logger.LogInformation($"Post(Face) was called => {face}");
        }
    }
}
