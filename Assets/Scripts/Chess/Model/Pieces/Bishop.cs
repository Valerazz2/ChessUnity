namespace Chess.Model.Pieces
{
    public class Bishop: Piece
    {
        public override PieceType GetPieceType()
        {
            return PieceType.Bishop;
        }

        public override bool AbleMoveTo(Square target)
        {
            var step = OwnSquare.Pos.GetStep(target.Pos);
            return !step.IsZero() && step.IsDiagonal() && CheckTiles(target);
        }

        public Bishop(Desk getDesk) : base(getDesk)
        {
        }
    }
}