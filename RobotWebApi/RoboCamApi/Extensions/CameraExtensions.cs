using ApiTest.Models;

namespace ApiTest.Extensions
{
    public static class CameraExtensions
    {
        private static readonly string fileName = "camera.json";
        public static Camera GetCameraFromJson()
        {
            return JsonManager.GetDeserilizedJsonObj<Camera>(fileName);
        }

        public static void PostCameraToJson(this Camera camera)
        {
            JsonManager.SerializeJsonObj(camera, fileName);
        }
    }
}
