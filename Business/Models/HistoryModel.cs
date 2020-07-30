using System;

namespace Business.Models
{
    public class HistoryModel
    {
        public int BookId { get; set; }
        public BookModel Book { get; set; }

        public int CardId { get; set; }
        public CardModel Card { get; set; }

        public DateTime TakeDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}