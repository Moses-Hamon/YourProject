using System.ComponentModel.DataAnnotations;

namespace YourProjectWebApp.Models
{
    public class Brand
    {
        public int BrandId { get; set; }

        // Annotations are used to apply restrictions which will help with good structure
        // and allow for input validation when creating the forms.
        [Required]
        [StringLength(50)]
        public string BrandName { get; set; }
    }
}