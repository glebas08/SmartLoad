namespace SmartLoad.Models
{
    public class LoadingSchemeItem
    {
        public int Id { get; set; }
        public int LoadingSchemeId { get; set; }
        public int ProductId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public string Destination { get; set; }

        // Навигационные свойства
        public LoadingScheme? LoadingScheme { get; set; }
        public Product? Product { get; set; }
    }
}
