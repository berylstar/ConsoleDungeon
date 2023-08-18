using System;
using System.Collections.Generic;
using System.Threading;

public interface ICharacter
{
    string Name { get; }
    int Health { get; set; }
    int Attack { get; set; }
    bool IsDead { get; }

    void TakeDamage(int damage);
}

public class Warrior : ICharacter
{
    public string Name { get; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public bool IsDead { get; set; }

    // 방어 태세 불리안 변수
    public bool IsDefense { get; set; }

    // 집중 카운트 변수
    public int Charge { get; set; }

    public Warrior(string _name, int _health, int _attack)
    {
        Name = _name;
        Health = _health;
        Attack = _attack;
        IsDefense = false;
        Charge = 0;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        IsDead = (Health <= 0);

        if (!IsDead)
            Console.WriteLine($"{Name}이(가) {damage}의 피해를 입었습니다.\n남은 체력 : {Health}");
    }

    public void ActionAttack(Monster monster)
    {
        // 집중 한 만큼 공격이 강해진다. 공격하면 집중 카운트는 초기화
        int damage = Attack + (int)(Attack / 2) * Charge;
        Charge = 0;

        Console.WriteLine($"{Name}이(가) 데미지 {damage}으로 {monster.Name}을(를) 공격! ");
        monster.TakeDamage(damage);

        IsDefense = false;
    }

    // 방어로 공격 방어 가능. 하지만 연속적인 방어는 불가능.
    public void ActionDefense()
    {
        if (!IsDefense)
        {
            Console.WriteLine($"{Name}이(가) 방어 태세를 갖추고 있다.");
            IsDefense = true;
        }
        else
        {
            Console.WriteLine($"{Name}은(는) 방어 태세에 실패했다...");
            IsDefense = false;
        }
    }

    // 집중 카운트 증가 메소드
    public void ActionConcentrate()
    {
        Console.WriteLine($"{Name}이(가) 집중하고 있다.");
        Charge += 1;
        IsDefense = false;
    }
}

public class Monster : ICharacter
{
    public string Name { get; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public bool IsDead { get; set; }
    public int Charge { get; set; }

    public Monster(string _name, int _health, int _attack)
    {
        Name = _name;
        Health = _health;
        Attack = _attack;
        Charge = 0;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        IsDead = (Health <= 0);

        if (IsDead)
            Console.WriteLine($"\t\t\t\t\t{Name}이(가) 쓰러졌습니다.");
        else
            Console.WriteLine($"\t\t\t\t\t{Name}이(가) {damage}의 피해를 입었습니다.\n\t\t\t\t\t남은 체력 : {Health}");
    }

    public void ActionAttack(Warrior warrior)
    {
        int damage = Attack + (int)(Attack / 2) * Charge;
        Charge = 0;
        Console.WriteLine($"\t\t\t\t\t{Name}이(가) 데미지 {damage}으로 {warrior.Name}을(를) 공격! ");

        // 방어 태세를 갖추고 있으면 공격 실패
        if (warrior.IsDefense)
            Console.WriteLine($"{warrior.Name}이(가) 공격을 막아냈다");
        else
            warrior.TakeDamage(Attack);
    }

    public void ActionConcentrate()
    {
        Console.WriteLine($"\t\t\t\t\t{Name}이(가) 집중하고 있다.");
        Charge += 1;
    }
}

public class Goblin : Monster
{
    public Goblin(int health, int attack) : base("고블린", health, attack) { }
}

public class Slime : Monster
{
    public Slime(int health, int attack) : base("슬라임", health, attack) { }
}

public class Dragon : Monster
{
    public Dragon(int health, int attack) : base("용", health, attack) { }
}

public interface IItem
{
    string Name { get; }

    void Use(Warrior warrior);
}

public class HealthPotion : IItem
{
    public string Name { get; }

    public HealthPotion() { Name = "체력 포션"; }

    public void Use(Warrior warrior)
    {
        Console.WriteLine("체력 포션을 사용합니다. 체력을 50 회복합니다.");

        warrior.Health = (warrior.Health <= 50) ? warrior.Health + 50 : 100;
    }
}

public class StrengthPotion : IItem
{
    public string Name { get; }

    public StrengthPotion() { Name = "공격력 포션"; }

    public void Use(Warrior warrior)
    {
        Console.WriteLine("공격력 포션을 사용합니다. 공격력이 10 증가합니다.");

        warrior.Attack += 10;
    }
}

public class Stage
{
    private Warrior warrior;
    private Monster monster;
    private List<IItem> items;
    private int level;

    private Random random;
    int itemChoice;

    public Stage(Warrior _warrior)
    {
        warrior = _warrior;
        items = new List<IItem>() { new HealthPotion(), new StrengthPotion() };
    }

    public void Start(Monster _monster, int _level)
    {
        monster = _monster;
        level = _level;
        random = new Random();

        Console.WriteLine("#######################");
        Console.WriteLine($"## 스테이지 {level} 시작 ! ##");
        Console.WriteLine("#######################");
        Console.WriteLine();

        Console.WriteLine($"\t\t\t\t\t{monster.Name} 등장 ! / 체력 : {monster.Health}, 공격력 : {monster.Attack}");

        while (!warrior.IsDead)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"{warrior.Name} / 체력 : {warrior.Health}, 공격력 : {warrior.Attack}");
            Console.WriteLine("액션을 선택하세요. 1:공격 / 2:방어 / 3:집중");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine();

            if (choice == 1)
                warrior.ActionAttack(monster);
            else if (choice == 2)
                warrior.ActionDefense();
            else if (choice == 3)
                warrior.ActionConcentrate();
            // 올바른 값을 입력하지 않으면 행동권이 넘어간다.
            else
                Console.WriteLine($"{warrior.Name}은(는) 혼란스러워하고 있다...");

            Console.WriteLine();

            if (monster.IsDead)
            {
                // 마지막 전투인 용은 보상을 주지 않는다.
                if (monster.Name == "용")
                    break;

                Console.WriteLine("전투에서 승리했습니다 !");
                Console.WriteLine("보상을 선택하세요. 1:체력 포션 / 2:공격력 포션");

                // 올바른 값을 선택해야 진행 가능
                while (true)
                {
                    itemChoice = int.Parse(Console.ReadLine()) - 1;

                    if (itemChoice == 0 || itemChoice == 1)
                        break;
                    else
                        Console.WriteLine("올바른 값을 입력하세요");
                }

                items[itemChoice].Use(warrior);
                break;
            }
            else
            {
                if (random.Next(0, 3) == 0)
                    monster.ActionConcentrate();
                else
                    monster.ActionAttack(warrior);
            }
        }
    }
}

internal class Program
{
    static void Main()
    {
        Console.WriteLine("던전에 오신 것을 환영합니다 !");
        Console.WriteLine("당신의 이름은 ?");

        Warrior warrior = new Warrior(Console.ReadLine(), 100, 20);
        Stage stage = new Stage(warrior);
        Random random = new Random();
        int iRand = random.Next(1, 6);

        // 몬스터의 체력과 공격력에 랜덤 요소를 넣어 리플레이 요소 첨가
        List<Monster> monsters = new List<Monster>();
        monsters.Add(new Goblin(100, 10));
        monsters.Add(new Goblin(100 - 5 * iRand, 10 + 2 * iRand));
        monsters.Add(new Slime(120, 10));
        monsters.Add(new Slime(130 + 5 * iRand, 10 - iRand));
        monsters.Add(new Dragon(150 + 10 * iRand, 20 - iRand));

        for (int i = 0; i < 5; i++)
        {
            stage.Start(monsters[i], i + 1);

            if (warrior.IsDead)
                break;
            else
                // 다음 레벨 진행할 때 집중 카운트 초기화
                warrior.Charge = 0;
        }

        if (warrior.IsDead)
        {
            Console.WriteLine($"{warrior.Name}은(는) 쓰러졌습니다...");
        }
        else
        {
            Console.WriteLine("승리했습니다 !");
            Console.WriteLine($"{warrior.Name}은(는) 전설의 용사로 기억되었습니다...");
        }
    }
}