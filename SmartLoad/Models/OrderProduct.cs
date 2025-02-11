using System.ComponentModel.DataAnnotations;

//Продукты входящие в заказ

namespace SmartLoad.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ID заказа обязателен")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "ID продукта обязателен")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Количество обязательно")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество должно быть положительным числом")]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        // Навигационное свойство
        public Order Order { get; set; }

        // Навигационное свойство
        public Product Product { get; set; }
    }
}