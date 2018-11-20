using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YourProjectDataService.DAL;

namespace YourProjectDataService.Model
{
    public class Staff : CRUDMethods, IObjectIdentifier
    {
        public long Id { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

    }
}