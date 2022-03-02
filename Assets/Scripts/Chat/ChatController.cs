using Grpc.Core;
using MagicOnion.Client;
using Chat.Shared.Services;
using Chat.Shared.Hubs;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Chat
{
    public partial class ChatUI : MonoBehaviour, IChatHubReceiver
    {
        private Channel _channel;
        private IMyFirstService _service;
        private IChatHub _chatHub;
        private bool _isJoin; 

        // UI
        [SerializeField] Text chatText;

        async void Awake()
        {
            _isJoin = false;
            _channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);
            _chatHub = StreamingHubClient.Connect<IChatHub, IChatHubReceiver>(_channel, this);
            userName = Random.value.ToString();
            JoinOrLeave();
        }

        async void OnDestroy()
        {
            await _chatHub.DisposeAsync();
            await _channel.ShutdownAsync();
        }

        public async Task LeaveChatRoom()
        {
            await this._chatHub.LeaveAsync();
        }

        // button event
        public async void JoinOrLeave()
        {
            if (this._isJoin)
            {
                await this._chatHub.LeaveAsync();
                this._isJoin = false;
//                this.JoinOrLeaveButtonText.text = "入室する";
                //メッセージ送信ボタンを非表示に
//                this.SendMessageButton.gameObject.SetActive(false);
            }
            else
            {
                await this._chatHub.JoinAsync(userName);
                this._isJoin = true;
//                this.JoinOrLeaveButtonText.text = "退室する";
                //メッセージ送信ボタンを表示
//                this.SendMessageButton.gameObject.SetActive(true);
            }
        }

        // button event
        public async void ChatSendMessage(string str)
        {
            await this._chatHub.SendMessageAsync(str);
        }

        // Server->Client API
        public void OnJoin(string name)
        {
            Debug.Log("join");
            //this.ChatText.text += $"\n{name}さんが入室しました";
        }

        public void OnLeave(string name)
        {
            Debug.Log("leave");
            //this.ChatText.text += $"\n{name}さんが退室しました";
        }

        public void OnSendMessage(string name, string message)
        {
            Debug.Log("send msg");
            CreateChatNode(ChatRoll.OTHERS, name, message);
            //this.ChatText.text += $"\n{name}：{message}";
        }
    }
}
