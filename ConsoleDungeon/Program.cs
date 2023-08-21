﻿using System;
using System.Collections.Generic;
using System.Threading;

// 캐릭터 인터페이스 : 플레이어, 몬스터들의 기본 스탯 = 이름 / 레벨 / 공격력 / 방어력 / 체력 / 스피드
public interface ICharacter
{
    string Name { get; }
    int Level { get; }
    int AP { get; }
    int DP { get; }
    int HP { get; }
    int Speed { get; }
}

public class Player : ICharacter
{
    public string Name { get; private set; }
    public string Job { get; private set; }
    public int Level { get; private set; }
    public int AP { get; private set; }
    public int DP { get; private set; }
    public int HP { get; private set; }
    public int Speed { get; private set; }
    public int Gold { get; private set; }
    public List<Equip> Inventory { get; private set; }      // 가지고 있는 장비

    private int exp = 0;        // 레벨 업을 위한 경험치
    public List<Equip> OnEquip { get; private set; }        // 장착되어 있는 장비

    public Player(string _name, string _job, int _ap, int _dp, int _hp, int _speed)
    {
        Name = _name;
        Job = _job;
        Level = 1;
        AP = _ap;
        DP = _dp;
        HP = _hp;
        Speed = _speed;
        Gold = 0;
        Inventory = new List<Equip>()       // 기본 지급 장비
        {
            new Equip("무쇠 갑옷", EquipType.Armor, "방어력 +1", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 1),
            new Equip("낡은 검", EquipType.Weapon, "공격력 +2", "쉽게 볼 수 있는 낡은 검입니다.", 0, 2),
        };
        OnEquip = new List<Equip>() { null, null, null, null };
    }

    public string OnEquipName(int idx)
    {
        if (OnEquip[idx] == null)
            return " ";
        else
            return OnEquip[idx].Name;
    }

    // 장비 효과 적용 / 해제
    public void EquipEffect(int index, bool onoff)
    {
        int pm = onoff == true ? 1 : -1;

        if      (index == 1) { DP += 1 * pm; }
        else if (index == 2) { AP += 2 * pm; }
        else if (index == 3) { AP += 7 * pm; }
    }

    public void GetExperience(int val)
    {
        exp += val;
    }
}

// 열겨형 장비 타입 = 방어구 / 무기구 / 장신구 / 아이템
public enum EquipType
{
    Armor = 0,
    Weapon = 1,
    Accessory = 2,
    Item = 3,
}

public class Equip
{
    public string Name { get; private set; }
    public EquipType Type { get; private set; }
    public string Effect { get; private set; }
    public string Sub { get; private set; }
    public char State { get; private set; }
    public int Price { get; private set; }
    public int Index { get; private set; }

    private bool isEquipped = false;    // 장착 = true / 해제 = false

    public Equip(string _name, EquipType _type, string _effect, string _sub, int _price, int _index)
    {
        Name = _name;
        Type = _type;
        Effect = _effect;
        Sub = _sub;
        Price = _price;
        State = ' ';
        Index = _index;
    }

    public void Switch(Player player)
    {
        // 장착
        if (!isEquipped)
        {
            // 이미 해당 타입의 장비가 장착되어 있을 때 해제 먼저
            if (player.OnEquip[(int)Type] != null)
            {
                player.OnEquip[(int)Type].State = ' ';
                player.OnEquip[(int)Type].isEquipped = false;
                player.EquipEffect(player.OnEquip[(int)Type].Index, false);
                player.OnEquip[(int)Type] = null;
            }

            player.OnEquip[(int)Type] = this;
            State = 'E';
            player.EquipEffect(Index, true);
        }
        // 해제
        else
        {
            player.OnEquip[(int)Type] = null;
            State = ' ';
            player.EquipEffect(Index, false);
        }

        isEquipped = !isEquipped;
    }
}

public class Shop
{
    public List<Equip> ShopList { get; set; }

    public Shop()
    {
        ShopList = new List<Equip>()
        {
            new Equip("수련자의 도복", EquipType.Armor, "방어력 +5", "수련자가 입었던 도복입니다.", 1000, 4),
            new Equip("모래 갑옷", EquipType.Armor, "체력 +10, 스피드 -5", "", 500, 5),

            new Equip("단검", EquipType.Weapon, "공격력 +3", "", 400, 3),
            new Equip("대검", EquipType.Weapon, "공격력 +10, 스피드-10", "엄청나게 크고 무겁습니다.", 1000, 3),

            new Equip("", EquipType.Accessory, "스피드 +10", "", 600, 6),

            new Equip("부적", EquipType.Item, "체력 +10", "", 600, 6),
        };
    }
}

public class GameController
{
    public Player Player { get; private set; }

    public void SetPlayer(Player _player) { Player = _player; }

    // 입력 받는 함수 : min <= input <= max 값만 받도록
    public int GetInput(int min, int max)
    {
        Console.WriteLine("\n원하시는 행동을 입력해주세요.");
        Console.Write(">> ");

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if ((min <= input && input <= max))
                    return input;
            }

            Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
            Console.Write(">> ");
        }
    }

    public void VillageEnterance()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Program.Talk($"스파르타 마을에 어서오세요. {Player.Name} 님");
            Program.Talk("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            Program.ColorWriteLine("1. 상태 보기");
            Program.ColorWriteLine("2. 인벤토리");
            Program.ColorWriteLine("3. 상점");
            Program.ColorWriteLine("4. 던전 입장");
            Program.ColorWriteLine("\n0. 게임 종료");

            int input = GetInput(0, 3);

            if (input == 1) ShowStatus();
            else if (input == 2) ShowInventory();
            else if (input == 3) ShowShop();
            else return;
        }
    }

    // 플레이어 스탯 확인
    public void ShowStatus()
    {
        Console.Clear();
        Console.WriteLine($"◇----------◇----------◇----------");
        Console.WriteLine($"| LV. {Player.Level:D2}");
        Console.WriteLine($"| {Player.Name}");
        Console.WriteLine($"| 직  업 : {Player.Job}");
        Console.WriteLine($"| 공격력 : {Player.AP}");
        Console.WriteLine($"| 방어력 : {Player.DP}");
        Console.WriteLine($"| 체  력 : {Player.HP}");
        Console.WriteLine($"| 스피드 : {Player.Speed}");
        Console.WriteLine($"|  GOLD  : {Player.Gold} G");
        Console.WriteLine($"◇----------◇----------◇----------");
        Console.WriteLine($"|     O     ← 장신구 : {Player.OnEquipName(0)}");
        Console.WriteLine($"|   - | -   ← 무기구 : {Player.OnEquipName(1)}");
        Console.WriteLine($"|     |     ← 방어구 : {Player.OnEquipName(2)}");
        Console.WriteLine($"|    | |    ← 아이템 : {Player.OnEquipName(3)}");
        Console.WriteLine($"◇----------◇----------◇----------");

        Program.ColorWriteLine("\n0. 나가기");

        _ = GetInput(0, 0);
    }

    // 인벤토리 - 장비 장착 / 해제 
    public void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("[아이템 목록]\n");

        for (int i = 0; i < 9; i++)
        {
            if (i < Player.Inventory.Count)
            {
                Equip equip = Player.Inventory[i];
                Console.WriteLine(string.Format("- ({0}) [{1}] {2} \t | {3} \t | {4} \t\t | {5}", i + 1, equip.State, equip.Name.PadRight(10), equip.Type, equip.Effect, equip.Sub));
            }
            else
            {
                Console.WriteLine(string.Format("- ({0})", i + 1));
            }
        }

        Program.ColorWriteLine("\n1 ~ 9. 해당 장비 장착/해제");
        Program.ColorWriteLine("0. 나가기");

        int input = GetInput(0, Player.Inventory.Count);

        if (0 < input && input <= Player.Inventory.Count)
        {
            Player.Inventory[input - 1].Switch(Player);
            ShowInventory();
        }
    }

    // 상점
    public void ShowShop()
    {
        Console.Clear();
        Program.Talk("필요한 아이템을 얻을 수 있는 상점입니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{Player.Gold} G\n");

        Console.WriteLine("[아이템 목록]");

    }
}

internal class Program
{
    // 문자열을 한 글자씩 출력하는 함수
    public static void Talk(string _str)
    {
        for (int i = 0; i < _str.Length; i++)
        {
            Console.Write(_str[i]);
            Thread.Sleep(25);
        }
        Console.WriteLine();
    }

    // 문자열의 배경색과 글자색을 변경시켜 출력하는 함수
    public static void ColorWriteLine(string _str, ConsoleColor _writeColor = ConsoleColor.Yellow, ConsoleColor _backColor = ConsoleColor.Black)
    {
        Console.BackgroundColor = _backColor;
        Console.ForegroundColor = _writeColor;
        Console.WriteLine(_str);
        Console.ResetColor();
    }

    static void Main()
    {
        GameController GC = new GameController();

        Talk("당신의 이름을 알려주세요.\n");
        Console.Write(">> ");
        string warriorName = Console.ReadLine();

        Talk("\n당신의 직업을 골라주세요.\n");
        ColorWriteLine("1. 검사");
        ColorWriteLine("2. 전사");
        ColorWriteLine("3. 도적");

        int jobChoice = GC.GetInput(1, 3);

        Player player;

        if      (jobChoice == 1) { player = new Player(warriorName, "검사", 10, 5, 100, 20); }
        else if (jobChoice == 2) { player = new Player(warriorName, "전사", 10, 7, 120, 10); }
        else                     { player = new Player(warriorName, "도적", 15, 3, 80, 30); }

        GC.SetPlayer(player);

        GC.VillageEnterance();
    }
}