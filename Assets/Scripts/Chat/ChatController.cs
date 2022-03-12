using Grpc.Core;
using MagicOnion.Client;
using Chat.Shared.Services;
using Chat.Shared.Hubs;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    public partial class ChatUI : MonoBehaviour, IChatHubReceiver
    {
        private Channel _channel;
        private CancellationTokenSource shutdownCancellation = new CancellationTokenSource();

        private IMyFirstService _service;
        private IChatHub _chatHub;
        private bool _isJoin; 

        // UI
        [SerializeField] Text chatText;

        async void Awake()
        {
//            _isJoin = false;
//            _channel = new Channel("testmagiconion.centralus.azurecontainer.io", 80, ChannelCredentials.Insecure);
//            _channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);
//            userName = UnityEngine.Random.value.ToString();
//            JoinOrLeave();
        }

        private async Task ConnectMagicOnionServer(string server_name, int server_port)
        {
            _channel = new Channel(server_name, server_port, ChannelCredentials.Insecure);
            while (!shutdownCancellation.IsCancellationRequested)
            {
                try
                {
                    CreateChatNode(ChatRoll.SYSTEM, "System", "connecting to server...");
                    Debug.Log($"connecting to server...");
                    _chatHub = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(_channel, this, cancellationToken: shutdownCancellation.Token);
                    Debug.Log($"Connection is established.");
                    break;
                }
                catch (Exception e)
                {
                    CreateChatNode(ChatRoll.SYSTEM, "System", $"error: {e.Message.ToString()}");
                    Debug.LogError(e);
                }

                Debug.Log($"Failed to connect to server. Retry after 5 secconds...");
                await Task.Delay(5000);
            }

            userName = UnityEngine.Random.value.ToString();
            JoinOrLeave();
        } 

        async void OnDestroy()
        {
            // Clean up Hub and channel
            shutdownCancellation.Cancel();
            await _chatHub?.DisposeAsync();
            await _channel?.ShutdownAsync();
        }

        public async void LeaveChatRoom()
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
