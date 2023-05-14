using Battleships.Core.Exceptions;
using System.Collections.Generic;

namespace Battleships.Core
{
   public class Ship
   {
      public Ship( ShipClass shipClass )
      {
         ShipClass = shipClass;
         LifesLeft = GetShipLength( shipClass );
      }
      public ShipClass ShipClass
      {
         get; private set;
      }
      public int LifesLeft
      {
         get; private set;
      }

      internal void Hit()
      {
         if ( LifesLeft == 0 )
         {
            throw new HitSankShipException();
         }
         LifesLeft--;
      }

      private static readonly Dictionary<ShipClass, int> _shipsLength = new Dictionary<ShipClass, int> {
            { ShipClass.Carrier,    5 },
            { ShipClass.Battleship, 4 },
            { ShipClass.Destroyer,  3 },
            { ShipClass.Submarine,  3 },
            { ShipClass.Cruiser,    2 }
        };
      public static int GetShipLength( ShipClass shipClass )
      {
         return _shipsLength[shipClass];
      }

      public int GetShipLength()
      {
         return GetShipLength( ShipClass );
      }
   }
}
