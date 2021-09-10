using ApiTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Extensions
{
    public static class FaceExtensions
    {
        private static readonly string fileName = "face.json";
        public static Face GetFaceFromJson()
        {
            return JsonManager.GetDeserilizedJsonObj<Face>(fileName);
        }

        public static void PostFaceToJson(this Face face)
        {
            JsonManager.SerializeJsonObj(face, fileName);
        }
    }
}
