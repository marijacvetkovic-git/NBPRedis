using System.ComponentModel.DataAnnotations;
namespace ReaderCult.Models

{
    public class ChatRoomMessage
    {
      public string id { get; set; }
      public string? from { get; set; }
      public DateTime date { get; set; }
      public string? message { get; set; }
      public string? room { get; set; }="groupChat";
    }

}