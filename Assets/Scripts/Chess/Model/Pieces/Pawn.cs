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
            return MoveIsForward(target) && target.Piece == null || AbleEat(target);
        }

        private bool MoveIsForward(Square targetSquare)
        {
            var distance = targetSquare.Pos - OwnSquare.Pos;
            if (!WasMoved && Math.Abs(distance.Y) == 2)
            {
                distance.Y /= 2;
            }
            return Color.GetNaturalDirection() == distance && CheckTiles(targetSquare);
        }

        private bool AbleEat(Square targetSquare)
        {
            var dist = targetSquare.Pos - OwnSquare.Pos;
           
            return dist.Y == Color.GetNaturalDirection().Y && targetSquare.Piece != null && 
                   Math.Abs(dist.X) == 1 && targetSquare.Piece.Color != Color;
        }

        public Pawn(Desk getDesk) : base(getDesk) { }
    }
}
