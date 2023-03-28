using System.ComponentModel.DataAnnotations;
namespace ReaderCult.Models

{
    public class Book
    {
       
        [Required]
        public string? ISBN { get; set; }
        [Required]
        public string? Naslov { get; set; } 
        [Required]
        public string? Autor { get; set; }
        [Required]
        public string? Zanr {get;set;}
        [Required]
        public DateTime DatumIzdavanja{get; set;}
        [Required]
        public string? UkratkoOKnjizi {get; set;}
        public float ProsecanRate{get; set;}



        
    }

}
