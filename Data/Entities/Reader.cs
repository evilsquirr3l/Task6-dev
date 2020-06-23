namespace Data.Entities
{
    public class Reader : BaseEntity
    {
        public string Name { get; set; }

        public Card Card { get; set; }
    }
}
