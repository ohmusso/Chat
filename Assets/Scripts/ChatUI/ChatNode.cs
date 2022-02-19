using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatNode : MonoBehaviour
{
    private ChatData chatData;
    [SerializeField] LayoutGroup layoutGroup;
    [SerializeField]  Image chatBoard;
    [SerializeField]  Text chatText;
    [SerializeField]  Image chatIcon;
    [SerializeField]  Sprite mineSprite;
    [SerializeField]  Sprite othersSprite;
    [SerializeField]  CanvasGroup canvasGroup;

    public void Init(ChatData data)
    {
        chatData = data;
        chatText.text = chatData.body;

        if (chatData.roll == ChatRoll.MINE)
        {
            chatIcon.sprite = mineSprite;
            layoutGroup.childAlignment = TextAnchor.MiddleRight;
            chatBoard.color = new Color(0f, 1f, 0.1f);
            chatIcon.transform.SetSiblingIndex(1);
        }

        else if (chatData.roll == ChatRoll.OTHERS)
        {
            chatIcon.sprite = othersSprite;
            layoutGroup.childAlignment = TextAnchor.MiddleLeft;
            chatBoard.color = new Color(1f, 1f, 1f);
            chatIcon.transform.SetSiblingIndex(0);
        }
        StartCoroutine(CheckTextSize());

//        Destroy(gameObject, 10f);
    }

    void Update()
    {
        
    }

    private IEnumerator CheckTextSize()
    {
        yield return new WaitForEndOfFrame();
        if (chatBoard.rectTransform.sizeDelta.x > this.GetComponent<RectTransform>().sizeDelta.x * 0.5f)
        {
            chatBoard.GetComponent<LayoutElement>().preferredWidth = this.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        }

        StartCoroutine(fadeout(500));
    }
    
    private IEnumerator fadeout(uint fadeflames)
    {
        float step = 1f / fadeflames;
        float alpha = 1f;

        if(step < 0.001f )
        {
            step = 0.01f;
        }

        for(uint i = 1; i <= fadeflames; i++)
        {
            alpha = alpha - step;
            if( alpha < 0f )
            {
                alpha = 0f;
            }
            canvasGroup.alpha = alpha;
            yield return null;
        }

        Destroy(gameObject);
    }
}
