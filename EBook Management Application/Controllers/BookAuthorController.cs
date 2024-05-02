using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Services.Interface;

namespace EBook_Management_Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookAuthorController : ControllerBase
    {
       // private readonly IConfiguration _configuration;
        private readonly IDatabaseManager _databasemanager;
        private readonly IAuthorDatabaseManager _authordatabasemanager;

        public BookAuthorController(IDatabaseManager databaseManager, IAuthorDatabaseManager authorDatabaseManager)
        {
            _databasemanager = databaseManager;
            _authordatabasemanager = authorDatabaseManager;
        }

        [HttpGet]
        [Route("GetAllBooks")]
        public ActionResult GetAllBoooks(){
            return Ok(_databasemanager.GetAllBooks());
        }

        [HttpGet]
        [Route("GetAllAuthors")]
        public ActionResult GetAllAuthors()
        {
            return Ok(_authordatabasemanager.GetAllAuthors());
        }

        [HttpPost]
        [Route("/AddBook")]
        public ActionResult AddBook([FromBody] DTOBooks book) {
            string books = _databasemanager.AddBook(book);
            return Ok(books);
        }

        [HttpPost]
        [Route("/AddAuthor")]
        public ActionResult AddAuthor([FromBody] DTOAuthor author)
        {
            string authors = _authordatabasemanager.AddAuthor(author);
            return Ok(authors);
        }

        [HttpPut]
        [Route("/UpdateBook/{id}")]
        public ActionResult UpdateBook([FromBody] BooksModel book)
        {
            return Ok(_databasemanager.UpdateBook(book));
        }

        [HttpPut]
        [Route("/UpdateAuthor/{id}")]
        public ActionResult UpdateAuthor([FromBody] AuthorModel author)
        {
            return Ok(_authordatabasemanager.UpdateAuthor(author));
        }

        [HttpDelete]
        [Route("/DeleteBook/{id}")]
        public ActionResult DeleteBook(Guid id)
        {
            bool result = _databasemanager.DeleteBook(id);
            if (result)
            {
                return Accepted("Book deleted successfully");

            }
            return NotFound("Book not found");
        }

        [HttpDelete]
        [Route("/DeleteAuthor/{id}")]
        public ActionResult DeleteAuthor(Guid id)
        {
            bool result = _authordatabasemanager.DeleteAuthor(id);
            if (result)
            {
                return Accepted("Author deleted successfully");

            }
            return NotFound("Author not found");
        }

        [HttpGet]
        [Route("/GetBookByTitle")]
        public ActionResult GetBookByTitle(string title)
        {
            var books = _databasemanager.GetBooksByTitle(title);
            if (books.Count == 0)
            {
                return NotFound("No book with given title");
            }
            return Ok(books);
        }

        [HttpGet]
        [Route("/GetBooksByGenre")]
        public ActionResult GetBooksByGenre(int genre_id)
        {
            var books = _databasemanager.GetBooksByGenre(genre_id);
            if (books.Count == 0)
            {
                return NotFound("No book with given genre found");
            }
            return Ok(books);   
        }

        [HttpGet("GetBooksByAuthorName")]
        public ActionResult GetBooksByAuthorName(string authorName)
        {
            try
            {
                var books = _databasemanager.GetBooksByAuthorName(authorName);
                if (books.Count == 0)
                {
                    return NotFound("No books found for the author.");
                }
                return Ok(books);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpGet("GroupBooksOnGenreName")]
        public ActionResult GroupBooksOnGenreName()
        {
            try
            {
                var booksOnGenre = _databasemanager.GroupBooksOnGenreName();
                if (booksOnGenre.Count == 0)
                {
                    return NotFound("No books found for the author.");
                }
                return Ok(booksOnGenre);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}
