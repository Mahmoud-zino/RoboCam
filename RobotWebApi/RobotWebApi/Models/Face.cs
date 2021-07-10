namespace RobotWebApi.Models
{
    public class Face
    {
        public float XPoint { get; set; }
        public float YPoint { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public override string ToString()
        {
            return $"Face info: x = {XPoint}, y = {YPoint}, width = {Width}, height: {Height}";
        }
    }
}
