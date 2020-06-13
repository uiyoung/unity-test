using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    // UI 타입을 키값으로 타입에 해당하는 목록들을 배열로 가지고 있는 딕셔너리
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    /*
     *enum값을 넘겨주면 그안의 애들을 인스펙터에서 이름으로 찾아서 _objects 딕셔너리에 저장하는 메서드
     *UI를 public으로 만들고 끌어다 매핑하는 것을 자동화 
     */
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);   // enum 목록들을 string[] 값으로 추출 
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        // 자식 게임오브젝트를 스캔해서 매개변수로 받은 enum과 같은 이름의 게임오브젝트를 배열에 저장
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
        }
    }

    /*
     * 매핑한 UI를 _objects 딕셔너리에서 꺼내오는 메서드
     * 매개변수로 int형을 받는데 가져올 enum값을 int로 형변환하여 넣으면 된다.
     */
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;   // _objects에서 꺼내온 타입이 UnityEngine.Object였으므로 T타입으로 형변환
    }

    // 자주 사용하는 것들은 Get<T>... 말고 바로 꺼내올 수 있게
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            default:
                break;
        }
    }
}
