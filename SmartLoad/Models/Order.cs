using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmartLoad.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Клиент обязателен")]
        public int DistributorId { get; set; }
        [ForeignKey("DistributorId")]
        public Distributor Distributor { get; set; }

        [Required(ErrorMessage = "Дата доставки обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата доставки")]
        public DateTime DeliveryDate { get; set; } = DateTime.UtcNow;

        // Поле для хранения идентификатора точки маршрута
        [Required(ErrorMessage = "Точка маршрута обязательна")]
        public int? RoutePointId { get; set; }

        // Навигационное свойство для точки маршрута
        public required RoutePoint RoutePoint { get; set; }

        // Навигационное свойство для продуктов в заказе
        public ICollection<OrderProduct> OrderProducts { get; set; }

        // Навигационное свойство для продуктов в схеме погрузки
        [NotMapped]
        [ValidateNever]
        public ICollection<LoadingProduct> LoadingProducts { get; set; }

        // Количество продуктов
        [Required(ErrorMessage = "Количество продуктов обязательно")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество продуктов должно быть положительным числом")]
        [Display(Name = "Количество продуктов")]
        public int ColProducts { get; set; }
    }
}
