using System;
using System.Collections.Generic;
using System.Threading;


//public class Character
//{
//    private string name;
//    private int level;
//    private int hp;
//    private int ap;
//    private int speed;
//}


public class GameController
{
    // 입력 받는 함수 : min <= input <= max 값만 받도록
    private int GetInput(int min, int max)
    {
        Console.WriteLine("\n원하시는 행동을 입력해주세요.");
        Console.Write(">> ");

        while (true)
        {
            int.TryParse(Console.ReadLine(), out int input);

            if (input < min || max < input)
            {
                Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
                Console.Write(">> ");
            }
            else
                return input;
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
}

internal class Program
{
    static void Main()
    {
        GameController GC = new GameController();
        GC.VillageEnterance();
    }
}