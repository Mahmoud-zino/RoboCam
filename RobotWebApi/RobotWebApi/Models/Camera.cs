namespace RobotWebApi.Models
{
    public class Camera
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public override string ToString()
        {
            return $"Camera info: width = {Width}, height: {Height}";
        }
    }
}
