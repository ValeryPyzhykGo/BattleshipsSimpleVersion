using System;
using Battleships.ConsoleWrapper;

namespace Battleships.ConsoleUI
{
   public class Program
   {
      public static void Main()
      {
         new BattleshipsConsoleGame().Start();
         Console.ReadLine();
      }

   }
}
