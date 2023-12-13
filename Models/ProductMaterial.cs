namespace Diana.Models
{
    public class ProductMaterial
    {
        public bool IsActive { get; set; }
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Material Material { get; set; }
        public int MaterialId { get; set; }

    }
}
