using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReaderCult.DTOs;
using ReaderCult.Models;
using ReaderCult.Services;

namespace ReadersCult.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IRedisServices _redisServices;
    private IConfiguration _config;

    public UserController(IRedisServices rS, IConfiguration cf){
        _redisServices= rS;
        _config= cf;
    }

    [Route("SignUp")]
    [HttpPost]
    public ActionResult SignUp([FromBody]UserDTO u){
        var user= new User();
        user.Name= u.Name;
        user.Surname= u.Surname;
        user.Username= u.Username;
        user.Password= u.Password;
        user.Role= u.Role;
        
        
        user= _redisServices.SignUp(user);

        if(user==null){
            return BadRequest("User already exists.");
        }
        return Ok(user);
    }

    [Route("GetUser/{username}")]
    [HttpGet]
    public ActionResult GetUser(string username)
    {
        var u = _redisServices.GetUser(username);
        if (u==null)
        {
            return StatusCode(202,"Username is not valid");
        }
        return Ok(u);
    }

    [Route("LogIn/{username}/{password}")]
    [HttpGet]
    public ActionResult LogIn(string username,string password)
    {
             
        User? u = _redisServices.LogIn(username,password);
        if (u==null)
        {
            return StatusCode(202,"Wrong username or password");
        }
        string token = CreateToken(u);
        return Ok(new{
            Token=token,
            Username=u.Username,
            Name=u.Name,
            Surname=u.Surname,
            Role=u.Role
        });
    }

    [Route("DeleteProfile/{username}/{password}")]
    [HttpDelete]
    public ActionResult DeleteProfile(string username,string password)
    {
        string u = _redisServices.DeleteProfile(username,password);
        if (u=="")
        {
            return StatusCode(202,"Nevalidan username ili password");
        }
            return Ok("Obrisano");
    }

    [Route("AddBookInMyList/{username}/{isbn}")]
    [HttpPut]
    public ActionResult AddBookInMyList(string username,string isbn)
        {
            string response= _redisServices.AddBookInList(username,isbn);
            if(response=="Book added to your list")
            {
                return Ok("Book added!");
            }
            else
            {
                return StatusCode(202,response);
            }
        }
  
    [Route("AddRateBook/{username}/{isbn}/{rate}")]
    [HttpPut]
    public ActionResult AddRateBook(string username,string isbn,string rate)
        {
            string provera = _redisServices.AddBookInList(username,isbn);
            if(provera=="Book already exitsts in this list")
            {
                string response= _redisServices.AddRateBook(username,isbn,rate);
                if(response=="Rate added to list")
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(202,response);

                }
            }
            else
            {
                    return StatusCode(202,"You need to add this book to the list first!");
            }

        }

    [Route("GetRateOfBook/{username}/{isbn}")]
    [HttpGet]
    public ActionResult GetRateOfBook(string username, string isbn){
        string rate= _redisServices.GetMyRateOfBook(username,isbn);
        if (rate==null){
            return StatusCode(202,"User hasn't rated this book");
        }
        else{
            return Ok(rate);
        }
    }

    [Route("GetAllBooksOfUser/{username}")]
    [HttpGet]
    public ActionResult GetAllBooksOfUser(string username){
        Dictionary<Book,string> lista= new Dictionary<Book,string>();
        string[] listOfBooks= _redisServices.GetAllBookReadByUser(username);
        if(listOfBooks.Length==0){
            return StatusCode(202,"No books yet");
        }
        foreach(var book in listOfBooks){
            Book k= _redisServices.GetBook(book);
            string rate= _redisServices.GetMyRateOfBook(username,k.ISBN);
            lista.Add(k,rate);
        }
        return Ok(lista.Select(p=>new{
            isbn=p.Key.ISBN,
            naslov=p.Key.Naslov,
            autor=p.Key.Autor,
            zanr=p.Key.Zanr,
            ukratko=p.Key.UkratkoOKnjizi,
            prosecna=p.Key.ProsecanRate,
            ocena=p.Value
        }));
    }
 private string CreateToken(User u)
        {
            string uloga="";
            if(u.Role=="user")
            {
                uloga="user";
            }
            else if(u.Role=="admin")
            {
                uloga="admin";
            }
         
            List<Claim> claims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, u.Username),
               new Claim(ClaimTypes.Role, uloga),
               new Claim(ClaimTypes.Expiration, DateTime.Now.AddMinutes(60).ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
}