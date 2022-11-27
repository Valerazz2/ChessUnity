namespace Chess.Model.Pieces
{
    public class King : Piece
    {
        public override PieceType GetPieceType()
        {
            return PieceType.King;
        }

        public override bool AbleMoveTo(Square target)
        {
            var dist = Vector2Int.Distance(target.Pos, Square.Pos);
            if (dist.X < 2 && dist.Y < 2 && dist != Vector2Int.Zero)
            {
                return CheckTile(target, Color);
            }

            return dist == new Vector2Int(2, 0) && AbleCastling(target);
        }
        private bool AbleCastling(Square target)
        {
            var rook = Desk.FindFirstFigureByStep(target, this);
            
            return !WasMoved && rook != null && rook.GetPieceType() == PieceType.Rook && !rook.WasMoved;
        }

        public King(Desk getDesk) : base(getDesk) {}
    }
}