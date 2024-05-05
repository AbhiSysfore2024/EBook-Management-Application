using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IDatabaseManager
    {
        List<BooksModel> GetAllBooks();
        string AddBook (DTOBooks books);
        string UpdateBook (UpdateBookModel books);
        bool DeleteBook(Guid id);
        
        List<BooksModel> GetBooksByTitle(string title);
        List<BooksModel> GetBooksByGenre(int genre_id);
        Dictionary<string, List<object>> GetBooksByAuthorName(string authorName);
        Dictionary<string, List <object>> GroupBooksOnGenreName();
        Dictionary<string, List<object>> GetAuthorsOfABook(string title);
    }
}
