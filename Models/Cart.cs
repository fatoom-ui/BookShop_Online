using BookShop.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Models
{
    public class Cart
    {
        private readonly AppDbContext _context;
        public Cart(AppDbContext context)
        {
            _context = context;
            CartItems = new List<CartItem>();
        }
        public string Id { get; set; }
        public List<CartItem> CartItems { get; set; }
        public static Cart GetCart(IServiceProvider services)
        {
            var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            ISession session = httpContextAccessor.HttpContext?.Session
                ?? throw new InvalidOperationException("Session is not available.");

            //ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            //var context = services.GetService<AppDbContext>();
            var context = services.GetRequiredService<AppDbContext>();

            string CartId = session.GetString(key: "Id") ?? Guid.NewGuid().ToString();
            session.SetString(key: "Id", CartId);
            return new Cart(context) { Id = CartId };
        }
        public CartItem Getcartitem(Book book)
        {
            var user= _context.CartItems.SingleOrDefault(ci => ci.Book.Id == book.Id && ci.CartId == Id);
            return user;

        }
        public void Addtocart(Book book,int quantity)
        {
            var cartitem = Getcartitem(book);
            if(cartitem==null)
            {
                cartitem = new CartItem
                {
                    Book = book,
                    quantity = quantity,
                    CartId = Id
                };
                _context.CartItems.Add(cartitem);
            }
            else
            {
                cartitem.quantity += quantity;

            }
            _context.SaveChanges();


        }
        public int Reducequantity(Book book)
        {
            var cartitem = Getcartitem(book);
            var remainingquantity = 0;
            if(cartitem!=null)
            {
                if(cartitem.quantity> 1)
                {
                    remainingquantity = --cartitem.quantity;

                }
                else
                {
                    _context.CartItems.Remove(cartitem);
                }
            }
            _context.SaveChanges();
            return remainingquantity;
        }
        public int Increasequantity(Book book)
        {
            var cartitem = Getcartitem(book);
            var remainingquantity = 0;
            if (cartitem != null)
            {
                if (cartitem.quantity > 0)  
                {
                    remainingquantity = ++cartitem.quantity;

                }
            }
            _context.SaveChanges();
            return remainingquantity;
        }
        public void Removefromcart(Book book)
        {
            var cartitem = Getcartitem(book);
            if(cartitem!=null)
            {
                _context.CartItems.Remove(cartitem);
            }
            _context.SaveChanges();

        }
        public void Clearcart()
        {
            var cartitem = _context.CartItems.Where(ci => ci.CartId == Id);
            _context.CartItems.RemoveRange(cartitem);
            _context.SaveChanges();
        }

        public List<CartItem> Getallcartitems()
        {
            if (CartItems == null)
            {
                CartItems = _context.CartItems
                    .Where(ci => ci.CartId == Id)
                    .Include(ci => ci.Book)
                    .ToList();
            }

            return CartItems;
        }

        //public List<CartItem> Getallcartitems()
        //{
        //    var carts = CartItems = CartItems ?? (_context.CartItems
        //        .Where(ci => ci.CartId == Id)
        //        .Include(ci => ci.Book)
        //        .ToList());
        //    return (carts);
        //}


        //return CartItems??
        //    (CartItems = _context.CartItems.Where(ci => ci.CartId == Id).Include(ci=>ci.Book).ToList());

        public int GetCarttotal()
        {
            return _context.CartItems.Where(ci => ci.CartId == Id)
                .Select(ci=>ci.Book.Price*ci.quantity)
                .Sum();
        }
    }
}
