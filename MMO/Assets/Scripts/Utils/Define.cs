using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster
    }

    public enum State
    {
        Idle,
        Moving,
        Skill,
        Die
    }

    public enum Layer
    { 
        Monster = 8,
        Ground = 9,
        Block = 10
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount
    }

    public enum UIEvent
    {
        Click,
        Drag
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,    // 맨처음으로 마우스를 누른상태
        PointerUp,      // 마우스를 한번이라도 누른상태에서 떼는 상태
        Click
    }

    public enum CameraMode
    {
        QuaterView,

    }
}
