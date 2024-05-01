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
        bool UpdateBook (DTOBooks books);
    
        bool DeleteBook(Guid id);
        List<BooksModel> GetBooksByTitle(string title);
        List<BooksModel> GetBooksByGenre(int genre_id);
    }
}
