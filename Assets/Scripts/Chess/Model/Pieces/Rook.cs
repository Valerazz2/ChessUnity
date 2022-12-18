namespace Chess.Model.Pieces
{
    public class Rook: Piece
    {
        public override PieceType GetPieceType()
        {
            return PieceType.Rook;
        }
        
        public override bool AbleMoveTo(Square target)
        {
            var step = Square.Pos.GetStep(target.Pos);
            return CheckTiles(target) && !step.IsZero() && !step.IsDiagonal();
        }

        public Rook(Desk getDesk) : base(getDesk) { }
    }
}