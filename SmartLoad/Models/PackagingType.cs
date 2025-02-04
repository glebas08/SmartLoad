using System.ComponentModel.DataAnnotations;

namespace SmartLoad.Models
{
    // Класс для таблицы тип упаковки 
    public class PackagingType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название должно быть не длиннее 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Вес обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Вес должен быть положительным числом")]
        [Display(Name = "Вес (кг)")]
        public float Weight { get; set; } // Масса упаковки (кг)

        [Required(ErrorMessage = "Длина обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом")]
        [Display(Name = "Длина (м)")]
        public float Length { get; set; } // Длина упаковки (м)
        
        [Required(ErrorMessage = "Ширина обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Ширина должна быть положительным числом")]
        [Display(Name = "Ширина (м)")]
        public float Width { get; set; } // Ширина упаковки (м)

        [Required(ErrorMessage = "Высота обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Высота должна быть положительным числом")]
        [Display(Name = "Высота (м)")]
        public float Height { get; set; } // Высота упаковки (м)
        
        [Required(ErrorMessage = "Объем обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Объем должен быть положительным числом")]
        [Display(Name = "Объем (м³)")]
        public float Volume { get; set; } // Объем упаковки (м³)
    }
}