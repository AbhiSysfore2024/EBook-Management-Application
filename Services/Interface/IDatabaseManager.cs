using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IDatabaseManager
    {
        List<BooksModel> GetAllBooks();
        string AddBook (DTOBooks books);
        bool UpdateBook (BooksModel books);
    
        bool DeleteBook(Guid id);
        List<BooksModel> GetBooksByTitle(string title);
        List<BooksModel> GetBooksByGenre(int genre_id);
        //List<(Guid AuthorID, Guid BookID, string FirstName, string Title)> GetBooksByAuthorName(string authorName);
        //List<object> GetBooksByAuthorName(string authorName);
        Dictionary<string, List<object>> GetBooksByAuthorName(string authorName);
        Dictionary<string, List <object>> GroupBooksOnGenreName();
    }
}
