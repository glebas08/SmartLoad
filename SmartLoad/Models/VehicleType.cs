using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLoad.Models
{
    public class VehicleType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом")]
        [Display(Name = "Длина")]
        public float Length { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Ширина должна быть положительным числом")]
        [Display(Name = "Ширина")]
        public float Width { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Высота должна быть положительным числом")]
        [Display(Name="Высота")]
        public float Height { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Максимальная грузоподъемность должна быть положительным числом")]
        [Display(Name="Максимальная грузоподъёмность")]
        public float MaxLoadCapacity { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Максимальный объем должен быть положительным числом")]
        [Display(Name="Максимальный объём")]
        public float MaxVolumeCapacity { get; set; } //

        [Range(0, int.MaxValue, ErrorMessage = "Количество осей должно быть положительным числом")]
        [Display(Name="Колличество осей")]
        public int AxleCount { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Максимальная нагрузка на ось должна быть положительным числом")]
        [Display(Name=" Максимальная нагрузка на ось")]
        public float MaxAxleLoad { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Собственный вес должен быть положительным числом")]
        [Display(Name ="Собственнный вес")]
        public float EmptyWeight { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Колесная база должна быть положительным числом")]
        [Display(Name="Колёсная база")]
        public float WheelBase { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Передний свес должен быть положительным числом")]
        [Display(Name="Передний свес")]
        public float FrontOverhang { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Задний свес должен быть положительным числом")]
        [Display(Name="Звдний свес")]
        public float RearOverhang { get; set; }

        [Required(ErrorMessage = "Типы дорог обязательны")]
        [StringLength(255, ErrorMessage = "Типы дорог не должны превышать 255 символов")]
        [Display(Name=" Тип дороги")]
        public string AllowedRoadTypes { get; set; }

        [StringLength(500, ErrorMessage = "Примечания не должны превышать 500 символов")]
        [Display(Name="Примечание")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Вид ТС обязателен")]
        [StringLength(100, ErrorMessage = "Вид ТС не должен превышать 100 символов")]
        [Display(Name="Вид ТС")]
        public string ViewType { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Высота сцепного устройства должна быть положительным числом")]
        [Display(Name="Высота сцепного устройства")]
        public float CouplingDevice { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Расстояние до шкворня должно быть положительным числом")]
        [Display(Name="Растояние ло шкворня")]
        public float Kingpindist { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Задний свес за сцепным устройством должен быть положительным числом")]
        [Display(Name="Задний свес")]
        public float OverBeyond { get; set; }

        // Навигационное свойство для транспортных средств
        public ICollection<Vehicle> Vehicles { get; set; }

        // Навигационное свойство для схем погрузки
        public ICollection<LoadingScheme> LoadingSchemes { get; set; }
    }
}