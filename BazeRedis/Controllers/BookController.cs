//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
using ReaderCult.DTOs;
using ReaderCult.Models;
using ReaderCult.Services;

namespace ReadersCult.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IRedisServices _redisServices;
    private IConfiguration _config;

    public BookController(IRedisServices rS, IConfiguration cf){
        _redisServices= rS;
        _config= cf;
    }
 
    [Route("AddBook")]
    [HttpPost]
    public ActionResult AddBook([FromBody]BookDTO b){
        var book= new Book();
        book.ISBN= b.ISBN;
        book.Naslov= b.Naslov;
        book.Autor= b.Autor;
        book.Zanr= b.Zanr;
        book.DatumIzdavanja= b.DatumIzdavanja;
        book.UkratkoOKnjizi= b.UkratkoOKnjizi;
        book.ProsecanRate= 0;
        book= _redisServices.AddBook(book);

        if(book==null){
            return BadRequest("Book already exists");
        }
        return Ok(new{
            id=book.ISBN
        });
    }

    [Route("DeleteBook/{isbn}")]
    [HttpDelete]
    public ActionResult DeleteBook(string isbn){
        var response= _redisServices.DeleteBook(isbn);
        if(response==null)return StatusCode(202,"book doesn't exist");
        return Ok(response);
    }

    [Route("AddToBooksThatWereRead/{isbn}")]
    [HttpPut]
    public ActionResult AddToBooksThatWereRead(string isbn){
        var response= _redisServices.AddToBooksThatWereRead(isbn);

        return Ok(response);
    }

    [Route("GetNotDoneBooks")]
    [HttpGet]
    public ActionResult GetNotDoneBooks(){
        var response= _redisServices.GetNotDoneBooks();
        return Ok(response);
    }

    [Route("GetDoneBooks")]
    [HttpGet]
    public ActionResult GetDoneBooks(){
        var response= _redisServices.GetDoneBooks();
        return Ok(response);
    }

    [Route("GetBookForThisWeek")]
    [HttpGet]
    public ActionResult GetBookForThisWeek(){
        var response= _redisServices.GetToReadBook();
        if(response==null)
            return StatusCode(202,"Book not assigned yet.");
        else
            return Ok(new{
                isbn=response.ISBN,
                naslov= response.Naslov,
                autor= response.Autor,
                zanr= response.Zanr,
                datumIzdavanja= response.DatumIzdavanja,
                ukratkoOKnjizi= response.UkratkoOKnjizi
            });
    }

    [Route("SetBookForThisWeek/{isbn}")]
    [HttpPut]
    public ActionResult SetBookForThisWeek(string isbn){
        var respone= _redisServices.SetBookToRead(isbn);
        return Ok(respone);
    }
    
}