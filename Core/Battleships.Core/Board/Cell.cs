namespace Battleships.Core.Board
{
   public class Cell
   {
      internal static Cell MissCell { get; } = new Cell() { Status = CellStatus.Miss };
      public Ship Ship
      {
         get; internal set;
      }
      public CellStatus Status
      {
         get; internal set;
      }
   }
}
