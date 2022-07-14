using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Model
{
    public class LoginModel
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
