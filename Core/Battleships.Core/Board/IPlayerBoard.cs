namespace Battleships.Core.Board
{
   public interface IPlayerBoard
   {
      int LifesLeft
      {
         get;
      }
      bool CheckCooridnates( char column, int row );
      bool TryPlaceShip( char column, int row, bool isVertical, Ship ship );
      CellStatus GetStatus( char column, int row );
      ShipClass? GetShipClass( char column, int row );
   }
}