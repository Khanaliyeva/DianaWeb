using Diana.Areas.AdminPanel.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Diana.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize]
    public class ProductController : Controller
    {
        AppDbContext _context { get; set; }
        IWebHostEnvironment _environment { get; set; }

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> product = await _context.Products
                .Include(b => b.ProductImages)
                .Include(b => b.productMaterials)
                .ThenInclude(b => b.Material)
                .Include(b => b.ProductColors)
                .ThenInclude(b => b.Color)
                .Include(b => b.productSizes)
                .ThenInclude(b => b.Size)
                .ToListAsync();
            return View(product);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProductVM)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            bool resultCategory = await _context.Categories.AnyAsync(c => c.Id == createProductVM.CategoryId);
            if (!resultCategory)
            {
                ModelState.AddModelError("CategoryId", "bele model movcud deyil");
                return View();
            }
            Product product = new Product()
            {
                Name = createProductVM.Name,
                Price = createProductVM.Price,
                Description = createProductVM.Description,
                CategoryId = createProductVM.CategoryId,
                ProductImages = new List<ProductImages>(),
                productSizes=new List<ProductSize>(),
                productMaterials=new List<ProductMaterial>(),
                ProductColors=new List<ProductColor>()
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }










        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return View("Error");
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok();
        }


        public async Task<IActionResult> Update(int id)
        {

            Product product = await _context.Products
                .Include(b => b.ProductImages)
                .Include(b => b.productMaterials)
                .ThenInclude(b => b.Material)
                .Include(b => b.ProductColors)
                .ThenInclude(b => b.Color)
                .Include(b => b.productSizes)
                .ThenInclude(b => b.Size)
                .Where(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return View("Error");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            UpdateProductVM updateProductVM = new UpdateProductVM()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ProductImages=product.ProductImages,
                ProductColors=product.ProductColors,
                productMaterials=product.productMaterials

            };

          
            return View(updateProductVM);
        }



        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM updateProductVM)
        {

            ViewBag.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            Product existProduct = await _context.Products
                .Include(b => b.ProductImages)
                .Include(b => b.productMaterials)
                .ThenInclude(b => b.Material)
                .Include(b => b.ProductColors)
                .ThenInclude(b => b.Color)
                .Include(b => b.productSizes)
                .ThenInclude(b => b.Size)
                .FirstOrDefaultAsync();
            if (existProduct == null)
            {
                return View("Error");
            }
            bool resultCategory = await _context.Categories.AnyAsync(c => c.Id == updateProductVM.CategoryId);
            if (!resultCategory)
            {
                ModelState.AddModelError("CategoryId", "bele model movcud deyil");
                return View();
            }
            existProduct.Name = updateProductVM.Name;
            existProduct.Description = updateProductVM.Description;
            existProduct.Price = updateProductVM.Price;
            existProduct.CategoryId = updateProductVM.CategoryId;
            existProduct.productMaterials = updateProductVM.productMaterials;
            existProduct.ProductColors = updateProductVM.ProductColors;
            existProduct.ProductImages = updateProductVM.ProductImages;

            



            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
