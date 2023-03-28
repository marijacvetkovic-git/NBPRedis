using System.ComponentModel.DataAnnotations;
namespace ReaderCult.DTOs

{
    public class BookDTO
    { 
        public string? ISBN { get; set; }
        public string? Naslov { get; set; } 
        public string? Autor { get; set; }
        public string? Zanr {get;set;}
        public DateTime DatumIzdavanja{get; set;}
        public string? UkratkoOKnjizi {get; set;}
        public int KonacanBroj{get; set;}

        
    }

}