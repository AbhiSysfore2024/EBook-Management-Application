using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interface;

namespace EBook_Management_Application.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorDatabaseManager _authordatabasemanager;
        public AuthorController(IAuthorDatabaseManager authorDatabaseManager)
        {
            _authordatabasemanager = authorDatabaseManager;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("GetAllAuthors")]
        public ActionResult GetAllAuthors()
        {
            return Ok(_authordatabasemanager.GetAllAuthors());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("/AddAuthor")]
        public ActionResult AddAuthor([FromBody] DTOAuthor author)
        {
            var result = _authordatabasemanager.AddAuthor(author);
            return result == false ? BadRequest() : Ok("Author added successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("/UpdateAuthor/{id}")]
        public ActionResult UpdateAuthor([FromBody] UpdateAuthorModel author)
        {
            return Ok(_authordatabasemanager.UpdateAuthor(author));
        }

        [Authorize(Roles = "Admin")]
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


    }
}
