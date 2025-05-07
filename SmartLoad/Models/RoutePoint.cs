using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SmartLoad.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLoad.Models
{
    public class RoutePoint
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название точки маршрута обязательно")]
        [StringLength(100, ErrorMessage = "Название точки маршрута должно быть не длиннее 100 символов")]
        [Display(Name = "Название точки маршрута")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Дата выгрузки обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата выгрузки")]
        public DateTime UnloadingDate { get; set; } = DateTime.UtcNow;

        //public int? RouteId { get; set; } // Сделали RouteId необязательным
        //[ValidateNever]
        //public Rout? Rout { get; set; } // Навигационное свойство для маршрута
        
        [ValidateNever]
        public ICollection<RoutePointMapping> RoutePointMappings { get; set; } // Связь с RoutePointMapping

        [ValidateNever]
        public ICollection<Order> Orders { get; set; } // Заказы, связанные с точкой маршрута

        // Вычисляемое свойство для получения точек маршрута
        [NotMapped]
        [ValidateNever]
        public IEnumerable<RoutePoint> RoutePoints => RoutePointMappings?.Select(rpm => rpm.RoutePoint);
    }
}
