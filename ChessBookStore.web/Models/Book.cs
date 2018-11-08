using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int Count { get; set; }
        //public Discont Discont { get; set; }
        //public int? DiscontId { get; set; }
        public string ImagePath { get; set; }
        public int? Year { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
