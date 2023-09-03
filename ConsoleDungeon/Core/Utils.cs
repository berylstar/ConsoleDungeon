using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Utils
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