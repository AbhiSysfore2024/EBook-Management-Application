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

    }
}
