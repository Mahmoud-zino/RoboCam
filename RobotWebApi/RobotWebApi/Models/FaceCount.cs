namespace RobotWebApi.Models
{
    //its easier to deserialize objects instead of primitiv data!
    public class FaceCount
    {
        public int faceCount { get; set; }

        public override string ToString()
        {
            return $"FaceCount info: {faceCount}";
        }
    }
}
