using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public DateTime DepartureDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Дата прибытия обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата прибытия")]
        public DateTime ArrivalDate { get; set; }=DateTime.UtcNow.AddDays(7);

        [StringLength(500, ErrorMessage = "Примечания не должны превышать 500 символов")]
        [Display(Name = "Примечания")]
        public string Notes { get; set; }
        [ValidateNever] 
        public ICollection<RoutePointMapping> RoutePointMappings { get; set; }

        //public List<RoutePoint> RoutePoints { get; set; } // Точки маршрута в данном маршруте
    }
}

