using Microsoft.EntityFrameworkCore;

namespace Diana.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Single(int Id)
        {
            if (Id == null && Id < 0) return BadRequest();
            Product product = await _context.Products
                .Include(b => b.ProductImages)
                .Include(b => b.productMaterials)
                .ThenInclude(b => b.Material)
                .Include(b => b.ProductColors)
                .ThenInclude(b => b.Color)
                .Include(b => b.productSizes)
                .ThenInclude(b => b.Size)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
