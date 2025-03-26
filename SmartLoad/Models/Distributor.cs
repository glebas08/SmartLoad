using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmartLoad.Models
{
    public class Distributor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название дистрибьютора обязательно")]
        [StringLength(100, ErrorMessage = "Название дистрибьютора должно быть не длиннее 100 символов")]
        [Display(Name = "Название дистрибьютора")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Примечания не должны превышать 500 символов")]
        [Display(Name = "Примечания")]
        public string Notes { get; set; }

        // Навигационное свойство для заказов
        [NotMapped]
        [ValidateNever]
        public ICollection<Order> Orders { get; set; }
    }
}

