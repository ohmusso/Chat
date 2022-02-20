using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField] GameObject chatUI;
    [SerializeField] GameObject entryPrefab;
    [SerializeField] GameObject content;
    private List<GameObject> Entries;

    // Start is called before the first frame update
    void Start()
    {
        Entries = new List<GameObject>();
        Entries.Add(CreateEntry(1));
        Entries.Add(CreateEntry(2));
        Entries.Add(CreateEntry(3));
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
}


public class EntryData
{
    public int id;

    public EntryData(int id)
    {
        this.id = id;
    }
}