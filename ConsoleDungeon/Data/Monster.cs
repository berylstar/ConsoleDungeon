using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 열거형 몬스터 타입
public enum MonsterType
{
    Goblin = 0,
    Slime = 1,
    Wolf = 2,
    Zombie = 3,
    Golem = 4,
    Ghost = 5,
    Witch = 6,
    Wizard = 7,
    Knight = 8,
    Guardian = 9,
}

// 몬스터 클래스
public class Monster : ICharacter
{
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int AP { get; private set; }
    public int DP { get; private set; }
    public int HP { get; private set; }
    public int Speed { get; private set; }
    public int Gold { get; private set; }

    public bool DefenseStance { get; set; }
    public int Focus { get; set; }

    // 무작위 몬스터 스탯 생성
    public Monster(string _name, int _level)
    {
        Name = _name;
        Level = _level;
        AP = 5 + Utils.random.Next(_level, _level * 3);
        DP = Utils.random.Next(_level, _level * 5);
        HP = 50 + Utils.random.Next(0, _level) * 10;
        Speed = 5 + Utils.random.Next(0, _level) * 5;
        Gold = 300 + Utils.random.Next(_level, _level * 2) * 100;
        DefenseStance = false;
        Focus = 0;
    }

    public Monster(string _name, int _level, int _ap, int _dp, int _hp, int _speed, int _gold)
    {
        Name = _name;
        Level = _level;
        DP = _dp;
        AP = _ap;
        HP = _hp;
        Speed = _speed;
        Gold = _gold;
        DefenseStance = false;
        Focus = 0;
    }

    public void GetDamaged(int damage)
    {
        HP -= damage;
    }

    public bool IsDefense()
    {
        if (Utils.random.Next(0, 101) <= DP)
            return true;
        else if (DefenseStance)
            return true;
        else
            return false;
    }
}
