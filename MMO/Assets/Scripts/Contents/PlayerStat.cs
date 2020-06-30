using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] protected int _exp;
    [SerializeField] protected int _gold;

    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;

            // 레벨업 체크
            int level = 1;
            while (true)
            {
                Data.Stat stat;
                // 다음 레벨에 해당하는 스탯이 없다면(만렙) 종료
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;

                // 레벨업 경험치에 도달하지 못했다면 종료
                if (_exp < stat.totalExp)
                    break;

                level++;
            }

            if (level != Level)
            {
                Debug.Log("level up!");
                Level = level;
                SetStat(Level);
            }
        }
    }

    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        SetStat(_level);

        _moveSpeed = 5f;
        _exp = 0;
        _gold = 0;
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        _defense = stat.defense;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
