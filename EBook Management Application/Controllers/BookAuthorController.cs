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
        private readonly IDatabaseManager _databasemanager;

        public BookAuthorController()
        {
            _databasemanager = new BooksStoredProcedure();
        }

        [HttpGet]
        [Route("GetAllBooks")]
        public ActionResult GetAllBoooks(){
            return Ok(_databasemanager.GetAllBooks());
        }

        [HttpPost]
        [Route("/AddBook")]
        public ActionResult AddBook([FromBody] DTOBooks book) {
            string books = _databasemanager.AddBook(book);
            return Ok(books);
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
    }
}
