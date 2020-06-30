using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    // 인벤 아이템은 아이템명, 아이템이미지 두가지로 구성되어있는데
    // 이렇게 별로 없을 경우 따로 구분하지 않고 GameObjects에 뭉뚱거려서 넣어도 된다.
    enum GameObjects
    {
        ItemIcon,
        ItemNameText
    }

    private string _name;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name;

        Get<GameObject>((int)GameObjects.ItemIcon).AddUIEvent((PointerEventData data) => { Debug.Log($"아이템클릭:{_name}"); });
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
