﻿namespace BookShop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public int quantity { get; set; }
        public string CartId { get; set; }
    }
}
