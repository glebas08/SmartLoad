using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLoad.Models
{
    public class LoadingScheme
    {
        public int Id { get; set; }

        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int RouteId { get; set; }
        public Rout Route { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LoadingDate { get; set; }

        public string Status { get; set; }

        public string Notes { get; set; }

        public List<LoadingProduct> LoadingProducts { get; set; } = new List<LoadingProduct>();
    }
}
