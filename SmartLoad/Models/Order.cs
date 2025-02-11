using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLoad.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Дата заказа обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата заказа")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Статус обязателен")]
        [StringLength(100, ErrorMessage = "Статус должен быть не длиннее 100 символов")]
        [Display(Name = "Статус")]
        public string Status { get; set; }

        [StringLength(500, ErrorMessage = "Примечания не должны превышать 500 символов")]
        [Display(Name = "Примечания")]
        public string Notes { get; set; } = string.Empty; // Устанавливаем значение по умолчанию

        // Связь с маршрутом
        [Required(ErrorMessage = "Маршрут обязателен")]
        public int RouteId { get; set; }

        // Навигационное свойство для маршрута
        [ForeignKey("RouteId")]
        public SmartLoad.Models.Rout Rout { get; set; }

        // Навигационное свойство для продуктов в заказе
        public ICollection<OrderProduct> OrderProducts { get; set; }

        // Навигационное свойство для продуктов в схеме погрузки
        public ICollection<LoadingProduct> LoadingProducts { get; set; }

        // Навигационное свойство для точек маршрута
        public ICollection<OrderRoutePoint> OrderRoutePoints { get; set; }
    }
}