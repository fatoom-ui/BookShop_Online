using BookShop.Data;
using BookShop.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Cart _cart;
        public CartController(AppDbContext context, Cart cart)
        {
            _context = context;
            _cart = cart;

        }
        public IActionResult Index()
        {
            var items = _cart.Getallcartitems();
            _cart.CartItems = items;
            return View(_cart);
        }
        public IActionResult Addtocart(int id)
        {
            var selecttobook = Getbookbyid(id);
            if (selecttobook != null)
            {
                _cart.Addtocart(selecttobook, quantity: 1);
            }
            return RedirectToAction("Index","Store");

        } 
        public IActionResult Removefromcart(int id)
        {
            var selecttobook= Getbookbyid(id);
            if (selecttobook != null)
            {
                _cart.Removefromcart(selecttobook);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Reducequantity(int id)
        {
            var selecttobook = Getbookbyid(id);
            if (selecttobook != null)
            {
                _cart.Reducequantity(selecttobook);
            }
            return RedirectToAction("Index");

        }
        public IActionResult Clearcart(int id)
        {
            _cart.Clearcart();
            return RedirectToAction("Index");
        }
        public IActionResult Increasequantity(int id)
        {
            var selecttobook = Getbookbyid(id);
            if (selecttobook != null)
            {
                _cart.Increasequantity(selecttobook);
            }
            return RedirectToAction("Index");

        }
        public Book Getbookbyid(int id)
        {
            var user = _context.Books.FirstOrDefault(b => b.Id == id);
            return (user);
        }
    }
}
