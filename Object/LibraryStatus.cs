using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Object
{
    public enum LibraryStatusEnum
    {
        [Description("Kitap Kullanıcıda")]
        [Display(Name = "InUser")]
        Kullanıcı = 0,
        [Description("Kitap Kütüphanede")]
        [Display(Name = "InLibrary")]
        Kütüphane = 1
    }
}
