using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    private Action<string> _createItemsCallback;

    void Start()
    {
        _createItemsCallback = (jsonArray) =>
        {
            StartCoroutine(CreateItemsCoroutine(jsonArray));
        };

        CreateItems();
    }

    public void CreateItems()
    {
        string userId = Main.Instance.userInfo.UserID;
        StartCoroutine(Main.Instance.web.GetItemsIDs(userId, _createItemsCallback));
    }

    private IEnumerator CreateItemsCoroutine(string jsonArrayString)
    {
        // parsing json array string as an array
        JArray jArray = JsonConvert.DeserializeObject<JArray>(jsonArrayString);

        for (int i = 0; i < jArray.Count; i++)
        {
            // create local variables
            bool isDone = false;    // are we done downloading?
            string id = jArray[i]["id"].ToString();
            string itemID = jArray[i]["itemID"].ToString();
            JObject itemInfoJson = new JObject();

            // create a callback to get the information from Web.cs
            StartCoroutine(Main.Instance.web.GetItem(itemID, (itemInfo) =>
            {
                isDone = true;
                JArray temp = JsonConvert.DeserializeObject<JArray>(itemInfo);
                itemInfoJson = temp[0] as JObject;
            }));

            // wait until the callback is called from web (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            // instantiate gameobject (item prefab)
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
            //Item item = go.AddComponent<Item>();
            //item.id = id;
            //item.itemID = itemID;
            go.transform.SetParent(this.transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;

            // Fill informations
            go.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"].ToString();
            go.transform.Find("Price").GetComponent<Text>().text = $"{itemInfoJson["price"]}G";
            go.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"].ToString();

            byte[] bytes = ImageManager.instance.LoadImage(itemID);

            // download from web
            if (bytes.Length == 0)
            {
                // create a callback to get the sprite from Web.cs
                StartCoroutine(Main.Instance.web.GetItemIcon(itemID, (downloadedBytes) =>
                {
                    Sprite sprite = ImageManager.instance.BytesToSprite(downloadedBytes);
                    Image itemIconImage = go.transform.Find("Icon").GetComponent<Image>();
                    itemIconImage.sprite = sprite;
                    itemIconImage.SetNativeSize();

                    ImageManager.instance.SaveImage(itemID, downloadedBytes);
                }));
            }
            // load from device
            else
            {
                Sprite sprite = ImageManager.instance.BytesToSprite(bytes);
                Image itemIconImage = go.transform.Find("Icon").GetComponent<Image>();
                itemIconImage.sprite = sprite;
                itemIconImage.SetNativeSize();
            }

            // Set Sell Button
            go.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(Main.Instance.web.SellItem(id, Main.Instance.userInfo.UserID, itemID, () => Destroy(go)));
            });
        }
    }
}
