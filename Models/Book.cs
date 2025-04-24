using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Book
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        public string Language { get; set; }
        [Required,MaxLength(13)]
        public string ISBN { get; set; }
        [Required,DataType(DataType.Date),Display(Name ="Date Published")]
        public DateTime DatePublished { get; set; }
        [Required,DataType(DataType.Currency)]
        public int Price { get; set; }
        [Display(Name ="Image URL")]
        public string ImageUrl { get; set; }

    }
}
