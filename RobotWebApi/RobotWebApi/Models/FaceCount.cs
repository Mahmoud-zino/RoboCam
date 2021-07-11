namespace RobotWebApi.Models
{
    //its easier to deserialize objects instead of primitiv data!
    public class FaceCount
    {
        public int Count { get; set; }

        public override string ToString()
        {
            return $"FaceCount info: {Count}";
        }
    }
}
