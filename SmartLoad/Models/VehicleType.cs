using System.ComponentModel.DataAnnotations;

namespace SmartLoad.Models
{
    //Класс для таблицы Тип транспортного средства
    public class VehicleType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Name { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом")]
        public float Length { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Ширина должна быть положительным числом")]
        public float Width { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Высота должна быть положительным числом")]
        public float Height { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Максимальная грузоподъемность должна быть положительным числом")]
        public float MaxLoadCapacity { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Максимальный объем должен быть положительным числом")]
        public float MaxVolumeCapasity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество осей должно быть положительным числом")]
        public int AxleCount { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Максимальная нагрузка на ось должна быть положительным числом")]
        public float MaxAxleLoad { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Собственный вес должен быть положительным числом")]
        public float EmptyWeight { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Колесная база должна быть положительным числом")]
        public float WheelBase { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Передний свес должен быть положительным числом")]
        public float FrontOverhang { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Задний свес должен быть положительным числом")]
        public float RearOverhang { get; set; }

        [Required(ErrorMessage = "Типы дорог обязательны")]
        [StringLength(255, ErrorMessage = "Типы дорог не должны превышать 255 символов")]
        public string AllowedRoadTypes { get; set; }

        [StringLength(500, ErrorMessage = "Примечания не должны превышать 500 символов")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Вид ТС обязателен")]
        [StringLength(100, ErrorMessage = "Вид ТС не должен превышать 100 символов")]
        public string ViewType { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Высота сцепного устройства должна быть положительным числом")]
        public float CouplingDevice { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Расстояние до шкворня должно быть положительным числом")]
        public float Kingpindist { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Задний свес за сцепным устройством должен быть положительным числом")]
        public float OverBeyond { get; set; }
    }
}