using UnityEngine;

namespace Chess.Model.Pieces
{
    public class Queen: Piece
    {
        public override PieceType GetPieceType()
        {
            return PieceType.Queen;
        }

        public override bool AbleMoveTo(Square target)
        {
            var step = OwnSquare.Pos.GetStep(target.Pos);
            return !step.IsZero() && CheckTiles(target);
        }

        public Queen(Desk getDesk) : base(getDesk) {}
    }
}