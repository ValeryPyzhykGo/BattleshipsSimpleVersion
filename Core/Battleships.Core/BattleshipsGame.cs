using Battleships.Core.AI;
using Battleships.Core.Board;
using Battleships.Core.Exceptions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo( "Batteships.Core.Tests" )]
namespace Battleships.Core
{
   public class BattleshipsGame : IBattleshipsGame
   {
      private readonly RandomHelper _rand = new RandomHelper();
      public IPlayerBoard PlayerBoard => _firstPlayerBoard;
      public IOpponentBoard OpponentBoard => _secondPlayerBoard;

      private readonly IBoard _firstPlayerBoard;
      private readonly IBoard _secondPlayerBoard;
      private readonly IAIPlayer _aIPlayer;

      public bool IsOver => ( _firstPlayerBoard as IOpponentBoard ).LifesLeft == 0
          || ( _secondPlayerBoard as IOpponentBoard ).LifesLeft == 0;

      public BattleshipsGame()
      {
         _firstPlayerBoard = new Board.Board();
         _secondPlayerBoard = new Board.Board();
         _aIPlayer = new AIPlayer();
         PlaceAllShipsRandomly();
      }
      internal BattleshipsGame( IBoard firstUserBoard, IBoard secondPlaeyerBoard, IAIPlayer aIPlayer )
      {
         _firstPlayerBoard = firstUserBoard;
         _secondPlayerBoard = secondPlaeyerBoard;
         _aIPlayer = aIPlayer;
         PlaceAllShipsRandomly();
      }

      public (Ship, Ship) MakeMoveAndLetOppoentMove( char column, int row )
      {
         if ( IsOver )
         {
            throw new MoveAfterGameOverException();
         }

         return (_secondPlayerBoard.MakeMove( column, row ), GetOpponentMove());
      }

      public Player? GetWinner()
      {
         if ( !IsOver )
         {
            return null;
         }
         return ( _firstPlayerBoard as IOpponentBoard ).LifesLeft == 0 ? Player.Computer : Player.User;
      }
      private Ship GetOpponentMove()
      {
         var (column, row) = _aIPlayer.MakeMove( _firstPlayerBoard );
         return _firstPlayerBoard.MakeMove( column, row );
      }

      private void PlaceAllShipsRandomly()
      {
         foreach ( var shipClass in new List<ShipClass> { ShipClass.Battleship, ShipClass.Destroyer, ShipClass.Destroyer } )
         {
            PlaceSheepRandomly( shipClass, _firstPlayerBoard );
            PlaceSheepRandomly( shipClass, _secondPlayerBoard );
         }
      }

      private void PlaceSheepRandomly( ShipClass shipClass, IPlayerBoard board )
      {
         var ship = new Ship( shipClass );
         while ( !board.TryPlaceShip( _rand.GetColumn(), _rand.GetRow(), _rand.GetBooleand(), ship ) )
         {
         }
      }
   }
}
