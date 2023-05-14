using Battleships.Core.Board;

namespace Battleships.Core.AI
{
   internal interface IAIPlayer
   {
      (char, int) MakeMove( IOpponentBoard opponentBoard );
   }
}