using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Utils
{
    // 입력 받는 함수 : min <= input <= max 값만 받도록
    public static int GetInput(int min, int max)
    {
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.Write(">> ");

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (min <= input && input <= max)
                    return input;
            }

            Console.WriteLine();
            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
            Console.Write(">> ");
        }
    }

    // 키보드 입력 받기
    public static int GetKeyInput(int xCursor, int yCursor, string[] choices)
    {
        int pick = 0;

        while (true)
        {
            int y = yCursor;

            for (int i = 0; i < choices.Length; i++)
            {
                Console.SetCursorPosition(xCursor, y);

                if (i == pick)
                    ColorWriteLine("- " + choices[i], ConsoleColor.Black, ConsoleColor.White);
                else
                    Console.WriteLine("- " + choices[i]);

                y += 2;
            }

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    pick = (pick == 0) ? choices.Length - 1 : pick - 1;
                    break;

                case ConsoleKey.DownArrow:
                    pick = (pick == choices.Length - 1) ? 0 : pick + 1;
                    break;

                case ConsoleKey.Enter:
                    return pick;
            }
        }
    }

    // 문자열을 한 글자씩 출력하는 함수
    public static void Talk(string _str)
    {
        foreach (char c in _str)
        {
            Console.Write(c);
            Thread.Sleep(1);
        }

        Console.WriteLine();
    }

    // 문자열을 한 글자씩 출력하는 함수
    public static void Talk(int x, int y, string _str)
    {
        Console.SetCursorPosition(x, y);
        foreach (char c in _str)
        {
            Console.Write(c);
            Thread.Sleep(1);
        }

        Console.WriteLine();
    }

    // 문자열의 글자색을 변경시켜 출력하는 함수
    public static void ColorWriteLine(string _str, ConsoleColor _writeColor = ConsoleColor.Yellow, ConsoleColor _backColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = _writeColor;
        Console.BackgroundColor = _backColor;
        Console.WriteLine(_str);
        Console.ResetColor();
    }

    public static void ColorWriteLine(int x, int y, string _str, ConsoleColor _writeColor = ConsoleColor.Yellow, ConsoleColor _backColor = ConsoleColor.Black)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = _writeColor;
        Console.BackgroundColor = _backColor;
        Console.WriteLine(_str);
        Console.ResetColor();
    }

    // 커서의 위치를 옮기고 문자열을 출력하는 함수
    public static void WriteOnPosition(int left, int top, string _str)
    {
        Console.SetCursorPosition(left, top);
        Console.WriteLine(_str);
    }

    public static void DrawRect(int x, int y, int width, int height, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.SetCursorPosition(x, y);
        Console.WriteLine("┌" + new string('─', width - 2) + "┐");

        for (int i = 0; i < height - 2; i++)
        {
            Console.SetCursorPosition(x, Console.CursorTop);
            Console.WriteLine("│" + new string(' ', width - 2) + "│");
        }

        Console.SetCursorPosition(x, Console.CursorTop);
        Console.WriteLine("└" + new string('─', width - 2) + "┘");
        Console.ResetColor();
    }

    public static void DrawDot(int x, int y, string _str)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(_str);
        Console.ResetColor();
    }

    public static Random random = new Random();
}