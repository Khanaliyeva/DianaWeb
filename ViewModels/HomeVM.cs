using Diana.Models;
using System.Reflection.Metadata;

namespace Diana.ViewModels
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Category> categories { get; set; }
    }
}
