using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceController : ControllerBase
    {
        Face face;
        private readonly ILogger<FaceController> logger;

        public FaceController(ILogger<FaceController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public Face Get()
        {
            logger.LogInformation($"Get(Face) was called => {this.face}");
            return this.face;
        }

        [HttpPost]
        public void Post([FromBody]Face face)
        {
            this.face = face;
            logger.LogInformation($"Post(Face) was called => {face}");
        }
    }
}
