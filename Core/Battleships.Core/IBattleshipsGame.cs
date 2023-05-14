using Battleships.Core.Board;

namespace Battleships.Core
{
   public interface IBattleshipsGame
   {
      IOpponentBoard OpponentBoard
      {
         get;
      }
      IPlayerBoard PlayerBoard
      {
         get;
      }

      bool IsOver
      {
         get;
      }

      (Ship, Ship) MakeMoveAndLetOppoentMove( char column, int row );
      Player? GetWinner();
   }
}