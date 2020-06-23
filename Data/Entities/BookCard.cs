using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class BookCard : BaseEntity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }
    }
}
