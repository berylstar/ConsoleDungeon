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

    bool DefenseStance { get; set; }
    int Focus { get; set; }

    void GetDamaged(int damage);
    bool IsDefense();
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
    public List<Equip> OnEquip { get; private set; }        // 장착되어 있는 장비

    private int exp = 0;        // 레벨 업을 위한 경험치

    public bool DefenseStance { get; set; }     // 방어 태세
    public int Focus { get; set; }              // 집중 카운트

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
        DefenseStance = false;
        Focus = 0;
    }

    // 인벤토리의 장비 이름 반환, 없으면 ""
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
        // 장착되어 있으면 효과 해제 이후 판매
        if (_equip.IsEquipped)
            EquipEffect(_equip);

        Inventory.Remove(_equip);
    }

    // 장비 효과 적용 / 해제
    public void EquipEffect(Equip equip)
    {
        int pm = equip.IsEquipped ? -1 : 1;

        if (equip.Index == 1) { DP += 1 * pm; }
        else if (equip.Index == 2) { AP += 2 * pm; }
        else if (equip.Index == 3) { DP += 7 * pm; }
        else if (equip.Index == 4) { AP -= 2 * pm; DP += 15 * pm; }
        else if (equip.Index == 5) { DP += 5 * pm; HP += 10 * pm; }
        else if (equip.Index == 6) { DP += 7 * pm; Speed -= 10 * pm; }
        else if (equip.Index == 7) { DP += 3 * pm; }
        else if (equip.Index == 8) { AP += 5 * pm; Speed -= 10 * pm; }
        else if (equip.Index == 9) { AP += 2 * pm; Speed += 10 * pm; }
        else if (equip.Index == 10) { AP += 7 * pm; HP -= 15 * pm; }
        else if (equip.Index == 11) { AP += 10 * pm; }
        else if (equip.Index == 12) { Speed += 20 * pm; }
        else if (equip.Index == 13) { DP += 5 * pm; }
        else if (equip.Index == 14) { HP += 5 * pm; }
        else if (equip.Index == 15) { AP += 10 * pm; DP -= 20 * pm; }
        else if (equip.Index == 16) { Speed += 10 * pm; }
        else if (equip.Index == 17) { AP -= 2 * pm; HP += 20 * pm; }
        else if (equip.Index == 18) { DP += 10 * pm; HP -= 10 * pm; }
        else if (equip.Index == 19) { AP += 3 * pm; }
        else if (equip.Index == 20) { AP += 1 * pm; Speed += 10 * pm; }
        else if (equip.Index == 21) { HP += 30 * pm; }
        else if (equip.Index == 22) { AP += 5 * pm; Speed -= 10 * pm; }
    }

    // 경험치 획득 + 레벨 업 확인
    public void GetExperience(int val)
    {
        exp += val;

        while (Level <= exp)
        {
            exp -= Level;

            Utils.Talk("\n ※ 레벨 업 ! ※");
            Utils.Talk($" ※공격력 + {(int)Math.Ceiling(Level / 3f)}, 체력 + 10 ※");

            AP += (int)Math.Ceiling(Level / 3f);
            HP += 10;

            Level += 1;
        }
    }

    // 피해 입었을 때
    public void GetDamaged(int damage)
    {
        HP -= damage;
    }

    // 방어 여부 판단
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

// 열겨형 장비 타입 = 장신구 / 무기구 / 방어구 / 아이템
public enum EquipType
{
    Accessory = 0,
    Weapon = 1,
    Armor = 2,
    Item = 3,
}

// 장비 클래스
public class Equip
{
    public string Name { get; private set; }
    public EquipType Type { get; private set; }
    public string Effect { get; private set; }
    public string Sub { get; private set; }
    public char State { get; private set; }
    public int Price { get; private set; }
    public int Index { get; private set; }          // 효과 적용을 위한 장비 시리얼 넘버
    public bool IsEquipped { get; private set; }    // 장착 확인 변수

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

    // 장비 장착 / 해제
    public void Switch(Player player)
    {
        // 장비 장착
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

            // 해제 된 이후 현재 장비 장착
            player.OnEquip[(int)Type] = this;
            State = 'E';
            player.EquipEffect(this);
            IsEquipped = true;
        }
        // 장비 해제
        else
        {
            player.OnEquip[(int)Type] = null;
            State = ' ';
            player.EquipEffect(this);
            IsEquipped = false;
        }
    }
}

// 상점 클래스 - 장비 사고 팔기
public class Shop
{
    public List<Equip> AllEquips { get; private set; }
    public List<Equip> ShopList { get; private set; }

    private readonly Player player;

    public Shop(Player _player)
    {
        player = _player;

        // 전체 장비 초기화
        AllEquips = new List<Equip>()
        {
            new Equip("수련자의 도복", EquipType.Armor, "방어력 +7", "수련 집중에 효과적입니다.", 2500, 3),
            new Equip("청동 갑옷", EquipType.Armor, "방어력 +15, 공격력 -2", "확실히 무거운 갑옷입니다.", 1000, 4),
            new Equip("호랑이 가죽", EquipType.Armor, "방어력 +5, 체력 +10", "호랑이는 죽어서 가죽을 남기고", 1500, 5),
            new Equip("검투사의 보호대", EquipType.Armor, "방어력 +7, 스피드 -10", "검투사 전용 보호구입니다.", 1800, 6),
            new Equip("천 갑옷", EquipType.Armor, "방어력 +3", "가성비 좋은 갑옷입니다.", 500, 7),

            new Equip("커다란 대검", EquipType.Weapon, "공격력 +5, 스피드 -10", "엄청나게 크고 무겁습니다.", 1000, 8),
            new Equip("대나무 창", EquipType.Weapon, "공격력 +2, 스피드 +10", "가볍고 날카롭습니다.", 1500, 9),
            new Equip("양날의 검", EquipType.Weapon, "공격력 +7, 체력 -15", "말 그대로 \"양날의 검\"", 800, 10),
            new Equip("검은 검", EquipType.Weapon, "공격력 +10", "sword = sword", 3000, 11),
            new Equip("단검", EquipType.Weapon, "스피드 +20", "작지만 누구보다 빠르다.", 1200, 12),

            new Equip("빛나는 왕관", EquipType.Accessory, "방어력 +5", "아직은 반짝반짝합니다.", 800, 13),
            new Equip("낡은 투구", EquipType.Accessory, "체력 +5", "누군가가 사용했던 것 같습니다.", 0, 14),
            new Equip("붉은 목걸이", EquipType.Accessory, "공격력 +10, 방어력 -20", "쓴 사람은 저주를 받습니다.", 1200, 15),
            new Equip("낡은 후드", EquipType.Accessory, "스피드 +10", "얼굴을 가리는데 효과적입니다.", 500, 16),
            new Equip("검은 반지", EquipType.Accessory, "체력 +20, 공격력 -2", "쇠약해지지만, 살아남을 겁니다.", 800, 17),

            new Equip("부적", EquipType.Item, "방어력 +10, 체력 -10", "용한 부적입니다.", 1500, 18),
            new Equip("맹독병", EquipType.Item, "공격력 +3", "무기에 발라보세요.", 1200, 19),
            new Equip("보급형 아이템", EquipType.Item, "공격력 +1, 스피드 +5", "초보 모험가를 위한 아이템.", 0, 20),
            new Equip("비상약", EquipType.Item, "체력 +30", "비상시를 대비합니다.", 1500, 21),
            new Equip("모래 주머니", EquipType.Item, "공격력 +5, 스피드 -10", "모래주머니 수련법", 800, 22),
        };

        ShopList = new List<Equip>();

        SetShop();
    }

    // 상점에서는 무작위의 5개의 상품만 판매
    public void SetShop()
    {
        foreach (Equip equip in ShopList)
        {
            AllEquips.Add(equip);
        }

        ShopList.Clear();

        for (int i = 0; i < 5; i++)
        {
            int idx = Utils.random.Next(0, AllEquips.Count);

            ShopList.Add(AllEquips[idx]);
            AllEquips.RemoveAt(idx);
        }
    }

    // 상점에서 장비 구입
    public void Buy(int index)
    {
        player.Gold -= ShopList[index].Price;
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

// 게임 컨트롤러 클래스 - 전체 게임 관리
public class GameController
{
    private Player player;
    private Shop shop;
    private Dungeon dungeon;
    private Monster mon;

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

    // 게임 시작 인트로 : 이름 & 직업 설정
    public void GameStart()
    {
        Utils.Talk("당신의 이름을 알려주세요.\n");
        Console.Write(">> ");
        string warriorName = Console.ReadLine();

        Utils.Talk("\n당신의 직업을 골라주세요.\n");
        Utils.ColorWriteLine("1. 검사 : 밸런스형");
        Utils.ColorWriteLine("2. 전사 : 방어형");
        Utils.ColorWriteLine("3. 도적 : 스피드형");

        int jobChoice = GetInput(1, 3);

        Player _player;
        if (jobChoice == 1) { _player = new Player(warriorName, "검사", 10, 5, 100, 20); }
        else if (jobChoice == 2) { _player = new Player(warriorName, "전사", 10, 10, 120, 10); }
        else { _player = new Player(warriorName, "도적", 12, 0, 80, 30); }

        player = _player;
        shop = new Shop(player);

        dungeon = new Dungeon();
    }

    // 마을
    public void VillageEnterance()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Utils.Talk($"스파르타 마을에 어서오세요. {player.Name} 님");
            Utils.Talk("이곳에서 콘솔 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            Utils.ColorWriteLine("1. 상태 보기");
            Utils.ColorWriteLine("2. 인벤토리");
            Utils.ColorWriteLine("3. 상점 - 구매");
            Utils.ColorWriteLine("4. 상점 - 판매");
            Utils.ColorWriteLine($"\n5. 던전 입장 - {dungeon.Type}", ConsoleColor.Red);
            Utils.ColorWriteLine("\n0. 게임 종료");

            int input = GetInput(0, 5);

            if (input == 1) ShowStatus();
            else if (input == 2) ShowInventory();
            else if (input == 3) ShowShopForBuy();
            else if (input == 4) ShowShopForSell();
            else if (input == 5) { InDungeon(); }
            else return;
        }
    }

    // 플레이어 스탯 확인
    private void ShowStatus()
    {
        Console.Clear();
        Console.WriteLine($"◇----------◇----------◇----------");
        Console.WriteLine($"| LV. {player.Level:D2} \t {player.Name}");
        Console.WriteLine("|");
        Console.WriteLine($"| 직  업 : {player.Job}");
        Console.WriteLine($"| 체  력 : {player.HP}");
        Console.WriteLine($"| 공격력 : {player.AP}");
        Console.WriteLine($"| 방어력 : {player.DP}");
        Console.WriteLine($"| 스피드 : {player.Speed}");
        Console.WriteLine($"|  GOLD  : {player.Gold} G");
        Console.WriteLine($"◇----------◇----------◇----------");
        Console.WriteLine($"|     O     ← 장신구 : {player.OnEquipName(0)}");
        Console.WriteLine($"|   - | -   ← 무기구 : {player.OnEquipName(1)}");
        Console.WriteLine($"|     |     ← 방어구 : {player.OnEquipName(2)}");
        Console.WriteLine($"|    | |    ← 아이템 : {player.OnEquipName(3)}");
        Console.WriteLine($"◇----------◇----------◇----------");

        Utils.ColorWriteLine("\n0. 나가기");

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

        Utils.ColorWriteLine("\n1 ~ 9. 해당 장비 장착/해제");
        Utils.ColorWriteLine("0. 나가기");

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
        Utils.Talk("필요한 장비를 얻을 수 있는 상점입니다.\n");
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

        Utils.ColorWriteLine("\n1 ~ 5. 해당 장비 구매");
        Utils.ColorWriteLine("0. 나가기");

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
        Utils.Talk("필요없는 장비를 팔아보세요.");
        Utils.Talk("원래 가격의 50%를 돌려드립니다.\n");
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

        Utils.ColorWriteLine("\n1 ~ 9. 해당 장비 판매");
        Utils.ColorWriteLine("0. 나가기");

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

    // 던전 입장 - 전투 전 던전 로비
    private void InDungeon()
    {
        while (dungeon.Count > 0)
        {
            Console.Clear();
            Console.WriteLine("===============================");
            Utils.Talk($"{dungeon.Type} 난이도의 던전입니다.");
            Utils.Talk($"앞으로 {dungeon.Count}마리의 몬스터를 처치해야 합니다.\n");

            Utils.ColorWriteLine("1. 상태 보기");
            Utils.ColorWriteLine("2. 인벤토리");
            Utils.ColorWriteLine("\n3. 이어서 진행하기", ConsoleColor.Red);

            int input = GetInput(1, 3);

            if (input == 1) ShowStatus();
            else if (input == 2) ShowInventory();
            else ShowBattle();
        }

        // 마지막 보스를 잡으면 게임 승리
        if (dungeon.Type == DungeonType.Boss)
        {
            GameWin();
        }
        // 던전 난이도 증가, 상점 새로고침
        else
        {
            dungeon.GetStrong();
            shop.SetShop();
        }
    }

    // 몬스터와 전투
    private void ShowBattle()
    {
        mon = dungeon.Monsters[0];
        Program.script.Enqueue($"{mon.Name}이 나타났습니다 !");

        while (mon.HP > 0)
        {
            Console.Clear();
            Utils.CursorWrite(35, 0, $"◇----------◇----------◇----------");
            Utils.CursorWrite(35, 1, $"| LV. {mon.Level:D2} \t {mon.Name}");
            Utils.CursorWrite(35, 2, "|");
            Utils.CursorWrite(35, 3, $"| 체  력 : {mon.HP}");
            Utils.CursorWrite(35, 4, $"| 공격력 : {mon.AP}");
            Utils.CursorWrite(35, 5, $"| 방어력 : {mon.DP}");
            Utils.CursorWrite(35, 6, $"| 스피드 : {mon.Speed}");
            Utils.CursorWrite(35, 7, $"| 집  중 : {mon.Focus}");
            Utils.CursorWrite(35, 8, $"◇----------◇----------◇----------");

            Console.WriteLine($"◇----------◇----------◇----------");
            Console.WriteLine($"| LV. {player.Level:D2} \t {player.Name}");
            Console.WriteLine("|");
            Console.WriteLine($"| 체  력 : {player.HP}");
            Console.WriteLine($"| 공격력 : {player.AP}");
            Console.WriteLine($"| 방어력 : {player.DP}");
            Console.WriteLine($"| 스피드 : {player.Speed}");
            Console.WriteLine($"| 스피드 : {player.Speed}");
            Console.WriteLine($"| 집  중 : {player.Focus}");
            Console.WriteLine($"◇----------◇----------◇----------\n");

            while (Program.script.Count > 0)
            {
                Utils.Talk(Program.script.Dequeue());
            }

            Utils.ColorWriteLine("\n1. 공격");
            Utils.ColorWriteLine("2. 방어");
            Utils.ColorWriteLine("3. 집중");

            int input = GetInput(1, 3);

            if (mon.Speed > player.Speed)
            {
                CharacterAction(Utils.random.Next(0, 6) % 3 + 1, mon, player);
                Console.WriteLine();
                CharacterAction(input, player, mon);
            }
            else
            {
                CharacterAction(input, player, mon);
                Console.WriteLine();
                CharacterAction(Utils.random.Next(0, 6) % 3 + 1, mon, player);
            }

            if (player.HP < 0)
            {
                GameFail();
            }
        }

        Utils.Talk($"{mon.Name}은 쓰러졌습니다.");

        Utils.Talk($"{mon.Gold} G 획득 !");
        player.Gold += mon.Gold;

        Utils.Talk($"{mon.Level}의 경험치 획득 !");
        player.GetExperience(mon.Level);

        Utils.ColorWriteLine("\n0. 돌아가기");

        _ = GetInput(0, 0);

        Program.script.Clear();
        dungeon.Monsters.RemoveAt(0);
        dungeon.Count -= 1;

    }

    // 전투 중 ICharacter의 행동
    private void CharacterAction(int input, ICharacter main, ICharacter target)
    {
        if (main.HP <= 0)
            return;

        if (input == 1)
        {
            main.DefenseStance = false;

            if (target.IsDefense())
            {
                Program.script.Enqueue($"{main.Name}의 공격 !");
                Program.script.Enqueue($"하지만, {target.Name}은 {main.Name}의 공격을 막아냈다 !");
            }
            else
            {
                target.GetDamaged(main.AP * (2 * main.Focus + 1));
                Program.script.Enqueue($"{main.Name}의 공격 ! {target.Name}에게 {main.AP * (2 * main.Focus + 1)}의 데미지");
            }

            main.Focus = 0;
        }
        else if (input == 2)
        {
            if (main.DefenseStance)
            {
                main.DefenseStance = false;
                Program.script.Enqueue($"{main.Name}은 힘들어, 연속으로 방어 태세를 갖추지 못했다...");
            }
            else
            {
                main.DefenseStance = true;
                Program.script.Enqueue($"{main.Name}은 방어 태세에 들어갔다.");
            }

        }
        else if (input == 3)
        {
            main.DefenseStance = false;
            main.Focus += 1;
            Program.script.Enqueue($"{main.Name}은 집중하고 있다...");
        }
    }

    // 게임 종료 - 승리
    public void GameWin()
    {
        Console.Clear();
        Console.WriteLine("====================================");
        Utils.Talk($"축하합니다. {player.Name} 님");
        Utils.Talk("던전을 정복했습니다 !");

        Utils.ColorWriteLine("\n0. 게임 종료");

        int input = GetInput(0, 0);

        if (input == 0) Environment.Exit(0);
    }

    // 게임 종료 - 패배
    public void GameFail()
    {
        Console.Clear();
        Utils.Talk($"{player.Name}은 쓰러졌습니다...");
        Utils.Talk($"다시 도전해보세요 !");

        Utils.ColorWriteLine("\n0. 게임 종료");

        int input = GetInput(0, 0);

        if (input == 0) Environment.Exit(0);
    }
}

internal class Program
{
    // 전투 메세지를 담고 있는 큐
    public static Queue<string> script = new Queue<string>();

    static void Main()
    {
        GameController GC = new GameController();
        GC.GameStart();
        GC.VillageEnterance();
    }
}