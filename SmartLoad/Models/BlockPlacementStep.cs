namespace SmartLoad.Models
{
    public class BlockPlacementStep
    {
        public int Id { get; set; }
        public int SchemeId { get; set; } // ID схемы загрузки
        public int StepNumber { get; set; } // Номер шага

        public int ProductId { get; set; } // ID продукта
        public string ProductName { get; set; }

        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double PositionZ { get; set; }

        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public double Weight { get; set; }
        public string Destination { get; set; }

        public string ScreenshotBase64 { get; set; } // Base64 изображение
    }
}
