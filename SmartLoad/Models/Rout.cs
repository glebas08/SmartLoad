using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

//будет представлять маршрут, по которому будут доставляться заказы

namespace SmartLoad.Models
{
    public class Rout
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название маршрута обязательно")]
        [StringLength(100, ErrorMessage = "Название маршрута должно быть не длиннее 100 символов")]
        [Display(Name = "Название маршрута")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Дата отправления обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата отправления")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Дата прибытия обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата прибытия")]
        public DateTime ArrivalDate { get; set; }

        [StringLength(500, ErrorMessage = "Примечания не должны превышать 500 символов")]
        [Display(Name = "Примечания")]
        public string Notes { get; set; }

        // Навигационное свойство для точек маршрута
        public ICollection<RoutePoint> RoutePoints { get; set; }

        // Навигационное свойство для заказов
        public ICollection<Order> Orders { get; set; }

        // Навигационное свойство для схем погрузки
        public ICollection<LoadingScheme> LoadingSchemes { get; set; }


        public DateTime RouteDate { get; set; }

        public void SetRouteDate(DateTime date)
        {
            RouteDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }
    }
}