using MyLove.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLove.ViewModels
{
    public class ChatViewModel
    {
        public string UserName { get; set; }
        public string UserMessage { get; set; }
        public string BotName { get; set; }
        public string BotMessage { get; set; }
        public string Alignment { get; set; }
        public ChatViewModel() { }
        public ChatViewModel(string userMsg, string botMsg = "Typing...", string username = "User", string botName="Bot", string align="Start")
        {
            UserName = Bot.UserName;
            UserMessage = userMsg;
            BotName = Bot.BotName;
            BotMessage = botMsg;
            Alignment = align;
        }
        public ChatViewModel UpdateBotReply(ChatViewModel chat,string botMsg)
        {
            chat.BotMessage = botMsg;
            return chat;
        }
        public List<ChatViewModel> sendMeHistory_testing()
        {
            var list = new List<ChatViewModel>();
            list.Add(new ChatViewModel("Hi","Hi"));
            list.Add(new ChatViewModel("What is your name", "I am Bot 1.1!"));
            list.Add(new ChatViewModel ("How are you","I am good :)"));
            return list;
        }
    }
}
