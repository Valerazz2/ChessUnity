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
        public Square OwnSquare;

        protected Piece(Desk getDesk) : base(getDesk) {}

        public bool TryMoveSuccess(Square target)
        {
            var targetPiece = target.Piece;
            var oldSquare = OwnSquare;
        
            MoveToWithOutChecking(target);

            var king = Desk.FindKing(Color);
            var isCheck = IsCheckTo(king);
        
            MoveToWithOutChecking(oldSquare);
            
            target.Piece = targetPiece;
            
            return !isCheck;
        }

        public void MoveToWithOutChecking(Square target)
        {
            OwnSquare.Piece = null;
            target.Piece = this;
            OwnSquare = target;
        }
        public void MoveTo(Square target)
        {
            if (AbleMoveTo(target) && TryMoveSuccess(target))
            {
                WasMoved = true;
                OwnSquare.Piece = null;
                target.Piece = this;
                OwnSquare = target;   
                return;
            }
            throw new Exception("Loshara");
        }
        public abstract PieceType GetPieceType();
        public abstract bool AbleMoveTo(Square target);

        private bool IsCheckTo(Piece king)
        {
            var oppositeColor = king.Color.Invert();
            return Desk.FindPieceColor(oppositeColor).Any(figure => figure.AbleMoveTo(king.OwnSquare));
        }
        protected bool CheckTile(Square square, ChessColor chessColor)
        {
            return square.Piece == null || square.Piece.Color != chessColor;
        }

        public bool AbleMoveAnyWhere()
        {
            return Desk.Squares.Cast<Square>().Any(square => AbleMoveTo(square) && TryMoveSuccess(square));
        }

        public List<Square> AbleMoveTiles()
        {
            List<Square> squers = new List<Square>();
            foreach (var square in Desk.Squares)
            {
                if (AbleMoveTo(square) && TryMoveSuccess(square))
                {
                    squers.Add(square);
                }
            }

            return squers;
        }

        protected bool CheckTiles(Square target)
        {
            var step = OwnSquare.Pos.GetStep(target.Pos);
            for (var pos = OwnSquare.Pos + step; pos != target.Pos; pos += step)
            {
                if (Desk.GetPieceAt(pos) != null)
                {
                    return false;
                }
            }
            return CheckTile(target, Color);
        }
    }
}
