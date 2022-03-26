using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class InputLocation : MonoBehaviour
{
    [SerializeField] InputField inputLatitude;
    [SerializeField] InputField inputLongitude;
    public LocationInfo info { get; private set; }

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
#else
#endif // UNITY_ANDROID
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
    }
}
