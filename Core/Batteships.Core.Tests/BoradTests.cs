using Battleships.Core.Board;
using Battleships.Core.Exceptions;
using NUnit.Framework;
using System;

namespace Battleships.Core.Tests
{
   [TestFixture]
   public class BoardTests
   {
      private Board.Board _board;

      [SetUp]
      public void Setup()
      {
         _board = new Board.Board();
      }

      [Test]
      public void ValidateAndConvertCoordinates_WithValidCoordinates_ShouldReturnConvertedCoordinates()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         var expectedCoordinates = (0, 0);

         // Act
         var result = _board.ValidateAndConvertCoordinates( column, row );

         // Assert
         Assert.AreEqual( expectedCoordinates, result );
      }

      [Test]
      public void ValidateAndConvertCoordinates_WithInvalidColumn_ShouldThrowArgumentOutOfRangeException()
      {
         // Arrange
         char column = 'K';
         int row = 1;

         // Act & Assert
         Assert.Throws<ArgumentOutOfRangeException>( () => _board.ValidateAndConvertCoordinates( column, row ) );
      }

      [Test]
      public void ValidateAndConvertCoordinates_WithInvalidRow_ShouldThrowArgumentOutOfRangeException()
      {
         // Arrange
         char column = 'A';
         int row = 11;

         // Act & Assert
         Assert.Throws<ArgumentOutOfRangeException>( () => _board.ValidateAndConvertCoordinates( column, row ) );
      }

      [Test]
      public void GetStatus_WhenCellIsNull_ShouldReturnUndiscovered()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         var expectedStatus = CellStatus.Undescovered;

         // Act
         var result = _board.GetStatus( column, row );

         // Assert
         Assert.AreEqual( expectedStatus, result );
      }

      [Test]
      public void GetStatus_WhenCellIsNotNull_ShouldReturnCellStatus()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         var ship = new Ship( ShipClass.Battleship );
         _board.TryPlaceShip( column, row, true, ship );
         var expectedStatus = CellStatus.Undescovered;

         // Act
         var result = _board.GetStatus( column, row );

         // Assert
         Assert.AreEqual( expectedStatus, result );
      }

      [Test]
      public void GetShipClass_WhenCellIsNull_ShouldReturnNull()
      {
         // Arrange
         char column = 'A';
         int row = 1;

         // Act
         var result = _board.GetShipClass( column, row );

         // Assert
         Assert.IsNull( result );
      }

      [Test]
      public void GetShipClass_WhenCellIsNotNull_ShouldReturnShipClass()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         var ship = new Ship( ShipClass.Battleship );
         _board.TryPlaceShip( column, row, true, ship );
         var expectedShipClass = ShipClass.Battleship;

         // Act
         var result = _board.GetShipClass( column, row );

         // Assert
         Assert.AreEqual( expectedShipClass, result );
      }


      [Test]
      public void TryPlaceShip_WithValidCoordinatesAndHorizontalPlacement_ShouldReturnTrueAndPlaceShip()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         bool isVertical = false;
         var ship = new Ship( ShipClass.Battleship );

         // Act
         var result = _board.TryPlaceShip( column, row, isVertical, ship );

         // Assert
         Assert.IsTrue( result );
         Assert.AreEqual( ShipClass.Battleship, _board.GetShipClass( column, row ) );
         Assert.AreEqual( CellStatus.Undescovered, _board.GetStatus( column, row ) );
         Assert.AreEqual( 4, _board.LifesLeft );
      }

      [Test]
      public void TryPlaceShip_WithValidCoordinatesAndVerticalPlacement_ShouldReturnTrueAndPlaceShip()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         bool isVertical = true;
         var ship = new Ship( ShipClass.Battleship );

         // Act
         var result = _board.TryPlaceShip( column, row, isVertical, ship );

         // Assert
         Assert.IsTrue( result );
         Assert.AreEqual( ShipClass.Battleship, _board.GetShipClass( column, row ) );
         Assert.AreEqual( CellStatus.Undescovered, _board.GetStatus( column, row ) );
         Assert.AreEqual( 4, _board.LifesLeft );
      }

      [Test]
      public void TryPlaceShip_WithInvalidCoordinates_ShouldReturnFalseAndNotPlaceShip()
      {
         // Arrange
         char column = 'K';
         int row = 1;
         bool isVertical = false;
         var ship = new Ship( ShipClass.Battleship );

         // Act
         var result = _board.TryPlaceShip( column, row, isVertical, ship );

         // Assert
         Assert.IsFalse( result );
         Assert.AreEqual( 0, _board.LifesLeft );
      }

      [Test]
      [TestCase( "J", 1, false )]
      [TestCase( "C", 9, true )]
      public void TryPlaceShip_WithShipOutsideTheField_ShouldReturnFalseAndNotPlaceShip( char column, int row, bool isVertical)
      {
         // Arrange
         var ship = new Ship( ShipClass.Battleship );

         // Act
         var result = _board.TryPlaceShip( column, row, isVertical, ship );

         // Assert
         Assert.IsFalse( result );
         Assert.AreEqual( 0, _board.LifesLeft );
      }

      [Test]
      public void TryPlaceShip_WithInvalidCoordinatesNegativeNumber_ShouldThrowArgumentOutOfRangeException()
      {
         // Arrange
         char column = 'A';
         int row = -1;
         bool isVertical = false;
         var ship = new Ship( ShipClass.Battleship );

         // Act
         var result = _board.TryPlaceShip( column, row, isVertical, ship );

         // Assert
         Assert.IsFalse( result );
         Assert.AreEqual( 0, _board.LifesLeft );
      }

      [Test]
      public void TryPlaceShip_WithOverlappingShips_ShouldReturnFalseAndNotPlaceShip()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         bool isVertical = false;
         var ship1 = new Ship( ShipClass.Battleship );
         var ship2 = new Ship( ShipClass.Carrier );

         _board.TryPlaceShip( column, row, isVertical, ship1 ); // Place first ship

         // Act
         var result = _board.TryPlaceShip( column, row, isVertical, ship2 ); // Try to place second ship on the same position

         // Assert
         Assert.IsFalse( result );
         Assert.AreEqual( ShipClass.Battleship, _board.GetShipClass( column, row ) );
         Assert.AreEqual( CellStatus.Undescovered, _board.GetStatus( column, row ) );
         Assert.AreEqual( 4, _board.LifesLeft );
      }

      [Test]
      public void MakeMove_WithMissedCell_ShouldReturnNullAndSetMissStatus()
      {
         // Arrange
         char column = 'A';
         int row = 1;

         // Act
         var result = ( _board as IOpponentBoard ).MakeMove( column, row );

         // Assert
         Assert.IsNull( result );
         Assert.AreEqual( CellStatus.Miss, _board.GetStatus( column, row ) );
         Assert.AreEqual( 0, _board.LifesLeft );
      }

      [Test]
      public void MakeMove_CellIsAlreadyDiscovered_ThrowsCellIsAlreadyDiscoveredException()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         _board.TryPlaceShip( column, row, false, new Ship( ShipClass.Battleship ) );

         // Act & Assert
         ( _board as IOpponentBoard ).MakeMove( column, row );
         Assert.Throws<CellIsAlreadyDiscoveredException>( () => (_board as IOpponentBoard).MakeMove( column, row ) );
      }

      [Test]
      public void MakeMove_CellContainsShip_ReturnsShipAndUpdatesStatus()
      {
         // Arrange
         char column = 'A';
         int row = 1;
         var ship = new Ship( ShipClass.Battleship );
         _board.TryPlaceShip( column, row, false, ship );

         // Act
         var result = ( _board as IOpponentBoard ).MakeMove( column, row );

         // Assert
         Assert.IsNotNull( result );
         Assert.AreEqual( ShipClass.Battleship, result.ShipClass );
         Assert.AreEqual( CellStatus.Hit, _board.GetStatus( column, row ) );
         Assert.AreEqual( ship.LifesLeft , result.LifesLeft );
      }

      [TestCase( 'A', 1, ExpectedResult = true )]
      [TestCase( 'J', 10, ExpectedResult = true )]
      [TestCase( 'A', 0, ExpectedResult = false )]
      [TestCase( 'K', 5, ExpectedResult = false )]
      [TestCase( 'B', 12, ExpectedResult = false )]
      [TestCase( 'a', 1, ExpectedResult = false )]
      [TestCase( 'A', -1, ExpectedResult = false )]
      [TestCase( 'С', 2, ExpectedResult = false )]
      public bool CheckCoordinates_ValidInputs_ReturnsCorrectResult( char column, int row )
      {
         // Act
         bool result = (_board as IPlayerBoard).CheckCooridnates( column, row );

         // Assert
         return result;
      }
   }
}
