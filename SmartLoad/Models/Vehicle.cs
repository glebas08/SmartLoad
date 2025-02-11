using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

//будет представлять конкретные транспортные средства, которые будут использоваться для доставки заказов.

namespace SmartLoad.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Тип транспортного средства обязателен")]
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "Максимальная грузоподъемность тягача обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальная грузоподъемность тягача должна быть положительным числом")]
        [Display(Name = "Макс. грузоподъемность тягача (кг)")]
        public float MaxLoadCapacityTractor { get; set; }

        [Required(ErrorMessage = "Максимальный объем тягача обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальный объем тягача должен быть положительным числом")]
        [Display(Name = "Макс. объем тягача (м³)")]
        public float MaxVolumeCapacityTractor { get; set; }

        [Required(ErrorMessage = "Максимальная грузоподъемность полуприцепа обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальная грузоподъемность полуприцепа должна быть положительным числом")]
        [Display(Name = "Макс. грузоподъемность полуприцепа (кг)")]
        public float MaxLoadCapacityTrailer { get; set; }

        [Required(ErrorMessage = "Максимальный объем полуприцепа обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальный объем полуприцепа должен быть положительным числом")]
        [Display(Name = "Макс. объем полуприцепа (м³)")]
        public float MaxVolumeCapacityTrailer { get; set; }

        // Навигационное свойство для схем погрузки
        public ICollection<LoadingScheme> LoadingSchemes { get; set; }
    }
}