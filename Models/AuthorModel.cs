using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AuthorModel
    {
        public Guid AuthorID { get; set; }
        public Name Name { get; set; }
        public string Biography { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DateTime getCreatedon()
        {
            return this.CreatedAt;
        }

        public DateTime getUpdatedon()
        {
            return this.UpdatedAt;
        }

        public AuthorModel()
        {

        }

        public AuthorModel ( DTOAuthor author )
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            AuthorID = Guid.NewGuid();
            this.Name = (Name)author.Name;
            this.Biography = author.Biography;
            this.BirthDate = author.BirthDate;
            this.Country = author.Country;
        }
    }

    public class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
