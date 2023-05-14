using Battleships.Core;

namespace Battleships.ConsoleWrapper
{
   /// <summary>
   /// This interface needed only for unit tesing purposes.
   /// </summary>
   internal interface IBattleshipsGameFactory
   {
      internal IBattleshipsGame Create();
   }
}
