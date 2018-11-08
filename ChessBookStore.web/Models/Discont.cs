using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class Discont
    {
        
        public uint Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int Percentage { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Discont() => Books = new List<Book>();
    }
}
