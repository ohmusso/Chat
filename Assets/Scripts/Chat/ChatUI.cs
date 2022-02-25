using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chat
{
    public enum ChatRoll
    {
        MINE,
        OTHERS
    }

   public class ChatData
    {
        public int id;
        public ChatRoll roll;
        public string body;

        public ChatData(int id, ChatRoll roll, string body)
        {
            this.id = id;
            this.roll = roll;
            this.body = body;
        }
    }

    public class ChatUI : MonoBehaviour
    {
        private int id = 0;
        [SerializeField] InputField chatInputField;
        [SerializeField] GameObject chatNodePrefab;
        [SerializeField] GameObject chatArea;
        [SerializeField] GameObject home;
        private Vector2 areaSize;

        void Start()
        {
            StartCoroutine(Test());
        }

        public void OnClickHomeButton()
        {
            this.gameObject.SetActive(false);
            home.SetActive(true);
        }

        public void CreateChatNode(ChatRoll roll)
        {
            id++;
            string str = chatInputField.text;
            chatInputField.text = "";
            ChatData data = new ChatData(id, roll, str);

            // Create ChatNode
            var chatNode = Instantiate<GameObject>(chatNodePrefab, chatArea.transform, false);
            chatNode.GetComponent<ChatNode>().Init(data);

            Debug.Log("id:" + data.id + " roll: " + roll.ToString() + " body: " + str);
        }

        private IEnumerator Test()
        {
            for(int i = 0; i < 10; i++){
                yield return new WaitForSeconds(1);
                CreateChatNode(ChatRoll.MINE);
            }
        }
    }
}

