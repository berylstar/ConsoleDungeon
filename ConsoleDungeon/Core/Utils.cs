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

    // 문자열의 글자색을 변경시켜 출력하는 함수
    public static void ColorWriteLine(string _str, ConsoleColor _writeColor = ConsoleColor.Yellow)
    {
        Console.ForegroundColor = _writeColor;
        Console.WriteLine(_str);
        Console.ResetColor();
    }

    // 커서의 위치를 옮기고 문자열을 출력하는 함수
    public static void CursorWrite(int left, int top, string _str)
    {
        Console.SetCursorPosition(left, top);
        Console.WriteLine(_str);
    }

    public static Random random = new Random();
}