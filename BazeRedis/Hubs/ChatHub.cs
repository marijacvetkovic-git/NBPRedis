using Microsoft.AspNetCore.SignalR;
using ReaderCult.Models;
using ReaderCult.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

    public class ChatHub: Hub{
        private readonly IRedisServices _redisServices;
        private readonly IChatService _chatService;
        public ChatHub(IRedisServices redisServices, IChatService chatService){
            _redisServices= redisServices;
            _chatService= chatService;
        }

        public async Task SendMessage(string message){
            ChatRoomMessage msg= JsonSerializer.Deserialize<ChatRoomMessage>(message);
            if(msg!=null){
                await _chatService.SendMessage(msg);
            }
        }

    }