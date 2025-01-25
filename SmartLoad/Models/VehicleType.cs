namespace SmartLoad.Models
{
    //Клас для таблицы Тип транспортного средства
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        //Длина ТС в метрах
        public float Length { get; set; }

        //Ширина ТС 
        public float Width { get; set; }

        //Высота ТС
        public float Height { get; set; }

        //Максимальная грузоподъёмность в кг
        public float MaxLoadCapacity { get; set; }

        //Максимальный объём 
        public float MaxVolumeCapasity { get; set; }

        //Количество осей 
        public int AxleCount { get; set; }

        //Максимально допустимая нагрузка на одну ось
        public float MaxAxleLoad { get; set; }

        //Собственный вес ТС
        public float EmptyWeight { get; set; }

        //База колёс (растояние между передней и задней осями, в метрах)
        public float WheelBase { get; set; }

        //Передний свес 
        public float FrontOverhang { get; set; }

        //Задний свес 
        public float RearOverhang { get; set; }

        //Тип дорог, по которым может передвигаться ТС (например, городские, междугородние, горные)
        public string AllowedRoadTypes { get; set; }

        //Дополнительные примечасния или особенности
        public string Notes { get; set; }

        //Вид ТС
        public string ViewType { get; set; }

        //Высота седельно-сцепного устройства (важна для полуприцепа)
        public float CouplingDevice { get; set; }

        // Расстояние от переднего свеса до шкворня. Это расстояние влияет на маневренность полуприцепа.
        public float Kingpindist { get; set; }

        // Задний свес за седельно-сцепным устройством, важен для расчета распределения нагрузки на оси.
        public float OverBeyond { get; set; }

    }
}
