using System.ComponentModel.DataAnnotations;

namespace NetBlog.Web.Models.ViewModels
{
    public class UserViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$", ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
        public string Password { get; set; }
        public bool AdminRoleCheckBox { get; set; }
        public Guid DeleteUserId { get; set; } = Guid.Empty;
    }
}
