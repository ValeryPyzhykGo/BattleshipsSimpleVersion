using Battleships.Core;
using Battleships.Core.Exceptions;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo( "DynamicProxyGenAssembly2" )]
[assembly: InternalsVisibleTo( "Battleships.ConsoleWrapper.E2ETests" )]
[assembly: InternalsVisibleTo( "Battleships.ConsoleWrapper.UnitTests" )]
namespace Battleships.ConsoleWrapper
{
   public class BattleshipsConsoleGame
   {
      private readonly IConsoleWraper _console;
      private readonly IBoardsConsoleUI _boardsUI;
      private readonly IBattleshipsGameFactory _gameSetUpFactory;
      private readonly IBattleshipsConsoleGameMessages _messages;

      public BattleshipsConsoleGame( IBattleshipsConsoleGameMessages messages = null )
      {
         _messages = messages ?? new BattleshipsConsoleGameMessagesEng();
         _console = new ConsoleWraper();
         _boardsUI = new BoardsConsoleUI( _messages, _console );
      }

      internal BattleshipsConsoleGame( IBattleshipsConsoleGameMessages messages, IConsoleWraper console, IBoardsConsoleUI boardsUI = null, IBattleshipsGameFactory gameSetUpFactory = null )
      {
         _console = console;
         _messages = messages;
         _boardsUI = boardsUI ?? new BoardsConsoleUI( _messages, _console );
         _gameSetUpFactory = gameSetUpFactory;
      }

      #region Game
      public void Start()
      {
         try
         {
            do
            {
               PlayGame(_gameSetUpFactory?.Create() ?? new BattleshipsGame()) ;
            } while ( AskBoolednQuestionWithRetry( _messages.WantToRestartMessage ) );
         }
         catch (Exception e)
         {
            _console.WriteLine( _messages.GameIsBrokenMessage );
         }
      }

      private void PlayGame( IBattleshipsGame game )
      {
         _console.Clear();
         _boardsUI.ShowBoards( game.PlayerBoard, game.OpponentBoard );
         var (playerMoveResult, aiMoveResult) = MakeMoveWithRetryInCaseOfBadMove( game );
         while ( !game.IsOver )
         {
            _console.Clear();
            _boardsUI.ShowBoards( game.PlayerBoard, game.OpponentBoard );
            WriteMoveResult( _messages.UserName, playerMoveResult );
            WriteMoveResult( _messages.AIName, aiMoveResult );
            (playerMoveResult, aiMoveResult) = MakeMoveWithRetryInCaseOfBadMove( game );
         }

         _console.WriteLine( _messages.GameOver );
         _console.WriteLine( _messages.GetPlayerWinMessage( game.GetWinner().Value ) );
      }

      private (Ship, Ship) MakeMoveWithRetryInCaseOfBadMove( IBattleshipsGame game )
      {
         while ( true )
         {
            var (column, row) = AskForCoordinatesWithRetry(game);
            try
            {
               return game.MakeMoveAndLetOppoentMove( column, row );
            }
            catch ( CellIsAlreadyDiscoveredException )
            {
               _console.WriteLine( _messages.GetCellIsAlreadyDiscoverdMessage( column, row ) );
               _console.WriteLine( _messages.TryAgainMessage );
            }
         }
      }
      #endregion

      #region Output

      private void WriteMoveResult( string player, Ship ship )
      {
         if ( ship == null )
         {
            _console.WriteLine( _messages.GetMissMessage( player ) );
         }
         else
         {
            _console.WriteLine( _messages.GetHitMessage( player, ship.ShipClass ) );
            if ( ship.LifesLeft < 1 )
            {
               _console.WriteLine( _messages.GetSankMessage( ship.ShipClass ) );
            }
         }
         _console.WriteLine();
      }

      #endregion

      #region Input
      private bool AskBoolednQuestionWithRetry( string question )
      {
         _console.WriteLine( question );
         var line = _console.ReadLine().ToUpper();
         while ( line.Length != 1 || ( line[0] != _messages.YesAnswer && line[0] != _messages.NoAnswer ) )
         {
            _console.WriteLine( _messages.TryAgainYesNoQuestionMessage );
            line = _console.ReadLine().ToUpper();
         }
         return line[0] == _messages.YesAnswer;
      }

      private (char, int) AskForCoordinatesWithRetry(IBattleshipsGame game)
      {        
         _console.WriteLine( _messages.EnterCoordinates );
         var coordinates = _console.ReadLine();

         int row;

         while ( coordinates.Length < 2 || coordinates.Length > 3 || !int.TryParse( coordinates.Substring( 1, coordinates.Length - 1 ), out row ) || !game.PlayerBoard.CheckCooridnates( coordinates[0], row ) )
         {
            _console.WriteLine( _messages.CoordinatesAreInvalid );
            _console.WriteLine( _messages.TryAgainMessage );
            _console.WriteLine( _messages.EnterCoordinates );
            coordinates = _console.ReadLine();
         }

         return (coordinates[0], row);
      }
      #endregion
   }
}
