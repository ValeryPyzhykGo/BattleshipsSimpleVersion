using Battleships.Core;
using System.Collections.Generic;

namespace Battleships.ConsoleWrapper
{
   public class BattleshipsConsoleGameMessagesEng : IBattleshipsConsoleGameMessages
   {
      private static readonly Dictionary<ShipClass, char> _shipLetters = new Dictionary<ShipClass, char> {
            { ShipClass.Battleship, 'B' },
            { ShipClass.Destroyer,  'D' },
        };

      public string TryAgainMessage => "Please try again!";
      public string GameIsBrokenMessage => "Sorry, the game has crushed.";
      public string TryAgainYesNoQuestionMessage => "Enter Y for yes or N for not";
      public char YesAnswer => 'Y';
      public char NoAnswer => 'N';
      public string WantToRestartMessage => "Do you want to start a new game? (Y/N)";
      public string EnterCoordinates => "Please enter coordinates!";
      public string CoordinatesAreInvalid => "The cordinates are incorrect!";
      public string GameOver => "Game Over!";
      public string UserName => "You";
      public string AIName => "Computer";

      public string GetPlaceShipMessage( ShipClass shipClass )
      {
         return $"Please place a {shipClass} ship!";
      }

      public char GetShipLetter( ShipClass shipClass )
      {
         return _shipLetters[shipClass];
      }
      public string GetCellIsAlreadyDiscoverdMessage( char column, int row )
      {
         return $"The cell {column}{row} is alrady discovered.";
      }

      public string GetMissMessage( string playerName )
      {
         return $"{playerName} missed!";
      }

      public string GetHitMessage( string playerName, ShipClass shipClass )
      {
         return $"{playerName} hit {shipClass}!";
      }

      public string GetSankMessage( ShipClass shipClass )
      {
         return $"{shipClass} sank!";
      }

      public string GetPlayerWinMessage( Player player )
      {
         var playerName = player == Player.User ? UserName : AIName;
         return $"{playerName} won!";
      }
   }
}
