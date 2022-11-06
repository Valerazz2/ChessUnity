namespace Chess
{
    public class Knight: Figure
    {
        public override FigureType GetFigureType()
        {
            return FigureType.Knight;
        }

        public override bool AbleMoveTo(Tile target)
        {
            var dist = Vector2Int.Distance(target.pos, ownTile.pos);
            if (dist.X == 1 && dist.Y == 2 || dist.X == 2 && dist.Y == 1)
            {
                return CheckTile(target, color);
            }

            return false;
        }

        public Knight(Desk getDesk) : base(getDesk)
        {
        }
    }
}