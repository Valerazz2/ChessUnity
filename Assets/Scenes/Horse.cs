namespace Chess
{
    public class Horse: Figure
    {
        public override FigureType GetFigureType()
        {
            return FigureType.Horse;
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

        public Horse(Desk getDesk) : base(getDesk)
        {
        }
    }
}