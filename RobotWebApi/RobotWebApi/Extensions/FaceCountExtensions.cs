using Newtonsoft.Json.Linq;
using RobotWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotWebApi.Extensions
{
    public static class FaceCountExtensions
    {
        private static readonly string fileName = "faceCount.json";
        public static FaceCount GetFaceCountFromJson()
        {
            return JsonManager.GetDeserilizedJsonObj<FaceCount>(fileName);
        }

        public static void PostFaceCountToJson(this FaceCount faceCount)
        {
            JsonManager.SerializeJsonObj(faceCount, fileName);
        }
    }
}
