using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RobotWebApi.Models;
using System.IO;

namespace RobotWebApi
{
    public static class JsonManager
    {
        private static void CreateJsonFile(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
                return;

            File.Create(fileFullPath)
                .Close();
        }

        public static T GetDeserilizedJsonObj<T>(string fileName)
        {
            string fileFullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(fileFullPath));
            }
            catch
            {
                return default;
            }
        }

        public static void SerializeJsonObj<T>(T obj, string fileName)
        {
            string fileFullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            CreateJsonFile(fileFullPath);
            string jsonString = "";
            try
            {
                jsonString = JsonConvert.SerializeObject(obj);
            }
            catch
            {
                jsonString = JsonConvert.SerializeObject(default);
            }
            File.WriteAllText(fileFullPath, jsonString);
        }
    }
}
