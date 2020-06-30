using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField] protected Define.State _state = Define.State.Idle;
    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected GameObject _lockTarget;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    // 혹시 나중에 자식클래스에서 바꾸고싶은경우를 대비해서 virtual로 프로퍼티를 선언
    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Idle:
                    anim.CrossFade("IDLE", 0.1f);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);   // -1 : layer상관없으므로 아무값이나. 0: 처음부터 다시재생
                    break;
                case Define.State.Die:
                    anim.CrossFade("DIE", 0.1f);
                    break;
            }
        }
    }

    private void Start()
    {
        Init();
    }


    void Update()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    protected abstract void Init();
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateSkill() { }
}
