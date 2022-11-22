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
            return MoveIsForward(target) && TryMoveSuccess(target) || AbleEat(target) && TryMoveSuccess(target);
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
           
            return dist.Y == Color.GetNaturalDirection().Y && targetSquare.Piece != null && 
                   Math.Abs(dist.X) == 1 && targetSquare.Piece.Color != Color;
        }

        public Pawn(Desk getDesk) : base(getDesk)
        {
            price = 1;
        }
    }
}
