namespace Chess.Model.Pieces
{
    public class Bishop: Piece
    {
        public override PieceType GetFigureType()
        {
            return PieceType.Bishop;
        }

        public override bool AbleMoveTo(Square target)
        {
            var step = OwnSquare.Pos.GetStep(target.Pos);
            if (step.IsZero() || !step.IsDiagonal())
            {
                return false;
            }

            return CheckTiles(step, target);
        }

        public Bishop(Desk getDesk) : base(getDesk)
        {
        }
    }
}