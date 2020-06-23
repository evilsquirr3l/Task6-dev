namespace Data.Entities
{
    public class BookCard : BaseEntity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }
    }
}
