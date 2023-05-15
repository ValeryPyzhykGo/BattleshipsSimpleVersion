using Battleships.Core;
using Battleships.Core.Board;
using Moq;
using NUnit.Framework;

namespace Battleships.ConsoleWrapper.Tests
{
   [TestFixture]
   public class BattleshipsConsoleGameTests
   {
      private const string _gameOverMessage = "GameOverMessage test message";
      private const string _gameIsBroken = "The game is broken test message";
      private const string _someoneWonMessage = "Someone Won test message";

      private Mock<IConsoleWraper> _consoleWrapperMock;
      private Mock<IBattleshipsConsoleGameMessages> _battleshipsMessages;
      private Mock<IBoardsConsoleUI> _boardConsoleUI;
      private Mock<IBattleshipsGameFactory> _factory;
      private Mock<IBattleshipsGame> _game;
      private Mock<IPlayerBoard> _playerBoard;
      private Mock<IOpponentBoard> _opponentBoard;

      [SetUp]
      public void SetUp()
      {
         _consoleWrapperMock = new Mock<IConsoleWraper>();
         _battleshipsMessages = new Mock<IBattleshipsConsoleGameMessages>();
         _boardConsoleUI = new Mock<IBoardsConsoleUI>();
         _factory = new Mock<IBattleshipsGameFactory>();
         _game = new Mock<IBattleshipsGame>();
         _playerBoard = new Mock<IPlayerBoard>();
         _opponentBoard = new Mock<IOpponentBoard>();

         _playerBoard.Setup( x => x.CheckCooridnates( It.IsAny<char>(), It.IsAny<int>() ) ).Returns( true );
         _game.SetupGet( x => x.PlayerBoard ).Returns( () => _playerBoard.Object );
         _game.SetupGet( x => x.OpponentBoard ).Returns( () => _opponentBoard.Object );
         _game.SetupGet( x => x.IsOver ).Returns( true );
  

         _factory.Setup( x => x.Create() ).Returns( () => _game.Object );

         _battleshipsMessages.SetupGet( x => x.YesAnswer ).Returns( 'Y' );
         _battleshipsMessages.SetupGet( x => x.NoAnswer ).Returns( 'N' );

         _battleshipsMessages.SetupGet( x => x.GameOver ).Returns( _gameOverMessage );
         _battleshipsMessages.Setup( x => x.GetPlayerWinMessage( It.IsAny<Player>() ) ).Returns( _someoneWonMessage );
         _battleshipsMessages.SetupGet( x => x.GameIsBrokenMessage ).Returns( _gameIsBroken );

         _consoleWrapperMock.Setup( x => x.ReadLine() ).Returns("A1");
      }

      private void SetUpGameNotOverTimes( int times, Player winner, int numberOfGames =  1)
      {
         var timesToFromStart = times;
         _game.SetupGet( x => x.IsOver ).Returns( () => {
            times--;
            if ( times < 1 )
            {
               numberOfGames--;
               _game.Setup( x => x.GetWinner() ).Returns( winner );
               _consoleWrapperMock.SetupSequence( x => x.ReadLine() ).Returns( numberOfGames < 1 ? "N" : "Y" ).Returns("A1");

               times = timesToFromStart;
               return true;
            }
            _consoleWrapperMock.Setup( x => x.ReadLine() ).Returns( "A1" );
            return false;
         } );
      }

      [Test]
      public void Start_WithCorrectSetUp_ShouldFinishCorrectly()
      {
         // Arrange
         SetUpGameNotOverTimes( 5, Player.Computer );

         // Act
         new BattleshipsConsoleGame( _battleshipsMessages.Object, _consoleWrapperMock.Object, _boardConsoleUI.Object, _factory.Object ).Start();

         // Assert
         AssertGameHasFinishedCorrectly( Times.Once() );
      }

      [Test]
      [TestCase( 1 )]
      [TestCase( 5 )]
      [TestCase( 10 )]
      [TestCase( 100 )]
      public void Start_WithDifferentRestartCount_ShouldFinishCorrectlyCorrectAmountOfTimes(int restarts)
      {
         // Arrange
         SetUpGameNotOverTimes( 5, Player.Computer, restarts );

         // Act
         new BattleshipsConsoleGame( _battleshipsMessages.Object, _consoleWrapperMock.Object, _boardConsoleUI.Object, _factory.Object ).Start();

         // Assert
         AssertGameHasFinishedCorrectly( Times.Exactly( restarts ) );
      }

      [Test]
      [TestCase( 1 )]
      [TestCase( 5 )]
      [TestCase( 10 )]
      [TestCase( 100 )]
      public void Start_WithDifferentMoveCount_ShouldAksUserForCoordinatesCorrectAmountTimes(int movesBeforeGameOver)
      {
         const int extraReadLinesToAskAboutNewGame = 1;
         // Arrange
         SetUpGameNotOverTimes( movesBeforeGameOver, Player.Computer );

         // Act
         new BattleshipsConsoleGame( _battleshipsMessages.Object, _consoleWrapperMock.Object, _boardConsoleUI.Object, _factory.Object ).Start();

         // Assert
         AssertGameHasFinishedCorrectly( Times.Once() );
         _game.Verify( m => m.MakeMoveAndLetOppoentMove( It.IsAny<char>(), It.IsAny<int>() ), Times.Exactly( movesBeforeGameOver ) );
         _consoleWrapperMock.Verify(m => m.ReadLine(), Times.Exactly(movesBeforeGameOver + extraReadLinesToAskAboutNewGame ) );
         _boardConsoleUI.Verify( m => m.ShowBoards( _playerBoard.Object, _opponentBoard.Object ), Times.AtLeast(movesBeforeGameOver) );
         
      }

      [Test]
      [TestCase( Player.Computer )]
      [TestCase( Player.User )]
      public void Start_WithDifferentWinners_ShourdOutputCorrectWinner( Player winner )
      {
         // Arrange
         SetUpGameNotOverTimes( 15, winner);

         // Act
         new BattleshipsConsoleGame( _battleshipsMessages.Object, _consoleWrapperMock.Object, _boardConsoleUI.Object, _factory.Object ).Start();

         // Assert
         AssertGameHasFinishedCorrectly( Times.Once() );

         _battleshipsMessages.Verify( m => m.GetPlayerWinMessage( winner ), Times.Once() );
      }


      [Test]
      public void Start_WintGameExeption_ShouldShowAMessageAndNotThrowException()
      {
         // Arrange
         SetUpGameNotOverTimes( 5, Player.Computer );
         _game.Setup( m => m.MakeMoveAndLetOppoentMove( It.IsAny<char>(), It.IsAny<int>() ) ).Throws(new Exception());
         // Act
         new BattleshipsConsoleGame( _battleshipsMessages.Object, _consoleWrapperMock.Object, _boardConsoleUI.Object, _factory.Object ).Start();

         // Assert
         AssertGameHasFinishedWithError();
      }

      [Test]
      public void Start_WintBoardExeption_ShouldShowAMessageAndNotThrowException()
      {
         // Arrange
         SetUpGameNotOverTimes( 5, Player.Computer );
         _playerBoard.Setup( m => m.CheckCooridnates( It.IsAny<char>(), It.IsAny<int>() ) ).Throws( new Exception() );
         // Act
         new BattleshipsConsoleGame( _battleshipsMessages.Object, _consoleWrapperMock.Object, _boardConsoleUI.Object, _factory.Object ).Start();

         // Assert
         AssertGameHasFinishedWithError();
      }

      private void AssertGameHasFinishedWithError(  )
      {
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _gameIsBroken ) ), Times.Once() );
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _gameOverMessage ) ), Times.Never );
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _someoneWonMessage ) ), Times.Never );
      }
      private void AssertGameHasFinishedCorrectly( Times times )
      {
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _gameIsBroken ) ), Times.Never );
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _gameOverMessage ) ), times );
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _someoneWonMessage ) ), times );
         _battleshipsMessages.VerifyGet( m => m.WantToRestartMessage, times);
      }
   }
}
