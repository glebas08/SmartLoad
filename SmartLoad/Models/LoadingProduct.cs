using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLoad.Models
{
    public class LoadingProduct
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ID схемы загрузки обязателен")]
        public int LoadingSchemeId { get; set; }

        [Required(ErrorMessage = "ID продукта обязателен")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ID заказа обязателен")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "ID точки маршрута обязателен")]
        public int RoutePointId { get; set; }

        [Required(ErrorMessage = "Количество обязательно")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество должно быть положительным числом")]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Позиция X обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Позиция X должна быть положительным числом")]
        [Display(Name = "Позиция X (м)")]
        public float PositionX { get; set; }

        [Required(ErrorMessage = "Позиция Y обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Позиция Y должна быть положительным числом")]
        [Display(Name = "Позиция Y (м)")]
        public float PositionY { get; set; }

        [Required(ErrorMessage = "Позиция Z обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Позиция Z должна быть положительным числом")]
        [Display(Name = "Позиция Z (м)")]
        public float PositionZ { get; set; }

        // Навигационное свойство
        public LoadingScheme LoadingScheme { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
        public RoutePoint RoutePoint { get; set; }

        // Поле для хранения ID типа упаковки
        [Required(ErrorMessage = "Тип упаковки обязателен")]
        public int PackagingTypeId { get; set; }

        // Навигационное свойство для типа упаковки
        [ForeignKey("PackagingTypeId")]
        public PackagingType PackagingType { get; set; }
    }

    public class LoadingProductDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public ProductDto Name { get; set; }
        public PackagingTypeDto PackagingType { get; set; }
    }
}