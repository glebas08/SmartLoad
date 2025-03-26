using SmartLoad.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLoad.Models
{
    public class LoadingSchemeCreateViewModel
    {
        [Required(ErrorMessage = "Выберите транспортное средство")]
        [Display(Name = "Транспортное средство")]
        public int SelectedVehicleId { get; set; }

        [Required(ErrorMessage = "Выберите маршрут")]
        [Display(Name = "Маршрут")]
        public int SelectedRouteId { get; set; }

        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<Rout> Routes { get; set; } = new List<Rout>();
    }
}
