using System;
using System.Collections.Generic;

namespace SmartLoad.Models
{
    public class LoadingScheme
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int RoutId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        // Навигационные свойства
        public Vehicle? Vehicle { get; set; }
        public Rout? Rout { get; set; }
        public List<LoadingSchemeItem> LoadingSchemeItems { get; set; } = new List<LoadingSchemeItem>();
       // public virtual ICollection<LoadingProduct> LoadingProducts { get; set; } = new List<LoadingProduct>();
        public DateTime LoadingDate { get; set; }
    }
}
