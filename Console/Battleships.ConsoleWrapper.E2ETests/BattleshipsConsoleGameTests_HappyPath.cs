using NUnit.Framework;
using Battleships.ConsoleWrapper;
using Moq;
using System.Collections.Generic;

namespace Battleships.EndToEndTests
{
   public class BattleshipsConsoleGameTests_HappyPath
   {
      private IBattleshipsConsoleGameMessages _battleshipsMessages;
      private Mock<IConsoleWraper> _consoleWrapperMock;

      [SetUp]
      public void SetUp()
      {
         _battleshipsMessages = new BattleshipsConsoleGameMessagesEng();
         _consoleWrapperMock = new Mock<IConsoleWraper>();

      }

      private void SetUpSteps( params IEnumerable<string>[] listOfSteps )
      {
         var step = -1;
         var steps = new List<string>();
         foreach ( var s in listOfSteps )
         {
            steps.AddRange( s );
         }

         _consoleWrapperMock.Setup( x => x.ReadLine() ).Returns( () =>
         {
            step++;
            return steps[step];
         } );
      }

      [Test]
      public void Play_OneGame_ShouldFinishGameWithoutErrors()
      {
         // Arrange

         SetUpSteps( Steps.CorrectSuccessiveSteps, Steps.NoNewGame );

         var game = new BattleshipsConsoleGame( _battleshipsMessages, _consoleWrapperMock.Object );

         // Act
         game.Start();

         // Assert
         AssertGameHasFinishedCorrectly( Times.Once() );
      }

      [Test]
      public void Play_ThreeGames_ShouldFinishGameWithoutErrorsThreeTimes()
      {
         // Arrange

         SetUpSteps( Steps.CorrectSuccessiveSteps,
            Steps.NewGame,
            Steps.CorrectSuccessiveSteps,
            Steps.NewGame,
            Steps.CorrectSuccessiveSteps,
            Steps.NoNewGame );


         var game = new BattleshipsConsoleGame( _battleshipsMessages, _consoleWrapperMock.Object );

         // Act
         game.Start();

         // Assert
         AssertGameHasFinishedCorrectly( Times.Exactly( 3 ) );
      }

      public void AssertGameHasFinishedCorrectly( Times times )
      {
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _battleshipsMessages.GameOver ) ), times );
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _battleshipsMessages.GameIsBrokenMessage ) ), Times.Never );
      }
   }
}