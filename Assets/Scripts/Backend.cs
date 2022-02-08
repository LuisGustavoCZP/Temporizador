using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ProductCategory
{
    public int id;
    public string name;

    public override string ToString()
    {
        return $"Category (id:{id}, name:{name})";
    }
}

[Serializable]
public class ProductCategories
{
    public List<ProductCategory> categories;
    public override string ToString()
    {
        var p = string.Empty;
        foreach(var t in categories)
        {
            p = string.Concat(p, "\n   ", t);
        }
        return $"Temporizadores {categories.Count}{p}";
    }
}

public class Backend : MonoBehaviour
{
    string url = "http://localhost:3000/";

    // Start is called before the first frame update
    void Start()
    {
        GetCategories();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCategories ()
    {
        StartCoroutine(getRequest(url+ "categories", t => { JsonUtility.FromJson<ProductCategories>(t); }));
    }

    IEnumerator getRequest(string uri, Action<string> action)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            string t = uwr.downloadHandler.text;
            Debug.Log("Received: " + t);

            //var temps = ;
            action(t);
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
