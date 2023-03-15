using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Identity
{
    public enum RoleType : short
    {
        [Description("Admin")]
        [Display(Name = "ADMİN")]
        Admin = 0,
        [Description("Librarian")]
        [Display(Name = "lIBRARIAN")]
        Lb = 1,
        [Description("Operator")]
        [Display(Name = "OPERATOR")]
        Operator = 2
    }
}
