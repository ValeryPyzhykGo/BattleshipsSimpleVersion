using Battleships.Core.Exceptions;
using System;
using System.Linq;

namespace Battleships.Core.Board
{
   internal class Board : IBoard
   {
      private readonly Ship[] _ships = new Ship[5];
      private readonly Cell[,] _cells = new Cell[10, 10];
      public int LifesLeft { get; private set; } = 0;

      public bool CheckCooridnates( char column, int row )
      {
         return
            column >= BattleshipsGameConstans.FirstColumnLetter &&
            column <= BattleshipsGameConstans.LastColumnLetter &&
            row >= BattleshipsGameConstans.BoardFirstRowNumber &&
            row <= BattleshipsGameConstans.BoardLastRowNumber;
      }
      internal (int, int) ValidateAndConvertCoordinates( char column, int row )
      {
         if ( column < BattleshipsGameConstans.FirstColumnLetter || column > BattleshipsGameConstans.LastColumnLetter )
         {
            throw new ArgumentOutOfRangeException( $"Column symbol {column} is out of range." );
         }
         if ( row < BattleshipsGameConstans.BoardFirstRowNumber || row > BattleshipsGameConstans.BoardLastRowNumber )
         {
            throw new ArgumentOutOfRangeException( $"Row number {row} is out of range." );
         }

         return (column - BattleshipsGameConstans.FirstColumnLetter, row - 1);
      }
      public CellStatus GetStatus( char column, int row )
      {
         var (columnConverted, rowConverted) = ValidateAndConvertCoordinates( column, row );
         return _cells[columnConverted, rowConverted] == null ? CellStatus.Undescovered : _cells[columnConverted, rowConverted].Status;
      }

      public ShipClass? GetShipClass( char column, int row )
      {
         var (columnConverted, rowConverted) = ValidateAndConvertCoordinates( column, row );
         return _cells[columnConverted, rowConverted]?.Ship.ShipClass;
      }

      ShipClass? IPlayerBoard.GetShipClass( char column, int row )
      {
         var (columnConverted, rowConverted) = ValidateAndConvertCoordinates( column, row );
         return _cells[columnConverted, rowConverted]?.Ship?.ShipClass;
      }

      ShipClass? IOpponentBoard.GetShipClass( char column, int row )
      {
         var (columnConverted, rowConverted) = ValidateAndConvertCoordinates( column, row );
         return _cells[columnConverted, rowConverted]?.Status == CellStatus.Hit ? _cells[columnConverted, rowConverted].Ship.ShipClass : null;
      }

      public bool TryPlaceShip( char column, int row, bool isVertical, Ship ship )
      {
         if ( !CheckCoordinates( column, row ) )
         {
            return false;
         }
         var (columnConverted, rowConverted) = ValidateAndConvertCoordinates( column, row );

         var length = ship.GetShipLength();
         var columnToCheck = columnConverted;
         var rowToCheck = rowConverted;
         var i = 0;
         while ( i < length && columnToCheck < BattleshipsGameConstans.BoardSideSize && rowToCheck < BattleshipsGameConstans.BoardSideSize && _cells[columnToCheck, rowToCheck] == null )
         {
            columnToCheck = isVertical ? columnToCheck : columnToCheck + 1;
            rowToCheck = isVertical ? rowToCheck + 1 : rowToCheck;
            i++;
         }

         if ( columnToCheck == BattleshipsGameConstans.BoardSideSize || columnToCheck == BattleshipsGameConstans.BoardSideSize || _cells[columnToCheck, rowToCheck] != null )
         {
            return false;
         }

         for ( var j = 0; j < length; j++ )
         {
            if ( isVertical )
            {
               _cells[columnConverted, rowConverted + j] = new Cell() { Status = CellStatus.Undescovered, Ship = ship };
            }
            else
            {
               _cells[columnConverted + j, rowConverted] = new Cell() { Status = CellStatus.Undescovered, Ship = ship };
            }
         }
         LifesLeft += ship.LifesLeft;
         _ships[(int) ship.ShipClass] = ship;
         return true;
      }

      private bool CheckCoordinates( char column, int row )
      {
         return column >= BattleshipsGameConstans.FirstColumnLetter && column <= BattleshipsGameConstans.LastColumnLetter && row > 0 && row <= BattleshipsGameConstans.BoardSideSize;
      }

      Ship IOpponentBoard.MakeMove( char column, int row )
      {
         var (columnConverted, rowConverted) = ValidateAndConvertCoordinates( column, row );
         var cell = _cells[columnConverted, rowConverted];
         if ( cell != null && cell.Status != CellStatus.Undescovered )
         {
            throw new CellIsAlreadyDiscoveredException( column, row );
         }
         if ( cell?.Ship != null )
         {
            cell.Status = CellStatus.Hit;
            LifesLeft--;
            cell.Ship.Hit();
         }
         else
         {
            _cells[columnConverted, rowConverted] = Cell.MissCell;
            return null;
         }

         return cell.Ship;
      }
   }
}
