using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
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
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JArray temp = JsonConvert.DeserializeObject<JArray>(itemInfo);
                itemInfoJson = temp[0] as JObject;
            };

            StartCoroutine(Main.Instance.web.GetItem(itemID, getItemInfoCallback));

            // wait until the callback is alled from web (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            // instantiate gameobject (item prefab)
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
            go.transform.SetParent(this.transform);
            go.transform.localScale = new Vector3(1f, 1f, 1f);
            go.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"].ToString();
            go.transform.Find("Price").GetComponent<Text>().text = $"${itemInfoJson["price"]}";
            go.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"].ToString();
            go.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(Main.Instance.web.SellItem(id, Main.Instance.userInfo.UserID, itemID, () => Destroy(go)));
            });
        }
    }
}
