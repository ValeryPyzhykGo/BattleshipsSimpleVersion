using Battleships.Core.Board;

namespace Battleships.ConsoleWrapper
{
   internal interface IBoardsConsoleUI
   {
      void ShowBoards( IPlayerBoard playerBoard, IOpponentBoard opponentBoard );
   }
}