using NUnit.Framework;
using Battleships.ConsoleWrapper;
using Moq;
using System.Collections.Generic;

namespace Battleships.EndToEndTests
{
   public class BattleshipsConsoleGameTests_WrongIput
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
      public void Play_WithBadCoordinates_ShouldFinishGameWithoutErrors()
      {
         // Arrange
         var stepsWithBadCoordinates = new List<string> {
            "A22",
            "Some message",
            "A",
            "1",
            "10000000000000000",
            "-1"
         };

         SetUpSteps( stepsWithBadCoordinates, Steps.CorrectSuccessiveSteps, Steps.NoNewGame );

         // Act
         new BattleshipsConsoleGame( _battleshipsMessages, _consoleWrapperMock.Object ).Start();

         // Assert 
         AssertGameHasFinishedCorrectly( Times.Once() );

         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _battleshipsMessages.TryAgainMessage ) ), Times.Exactly( stepsWithBadCoordinates.Count ) );
      }

      [Test]
      public void Play_WithSameMoveTwice_ShouldFinishGameWithoutErrors()
      {
         // Arrange
         var stepsWithRepeatedMove = new List<string> {
            "A1",
            "A1"
         };

         SetUpSteps(
             stepsWithRepeatedMove,
             Steps.CorrectSuccessiveSteps,
             Steps.NoNewGame );

         // Act
         new BattleshipsConsoleGame( _battleshipsMessages, _consoleWrapperMock.Object ).Start();

         // Assert 
         AssertGameHasFinishedCorrectly( Times.Once() );

         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _battleshipsMessages.TryAgainMessage ) ), Times.Exactly(2) );
      }

      public void AssertGameHasFinishedCorrectly( Times times )
      {
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _battleshipsMessages.GameOver ) ), times );
         _consoleWrapperMock.Verify( m => m.WriteLine( It.Is<string>( input => input == _battleshipsMessages.GameIsBrokenMessage ) ), Times.Never );
      }
   }
}