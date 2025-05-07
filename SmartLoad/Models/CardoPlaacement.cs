namespace SmartLoad.Models
{
    public class CargoPlacement
    {
       public int ProductId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string Destination { get; set; }
        public int RoutePointPriority { get; set; }
        public string ErrorMessage { get; set; }
        public int RoutePointOrder { get; set; }
        /* public int ProductId { get; set; }
         public double X { get; set; }
         public double Y { get; set; }
         public double Z { get; set; }
         public float Weight { get; set; }
         public string Destination { get; set; }*/
    }
}
