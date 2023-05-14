using System;

namespace Battleships.ConsoleWrapper
{
   public class ConsoleWraper : IConsoleWraper
   {
      public void Write( string s )
      {
         Console.Write( s );
      }

      public void WriteLine( string s )
      {
            Console.WriteLine(s);
      }

      public void WriteLine()
      {
         Console.WriteLine();
      }

      public string ReadLine()
      {
         return Console.ReadLine();
      }

      public void Clear()
      {
         Console.Clear();
      }
   }
}
