using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // 서버에 붙였을때에는 id<->GameObject 짝지어 관리
    // 현재 게임에서는 id를 관리할 것이 아니고 플레이어는 한명이므로 플레이어는 GameObject타입, 몹은 키가없는 HashSet사용
    //Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>(); 
    //Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>(); 
    //Dictionary<int, GameObject> _env = new Dictionary<int, GameObject>();     // 인터랙티브 가능한 오브젝트(e.g. 약초, 바위캐기, 나무)

    private GameObject _player;
    private HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Unknown:
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1); // 한마리가 늘었다
                break;
            default:
                break;
        }

        return go;
    }

    // 어떤 WorldType인지 리턴하는 메서드
    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Unknown:
                break;
            case Define.WorldObject.Player:
                if (_player == go)
                    _player = null;
                break;
            case Define.WorldObject.Monster:
                if (_monsters.Contains(go))
                {
                    _monsters.Remove(go);
                    if (OnSpawnEvent != null)
                        OnSpawnEvent.Invoke(-1);    // 한마리가 줄었다는 의미
                }
                break;
            default:
                break;
        }
        Managers.Resource.Destroy(go);
    }
}
