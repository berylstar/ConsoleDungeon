using System;
using System.Collections.Generic;
using System.Text;

internal class Program
{
    static void Main()
    {
        // Console.SetWindowSize(120, 30);

        GameController GC = new GameController();
        GC.GameStart();
        GC.VillageEnterance();
    }
}