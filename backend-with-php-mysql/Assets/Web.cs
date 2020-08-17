using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(GetDate());
        //StartCoroutine(GetUsers());
        //StartCoroutine(Login("testuser", "1234"));
        //StartCoroutine(Register("testuser2", "1234"));
    }

    public IEnumerator GetDate()
    {
        string uri = "http://193.122.104.212/test/getdate.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received\n" + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator GetUsers()
    {
        string uri = "http://193.122.104.212/test/getusers.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received\n" + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://193.122.104.212/test/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator Register(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://193.122.104.212/test/register-user.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log(www.downloadHandler.text);
        }
    }
}
