using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public int Year { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<BookCard> Cards { get; set; }

        public Book()
        {
            Cards = new List<BookCard>();
        }
    }
}
