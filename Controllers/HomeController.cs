using Diana.DAL;
using Diana.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.categories = _context.Categories.ToList();
            homeVM.Products =_context.Products
                .Include(b=>b.ProductImages)
                .Include(b=>b.productMaterials)
                .ThenInclude(b => b.Material)
                .Include(b=>b.ProductColors)
                .ThenInclude(b=>b.Color)
                .Include(b=>b.productSizes)
                .ThenInclude(b=>b.Size)
                .ToList();
         
             
            return View(homeVM);
        }
    }
}
