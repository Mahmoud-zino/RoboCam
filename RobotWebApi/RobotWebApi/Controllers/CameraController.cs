using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotWebApi.Models;
using static RobotWebApi.Extensions.CameraExtensions;

namespace RobotWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CameraController : ControllerBase
    {
        private readonly ILogger<FaceController> logger;

        public CameraController(ILogger<FaceController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public Camera Get()
        {
            Camera camera = GetCameraFromJson();
            logger.LogInformation($"Get(Camera) was called => {camera}");
            return camera;
        }

        [HttpPost]
        public void Post([FromBody]Camera camera)
        {
            camera.PostCameraToJson();
            logger.LogInformation($"Post(Camera) was called => {camera}");
        }
    }
}
