using MyLove.Business;
using MyLove.Interfaces;
using MyLove.Services;
using MyLove.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyLove.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Chat : ContentPage
    {
        List<ChatViewModel> chats = null;
        Bot myBot = null;
        public static bool isRecording = false;
        public Chat()
        {
            InitializeComponent();
            myBot = new Bot();
            showHistory();
        }
        public void showHistory()
        {
            chats = new ChatViewModel().sendMeHistory_testing();
            lstChatHistory.ItemsSource = chats;

            string newMsg = "Say hi. You can text pretty much anything. I will try my best to reply.";
            var itm = new ChatViewModel("Help", newMsg);
            chats.Add(itm);
            lstChatHistory.ItemsSource = null;
            lstChatHistory.ItemsSource = chats;
            lstChatHistory.ScrollTo(itm, ScrollToPosition.Start, true);

            DependencyService.Get<ITextToSpeech>().Speak(newMsg);
            txtMessage.Focus();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            txtMessage.Focus();
        }

        public async void MicClicked(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                isRecording = true;
                btnMic.Text = "E";

            }
            else
            {
                isRecording = false;
                btnMic.Text = "R";
            }
            DependencyService.Get<ITextToSpeech>().Speak("On Click");
        }
        public async void SendClicked(object sender, EventArgs e)
        {
            //txtMessage.Focus();
            string txt = txtMessage.Text;
            txtMessage.Text = "";
            if (string.IsNullOrEmpty(txt))
            {
                return;
            }
            try
            {
                var itm = new ChatViewModel(txt);
                chats.Add(itm);
                lstChatHistory.ItemsSource = null;
                lstChatHistory.ItemsSource = chats;
                lstChatHistory.ScrollTo(itm, ScrollToPosition.Start, true);

                var reply = await myBot.GetBotReply(txt);

                DependencyService.Get<ITextToSpeech>().Speak(reply);
                chats.Remove(itm);
                itm.UpdateBotReply(itm, reply);
                chats.Add(itm);
                lstChatHistory.ItemsSource = null;
                lstChatHistory.ItemsSource = chats;
                lstChatHistory.ScrollTo(itm, ScrollToPosition.Start, true);
            }
            catch (Exception ex) {
                txtMessage.Text = txt;
            }
        }
    }
}