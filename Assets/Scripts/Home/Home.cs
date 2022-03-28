using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Grpc.Core;
using MagicOnion.Client;
using Chat.Shared.Services;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] GameObject chatUI;
    [SerializeField] GameObject entryPrefab;
    [SerializeField] GameObject content;
    private List<GameObject> Entries;
    private Channel _channel;
    private IMyFirstService _service;
    private string table = "chatdata";

    // Start is called before the first frame update
    void Start()
    {
        Entries = new List<GameObject>();
        Entries.Add(CreateEntry(1));
        Entries.Add(CreateEntry(2));
        Entries.Add(CreateEntry(3));

        _channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);
        _service = MagicOnionClient.Create<IMyFirstService>(_channel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject CreateEntry(int id)    
    {
        EntryData data = new EntryData(id);

        var entry = Instantiate<GameObject>(entryPrefab, content.transform, false);
        entry.GetComponent<Entry>().Init(data, OnClickEntry);

        return entry;
    }

    public void OnClickEntry(int id)
    {
        Debug.Log("Entry: " + id);
        this.gameObject.SetActive(false);
        chatUI.SetActive(true);
    }

    public void OnClickSendData()
    {
        _service.TestStorageAdd(table, "hello");
    }
    public void OnClickDeleteData()
    {
        _service.TestStorageDelete(table, "hello");
    }
    public void OnClickQueryData()
    {
        _service.TestStorageQuery(table, "hello");
    }
}


public class EntryData
{
    public int id;

    public EntryData(int id)
    {
        this.id = id;
    }
}
