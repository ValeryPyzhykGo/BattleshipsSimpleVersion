using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Core
{
   internal class RandomWrapper : IRandomWrapper
   {
      private readonly Random _rand = new Random();

      char IRandomWrapper.GetColumn()
      {
         return (char) ( BoardSize.FirstColumnLetter + _rand.Next( 0, BoardSize.BoardSideSize ) );
      }

      int IRandomWrapper.GetRow()
      {
         return _rand.Next( BoardSize.BoardFirstRowNumber, BoardSize.BoardLastRowNumber );
      }

      bool IRandomWrapper.GetBooleand()
      {
         return _rand.NextDouble() >= 0.5;
      }
   }
}
