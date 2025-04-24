using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModel
{
    public class AddRoleViewModel
    {
        [Required]
        [Display(Name ="Role ")]
        public string RoleName { get; set; }

    }
}
