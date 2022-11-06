namespace Chess.Model.Pieces
{
    public class Rook: Piece
    {
        public override PieceType GetFigureType()
        {
            return PieceType.Rook;
        }
        
        public override bool AbleMoveTo(Square target)
        {
            var step = OwnSquare.Pos.GetStep(target.Pos);
            if (step.IsZero() || step.IsDiagonal()) 
            {
                return false;
            }

            return CheckTiles(step, target);
        }

        public Rook(Desk getDesk) : base(getDesk) { }
    }
}