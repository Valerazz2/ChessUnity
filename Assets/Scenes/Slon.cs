using System;

namespace Chess
{
    public class Slon: Figure
    {
        public override FigureType GetFigureType()
        {
            return FigureType.Slon;
        }

        public override bool AbleMoveTo(Tile target)
        {
            var step = ownTile.pos.GetStep(target.pos);
            if (step.IsZero() || !step.IsDiagonal())
            {
                return false;
            }

            return CheckTiles(step, target);
        }

        public Slon(Desk getDesk) : base(getDesk)
        {
        }
    }
}