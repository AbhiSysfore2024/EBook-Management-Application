using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Models;
using Services;
using Services.Interface;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EBook_Management_Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookAuthorController : ControllerBase
    {
        private readonly IDatabaseManager _databasemanager;
        private readonly IAuthorDatabaseManager _authordatabasemanager;

        public BookAuthorController(IDatabaseManager databaseManager, IAuthorDatabaseManager authorDatabaseManager)
        {
            _databasemanager = databaseManager;
            _authordatabasemanager = authorDatabaseManager;
            //_configuration = configuration;
        }


        [HttpPost]
        public IActionResult Login(LoginRequest loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.UserName) ||
                string.IsNullOrEmpty(loginDTO.PassWord))
                    return BadRequest("Username and/or Password not specified");
                if (loginDTO.UserName.Equals("string") &&
                loginDTO.PassWord.Equals("string"))
                {
                    var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("YourSecretKeyForAuthenticationOfApplication"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "abhilash",
                        audience: "abhilash",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signinCredentials
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
                }
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();
        }


        [Authorize]
        [HttpGet]
        [Route("GetAllBooks")]
        public ActionResult GetAllBoooks(){
            return Ok(_databasemanager.GetAllBooks());
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllAuthors")]
        public ActionResult GetAllAuthors()
        {
            return Ok(_authordatabasemanager.GetAllAuthors());
        }

        [Authorize]
        [HttpPost]
        [Route("/AddBook")]
        public ActionResult AddBook([FromBody] DTOBooks book) {
            string books = _databasemanager.AddBook(book);
            return Ok(books);
        }

        [Authorize]
        [HttpPost]
        [Route("/AddAuthor")]
        public ActionResult AddAuthor([FromBody] DTOAuthor author)
        {
            var result = _authordatabasemanager.AddAuthor(author);
            return result == false ? BadRequest() : Ok("Author added successfully");
        }

        [Authorize]
        [HttpPut]
        [Route("/UpdateBook/{id}")]
        public ActionResult UpdateBook([FromBody] UpdateBookModel book)
        {
            return Ok(_databasemanager.UpdateBook(book));
        }

        [Authorize]
        [HttpPut]
        [Route("/UpdateAuthor/{id}")]
        public ActionResult UpdateAuthor([FromBody] UpdateAuthorModel author)
        {
            return Ok(_authordatabasemanager.UpdateAuthor(author));
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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
