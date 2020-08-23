using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class ButtonExtension
{
    public static void AddEventListener(this Button button, int param, Action<int> OnClick)
    {
        button.onClick.AddListener(() => OnClick(param));
    }
}

[Serializable]
public class Item
{
    public Sprite Icon;
    public string IconURL;
    public string Name;
    public string Description;
}

public class ItemList : MonoBehaviour
{
    private Item[] _items;
    [SerializeField] Sprite _defaultIcon;



    void Start()
    {
        //DrawUI();
        StartCoroutine(GetItems());
    }

    private void DrawUI()
    {
        GameObject buttonTemplate = transform.GetChild(0).gameObject;
        GameObject go;

        //int length = items.Length;
        int length = _items.Length;
        for (int i = 0; i < length; i++)
        {
            go = Instantiate(buttonTemplate, this.transform);
            go.transform.GetChild(0).GetComponent<Image>().sprite = _items[i].Icon;
            go.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            go.transform.GetChild(1).GetComponent<Text>().text = _items[i].Name;
            go.transform.GetChild(2).GetComponent<Text>().text = _items[i].Description;
            go.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }

        Destroy(buttonTemplate);
    }

    private void ItemClicked(int itemIndex)
    {
        Debug.Log($"---item {itemIndex} clicked---");
        Debug.Log($"{_items[itemIndex].Name}");
        Debug.Log($"{_items[itemIndex].Description}");
    }

    IEnumerator GetItems()
    {
        string url = "https://uiyoung.cf/test/get-all-items.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"Error : {webRequest.error}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"HTTP Error : {webRequest.error}");
                    break;
                case UnityWebRequest.Result.Success:
                    //JArray jsonArray = JArray.Parse(webRequest.downloadHandler.text);
                    _items = JsonConvert.DeserializeObject<Item[]>(webRequest.downloadHandler.text);

                    StartCoroutine(GetItemIcons());
                    break;
            }
        }
    }

    IEnumerator GetItemIcons()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            string iconURL = _items[i].IconURL;
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(iconURL))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    // error : show default image
                    _items[i].Icon = _defaultIcon;
                    Debug.Log(webRequest.error);
                }
                else
                {
                    Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                    Debug.Log($"{i} : {texture}");
                    _items[i].Icon = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero, 10f);
                }
            }
        }
        DrawUI();
    }
}
