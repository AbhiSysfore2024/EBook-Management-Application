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
        string AddBook (DTOBooks books);
        //bool UpdateBook (BooksModel books);
        List<BooksModel> GetAllBooks();
        bool DeleteBook(Guid id);
        List<BooksModel> GetBooksByTitle(string title);
    }
}
