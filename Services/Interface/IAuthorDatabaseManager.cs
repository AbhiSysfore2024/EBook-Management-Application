using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAuthorDatabaseManager
    {
        List<AuthorModel> GetAllAuthors();
        string AddAuthor(DTOAuthor author);
        string UpdateAuthor(AuthorModel author);
        bool DeleteAuthor(Guid id);
    }
}
