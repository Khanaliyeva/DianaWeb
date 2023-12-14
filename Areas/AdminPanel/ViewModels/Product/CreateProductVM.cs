using System.ComponentModel.DataAnnotations.Schema;

namespace Diana.Areas.AdminPanel.ViewModels.Product
{
    public class CreateProductVM
    {
        public string Name { get; set; } 
        public string Description { get; set; }
        public double Price { get; set; }
        public bool? Availability { get; set; }
        public string? ImgUrl { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ProductImages>? ProductImages { get; set; }
        public List<ProductColor>? ProductColors { get; set; }
        public List<ProductMaterial>? productMaterials { get; set; }
        public List<ProductSize>? productSizes { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
