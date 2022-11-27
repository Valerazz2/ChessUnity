namespace Chess.Model.Pieces
{
    public class Knight: Piece
    {
        public override PieceType GetPieceType()
        {
            return PieceType.Knight;
        }

        public override bool AbleMoveTo(Square target)
        {
            var dist = Vector2Int.Distance(target.Pos, Square.Pos);
            if (dist.X == 1 && dist.Y == 2 || dist.X == 2 && dist.Y == 1)
            {
                return CheckTile(target, Color);
            }

            return false;
        }

        public Knight(Desk getDesk) : base(getDesk) { }
    }
}