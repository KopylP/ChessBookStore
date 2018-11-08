using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public Author() => Books = new List<Book>();
    }
}
