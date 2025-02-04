using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLoad.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название должно быть не длиннее 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Тип упаковки обязателен")]
        [Display(Name="Тип упаковки")]
        public int PackagingTypeId { get; set; }

        [ForeignKey("PackagingTypeId")]
        public PackagingType PackagingType { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Высота должна быть положительным числом")]
        [Display(Name="Высота (м)")]
        public float Height { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом")]
        [Display(Name = "Длина (м)")]
        public float Length { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Ширина должна быть положительным числом")]
        [Display(Name = "Ширина (м)")]
        public float Width { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Объем должен быть положительным числом")]
        [Display(Name = " Объём (м³)")]
        public float Volume { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Вес должен быть положительным числом")]
        [Display(Name = "Масса (кг)")]
        public float Weight { get; set; }
    }
}
