using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChatRoll
{
    MINE,
    OTHERS
}

public class ChatUI : MonoBehaviour
{
    private int id = 0;
    [SerializeField] InputField chatInputField;
    [SerializeField] GameObject chatNodePrefab;
    [SerializeField] GameObject content;
    [SerializeField] GameObject home;

    public void OnClickMineButton()
    {
        CreateChatNode(ChatRoll.MINE);
    }

    public void OnClickOhtersButton()
    {
        CreateChatNode(ChatRoll.OTHERS);
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
        var chatNode = Instantiate<GameObject>(chatNodePrefab, content.transform, false);
        chatNode.GetComponent<ChatNode>().Init(data);

        Debug.Log("id:" + data.id + " roll: " + roll.ToString() + " body: " + str);
    }
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
