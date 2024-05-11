using System;
using System.Diagnostics;
using Game.Components;

namespace Game
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            new SpaceShooterWindow().Run(60);
        }
        
    }
}
