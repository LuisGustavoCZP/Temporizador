using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Temporizador {
    public int id;
    public float time;

    public override string ToString()
    {
        return $"Temporizador(id:{id}, time:{time})";
    }
}

public class Backend : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var url = "http://localhost:3000/";
        StartCoroutine(getRequest(url));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator getRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            var t = uwr.downloadHandler.text;
            var d = JsonUtility.FromJson<Temporizador>(t);
            Debug.Log("Received: " + d);
        }
    }

    IEnumerator postRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    IEnumerator putRequest(string url)
    {
        byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes("Hello, This is a test");
        UnityWebRequest uwr = UnityWebRequest.Put(url, dataToPut);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    IEnumerator deleteRequest(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Delete(url);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Deleted");
        }
    }
}
