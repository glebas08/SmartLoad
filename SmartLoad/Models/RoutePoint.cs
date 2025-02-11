using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// связь между заказами и точками маршрута, так как это много-ко-многим отношение.

namespace SmartLoad.Models
{
    public class RoutePoint
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ID маршрута обязателен")]
        public int RouteId { get; set; }

        [Required(ErrorMessage = "Название точки маршрута обязательно")]
        [StringLength(100, ErrorMessage = "Название точки маршрута должно быть не длиннее 100 символов")]
        [Display(Name = "Название точки маршрута")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Дата выгрузки обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата выгрузки")]
        public DateTime UnloadingDate { get; set; }

        // Навигационное свойство для маршрута
        public SmartLoad.Models.Rout Rout { get; set; }

        // Навигационное свойство для заказов через OrderRoutePoint
        public ICollection<OrderRoutePoint> OrderRoutePoints { get; set; }

        // Навигационное свойство для продуктов в схеме погрузки
        public ICollection<LoadingProduct> LoadingProducts { get; set; }
    }
}