using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLoad.Models
{
    public class OrderRoutePoint
    {
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "ID заказа обязателен")]
        public int OrderId { get; set; }

        [Key, Column(Order = 1)]
        [Required(ErrorMessage = "ID точки маршрута обязателен")]
        public int RoutePointId { get; set; }

        // Навигационное свойство для заказа
        public Order Order { get; set; }

        // Навигационное свойство для точки маршрута
        public RoutePoint RoutePoint { get; set; }
    }
}



//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace SmartLoad.Models
//{
//    public class OrderRoutePoint
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required(ErrorMessage = "ID заказа обязателен")]
//        public int OrderId { get; set; }

//        [Required(ErrorMessage = "ID точки маршрута обязателен")]
//        public int RoutePointId { get; set; }

//        // Навигационное свойство для заказа
//        public Order Order { get; set; }

//        // Навигационное свойство для точки маршрута
//        public RoutePoint RoutePoint { get; set; }
//    }
//}