using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLoad.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название должно быть не длиннее 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        // Навигационное свойство для связанных типов упаковок
        public ICollection<PackagingType> PackagingTypes { get; set; } = new List<PackagingType>();

        // Навигационное свойство для связанных продуктов в схеме погрузки
        public ICollection<LoadingProduct> LoadingProducts { get; set; } = new List<LoadingProduct>();
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}


//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace SmartLoad.Models
//{
//    public class Product
//    {
//        public int Id { get; set; }

//        [Required(ErrorMessage = "Название обязательно")]
//        [StringLength(100, ErrorMessage = "Название должно быть не длиннее 100 символов")]
//        [Display(Name = "Название")]
//        public string Name { get; set; }

//        // Навигационное свойство для связанных типов упаковок
//        public ICollection<PackagingType> PackagingTypes { get; set; }

//        // Навигационное свойство для связанных продуктов в схеме погрузки
//        public ICollection<LoadingProduct> LoadingProducts { get; set; }
//    }
//}