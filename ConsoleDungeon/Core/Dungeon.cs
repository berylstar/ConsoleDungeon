using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 열거형 던전 타입 / Easy : 3, Normal : 4, Hard : 6, Boss
public enum DungeonType
{
    Easy = 3,
    Normal = 4,
    Hard = 6,
    Boss = 10,
}

// 던전 클래스 - 몬스터 생성, 난이도
public class Dungeon
{
    public DungeonType Type { get; private set; }
    public int Count { get; set; }
    public List<Monster> Monsters { get; private set; }

    public Dungeon()
    {
        Type = DungeonType.Easy;
        Count = (int)DungeonType.Easy;

        Monsters = new List<Monster>();
        SetMonsters();
    }

    // 던전에 몬스터 생성
    private void SetMonsters()
    {
        for (int i = 0; i < Count; i++)
        {
            Monsters.Add(new Monster(((MonsterType)Utils.random.Next(0, 10)).ToString(), i + 1));
        }
    }

    // 던전 난이도 상승 함수
    public void GetStrong()
    {
        switch (Type)
        {
            case DungeonType.Easy:
                Type = DungeonType.Normal;
                Count = (int)DungeonType.Normal;
                SetMonsters();
                break;
            case DungeonType.Normal:
                Type = DungeonType.Hard;
                Count = (int)DungeonType.Hard;
                SetMonsters();
                break;
            case DungeonType.Hard:
                Type = DungeonType.Boss;
                Count = 1;
                Monsters.Add(new Monster("Dragon", 10, 40, 40, 200, 40, 10000));
                break;
        }
    }
}
