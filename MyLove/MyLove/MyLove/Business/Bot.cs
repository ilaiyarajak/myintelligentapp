using MyLove.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyLove.Business
{
    public class Bot
    {
        public static string UserName = "You";
        public static string BotName = "Bot";
        private Luis luis = null;
        public static int boringChatCount = 0;
        public static int maxBoringChatCount = 1;
        public Dictionary<string, List<string>> myReplyKnowledge = null;

        public static Random rnd = new Random();
        public Bot() {
            luis = new Luis();
            myReplyKnowledge = new Dictionary<string, List<string>>();
            //rnd = new Random();

            loadMyReplyKnowledge_Testing();
        }
        private async Task<bool> loadMyReplyKnowledge_Testing() {
            //myReplyKnowledge.Add("Meeting", new List<string> {"Sure we can meet at [REPLACE1] by [REPLACE2]","Sure","Why not.", "Would be great to meet you at [REPLACE1]" });
            //myReplyKnowledge.Add("Question", new List<string> { "Yes", "All time", "All time for you", "Yes. Tell me","I am here" });
            string url = "https://xhacknight17.blob.core.windows.net/app/textmeknowledge.txt";
            
            string task = await luis.MakeAsyncRequest(url,"","text/html");
            JsonValue collection = JsonValue.Parse(task);
            for(int i=0;i< collection.Count; i++)
            {
                if (!myReplyKnowledge.ContainsKey(collection[i]["Key"]))
                {
                    myReplyKnowledge.Add(collection[i]["Key"], collection[i]["Value"].ToString().Split(',').ToList());
                }
            }
            return true;
        }

        public async Task<string> GetBotReply(string userMsg)
        {
            var task = await luis.MakeAsyncRequest(userMsg);
            var res = JsonConvert.DeserializeObject<LuisResponce>(task);
            return  processLuisResponce(res);
        }

        public string processLuisResponce(LuisResponce luisResp)
        {
            string replymsg = string.Empty;
            double score = 0;
            Double.TryParse(luisResp.topScoringIntent.score, out score);
            if (score < 0.5 && luisResp.entities.Count==0) luisResp.topScoringIntent.intent = "None";
            if (boringChatCount >= maxBoringChatCount)
            {
                luisResp.topScoringIntent.intent = "Boring";
                //boringChatCount = 0;
            }
            switch (luisResp.topScoringIntent.intent)
            {
                case "Repeatme":
                    replymsg = luisResp.query.Contains(" ")?luisResp.query.Remove(0,4):luisResp.query;
                    boringChatCount++;
                    break;
                case "None":
                    replymsg = luisResp.query.Contains(" ") ? "That's fine. But Say something else" : luisResp.query;
                    boringChatCount++;
                    break;
                default:
                    replymsg = FectReplyFromMyKnowledge(luisResp.topScoringIntent.intent);
                    for(int i=0;i< luisResp.entities.Count; i++)
                    {
                        replymsg = replymsg.Replace("[REPLACE"+(i+1)+"]", luisResp.entities[i].entity);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        replymsg = replymsg.Replace("[REPLACE" + (i + 1) + "]", "");
                    }
                    boringChatCount = 0;
                    break;
            }
            return replymsg.Trim()==""? "That's fine. But Say something else" : replymsg;
        }
        public string FectReplyFromMyKnowledge(string key)
        {
            string reply = string.Empty;
            if (myReplyKnowledge.ContainsKey(key))
            {
                List<string> replies = myReplyKnowledge[key];
                reply = replies[rnd.Next(replies.Count)];
            }
            return reply.Replace("\"", "");
        }
    }
}
