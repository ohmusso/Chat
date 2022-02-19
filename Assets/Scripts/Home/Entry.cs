using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Entry : MonoBehaviour
{
    [Serializable] public class OnClickEvent : UnityEvent<int> {}
    [SerializeField] OnClickEvent onClickEvent = null;
    private int id = 0;
    public delegate void onClickCallBack(int id);  

    public void Init(EntryData data, onClickCallBack callback)
    {
        onClickEvent.AddListener(new UnityAction<int>(callback));
        id = data.id;
    }

    public void Click()
    {
        onClickEvent?.Invoke(id);
    }

}
