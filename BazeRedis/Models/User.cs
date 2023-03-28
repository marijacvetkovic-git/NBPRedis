using System.ComponentModel.DataAnnotations;
namespace ReaderCult.Models

{
    public class User
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public string? Password { get; set; }

        public string? Role{get;set;}


    }

}