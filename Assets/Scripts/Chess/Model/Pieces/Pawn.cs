using System;

namespace Chess.Model.Pieces
{
    public class Pawn : Piece
    {
        public override PieceType GetPieceType()
        {
            return PieceType.Pawn;
        }

        public override bool AbleMoveTo(Square target)
        {
            return MoveIsForward(target) || AbleEat(target);
        }
        
        private bool MoveIsForward(Square targetSquare)
        {
            var distance = targetSquare.Pos - Square.Pos;
            if (!WasMoved && Math.Abs(distance.Y) == 2)
            {
                distance.Y /= 2;
            }
            return Color.GetNaturalDirection() == distance && CheckTiles(targetSquare) && targetSquare.Piece == null;
        }

        private bool AbleEat(Square targetSquare)
        {
            var dist = targetSquare.Pos - Square.Pos;
            if (dist.Y == Color.GetNaturalDirection().Y && targetSquare.Piece != null && 
                Math.Abs(dist.X) == 1 && targetSquare.Piece.Color != Color)
            {
                return true;
            }
            
            return TakeOnThePass(targetSquare);
        }

        private bool TakeOnThePass(Square target)
        {
            if (Desk.prevMove.Piece != null)
            {
                var dist = Desk.prevMove.Piece.Square.Pos - Square.Pos;
                var deltaY = Desk.prevMove.MovedFrom.Pos.Y - Desk.prevMove.Piece.Square.Pos.Y;
            
                return Math.Abs(dist.X) == 1 && Desk.prevMove.Piece.GetPieceType() == PieceType.Pawn &&
                       Desk.GetPieceAt(Square.Pos + new Vector2Int(dist.X, 0)) == Desk.prevMove.Piece &&
                       Math.Abs(deltaY) == 2 && Desk.GetSquareAt(Square.Pos + new Vector2Int(dist.X, deltaY / 2)) == target
                       && target.Piece == null;
            }
            return false;
        }

        public Pawn(Desk getDesk) : base(getDesk) { }
    }
}
