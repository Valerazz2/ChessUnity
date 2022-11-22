using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Model
{
    public abstract class Piece : DeskObj
    {
        public bool WasMoved;
        public ChessColor Color;
        public Square Square;
        protected int price;

        protected Piece(Desk getDesk) : base(getDesk) {}

        protected bool TryMoveSuccess(Square target)
        {
            Debug.Log("yep");
            var targetPiece = target.Piece;
            var oldSquare = Square;
        
            MoveToWithOutChecking(target);

            var king = Desk.FindKing(Color);
            var isCheck = Desk.IsCheckTo(king);
        
            MoveToWithOutChecking(oldSquare);
            
            target.Piece = targetPiece;
            
            return !isCheck;
        }

        public void MoveToWithOutChecking(Square target)
        {
            Square.Piece = null;
            target.Piece = this;
            Square = target;
        }
        public void MoveTo(Square target)
        {
            if (AbleMoveTo(target))
            {
                if (target.Piece != null)
                {
                    Desk.OnPieceRemoveInvoke(target.Piece);
                }
                WasMoved = true;
                Square.Piece = null;
                target.Piece = this;
                Square = target;
                return;
            }
            throw new Exception("Loshara");
        }
        public abstract PieceType GetPieceType();
        public abstract bool AbleMoveTo(Square target);
        
        protected bool CheckTile(Square square, ChessColor chessColor)
        {
            return square.Piece == null || square.Piece.Color != chessColor;
        }

        public bool AbleMoveAnyWhere()
        {
            return Desk.ISquares.Any(square => AbleMoveTo(square));
        }

        protected bool CheckTiles(Square target)
        {
            var step = Square.Pos.GetStep(target.Pos);
            for (var pos = Square.Pos + step; pos != target.Pos; pos += step)
            {
                if (Desk.GetPieceAt(pos) != null)
                {
                    return false;
                }
            }
            return CheckTile(target, Color);
        }
        public bool ReachedLastSquare()
        {
            return (Square.Pos.Y == 0 || Square.Pos.Y == Desk.DeskSizeY - 1) && GetPieceType() == PieceType.Pawn ;
        }
    }
}
