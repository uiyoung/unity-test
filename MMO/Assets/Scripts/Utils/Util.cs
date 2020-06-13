using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /* 컴포넌트가 없으면 추가하고 있으면 받아오는 메서드 */
    public static T GetOrAddComponent<T>(GameObject go) where T:UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    /*
     * 자식 컴포넌트를 쭉 스캔하여 T 타입이고, 매개변수 name과 같은 이름의 게임오브젝트를 찾아주는 메서드 
     * go : 최상위 부모
     * name : 게임 오브젝트 이름. 기본값 null로 입력 안하면 이름은 비교하지않고 T 타입만 일치하면 리턴
     * recursive : 자식의 자식도 찾을것인지 
     */
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (!recursive)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i); // i번째 자식 transform

                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                // isNullOrEmpty 쓴 이유 : name 매개변수를 안쓴 경우를 위해
                // name이 비어있거나 내가 원하는 name이면 그걸 리턴해주세요. 
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }
}
