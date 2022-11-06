using System.IO;

namespace Chess
{
    public class Ferz: Figure
    {
        public override FigureType GetFigureType()
        {
            return FigureType.Ferz;
        }

        public override bool AbleMoveTo(Tile target)
        {
            var step = ownTile.pos.GetStep(target.pos);
            
            return CheckTiles(step, target) && !step.IsZero();
        }

        public Ferz(Desk getDesk) : base(getDesk) {}
    }
}