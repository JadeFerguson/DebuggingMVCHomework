using CPW219_AspnetMVC_CRUD_Debugging.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPW219_AspnetMVC_CRUD_Debugging.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // refactored this
            List<Product> products = await (from product in _context.Product
                                            select product).ToListAsync();

            return View(products);
        }

        [HttpGet] // added in
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Product.Add(product); // Prepares insert
                await _context.SaveChangesAsync(); // Executes pending insert

                ViewData["Message"] = $"{product.Name} was added successfully";

                return View();
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Product? productToEdit = await _context.Product.FindAsync(id);

            if (productToEdit == null)
            {
                return NotFound();
            }

            return View(productToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{product.Name} was updated successfully";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product? productToDelete = await _context.Product.FindAsync(id);

            if (productToDelete == null)
            {
                return NotFound();
            }

            return View(productToDelete);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product? productToDelete = await _context.Product.FindAsync(id);

            if (productToDelete != null)
            {
                _context.Product.Remove(productToDelete);
                await _context.SaveChangesAsync();

                TempData["Message"] = productToDelete.Name + " was deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "This game was already deleted";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            Product? productDetails = await _context.Product.FindAsync(id);

            if (productDetails == null)
            {
                return NotFound();
            }

            return View(productDetails);

        }
    }
}
