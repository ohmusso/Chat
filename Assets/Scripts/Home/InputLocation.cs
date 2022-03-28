using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class InputLocation : MonoBehaviour
{
    [SerializeField] InputField inputLatitude;
    [SerializeField] InputField inputLongitude;
    [SerializeField] GameObject mapControllerObject;
    [SerializeField] GameObject staticMap;
    private StaticMapController mapController;
    public LocationInfo info { get; private set; }

    private void Start()
    {
        mapController = mapControllerObject.GetComponent<StaticMapController>();
        mapController.update += UpdateMap;
    }

    private void UpdateMap()
    {
        Debug.Log("invoke update map");
        staticMap.GetComponent<Image>().sprite = Sprite.Create(mapController.tex, new Rect(0f, 0f, mapController.tex.width, mapController.tex.height), new Vector2(0.5f, 0.5f));
    }

    public void OnClickGetLocationButton()
    {
        Debug.Log("start location service");
        StartCoroutine(StartLocationServiceOnce());
    }

    IEnumerator StartLocationServiceOnce()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("location service not enable");
#if UNITY_ANDROID
            Permission.RequestUserPermission(Permission.FineLocation);
#endif // UNITY_ANDROID
#if UNITY_EDITOR
        StartCoroutine(mapController.getStaticMap());
#endif
            yield break;
        }

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        Debug.Log("location status: " + Input.location.status.ToString());
        int maxWait = 20;
        while (Input.location.status != LocationServiceStatus.Running && maxWait > 0)
        {
            Debug.Log("location service stating...");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("location service timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            info = Input.location.lastData;
            inputLatitude.text = info.latitude.ToString();
            inputLongitude.text = info.longitude.ToString();
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();

        StartCoroutine(mapController.getStaticMap());
    }
}
