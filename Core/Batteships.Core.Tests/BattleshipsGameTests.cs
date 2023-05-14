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
      public void MakeMoveAndLetOppoentMove_GameIsOver_ThrowsMoveAfterGameOverException()
      {
         // Arrange
         _firstPlayerBoardMock.As<IOpponentBoard>().Setup( b => b.LifesLeft ).Returns( 0 );

         // Act & Assert
         Assert.Throws<MoveAfterGameOverException>( () => _game.MakeMoveAndLetOppoentMove( 'A', 1 ) );
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