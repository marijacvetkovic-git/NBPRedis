using ReaderCult.DTOs;
using ReaderCult.Models;
using StackExchange.Redis;
//using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;
using System.Text.Json;


namespace ReaderCult.Services{
    public class ChatService : IChatService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHubContext<Hub> _hubContext;
        public ChatService(IConnectionMultiplexer redis, IHubContext<Hub> hc){
            _redis= redis;
            _hubContext= hc;
        }

        public async Task SendMessage(ChatRoomMessage message)
        {
            var jsonMsg= JsonSerializer.Serialize<ChatRoomMessage>(message);
            string v= jsonMsg;
            var db= _redis.GetDatabase();
            await db.PublishAsync("groupChat", jsonMsg);

        }

    }
}
