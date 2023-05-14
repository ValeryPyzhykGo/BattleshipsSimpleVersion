namespace Battleships.Core.Exceptions
{
   public class HitSankShipException : BattleshipsGameException
   {
      public HitSankShipException() : base( "Sank ship cannot be hit." )
      {
      }
   }
}
