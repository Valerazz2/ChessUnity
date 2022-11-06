using System;
using System.Collections.Generic;

namespace Chess.Model
{
    public abstract class Piece : DeskObj
    {
        public bool wasMoved;
        public ChessColor color;
        public Square OwnSquare;

        protected Piece(Desk getDesk) : base(getDesk) {}

        public bool TryMoveSuccess(Square target)
        {
            Piece piece = null;
            if (target.Piece != null)
            {
                piece = target.Piece;
            }
            Square oldSquare = OwnSquare;
        
            MoveToWithOutChecking(target);

            Piece king = Desk.FindKing(color);
            if (king == null)
            {
                throw new Exception("No King in desk");
            }
            bool isCheck = IsCheckTo(king);
        
            MoveToWithOutChecking(oldSquare);
            if (piece != null)
            {
                piece.OwnSquare = target;
                target.Piece = piece;
            }
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
                wasMoved = true;
                OwnSquare.Piece = null;
                target.Piece = this;
                OwnSquare = target;   
                return;
            }
            throw new Exception("Loshara");
        }
        public abstract PieceType GetFigureType();
        public abstract bool AbleMoveTo(Square target);

        private bool IsCheckTo(Piece king)
        {
            ChessColor oppositeColor = king.color == ChessColor.White ? ChessColor.Black : ChessColor.White;
            foreach (var figure in Desk.AllFigureColor(oppositeColor))
            {
                if (figure.AbleMoveTo(king.OwnSquare))
                {
                    return true;
                }
            }
            return false;
        }
        protected bool CheckTile(Square square, ChessColor chessColor)
        {
            if (square.Piece == null || square.Piece.color != chessColor)
            {
                return true;
            }
            return false;
        }

        public bool AbleMoveAnyWhere()
        {
            foreach (var tile in Desk.desk)
            {
                if (AbleMoveTo(tile) && TryMoveSuccess(tile))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Square> AbleMoveTiles()
        {
            List<Square> tiles = new List<Square>();
            foreach (var tile in Desk.desk)
            {
                if (AbleMoveTo(tile) && TryMoveSuccess(tile))
                {
                    tiles.Add(tile); 
                }
            }
            return tiles;
        }

        protected bool CheckTiles(Vector2Int step, Square target)
        {
            for (var pos = OwnSquare.Pos + step; pos != target.Pos; pos += step)
            {
                if (Desk.GetFigureAt(pos) != null)
                {
                    return false;
                }
            }
            return CheckTile(target, color);
        }
    }
}
