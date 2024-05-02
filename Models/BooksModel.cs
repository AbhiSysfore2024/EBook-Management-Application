using System.Net;

namespace Models
{
    public class BooksModel
    {
        public Guid BookID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ISBN { get; set; }
        public DateTime Publication_Date { get; set; }
        public float Price { get; set; }
        public string Language { get; set; }
        public string Publisher { get; set; }
        public int PageCount { get; set; }
        public float AvgRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
        public int BookGenre { get; set; }
        public bool IsAvailable { get; set; }
        public List<Guid> AuthorID {  get; set; }

        public DateTime getCreatedon()
        {
            return this.CreatedAt;
        }

        public BooksModel()
        {

        }

        public BooksModel(DTOBooks books)
        {
            IsAvailable = true;
            BookID = Guid.NewGuid();
            this.AuthorID = books.AuthorID;
            CreatedAt = DateTime.Now;
            this.Title = books.Title;
            this.Description = books.Description;
            this.ISBN = books.ISBN;
            this.Publication_Date = books.Publication_Date;
            this.Price = books.Price;
            this.Language = books.Language;
            this.Publisher = books.Publisher;
            this.PageCount = books.PageCount;
            this.AvgRating = books.AvgRating;
            this.BookGenre = books.BookGenre;
            UpdatedAt = DateTime.Now;
        }
    }
}