using Battleships.Core.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Battleships.Core.Tests
{
   [TestFixture]
   public class ShipTests
   {
      [Test]
      public void Constructor_WhenShipCreated_ShouldSetShipClassAndLifesLeft()
      {
         // Arrange
         var shipClass = ShipClass.Battleship;

         // Act
         var ship = new Ship( shipClass );

         // Assert
         Assert.AreEqual( shipClass, ship.ShipClass );
         Assert.AreEqual( ship.GetShipLength(), ship.LifesLeft );
      }

      [Test]
      public void Hit_WhenShipIsNotSunk_ShouldDecrementLifesLeft()
      {
         // Arrange
         var ship = new Ship( ShipClass.Destroyer );
         var initialLifesLeft = ship.LifesLeft;

         // Act
         ship.Hit();

         // Assert
         Assert.AreEqual( initialLifesLeft - 1, ship.LifesLeft );
      }

      [Test]
      public void Hit_WhenShipIsSunk_ShouldThrowException()
      {
         // Arrange
         var ship = new Ship( ShipClass.Battleship );
         while ( ship.LifesLeft > 0 )
         {
            ship.Hit();
         }

         // Act & Assert
         Assert.Throws<HitSankShipException>( () => ship.Hit() );
      }

      [Test]
      public void GetShipLength_WhenValidShipClass_ShouldReturnCorrectLength()
      {
         // Arrange
         var shipClass = ShipClass.Destroyer;
         var expectedLength = 3;

         // Act
         var length = Ship.GetShipLength( shipClass );

         // Assert
         Assert.AreEqual( expectedLength, length );
      }

      [Test]
      public void GetShipLength_WhenInvalidShipClass_ShouldThrowException()
      {
         // Arrange
         var shipClass = (ShipClass) 10;

         // Act & Assert
         Assert.Throws<KeyNotFoundException>( () => Ship.GetShipLength( shipClass ) );
      }

      [Test]
      public void GetShipLength_WithoutSpecifyingShipClass_ShouldReturnShipClassLength()
      {
         // Arrange
         var shipClass = ShipClass.Battleship;
         var ship = new Ship( shipClass );

         // Act
         var length = ship.GetShipLength();

         // Assert
         Assert.AreEqual( Ship.GetShipLength( shipClass ), length );
      }
   }
}
