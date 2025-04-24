using BookShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    [AllowAnonymous]

    public class StoreController : Controller
    {
        private readonly AppDbContext _context;
        public StoreController(AppDbContext context)
        {
            _context = context;
        }
        //[AllowAnonymous]
        public async Task<IActionResult> Index(string searchstring,string minPrice,string maxPrice)
        {
            var books = _context.Books.Select(b => b);
            if(!string.IsNullOrEmpty(searchstring))
            {
                books = books.Where(b => b.Title.Contains(searchstring) || b.Author.Contains(searchstring));

            }
            if(!string.IsNullOrEmpty(minPrice))
            {

                var min = int.Parse(minPrice);
                books = books.Where(b => b.Price >= min);
            }
            if (!string.IsNullOrEmpty(maxPrice))
            {

                var max = int.Parse(maxPrice);
                books = books.Where(b => b.Price <= max);
            }
            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    }
}
