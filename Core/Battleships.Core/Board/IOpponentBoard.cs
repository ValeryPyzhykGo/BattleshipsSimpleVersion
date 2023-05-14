namespace Battleships.Core.Board
{
   public interface IOpponentBoard
   {
      public CellStatus GetStatus( char column, int row );
      public ShipClass? GetShipClass( char column, int row );
      public Ship MakeMove( char column, int row );
      public int LifesLeft
      {
         get;
      }
   }
}
