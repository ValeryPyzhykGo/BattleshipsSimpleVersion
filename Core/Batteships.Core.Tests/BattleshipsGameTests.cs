using Battleships.Core;
using Battleships.Core.AI;
using Battleships.Core.Board;
using Battleships.Core.Exceptions;
using NUnit.Framework;
using Moq;

namespace Battleships.Core.Tests
{
   [TestFixture]
   public class BattleshipsGameTests
   {
      private Mock<IBoard> _firstPlayerBoardMock;
      private Mock<IBoard> _secondPlayerBoardMock;
      private Mock<IAIPlayer> _aiPlayerMock;
      private Mock<IRandomWrapper> _rand;
      private BattleshipsGame _game;

      [SetUp]
      public void Setup()
      {
         _firstPlayerBoardMock = new Mock<IBoard>();
         _secondPlayerBoardMock = new Mock<IBoard>();
         _aiPlayerMock = new Mock<IAIPlayer>();
         _rand = new Mock<IRandomWrapper>();

         _firstPlayerBoardMock.Setup( x => x.TryPlaceShip(It.IsAny<char>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<Ship>() ) ).Returns(true) ;
         _secondPlayerBoardMock.Setup( x => x.TryPlaceShip( It.IsAny<char>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<Ship>() ) ).Returns( true );
         _game = new BattleshipsGame( _firstPlayerBoardMock.Object, _secondPlayerBoardMock.Object, _aiPlayerMock.Object, _rand.Object );
      }


      [Test]
      public void Constructor_Default_ShouldPlaceAllShipsRandomly()
      {
         // Act
         var game = new BattleshipsGame();
         // Assert
         Assert.AreEqual( 10, game.PlayerBoard.LifesLeft );
         Assert.AreEqual( 10, game.OpponentBoard.LifesLeft );
      }

      [Test]
      public void Constructor_WithDependencies_ShouldPlaceAllShipsRandomly()
      {    
         // Assert
         _firstPlayerBoardMock.Verify( b => b.TryPlaceShip( It.IsAny<char>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<Ship>() ), Times.Exactly( 3 ) );
         _secondPlayerBoardMock.Verify( b => b.TryPlaceShip( It.IsAny<char>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<Ship>() ), Times.Exactly( 3 ) );
      }

      [Test]
      public void MakeMoveAndLetOppoentMove_GameIsOver_ThrowsMoveAfterGameOverException()
      {
         // Arrange
         _firstPlayerBoardMock.As<IOpponentBoard>().Setup( b => b.LifesLeft ).Returns( 0 );

         // Act & Assert
         Assert.Throws<MoveAfterGameOverException>( () => _game.MakeMoveAndLetOppoentMove( 'A', 1 ) );
      }

      [Test]
      [TestCase( ShipClass.Destroyer, null )]
      [TestCase( null, ShipClass.Destroyer )]
      [TestCase( ShipClass.Battleship, null )]
      [TestCase( null, ShipClass.Battleship )]
      [TestCase( ShipClass.Destroyer, ShipClass.Battleship )]
      [TestCase( ShipClass.Battleship, ShipClass.Destroyer )]
      [TestCase( ShipClass.Battleship, ShipClass.Battleship )]
      [TestCase( ShipClass.Destroyer, ShipClass.Battleship )]
      public void MakeMoveAndLetOppoentMove_ShouldReturnCorrectShips( ShipClass? opponentShipClass, ShipClass? playerShipClass )
      {
         // Arrange
         _firstPlayerBoardMock.As<IOpponentBoard>().Setup( b => b.LifesLeft ).Returns( 1 );
         _secondPlayerBoardMock.As<IOpponentBoard>().Setup( b => b.LifesLeft ).Returns( 1 );

         char column = 'A';
         int row = 1;
         var playerShip = playerShipClass != null ? new Ship( playerShipClass.Value ) : null;
         var opponentShip = opponentShipClass != null ? new Ship( opponentShipClass.Value ) : null;
         _secondPlayerBoardMock.Setup( b => b.MakeMove( column, row ) ).Returns( opponentShip );
         _aiPlayerMock.Setup( a => a.MakeMove( _firstPlayerBoardMock.Object ) ).Returns( ('B', 2) );
         _firstPlayerBoardMock.Setup( b => b.MakeMove( 'B', 2 ) ).Returns( playerShip );

         // Act
         var (resultOpponentShip, resultPlayerShip) = _game.MakeMoveAndLetOppoentMove( column, row );

         // Assert
         Assert.AreSame( playerShip, resultPlayerShip );
         Assert.AreSame( opponentShip, resultOpponentShip );
         _firstPlayerBoardMock.Verify( b => b.MakeMove( 'B', 2 ), Times.Once() );
         _aiPlayerMock.Verify( a => a.MakeMove( _firstPlayerBoardMock.Object ), Times.Once );
         _secondPlayerBoardMock.Verify( b => b.MakeMove( column, row ), Times.Once );
      }

      [Test]
      [TestCase( 0, 2, Player.Computer)]
      [TestCase( 10, 0, Player.User )]
      [TestCase( 1, 1, null )]
      public void GetWinner_SomebodyWin_ShouldReturnCorrentWinner(int firstBoarLifes, int secondBoardLifes, Player? winner)
      {
         // Arrange
         _firstPlayerBoardMock.As<IOpponentBoard>().Setup( b => b.LifesLeft ).Returns( firstBoarLifes );
         _secondPlayerBoardMock.As<IOpponentBoard>().Setup( b => b.LifesLeft ).Returns( secondBoardLifes );

         // Act
         var result = _game.GetWinner();

         // Assert
         Assert.AreEqual( winner , result );
      }
   }
}