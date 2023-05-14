namespace Battleships.ConsoleWrapper
{
   public interface IConsoleWraper
   {
      public void Write( string s );

      public void WriteLine( string s );

      public void WriteLine();

      public string ReadLine();

      public void Clear();
   }
}
