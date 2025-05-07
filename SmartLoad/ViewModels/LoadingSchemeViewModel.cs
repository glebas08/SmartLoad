using SmartLoad.Models;
using System.Collections.Generic;

namespace SmartLoad.ViewModels
{
    public class LoadingSchemeViewModel
    {
        public LoadingScheme LoadingScheme { get; set; }
        public string ViewType { get; set; } = "3D";
        public List<DestinationGroupViewModel> DestinationGroups { get; set; }
        // Добавляем свойство для хранения нагрузок на оси
        public Dictionary<string, float> AxleLoads { get; set; } = new Dictionary<string, float>();
        // Добавляем свойство для сообщения об ошибке
        public string ErrorMessage { get; set; }
    }

    public class DestinationGroupViewModel
    {
        public string Destination { get; set; }
        public int ColorIndex { get; set; }
        public List<LoadingProductViewModel> Items { get; set; }
    }

    public class LoadingProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float DisplayX { get; set; }
        public float DisplayY { get; set; }
        public float DisplayZ { get; set; }
        public string Destination { get; set; }
        public Product Product { get; set; }

    }

    public class LoadingSchemeDetailsViewModel
    {
        public LoadingScheme LoadingScheme { get; set; }
        public List<CargoPlacement> CargoPlacement { get; set; }
        public string ErrorMessage { get; set; }
    }
}
