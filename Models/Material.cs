namespace Diana.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductMaterial>? ProductMaterials { get; set; }
    }
}
