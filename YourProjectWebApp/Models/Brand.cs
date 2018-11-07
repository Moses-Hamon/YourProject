using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YourProjectWebService.Models
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