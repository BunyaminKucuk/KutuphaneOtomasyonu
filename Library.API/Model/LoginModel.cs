using System.ComponentModel.DataAnnotations;

namespace Library.API.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Kullanıcı adı giriniz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Şifre giriniz")]
        public string Password { get; set; }
    }
}
