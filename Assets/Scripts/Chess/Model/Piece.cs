using System;
using System.Linq;


namespace Chess.Model
{
    public abstract class Piece : DeskObj
    {
        public bool WasMoved;
        public ChessColor Color;
        public Square Square;

        protected Piece(Desk getDesk) : base(getDesk) {}

        public bool TryMoveSuccess(Square target)
        {
            //Debug.Log(this + " " + Color + " " + Square.Pos.X + " " + Square.Pos.Y + 
                      //" to " + target.Pos.X + " " + target.Pos.Y);
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
            if (AbleMoveTo(target) && TryMoveSuccess(target))
            {
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
            foreach (var square in Desk.ISquares)
            {
                if (AbleMoveTo(square) && TryMoveSuccess(square))
                {
                    return true;
                }
            }
            return Desk.ISquares.Any(square => AbleMoveTo(square) && TryMoveSuccess(square));
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
