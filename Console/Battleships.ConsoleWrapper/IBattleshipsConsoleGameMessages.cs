using Battleships.Core;
using System.Collections.Generic;

namespace Battleships.ConsoleWrapper
{
   public interface IBattleshipsConsoleGameMessages
   {
      string TryAgainMessage
      {
         get;
      }
      string GameIsBrokenMessage
      {
         get;
      }
      string TryAgainYesNoQuestionMessage
      {
         get;
      }
      char YesAnswer
      {
         get;
      }
      char NoAnswer
      {
         get;
      }
      string WantToRestartMessage
      {
         get;
      }
      string EnterCoordinates
      {
         get;
      }
      string CoordinatesAreInvalid
      {
         get;
      }
      string GameOver
      {
         get;
      }
      string UserName
      {
         get;
      }
      string AIName
      {
         get;
      }

      char GetShipLetter( ShipClass shipClass );
      string GetCellIsAlreadyDiscoverdMessage( char column, int row );
      string GetMissMessage( string playerName );
      string GetHitMessage( string playerName, ShipClass shipClass );
      string GetSankMessage( ShipClass shipClass );
      string GetPlayerWinMessage( Player player );
   }
}
