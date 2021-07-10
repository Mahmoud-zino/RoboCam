using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotWebApi.Models;

namespace RobotWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CameraController : ControllerBase
    {
        Camera camera;
        private readonly ILogger<FaceController> logger;

        public CameraController(ILogger<FaceController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public Camera Get()
        {
            logger.LogInformation($"Get(Camera) was called => {this.camera}");
            return this.camera;
        }

        [HttpPost]
        public void Post([FromBody]Camera camera)
        {
            this.camera = camera;
            logger.LogInformation($"Post(Camera) was called => {camera}");
        }
    }
}
