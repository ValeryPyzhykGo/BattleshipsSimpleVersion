using System;
using Battleships.ConsoleWrapper;

namespace Battleships.ConsoleUI
{
   class Program
   {
      public static void Main()
      {
         new BattleshipsConsoleGame().Start();
         Console.ReadLine();
      }

   }
}
