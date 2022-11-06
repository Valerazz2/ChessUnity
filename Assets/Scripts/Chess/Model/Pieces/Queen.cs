namespace Chess.Model.Pieces
{
    public class Queen: Piece
    {
        public override PieceType GetFigureType()
        {
            return PieceType.Queen;
        }

        public override bool AbleMoveTo(Square target)
        {
            var step = OwnSquare.Pos.GetStep(target.Pos);
            
            return CheckTiles(step, target) && !step.IsZero();
        }

        public Queen(Desk getDesk) : base(getDesk) {}
    }
}