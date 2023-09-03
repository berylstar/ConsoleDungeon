using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 게임 컨트롤러 클래스 - 전체 게임 관리
public class GameController
{
    private Player player;
    private Monster mon;

    // 전투 메세지를 담고 있는 큐
    private static Queue<string> script = new Queue<string>();

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

        int jobChoice = Utils.GetInput(1, 3);

        Player _player;
        if (jobChoice == 1) { _player = new Player(warriorName, "검사", 10, 5, 100, 20); }
        else if (jobChoice == 2) { _player = new Player(warriorName, "전사", 10, 10, 120, 10); }
        else { _player = new Player(warriorName, "도적", 12, 0, 80, 30); }

        player = _player;
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
            Utils.ColorWriteLine($"\n5. 던전 입장 - {Dungeon.I.Type}", ConsoleColor.Red);
            Utils.ColorWriteLine("\n0. 게임 종료");

            int input = Utils.GetInput(0, 5);

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

        _ = Utils.GetInput(0, 0);
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

        int input = Utils.GetInput(0, player.Inventory.Count);

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

        for (int i = 0; i < Shop.I.ShopList.Count; i++)
        {
            Equip equip = Shop.I.ShopList[i];

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

        int input = Utils.GetInput(0, 5);

        // 예외 처리
        while (input != 0)
        {
            // 이미 판매된 장비였을 때
            if (Shop.I.ShopList[input - 1] == null)
            {
                Console.Write("\n이미 판매된 장비입니다. 다시 입력해주세요.");
                input = Utils.GetInput(0, 5);
            }
            // 돈이 부족할 때
            else if (player.Gold < Shop.I.ShopList[input - 1].Price)
            {
                Console.Write("\n골드가 부족합니다. 다른 장비를 구매하세요.");
                input = Utils.GetInput(0, 5);
            }
            // 인벤토리가 가득 찼을 때
            else if (player.Inventory.Count >= 9)
            {
                Console.Write("\n인벤토리가 가득 찼습니다. 구매하시려면 먼저 장비를 판매해주세요.");
                input = Utils.GetInput(0, 5);
            }
            else
                break;
        }

        // 그 외 상점에서 구매가 가능한 경우
        if (1 <= input && input <= 5)
        {
            Shop.I.Buy(player, input - 1);
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

        int input = Utils.GetInput(0, 9);

        if (input == 0)
            return;

        // 예외 처리 - 비어 있는 곳일 때
        while (input > player.Inventory.Count)
        {
            Console.Write("\n해당 인벤토리는 비어 있습니다. 다시 선택해주세요.");
            input = Utils.GetInput(0, 9);
        }

        Shop.I.Sell(player, input - 1);
        ShowShopForSell();
    }

    // 던전 입장 - 전투 전 던전 로비
    private void InDungeon()
    {
        while (Dungeon.I.Count > 0)
        {
            Console.Clear();
            Console.WriteLine("===============================");
            Utils.Talk($"{Dungeon.I.Type} 난이도의 던전입니다.");
            Utils.Talk($"앞으로 {Dungeon.I.Count}마리의 몬스터를 처치해야 합니다.\n");

            Utils.ColorWriteLine("1. 상태 보기");
            Utils.ColorWriteLine("2. 인벤토리");
            Utils.ColorWriteLine("\n3. 이어서 진행하기", ConsoleColor.Red);

            int input = Utils.GetInput(1, 3);

            if (input == 1) ShowStatus();
            else if (input == 2) ShowInventory();
            else ShowBattle();
        }

        // 마지막 보스를 잡으면 게임 승리
        if (Dungeon.I.Type == DungeonType.Boss)
        {
            GameWin();
        }
        // 던전 난이도 증가, 상점 새로고침
        else
        {
            Dungeon.I.GetStrong();
            Shop.I.SetShop();
        }
    }

    // 몬스터와 전투
    private void ShowBattle()
    {
        mon = Dungeon.I.Monsters[0];
        script.Enqueue($"{mon.Name}이 나타났습니다 !");

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

            while (script.Count > 0)
            {
                Utils.Talk(script.Dequeue());
            }

            Utils.ColorWriteLine("\n1. 공격");
            Utils.ColorWriteLine("2. 방어");
            Utils.ColorWriteLine("3. 집중");

            int input = Utils.GetInput(1, 3);

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

        _ = Utils.GetInput(0, 0);

        script.Clear();
        Dungeon.I.Monsters.RemoveAt(0);
        Dungeon.I.Count -= 1;

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
                script.Enqueue($"{main.Name}의 공격 !");
                script.Enqueue($"하지만, {target.Name}은 {main.Name}의 공격을 막아냈다 !");
            }
            else
            {
                target.GetDamaged(main.AP * (2 * main.Focus + 1));
                script.Enqueue($"{main.Name}의 공격 ! {target.Name}에게 {main.AP * (2 * main.Focus + 1)}의 데미지");
            }

            main.Focus = 0;
        }
        else if (input == 2)
        {
            if (main.DefenseStance)
            {
                main.DefenseStance = false;
                script.Enqueue($"{main.Name}은 힘들어, 연속으로 방어 태세를 갖추지 못했다...");
            }
            else
            {
                main.DefenseStance = true;
                script.Enqueue($"{main.Name}은 방어 태세에 들어갔다.");
            }

        }
        else if (input == 3)
        {
            main.DefenseStance = false;
            main.Focus += 1;
            script.Enqueue($"{main.Name}은 집중하고 있다...");
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

        int input = Utils.GetInput(0, 0);

        if (input == 0) Environment.Exit(0);
    }

    // 게임 종료 - 패배
    public void GameFail()
    {
        Console.Clear();
        Utils.Talk($"{player.Name}은 쓰러졌습니다...");
        Utils.Talk($"다시 도전해보세요 !");

        Utils.ColorWriteLine("\n0. 게임 종료");

        int input = Utils.GetInput(0, 0);

        if (input == 0) Environment.Exit(0);
    }
}
