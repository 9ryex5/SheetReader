using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CSVDownloader : MonoBehaviour
{
    public static CSVDownloader CSVD;

    private void Awake()
    {
        CSVD = this;
    }

    internal IEnumerator DownloadData(System.Action<string> onCompleted)
    {
        yield return new WaitForEndOfFrame();

        string downloadData = null;
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/" + PlayerPrefs.GetString("sheetID") + "/export?format=csv"))
        {
            Debug.Log("Starting Download...");
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("...Download Error: " + webRequest.error);
            }
            else
            {
                downloadData = webRequest.downloadHandler.text;
            }
        }

        onCompleted(downloadData);
    }
}