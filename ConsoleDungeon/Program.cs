using System;
using System.Collections.Generic;
using System.Text;
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
    public int Gold { get; set; }
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
        Gold = 1000;
        Inventory = new List<Equip>()       // 기본 지급 장비
        {
            new Equip("무쇠 갑옷", EquipType.Armor, "방어력 +1", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 1),
            new Equip("낡은 검", EquipType.Weapon, "공격력 +2", "쉽게 볼 수 있는 낡은 검입니다.", 0, 2),
        };
        OnEquip = new List<Equip>() { null, null, null, null };
    }

    // 장비 이름 반환, 없으면 ""
    public string OnEquipName(int idx)
    {
        if (OnEquip[idx] == null)
            return " ";
        else
            return OnEquip[idx].Name;
    }

    // 장비 획득
    public void GetEquip(Equip _equip)
    {
        Inventory.Add(_equip);
    }

    // 장비 판매
    public void SellEquip(Equip _equip)
    {
        if (_equip.IsEquipped)
            EquipEffect(_equip);

        Inventory.Remove(_equip);
    }

    // 장비 효과 적용 / 해제
    public void EquipEffect(Equip equip)
    {
        int pm = equip.IsEquipped ? -1 : 1;

        if      (equip.Index == 1) { DP += 1 * pm; }
        else if (equip.Index == 2) { AP += 2 * pm; }
    }

    // 경험치 획득
    public void GetExperience(int val)
    {
        exp += val;
    }
}

// 열겨형 장비 타입 = 장신구 / 무기구 / 방어구 / 아이템
public enum EquipType
{
    Accessory = 0,
    Weapon = 1,
    Armor = 2,
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
    public bool IsEquipped { get; private set; }    // 장착 = true / 해제 = false

    public Equip(string _name, EquipType _type, string _effect, string _sub, int _price, int _index)
    {
        Name = _name;
        Type = _type;
        Effect = _effect;
        Sub = _sub;
        Price = _price;
        State = ' ';
        Index = _index;
        IsEquipped = false;
    }

    public void Switch(Player player)
    {
        // 장착
        if (!IsEquipped)
        {
            // 이미 해당 타입의 장비가 장착되어 있을 때 해제 먼저
            if (player.OnEquip[(int)Type] != null)
            {
                player.OnEquip[(int)Type].State = ' ';
                player.OnEquip[(int)Type].IsEquipped = false;
                player.EquipEffect(player.OnEquip[(int)Type]);
                player.OnEquip[(int)Type] = null;
            }

            player.OnEquip[(int)Type] = this;
            State = 'E';
            player.EquipEffect(this);
            IsEquipped = true;
        }
        // 해제
        else
        {
            player.OnEquip[(int)Type] = null;
            State = ' ';
            player.EquipEffect(this);
            IsEquipped = false;
        }
    }
}

public class Shop
{
    public List<Equip> AllEquips { get; private set; }
    public List<Equip> ShopList { get; private set; }

    private readonly Player player;

    public Shop(Player _player)
    {
        player = _player;

        AllEquips = new List<Equip>()
        {
            new Equip("수련자의 도복", EquipType.Armor, "방어력 +5", "수련자가 입었던 도복입니다.", 1000, 3),
            new Equip("모래 갑옷", EquipType.Armor, "체력 +10, 스피드 -5", "", 500, 4),

            new Equip("단검", EquipType.Weapon, "공격력 +3", "", 400, 5),
            new Equip("대검", EquipType.Weapon, "공격력 +10, 스피드 -10", "엄청나게 크고 무겁습니다.", 1000, 6),

            new Equip("ㅁㄴㅇㅁㄴㅇㅁㄴㅇㅁ", EquipType.Accessory, "스피드 +10", "", 600, 7),

            new Equip("부적", EquipType.Item, "체력 +10", "", 600, 8),
        };

        ShopList = new List<Equip>();

        SetShop();
    }

    // 무작위의 5개의 상품만 판매
    private void SetShop()
    {
        foreach(Equip equip in ShopList)
        {
            AllEquips.Add(equip);
        }

        ShopList.Clear();

        for (int i = 0; i < 5; i++)
        {
            int idx = Program.random.Next(0, AllEquips.Count);

            ShopList.Add(AllEquips[idx]);
            AllEquips.RemoveAt(idx);
        }
    }

    // 상점에서 장비 구입
    public void Buy(int index)
    {
        player.GetEquip(ShopList[index]);
        ShopList[index] = null;
    }

    // 상점에서 장비 판매
    public void Sell(int index)
    {
        Equip equip = player.Inventory[index];
        player.SellEquip(equip);
        player.Gold += equip.Price / 2;
    }
}

public class GameController
{
    private Player player;
    private Shop shop;

    // 입력 받는 함수 : min <= input <= max 값만 받도록
    private int GetInput(int min, int max)
    {
        Console.WriteLine("\n원하시는 행동을 입력해주세요.");
        Console.Write(">> ");

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (min <= input && input <= max)
                    return input;
            }

            Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
            Console.Write(">> ");
        }
    }

    public void GameStart()
    {
        Program.Talk("당신의 이름을 알려주세요.\n");
        Console.Write(">> ");
        string warriorName = Console.ReadLine();

        Program.Talk("\n당신의 직업을 골라주세요.\n");
        Program.ColorWriteLine("1. 검사");
        Program.ColorWriteLine("2. 전사");
        Program.ColorWriteLine("3. 도적");

        int jobChoice = GetInput(1, 3);

        Player _player;
        if (jobChoice == 1) { _player = new Player(warriorName, "검사", 10, 5, 100, 20); }
        else if (jobChoice == 2) { _player = new Player(warriorName, "전사", 10, 7, 120, 10); }
        else { _player = new Player(warriorName, "도적", 15, 3, 80, 30); }

        player = _player;
        shop = new Shop(player);        
    }

    public void VillageEnterance()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Program.Talk($"스파르타 마을에 어서오세요. {player.Name} 님");
            Program.Talk("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            Program.ColorWriteLine("1. 상태 보기");
            Program.ColorWriteLine("2. 인벤토리");
            Program.ColorWriteLine("3. 상점 - 구매");
            Program.ColorWriteLine("4. 상점 - 판매");
            Program.ColorWriteLine("5. 던전 입장");
            Program.ColorWriteLine("\n0. 게임 종료");

            int input = GetInput(0, 4);

            if (input == 1) ShowStatus();
            else if (input == 2) ShowInventory();
            else if (input == 3) ShowShopForBuy();
            else if (input == 4) ShowShopForSell();
            else return;
        }
    }

    // 플레이어 스탯 확인
    private void ShowStatus()
    {
        Console.Clear();
        Console.WriteLine($"◇----------◇----------◇----------");
        Console.WriteLine($"| LV. {player.Level:D2} \t {player.Name}\n");
        Console.WriteLine("|");
        Console.WriteLine($"| 직  업 : {player.Job}");
        Console.WriteLine($"| 공격력 : {player.AP}");
        Console.WriteLine($"| 방어력 : {player.DP}");
        Console.WriteLine($"| 체  력 : {player.HP}");
        Console.WriteLine($"| 스피드 : {player.Speed}");
        Console.WriteLine($"|  GOLD  : {player.Gold} G");
        Console.WriteLine($"◇----------◇----------◇----------");
        Console.WriteLine($"|     O     ← 장신구 : {player.OnEquipName(0)}");
        Console.WriteLine($"|   - | -   ← 무기구 : {player.OnEquipName(1)}");
        Console.WriteLine($"|     |     ← 방어구 : {player.OnEquipName(2)}");
        Console.WriteLine($"|    | |    ← 아이템 : {player.OnEquipName(3)}");
        Console.WriteLine($"◇----------◇----------◇----------");

        Program.ColorWriteLine("\n0. 나가기");

        _ = GetInput(0, 0);
    }

    // 인벤토리 - 장비 장착 / 해제 
    private void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("[아이템 목록]\n");

        for (int i = 0; i < 9; i++)
        {
            if (i < player.Inventory.Count)
            {
                Equip equip = player.Inventory[i];
                Console.WriteLine(string.Format("({0}) [{1}] {2}| {3}| {4}| {5}| {6} G",
                                                                                                i + 1,
                                                                                                equip.State,
                                                                                                equip.Name + "".PadRight(20 - Encoding.Default.GetBytes(equip.Name).Length),
                                                                                                equip.Type.ToString().PadRight(10),
                                                                                                equip.Effect + "".PadRight(25 - Encoding.Default.GetBytes(equip.Effect).Length),
                                                                                                equip.Sub + "".PadRight(35 - Encoding.Default.GetBytes(equip.Sub).Length),
                                                                                                equip.Price));
            }
            else
            {
                Console.WriteLine(string.Format("({0})", i + 1));
            }
        }

        Program.ColorWriteLine("\n1 ~ 9. 해당 장비 장착/해제");
        Program.ColorWriteLine("0. 나가기");

        int input = GetInput(0, player.Inventory.Count);

        if (0 < input && input <= player.Inventory.Count)
        {
            player.Inventory[input - 1].Switch(player);
            ShowInventory();
        }
    }

    // 상점 - 장비 구매
    private void ShowShopForBuy()
    {
        Console.Clear();
        Program.Talk("필요한 장비를 얻을 수 있는 상점입니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{player.Gold} G\n");

        Console.WriteLine("[장비 목록]");

        for (int i = 0; i < shop.ShopList.Count; i++)
        {
            Equip equip = shop.ShopList[i];
            
            if (equip != null)
            {
                Console.WriteLine(string.Format("({0}) {1}| {2}| {3}| {4}| {5} G",
                                                                                                i + 1,
                                                                                                equip.Name + "".PadRight(20 - Encoding.Default.GetBytes(equip.Name).Length),
                                                                                                equip.Type.ToString().PadRight(10),
                                                                                                equip.Effect + "".PadRight(25 - Encoding.Default.GetBytes(equip.Effect).Length),
                                                                                                equip.Sub + "".PadRight(35 - Encoding.Default.GetBytes(equip.Sub).Length),
                                                                                                equip.Price));
            }
            else
            {
                Console.WriteLine($"({i + 1}) - 판매 완료 -");
            }
        }

        Program.ColorWriteLine("\n1 ~ 5. 해당 장비 구매");
        Program.ColorWriteLine("0. 나가기");

        int input = GetInput(0, 5);

        // 예외 처리
        while (input != 0)
        {
            // 이미 판매된 장비였을 때
            if (shop.ShopList[input - 1] == null)
            {
                Console.Write("\n이미 판매된 장비입니다. 다시 입력해주세요.");
                input = GetInput(0, 5);
            }
            // 돈이 부족할 때
            else if (player.Gold < shop.ShopList[input - 1].Price)
            {
                Console.Write("\n골드가 부족합니다. 다른 장비를 구매하세요.");
                input = GetInput(0, 5);
            }
            // 인벤토리가 가득 찼을 때
            else if (player.Inventory.Count >= 9)
            {
                Console.Write("\n인벤토리가 가득 찼습니다. 구매하시려면 먼저 장비를 판매해주세요.");
                input = GetInput(0, 5);
            }
            else
                break;
        }

        // 그 외 상점에서 구매가 가능한 경우
        if (1 <= input && input <= 5)
        {
            shop.Buy(input - 1);
            ShowShopForBuy();
        }
    }

    // 상점 - 장비 판매
    private void ShowShopForSell()
    {
        Console.Clear();
        Program.Talk("필요없는 장비를 팔아보세요.");
        Program.Talk("원래 가격의 50%를 돌려드립니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{player.Gold} G\n");

        Console.WriteLine("[인벤토리 목록]");

        for (int i = 0; i < 9; i++)
        {
            if (i < player.Inventory.Count)
            {
                Equip equip = player.Inventory[i];
                Console.WriteLine(string.Format("({0}) {1}| {2}| {3}| {4}| {5} G",
                                                                                                i + 1,
                                                                                                equip.Name + "".PadRight(20 - Encoding.Default.GetBytes(equip.Name).Length),
                                                                                                equip.Type.ToString().PadRight(10),
                                                                                                equip.Effect + "".PadRight(25 - Encoding.Default.GetBytes(equip.Effect).Length),
                                                                                                equip.Sub + "".PadRight(35 - Encoding.Default.GetBytes(equip.Sub).Length),
                                                                                                equip.Price));
            }
            else
            {
                Console.WriteLine(string.Format("({0})", i + 1));
            }
        }

        Program.ColorWriteLine("\n1 ~ 9. 해당 장비 판매");
        Program.ColorWriteLine("0. 나가기");

        int input = GetInput(0, 9);

        if (input == 0)
            return;

        // 예외 처리 - 비어 있는 곳일 때
        while (input > player.Inventory.Count)
        {
            Console.Write("\n해당 인벤토리는 비어 있습니다. 다시 선택해주세요.");
            input = GetInput(0, 9);
        }

        shop.Sell(input - 1);
        ShowShopForSell();
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
            Thread.Sleep(1); // 25
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

    public static Random random = new Random();

    static void Main()
    {
        GameController GC = new GameController();
        GC.GameStart();
        GC.VillageEnterance();
    }
}