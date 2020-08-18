using SimpleJSON;
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
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //create local variables
            bool isDone = false;    // are we done downloading?
            string itemId = jsonArray[i].AsObject["itemID"];
            JSONObject itemInfoJson = new JSONObject();

            // create a callback to get the information from Web.cs
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(Main.Instance.web.GetItem(itemId, getItemInfoCallback));

            // wait until the callback is alled from web (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            // instantiate gameobject (item prefab)
            GameObject go = Resources.Load<GameObject>("Prefabs/Item");
            go.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            go.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            go.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];
            Instantiate(go, this.transform);
        }
    }
}
