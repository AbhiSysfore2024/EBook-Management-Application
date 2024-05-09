using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Models;
using Services;
using Services.Interface;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace EBook_Management_Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookAuthorController : ControllerBase
    {
        private readonly ILoginRequest _loginRequest;
        private readonly IDatabaseManager _databasemanager;

        public BookAuthorController(ILoginRequest loginRequest, IDatabaseManager databaseManager)
        {
            _loginRequest = loginRequest;
            _databasemanager = databaseManager;
        }

        [HttpPost]
        [Route("/Signup")]
        public ActionResult Signup(LoginRequest loginRequest)
        {
            try
            {
                var signup = _loginRequest.Signup(loginRequest);
                if(signup == "Username or password cannot be empty or whitespace")
                {
                    return BadRequest(signup);
                }
                else if (signup == "Role must be either 'Admin' or 'User'")
                {
                    return BadRequest(signup);
                }
                return Ok("Success");
            }
            catch
            {
                return BadRequest("An error occurred while trying to signup");
            }
        }

        [HttpPost]
        [Route("/Authentication")]
        public IActionResult GenerateJwtToken(DTOLoginRequest loginDTO)
        {
            try
            {
                var role = _loginRequest.RoleAssigned(loginDTO);
                var jwtToken = _loginRequest.GenerateJwtToken(loginDTO, role);
                if (jwtToken == "Invalid username or password")
                {
                    return BadRequest(jwtToken);
                }
                return Ok(jwtToken);
            }
            catch
            {
                return BadRequest("An error occurred in generating the token");
            }
        }


        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("/GetAllBooks")]
        public ActionResult GetAllBoooks(){
            return Ok(_databasemanager.GetAllBooks());
        }


        [Authorize(Roles = "Admin")]
        [Authorize]
        [HttpPost]
        [Route("/AddBook")]
        public ActionResult AddBook([FromBody] DTOBooks book) {
            string books = _databasemanager.AddBook(book);
            return Ok(books);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("/UpdateBook/{id}")]
        public ActionResult UpdateBook([FromBody] UpdateBookModel book)
        {
            return Ok(_databasemanager.UpdateBook(book));
        }


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin, User")]
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

        [Authorize(Roles = "Admin, User")]
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

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GetBooksByAuthorName")]
        public ActionResult GetBooksByAuthorName(string authorName)
        {
            var books = _databasemanager.GetBooksByAuthorName(authorName);
            if (books.Count == 0)
            {
                return NotFound("No books found for the author.");
            }
            return Ok(books);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GroupBooksOnGenreName")]
        public ActionResult GroupBooksOnGenreName()
        {
            var booksOnGenre = _databasemanager.GroupBooksOnGenreName();
            if (booksOnGenre.Count == 0)
            {
                return NotFound("No books found for the author.");
            }
            return Ok(booksOnGenre);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("/GetAuthorsOfABook")]
        public ActionResult GetAuthorsOfABook(string title) 
        {
            var authorsOfBook = _databasemanager.GetAuthorsOfABook(title);
            if (authorsOfBook.Count == 0)
            {
                return NotFound("Book not found");
            }
            return Ok(authorsOfBook);
        }
    }
}
