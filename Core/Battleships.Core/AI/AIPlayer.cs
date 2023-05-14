using System;
using Battleships.Core.Board;

namespace Battleships.Core.AI
{
   internal class AIPlayer : IAIPlayer
   {
      private readonly RandomHelper _rand = new RandomHelper();
      public (char, int) MakeMove( IOpponentBoard opponentBoard )
      {
         var column = _rand.GetColumn();
         var row = _rand.GetRow();
         while ( opponentBoard.GetStatus( column, row ) != CellStatus.Undescovered )
         {
            column = _rand.GetColumn();
            row = _rand.GetRow();
         }
         return (column, row);
      }


   }
}
