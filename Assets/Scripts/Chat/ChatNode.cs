using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chat
{    public class ChatNode : MonoBehaviour
    {
        private ChatData chatData;
        [SerializeField]  Image chatBoard;
        [SerializeField]  Text chatText;
        [SerializeField] CanvasGroup canvasGroup;
        private Vector2 areaSize;

        public void Init(ChatData data)
        {
            chatData = data;
            StartCoroutine(InitTransform());
        }

        private IEnumerator InitTransform()
        {
            canvasGroup.alpha = 0;

            yield return new WaitForEndOfFrame();
            // resize
            if (chatBoard.rectTransform.sizeDelta.x > this.GetComponent<RectTransform>().sizeDelta.x * 0.5f)
            {
                chatBoard.GetComponent<LayoutElement>().preferredWidth = this.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
            }
            yield return new WaitForEndOfFrame();
            // positioning
            areaSize = this.transform.parent.GetComponent<RectTransform>().rect.size;
            this.transform.localPosition = Chat.Position.FitAreaNodeAncUpLeft(areaSize, this.GetComponent<RectTransform>().rect.size, Chat.Position.RndPos(areaSize));

            canvasGroup.alpha = 1;
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

}