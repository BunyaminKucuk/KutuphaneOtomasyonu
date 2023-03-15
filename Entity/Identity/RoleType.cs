using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entity.Identity
{
    public enum RoleType : short
    {
        [Description("Admin")]
        [Display(Name = "ADMIN")]
        Admin = 0,
        [Description("Librarian")]
        [Display(Name = "lIBRARIAN")]
        Librarian = 1,
        [Description("Customer")]
        [Display(Name = "CUSTOMER")]
        Customer = 2
    }
}
