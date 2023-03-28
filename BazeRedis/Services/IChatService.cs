using System.ComponentModel.DataAnnotations;
using ReaderCult.DTOs;
using ReaderCult.Models;

namespace ReaderCult.Services

{
    public interface IChatService
    {
        Task SendMessage(ChatRoomMessage message);
    }

}