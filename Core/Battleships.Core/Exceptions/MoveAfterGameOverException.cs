namespace Battleships.Core.Exceptions
{
   public class MoveAfterGameOverException : BattleshipsGameException
   {
      public MoveAfterGameOverException() : base( "Cannot make a move after game over." )
      {
      }
   }
}
