using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chat
{
    public enum ChatRoll
    {
        MINE,
        OTHERS,
        SYSTEM
    }

   public class ChatData
    {
        public ChatRoll roll;
        public string name;
        public string body;

        public ChatData(ChatRoll roll, string name, string body)
        {
            this.roll = roll;
            this.name = name;
            this.body = body;
        }
    }

    public partial class ChatUI : MonoBehaviour
    {
        [SerializeField] InputField chatInputField;
        [SerializeField] InputField chatServerName;
        [SerializeField] InputField chatServerPort;
        [SerializeField] GameObject chatNodePrefab;
        [SerializeField] GameObject chatArea;
        [SerializeField] GameObject home;
        private Vector2 areaSize;
        private string userName;

        void Start()
        {
//            StartCoroutine(Test());
        }

        public void OnClickHomeButton()
        {
            LeaveChatRoom();
            this.gameObject.SetActive(false);
            home.SetActive(true);
        }

        public async void OnConnectButton()
        {
            string serverName = (chatServerName.text == "") ? "localhost" : chatServerName.text;
            string serverPort = (chatServerPort.text == "") ? "5000" : chatServerPort.text;
            await ConnectMagicOnionServer(serverName, Int32.Parse(serverPort));
        }

        public void OnClickSendButton()
        {
            string str = chatInputField.text;
            chatInputField.text = "";

            ChatSendMessage(str);
        }

        public void CreateChatNode(ChatRoll roll, string name, string str)
        {
            ChatData data = new ChatData(roll, name, str);

            // Create ChatNode
            var chatNode = Instantiate<GameObject>(chatNodePrefab, chatArea.transform, false);
            chatNode.GetComponent<ChatNode>().Init(data);

            Debug.Log("roll:" + roll.ToString() + " name: " + name + " body: " + str);
        }
    }
}

