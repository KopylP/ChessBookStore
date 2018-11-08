using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class EditBookViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        [Required]
        public decimal Price { get; set; }
        public List<Author> Authors { get; set; }
        [Required]
        public int Count { get; set; }
        public List<Discont> Disconts { get; set; }
        public int? Year { get; set; }
        public List<Category> Categories { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int? DiscontId { get; set; }
        [Required]
        public string AuthorName { get; set; }
    }
}
