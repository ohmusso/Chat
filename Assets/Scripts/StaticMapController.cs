using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class StaticMapController : MonoBehaviour
{
    //Google Maps Static API URL
    private const string STATIC_MAP_URL = "https://maps.googleapis.com/maps/api/staticmap?key=${apikey}&zoom=15&size=640x640&scale=2&maptype=terrain&style=element:labels|visibility:off";
    public Texture2D tex { get; private set; }
    public event Action update;

    void Start()
    {
        Debug.Log("map controller start");
    }
  
    public IEnumerator getStaticMap()
    {
        var query = "";
 
#if UNITY_EDITOR
        // query parameter: center
        query += "&center=" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", 35.6809591.ToString(), 139.7673068.ToString()));
        // query parameter: markers
        query += "&markers=" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", 35.6809591.ToString(), 139.7673068.ToString()));
#else
        // query parameter: center
        query += "&center=" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", Input.location.lastData.latitude, Input.location.lastData.longitude));
        // query parameter: markers
        query += "&markers=" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", Input.location.lastData.latitude, Input.location.lastData.longitude));
#endif
 
        var req = UnityWebRequestTexture.GetTexture(STATIC_MAP_URL + query);
        Debug.Log("request uri: " + req.uri.ToString());
        yield return req.SendWebRequest();
 
        if (req.error == null)
        {
            Debug.Log("OK");
            tex = ((DownloadHandlerTexture)req.downloadHandler).texture;
            update?.Invoke();
        }
        else
        {
            Debug.Log("erorr");

        }
    }
}
