using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if(prefab == null)
        {
            Debug.Log("$Failed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);  // Object 붙인이유 : 안붙이면 이 클래스의 Instantiate()를 실행해버려서 재귀가 되어버린다.
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
