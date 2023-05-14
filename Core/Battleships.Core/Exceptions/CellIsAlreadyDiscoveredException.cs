namespace Battleships.Core.Exceptions
{
   public class CellIsAlreadyDiscoveredException : BattleshipsGameException
   {
      public CellIsAlreadyDiscoveredException( char column, int row ) : base( $"The cell {column}{row} is alrady discovered." )
      {
      }
   }
}
