namespace RobotWebApi.Models
{
    public class Camera
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToString()
        {
            return $"Camera info: width = {Width}, height: {Height}";
        }
    }
}
