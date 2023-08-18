using System;
using System.Collections.Generic;


public class Player
{
    private string name;
    private string job;
    private int level;
    private int ap;
    private int dp;
    private int hp;
    private int speed;
    private int gold;
    private List<Equip> inventory;

    public string Name => name;
    public string Job => job;
    public int Level => level;
    public int AP => ap;
    public int DP => dp;
    public int HP => hp;
    public int Speed => speed;
    public int Gold => gold;
    public List<Equip> Inventory => inventory;

    public Player(string _name, string _job, int _ap, int _dp, int _hp, int _speed)
    {
        name = _name;
        job = _job;
        level = 1;
        ap = _ap;
        dp = _dp;
        hp = _hp;
        speed = _speed;
        gold = 0;
        inventory = new List<Equip>() { new Equip("무쇠 갑옷", "방어력 +5", "무쇠로 만든 갑옷"), new Equip("낡은 검", "공격력 +2", "기본 중에 기본") };
    }
}

public class Equip
{
    private string name;
    private string effect;
    private string sub;
    public char state;

    public string Name => name;
    public string Effect => effect;
    public string Sub => sub;
    public char State => state;

    public Equip(string _name, string _effect, string _sub)
    {
        name = _name;
        effect = _effect;
        sub = _sub;
        state = ' ';
    }
}


public class GameController
{
    // 입력 받는 함수 : min <= input <= max 값만 받도록
    private int GetInput(int min, int max)
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
        Console.WriteLine("====================================");
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");

        int input = GetInput(1, 2);

        Console.WriteLine(input);
    }

    public void ShowStatus(Player player)
    {
        Console.WriteLine($"LV. {player.Level}");
        Console.WriteLine($"{player.Name} ({player.Job})");
        Console.WriteLine($"공격력 : {player.AP}");
        Console.WriteLine($"방어력 : {player.DP}");
        Console.WriteLine($"체  력 : {player.HP}");
        Console.WriteLine($"GOLD : {player.Gold} G");

        Console.WriteLine("\n0. 나가기");

        int input = GetInput(0, 0);
        Console.WriteLine(input);
    }

    public void ShowInventory(Player player)
    {
        Console.WriteLine("[아이템 목록]\n");

        foreach (Equip equip in player.Inventory)
        {
            Console.WriteLine($"- [{equip.State}] {equip.Name}\t\t| {equip.Effect} | \t\t{equip.Sub}");
        }

        Console.WriteLine("\n1. 장착 관리");
        Console.WriteLine("\n0. 나가기");
    }
}

internal class Program
{
    static void Main()
    {
        GameController GC = new GameController();
        // GC.VillageEnterance();

        Player player = new Player("name", "전사", 10, 10, 100, 10);
        GC.ShowStatus(player);
        // GC.ShowInventory(player);
    }
}