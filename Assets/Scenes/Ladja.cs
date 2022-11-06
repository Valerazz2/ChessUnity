using System;

namespace Chess
{
    public class Ladja: Figure
    {
        public override FigureType GetFigureType()
        {
            return FigureType.Ladja;
        }
        
        public override bool AbleMoveTo(Tile target)
        {
            var step = ownTile.pos.GetStep(target.pos);
            if (step.IsZero() || step.IsDiagonal()) 
            {
                return false;
            }

            return CheckTiles(step, target);
        }

        public Ladja(Desk getDesk) : base(getDesk) { }
    }
}