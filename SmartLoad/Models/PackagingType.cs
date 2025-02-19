using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLoad.Models
{
    public class PackagingType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название должно быть не длиннее 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Длина обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом")]
        [Display(Name = "Длина (м)")]
        public float Length { get; set; }

        [Required(ErrorMessage = "Ширина обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Ширина должна быть положительным числом")]
        [Display(Name = "Ширина (м)")]
        public float Width { get; set; }

        [Required(ErrorMessage = "Высота обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Высота должна быть положительным числом")]
        [Display(Name = "Высота (м)")]
        public float Height { get; set; }

        [Required(ErrorMessage = "Объем обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Объем должен быть положительным числом")]
        [Display(Name = "Объем (м³)")]
        public float Volume => Length * Width * Height;

        [Required(ErrorMessage = "Вес обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Вес должен быть положительным числом")]
        [Display(Name = "Вес (кг)")]
        public float Weight { get; set; }

        // Связь с продуктом
        [Required(ErrorMessage = "Продукт обязателен")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }

    public class PackagingTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
    }
}