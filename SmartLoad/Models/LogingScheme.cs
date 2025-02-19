using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLoad.Models
{
    public class LoadingScheme
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Тип транспортного средства обязателен")]
        public int VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Транспортное средство обязательно")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Маршрут обязателен")]
        public int RouteId { get; set; }

        [Required(ErrorMessage = "Дата загрузки обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата загрузки")]
        public DateTime LoadingDate { get; set; }

        [Required(ErrorMessage = "Статус обязателен")]
        [StringLength(100, ErrorMessage = "Статус должен быть не длиннее 100 символов")]
        [Display(Name = "Статус")]
        public string Status { get; set; }

        [StringLength(500, ErrorMessage = "Примечания не должны превышать 500 символов")]
        [Display(Name = "Примечания")]
        public string Notes { get; set; }

        // Навигационное свойство
        public VehicleType VehicleType { get; set; }

        // Навигационное свойство
        public Vehicle Vehicle { get; set; }

        // Навигационное свойство
        public SmartLoad.Models.Rout Rout { get; set; }

        // Навигационное свойство
        public ICollection<LoadingProduct> LoadingProducts { get; set; }
    }
}