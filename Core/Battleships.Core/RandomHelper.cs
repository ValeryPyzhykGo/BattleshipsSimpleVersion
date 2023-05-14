using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Core
{
   internal class RandomHelper
   {
      private readonly Random _rand = new Random();

      internal char GetColumn()
      {
         return (char) ( BattleshipsGameConstans.FirstColumnLetter + _rand.Next( 0, BattleshipsGameConstans.BoardSideSize ) );
      }

      internal int GetRow()
      {
         return _rand.Next( BattleshipsGameConstans.BoardFirstRowNumber, BattleshipsGameConstans.BoardLastRowNumber );
      }

      internal bool GetBooleand()
      {
         return _rand.Next( 0, 1 ) == 1 ;
      }
   }
}
