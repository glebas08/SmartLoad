namespace SmartLoad.Models
{
    public class RoutePointMapping
    {
        // Первичный ключ
        public int Id { get; set; }

        // Внешний ключ к маршруту
        public int RouteId { get; set; }

        // Навигационное свойство для маршрута
        public Rout Route { get; set; }

        // Внешний ключ к точке разгрузки
        public int RoutePointId { get; set; }

        // Навигационное свойство для точки разгрузки
        public RoutePoint RoutePoint { get; set; }

        // Порядковый номер точки в маршруте
        public int OrderInRoute { get; set; }

        // Дополнительные поля (опционально)
        public DateTime? EstimatedArrivalTime { get; set; } // Ожидаемое время прибытия
        public string? Notes { get; set; } // Примечания к точке в контексте маршрута
    }
}
