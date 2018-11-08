using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class AddAuthorViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string NameEn { get; set; }
    }
}
