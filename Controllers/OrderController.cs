using BookShop.Data;
using BookShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Cart _cart;
        public OrderController(AppDbContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var cartitems = _cart.Getallcartitems();
            _cart.CartItems = cartitems;
            //if(_cart.CartItems.Count==0)
            //{
             //ModelState.AddModelError("", "cart is empty,please add abook first");
            //}
            if(ModelState.IsValid)
            {
                Createorder(order);
                _cart.Clearcart();
                return View("CheckoutComplete",order);
            }
            return View(order);

        }
        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }
        public void Createorder(Order order)
        {
            order.Orderplaced = DateTime.Now;

            // Only get items from the current user's cart and include Book data
            var cartitems = _context.CartItems
                .Where(ci => ci.CartId == _cart.Id)
                .Include(ci => ci.Book)
                .ToList();

            foreach (var item in cartitems)
            {
                if (item.Book == null) continue; //Safety check

                var orderitem = new OrderItem()
                {
                    quantity = item.quantity,
                    BookId = item.Book.Id,
                    OrderId = order.Id,
                    Price = item.Book.Price * item.quantity
                };
                order.OrderItems.Add(orderitem);
                order.Ordertotal += orderitem.Price;
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        //public void Createorder(Order order)
        //{
        //    order.Orderplaced = DateTime.Now;
        //    var cartitems = _context.CartItems;
        //    foreach(var item in cartitems)
        //    {
        //        var orderitem = new OrderItem()
        //        {
        //            quantity=item.quantity,
        //            BookId=item.Book.Id,
        //            OrderId=order.Id,
        //            Price=item.Book.Price*item.quantity
        //        };
        //        order.OrderItems.Add(orderitem);
        //        order.Ordertotal += orderitem.Price;

        //    }
        //    _context.Orders.Add(order);
        //    _context.SaveChanges();


        //}
    }
}
