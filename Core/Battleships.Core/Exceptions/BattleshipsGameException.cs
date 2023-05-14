using System;

namespace Battleships.Core.Exceptions
{
   public class BattleshipsGameException : ApplicationException
   {
      public BattleshipsGameException( string message ) : base( message )
      {
      }
   }
}
