﻿using Models;
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
        bool AddAuthor(DTOAuthor author);
        string UpdateAuthor(UpdateAuthorModel author);
        bool DeleteAuthor(Guid id);
    }
}
