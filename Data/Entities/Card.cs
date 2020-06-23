using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Card : BaseEntity
    {
        public DateTime Created { get; set; } = DateTime.Now;

        public int ReaderId { get; set; }
        public Reader Reader { get; set; }

        public ICollection<BookCard> Books { get; set; } = new List<BookCard>();
    }
}
