using System;
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
        //StartCoroutine(Register("testuser5", "1234"));
    }

    public IEnumerator GetDate()
    {
        string uri = "https://uiyoung.cf/test/getdate.php";
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
                    Debug.LogError("http Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received\n" + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator GetUsers()
    {
        string uri = "https://uiyoung.cf/test/get-users.php";
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
                    Debug.LogError("http Error\n" + webRequest.error);
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

        using (UnityWebRequest www = UnityWebRequest.Post("https://uiyoung.cf/test/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
            {
                Debug.Log(www.downloadHandler.text);
                Main.Instance.userInfo.SetCredentials(username, password);
                Main.Instance.userInfo.UserID = www.downloadHandler.text;

                if (www.downloadHandler.text.Contains("Wrong Credentials") || www.downloadHandler.text.Contains("Username does not exists"))
                    Debug.Log("Try Again");
                else
                {
                    // If we login correctly
                    Main.Instance.userProfile.SetActive(true);
                    Main.Instance.login.gameObject.SetActive(false);
                }

            }
        }
    }

    public IEnumerator Register(string username, string password, string confirmPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);
        form.AddField("confirmPass", confirmPassword);

        using (UnityWebRequest www = UnityWebRequest.Post("https://uiyoung.cf/test/register-user.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator GetItemsIDs(string userID, Action<string> callback)
    {
        string uri = "https://uiyoung.cf/test/get-item-ids.php";
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("http Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    string jsonArray = webRequest.downloadHandler.text;

                    // 아이템이 하나도 없는 경우
                    if (jsonArray == "0")
                        break;
                    
                    callback(jsonArray);

                    break;
            }
        }
    }

    public IEnumerator GetItem(string itemID, Action<string> callback)
    {
        string uri = "https://uiyoung.cf/test/get-item.php";
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("http Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    string jsonArray = webRequest.downloadHandler.text;
                    callback(jsonArray);

                    break;
            }
        }
    }

    public IEnumerator SellItem(string id, string userID, string itemID, Action callback)
    {
        string uri = "https://uiyoung.cf/test/sell-item.php";
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("userID", userID);
        form.AddField("itemID", itemID);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("http Error\n" + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    callback();

                    break;
            }
        }
    }
}
