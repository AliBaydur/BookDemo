using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookDemo.Data;
using BookDemo.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

            if (book is null)
                return NotFound(); // 404 NotFound.         

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); //400 BadRequest.

                ApplicationContext.Books.Add(book);
                return StatusCode(201, book);//201 Created.
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //ex.message hata mesajının detayını verir.
            }
        }

        [HttpPut("{id:int}")] // Güncelleme için kullanıyoruz. Kaynak yoksa kaynağı oluşturmak içinde kullanılabilir.
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            //Check Book?
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));

            if (entity is null)
                return NotFound(); //404

            // Check id
            if (id != book.Id)
                return BadRequest(); //400

            ApplicationContext.Books.Remove(entity);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);
            return Ok(book);
        }

        [HttpDelete]
        public IActionResult DeleteALLBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));

            if (entity is null)
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"Book with id:{id} could not found."
                }); //404

            ApplicationContext.Books.Remove(entity);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name ="id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            //Check entity
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));

            if (entity is null)
                return NotFound(); //404

            bookPatch.ApplyTo(entity);
            return NoContent(); //204
        }
    }
}
