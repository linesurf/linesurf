﻿using System;

namespace Linesurf
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new LinesurfGame();
            game.Run();
        }
    }
}
